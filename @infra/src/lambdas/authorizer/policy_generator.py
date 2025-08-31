from auth_policy import AuthPolicy, HttpVerb
import logging

def create_base_policy(method_arn:str, principal_id:str) -> object:
	"""
	function to create a base policy
	"""
	logging.info(method_arn)
	tmp = method_arn.split(':')
	api_gateway_arn_tmp = tmp[5].split('/')
	aws_account_id = tmp[4]

	policy = AuthPolicy(principal_id, aws_account_id)
	policy.rest_api_id = api_gateway_arn_tmp[0]
	policy.region = tmp[3]
	policy.stage = api_gateway_arn_tmp[1]

	return policy

def create_policy_by_role(method_arn: str, principal_id: str, app_policy: dict) -> object:
	policy = create_base_policy(method_arn, principal_id)
	logging.info(policy)
	array_policy = app_policy[1]  # El array con las rutas y permisos

	if array_policy is None:
		policy.deny_all_methods()  # Se deniegan todos los métodos HTTP
	else:
		for item in array_policy:
			path = item['path']  # Ruta

			for acceso in item['permissions']:  # Acceso ya contiene los métodos HTTP (GET, POST, etc.)
				policy.allow_method(acceso, path)  # Se permite el método HTTP directamente

	return policy

def create_anonymous_policy(method_arn:str) -> object:
	"""
	function to create an annonymous policy
	"""
	policy = create_base_policy(method_arn, 'anonymous')
	return policy

def create_policy_m2m(method_arn: str, principal_id: str, scope: str) -> object:
    """
    function to create a policy based on scope for machine to machine
    """
    policy = None

    if scope and ( scope == "kom-services/read" or scope == "kom-services/write" ):
        policy = create_base_policy(method_arn, principal_id)
        policy.allow_method(HttpVerb.ALL, '/assistances/*')
        policy.allow_method(HttpVerb.ALL, '/masters/*')

    if scope and ( scope == "assistences-services/read"  or scope == "assistences-services/write"):
        policy = create_base_policy(method_arn, principal_id)
        policy.allow_method(HttpVerb.ALL, '/*')

    return policy
