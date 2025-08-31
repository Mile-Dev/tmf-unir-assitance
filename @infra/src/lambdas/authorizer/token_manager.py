import jwt
from jwt import PyJWKClient
import os
import logging

class TokenManager:
    """
    Class to validate and decode a token
    """

    REGION_NAME=os.getenv("AWS_REGION", "us-east-1")
    POOL_ID=os.getenv("USER_POOL_ID")
    CLIENT_ID=os.getenv('CLIENT_ID')
    PRINCIPAL_ATTR=os.getenv("PRINCIPAL_ATTR", "cognito:username")

    def __init__(self):
        self.decode_token = None
        self.token = None
        self.jwks_url=f"https://cognito-idp.{self.REGION_NAME}.amazonaws.com/{self.POOL_ID}/.well-known/jwks.json"
        self.jwks_client = PyJWKClient(self.jwks_url)
        self.user = {}

    def token_decoder(self, event:dict) -> dict:
        """
        decode JWT
        """
        if ('methodArn' not in event):
            logging.error('No se esta enviando methodArn')
            raise KeyError('Unauthorized')

        if 'authorization' in event['headers'] and event['headers']['authorization'] != 'null':
            self.token = event['headers']['authorization']

        elif 'Authorization' in event['headers'] and event['headers']['Authorization'] != 'null':
            self.token = event['headers']['Authorization']

        else:
            # policy = create_anonymous_policy(event['methodArn'])
            policy = None
            auth_response = policy.build()
            return auth_response

        self.token = self.token.replace("Bearer ", "")

    def sign_verifier(self):

        """
        Verify the sign of the token
        """

        try:
            signing_key = self.jwks_client.get_signing_key_from_jwt(self.token)
            #Verifica la firma
            self.decode_token = jwt.decode(self.token, signing_key.key, algorithms=["RS256"], options={'verify_aud': False})

        except jwt.ExpiredSignatureError as e:
            logging.error(str(e))
            raise Exception('Unauthorized')

        except Exception as e:
            logging.error(str(e))
            raise Exception('Server Error')

    def get_token_attr(self) -> tuple:
        """
        get the attrs from the token
        """
        try:
            cognito_id = self.decode_token[self.PRINCIPAL_ATTR]
            app_policy = self.get_user_permissions()

        except Exception as e:
            logging.info(self.decode_token)
            logging.error(str(e))
            raise Exception('Unauthorized')
        return cognito_id, app_policy

    def get_user_permissions(self):
        print("get_user_permission: ")

        user_email = self.get_email()
        user_groups = self.get_groups()

        self.user['email'] = user_email
        self.user['groups'] = user_groups

        logging.info(user_email)
        logging.info(user_groups)

        #If user_groups includes 'assistances-admin' or 'assistances-tmo' return all permissions
        if 'assistances-admin' or 'assistances-tmo' in user_groups:
            permissions = [{
                'path': '/*',
                'permissions': ['GET', 'POST', 'PUT', 'PATCH', 'DELETE']
            }]
            return user_email, permissions

        #If the user does not have a group
        permissions = None

        logging.info('Policy for user: ')
        logging.info(permissions)

        return user_email, permissions

    def is_user(self) -> bool:
        return bool(self.decode_token.get(self.PRINCIPAL_ATTR, False))

    def get_email(self) -> str:
        return self.decode_token.get('email', None)

    def get_groups(self) -> list:
        return self.decode_token.get('cognito:groups', [])