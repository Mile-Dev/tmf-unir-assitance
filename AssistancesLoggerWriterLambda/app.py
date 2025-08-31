import json
import boto3
import uuid
import logging
import os
from datetime import datetime, timezone
from decimal import Decimal

# Configure the root logger
logger = logging.getLogger()
logger.setLevel(os.environ.get("LOG_LEVEL", "INFO"))

state_machine_arn = os.environ.get("SF_TRACKING_MOK_ARN")

dynamodb = boto3.resource("dynamodb")
table = dynamodb.Table("AssistancesLoggerDB")
sf_client = boto3.client('stepfunctions')

class DecimalEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Decimal):
            return str(obj)
        return super().default(obj)


def start_execution(state_machine_arn,name, input_data):
    try:
        response = sf_client.start_execution(
            stateMachineArn=state_machine_arn,
            name=name,
            input=json.dumps(input_data)  # Input must be a JSON string
        )
        return response
    except Exception as e:
        print(f"Error starting execution: {e}")
        raise

def lambda_handler(event, context):
    """
    differenciate between origins, if sqs records go to handle_sqs_messages, if origin api gateway, check the method to redirect to create log or get logs
    """
    isOriginSQS = event.get("Records", None)

    logger.info(f"Received event: {json.dumps(event)}")

    if isOriginSQS:
        logger.info("Triggered by SQS")
        return handle_sqs_message(event)

    # if not sqs, check the method
    httpMethod = event.get("httpMethod", None)
    if httpMethod == "POST":
        return create_log(
            event["body"], event["requestContext"]["authorizer"]["user"]
        )  # event['body'] is a string, not a dict

    # return create_log(json.loads(event['body'])) # event['body'] is a string, not a dict
    elif httpMethod == "GET":
        if (
            not event["queryStringParameters"]
            or not event["queryStringParameters"]["eventId"]
        ):
            return {"statusCode": 400, "body": json.dumps("No eventId provided")}
        eventId = str(event["queryStringParameters"]["eventId"])
        return get_log(eventId)
    else:
        return {"statusCode": 400, "body": json.dumps("Invalid http method")}

    return {"statusCode": 200, "body": json.dumps("Mensajes procesados exitosamente")}


def handle_sqs_message(event):
    logger.info("Triggered by SQS")
    for record in event["Records"]:
        try:
            return create_log(record["body"])

        except Exception as e:
            print(f"Error procesando el mensaje: {e}")
            print(f"Mensaje original: {record['body']}")


def create_log(body, context=None):
    """
    Extract data from the event, considering it is a api proxy integration.
    key info to extract.
    body: { "eventId": 1, logData: { "description":"", "eventId":"", role:"","source":"","status":"","username":"" } }
    """
    try:
        # Extract data from the event
        body = json.loads(body)
        event_id = body.get("eventId")
        log_data = {
            "eventId": event_id,
            "action": body.get("action", "NOTE_REGISTER"),
            "description": body.get("description", ""),
            "sourceId": body.get("sourceId", ""),
            "sourceCode": body.get("sourceCode", ""),
            "statusEventId": body.get("statusEventId", ""),
            "statusEvent": body.get("statusEvent", ""),
            "sendToClient": body.get("sendToClient", False),
        }

        if context is not None:
            context = json.loads(context)
            log_data["role"] = context.get("groups")[0]
            log_data["username"] = context.get("email", "")
        else:
            log_data["role"] = body.get("role", "")
            log_data["username"] = body.get("userName", "")

        timestamp = datetime.now(timezone.utc).isoformat()
        unique_id = str(uuid.uuid4())

        item = {
            "PK": f"LOG#{event_id}",
            "SK": f"TS#{timestamp}",
            "logData": log_data,
            "timestamp": timestamp,
        }

        table.put_item(Item=item)

        action = body.get("action", "NOTE_REGISTER")
        actionSendToClient = body.get("sendToClient", False)
        sourceCode = body.get("sourceCode", "")
        name = f"{event_id}-{action}-{unique_id}"
        print(f"Codigo de actionSendToClient: {actionSendToClient}")
        logger.info(f"Codigo de actionSendToClient: {actionSendToClient}")

        if sourceCode == "MOK" and actionSendToClient:
            print(f"Codigo de actionSendToClient en el if: {actionSendToClient}")
            logger.info(f"Codigo de actionSendToClient en el if: {actionSendToClient}")

            execution_response = start_execution(state_machine_arn,name, item)

        return {"statusCode": 200, "body": json.dumps("Log created successfully")}
    except Exception as e:
        print(f"Error creating log: {e}")
        return {"statusCode": 500, "body": json.dumps("Error creating log")}


def get_log(eventId):
    """
    gets the logs based on an eventId: { "eventId": 1 }
    """
    try:
        # Get logs from DynamoDB
        response = table.query(
            KeyConditionExpression="PK = :pk",
            ExpressionAttributeValues={":pk": f"LOG#{eventId}"},
        )
        # Return logs
        return {"statusCode": 200, "body": json.dumps(response["Items"], cls=DecimalEncoder)}
    except Exception as e:
        print(f"Error getting logs: {e}")
        return {"statusCode": 500, "body": json.dumps("Error getting logs")}
