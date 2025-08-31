import boto3
import os
import json

cognito = boto3.client('cognito-idp')
USER_POOL_ID = os.environ.get('USER_POOL_ID')

def parse_query_params(event):
    # Handles both direct and encoded query strings
    params = event.get("queryStringParameters") or {}
    return {
        "group": params.get("group"),
        "email": params.get("email"),
        "enabled": params.get("enabled"),
        "userStatus": params.get("userStatus"),
        "limit": int(params.get("limit")) if params.get("limit") else 60,
        "nextToken": params.get("nextToken")
    }

def filter_users(users, enabled, user_status):
    def str_to_bool(val):
        return val.lower() in ["true", "1", "yes"] if val else None

    enabled_bool = str_to_bool(enabled)
    filtered = []
    for user in users:
        if enabled_bool is not None and user.get("Enabled") != enabled_bool:
            continue
        if user_status and user.get("UserStatus") != user_status:
            continue
        filtered.append({
            "username": user.get("Username"),
            "attributes": {attr["Name"]: attr["Value"] for attr in user.get("Attributes", [])},
            "enabled": user.get("Enabled"),
            "userStatus": user.get("UserStatus"),
            "createdAt": user.get("UserCreateDate").isoformat(),
            "lastModifiedAt": user.get("UserLastModifiedDate").isoformat()
        })
    return filtered

def lambda_handler(event, context):
    try:
        if not USER_POOL_ID:
            return {"statusCode": 500, "body": json.dumps({"error": "USER_POOL_ID not set"})}

        params = parse_query_params(event)

        users = []
        pagination_token = params["nextToken"]
        limit = min(max(params["limit"], 1), 60)  # Cognito max is 60

        # If filtering by group, use list_users_in_group
        if params["group"]:
            group_params = {
                "UserPoolId": USER_POOL_ID,
                "GroupName": params["group"],
                "Limit": limit
            }
            if pagination_token:
                group_params["NextToken"] = pagination_token

            response = cognito.list_users_in_group(**group_params)
            users = response.get("Users", [])
            next_token = response.get("NextToken")

        else:
            # Default to list_users, optionally filter by email
            list_params = {
                "UserPoolId": USER_POOL_ID,
                "Limit": limit
            }
            if pagination_token:
                list_params["PaginationToken"] = pagination_token
            if params["email"]:
                list_params["Filter"] = f'email = "{params["email"]}"'

            response = cognito.list_users(**list_params)
            users = response.get("Users", [])
            next_token = response.get("PaginationToken")

        filtered_users = filter_users(users, params["enabled"], params["userStatus"])

        return {
            "statusCode": 200,
            "body": json.dumps({
                "users": filtered_users,
                "nextToken": next_token
            }),
            "headers": {
                "Content-Type": "application/json"
            }
        }

    except Exception as e:
        return {
            "statusCode": 500,
            "body": json.dumps({"error": str(e)}),
            "headers": {
                "Content-Type": "application/json"
            }
        }
