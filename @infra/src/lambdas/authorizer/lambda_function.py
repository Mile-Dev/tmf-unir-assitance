import json

from policy_generator import create_policy_by_role, create_policy_m2m
from token_manager import TokenManager

def lambda_handler(event: dict, context: dict) -> dict:

    """
    function to return the policy with the access of the user to the different
    endpoints
    """

    print("event new: ", event)

    policy = None
    token_manager = TokenManager()

    auth_response = token_manager.token_decoder(event)

    if not auth_response:
        token_manager.sign_verifier()

        if not token_manager.is_user():
            policy = create_policy_m2m(event['methodArn'], token_manager.decode_token["sub"], token_manager.decode_token["scope"])

        else:
            cognito_id, app_policy = token_manager.get_token_attr()
            policy = create_policy_by_role(event['methodArn'], cognito_id, app_policy)

        auth_response = policy.build()

        #Agregar variables de contexto
        if token_manager.token is not None:
            context = {
                'Authorization': token_manager.token,
                'user': json.dumps(token_manager.user)
            }

        else:
            context = {}

        auth_response['context'] = context

    print("auth_response: ",auth_response)

    return auth_response