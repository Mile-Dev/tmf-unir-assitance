import json
import boto3
import uuid
import logging
import os
import base64
from datetime import datetime, timezone
from decimal import Decimal

# Configure the root logger
logger = logging.getLogger()
logger.setLevel(os.environ.get("LOG_LEVEL", "INFO"))

dynamodb = boto3.resource("dynamodb")
table = dynamodb.Table("AssistancesLoggerDB")

# ---------- Config ----------
# Parsear CLIENTS_MAP una sola vez en el cold start
try:
    CLIENTS_MAP = json.loads(os.environ.get("CLIENTS_MAP", "{}"))
    print(f"Campo CLIENTS_MAP: {CLIENTS_MAP}")
except Exception as e:
    logger.warning(f"CLIENTS_MAP inválido, usando vacío: {e}")
    CLIENTS_MAP = {}

# ---------- Utils ----------
class DecimalEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Decimal):
            return str(obj)
        return super().default(obj)

def _b64url_decode(seg: str) -> bytes:
    seg += "=" * (-len(seg) % 4)
    return base64.urlsafe_b64decode(seg.encode())

def _extract_client_id_from_jwt(token_or_bearer: str) -> str | None:
    """
    Decodifica el payload del JWT (SIN verificar firma) y retorna client_id || aud || sub.
    Acepta 'Bearer <token>' o '<token>'.
    """
    try:
        token = token_or_bearer
        if token.startswith("Bearer "):
            token = token.split(" ", 1)[1]
        parts = token.split(".")
        if len(parts) < 2:
            return None
        payload = json.loads(_b64url_decode(parts[1]).decode())
        return payload.get("client_id") or payload.get("aud") or payload.get("sub")
    except Exception as e:
        logger.debug(f"No se pudo decodificar JWT: {e}")
        return None

def _get_bearer_from_event(event: dict) -> str | None:
    """
    Obtiene el 'Bearer ...' del header Authorization (o de authorizer.Authorization).
    """
    headers = event.get("headers") or {}
    authz = headers.get("Authorization") or headers.get("authorization")
    if authz and authz.startswith("Bearer "):
        return authz

    # Algunos setups ponen el token en requestContext.authorizer.Authorization
    auth = (event.get("requestContext", {}) or {}).get("authorizer", {}) or {}
    authz2 = auth.get("Authorization")
    if authz2 and authz2.startswith("Bearer "):
        return authz2

    return None

def get_client_id_from_event(event: dict) -> str | None:
    bearer = _get_bearer_from_event(event)
    if bearer:
        return _extract_client_id_from_jwt(bearer)

    return None

def get_client_name_from_id(client_id: str | None) -> str:
    return CLIENTS_MAP.get(client_id or "", "UnknownClient")

def lambda_handler(event, context):
    """
    differenciate between origins, if sqs records go to handle_sqs_messages, if origin api gateway, check the method to redirect to create log or get logs
    """
    # Log seguro (no imprimir el JWT completo)
    safe_event = {
        "httpMethod": event.get("httpMethod"),
        "path": event.get("path") or event.get("resource"),
        "authorizerKeys": list(((event.get("requestContext", {}) or {}).get("authorizer", {}) or {}).keys())
    }
    logger.info(f"Received event (safe): {json.dumps(safe_event)}")

    isOriginSQS = event.get("Records", None)
    client_id = get_client_id_from_event(event)
    client_name = get_client_name_from_id(client_id)    
    logger.info(f"Resolved client_id={client_id}, client_name={client_name}")

    # if not sqs, check the method
    httpMethod = event.get("httpMethod", None)
    if httpMethod == "POST":
        return create_log(
            event["body"], client_name, client_id
        )  # event['body'] is a string, not a dict

    return {"statusCode": 200, "body": json.dumps("Mensajes procesados exitosamente")}

def _response(status, payload):
    return {
        "statusCode": status,
        "headers": {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Allow-Methods": "OPTIONS,POST"
        },
        "body": json.dumps(payload, ensure_ascii=False),
        "isBase64Encoded": False
    }

def create_log(body, client_name, client_id):
    """
    Extract data from the event, considering it is a api proxy integration.
    key info to extract.
    body: { "eventId": 1, logData: { "description":"", "eventId":"", role:"","source":"","status":"","username":"" } }
    """

    body = json.loads(body)
    event_id = body.get("eventId")    
    try:
        # Extract data from the event
        log_data = {
            "eventId": event_id,
            "action": body.get("action", "NOTE_REGISTER_CLIENT"),
            "description": body.get("description", ""),
            "sendToClient": body.get("sendToClient", False),
            "role": "User_External",
            "username": client_name,
        }      

        timestamp = datetime.now(timezone.utc).isoformat()
        unique_id = str(uuid.uuid4())

        item = {
            "PK": f"LOG#{event_id}",
            "SK": f"TS#{timestamp}",
            "logData": log_data,
            "timestamp": timestamp,
            "client_id": client_id,
        }

        table.put_item(Item=item)
        return _response(200, {
            "eventId": event_id,
            "status": "success",
            "message": "Log created successfully"
        })
    
    except Exception as e:
        print(f"Error creating log: {e}")
        return _response(500, {
            "eventId": event_id,
            "status": "error",
            "message": "Error creating log"
        })