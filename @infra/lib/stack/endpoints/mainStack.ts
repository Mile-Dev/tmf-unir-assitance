import * as cdk from "aws-cdk-lib";
import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as lambda from "aws-cdk-lib/aws-lambda";
import * as apigateway from "aws-cdk-lib/aws-apigateway";
import * as cognito from "aws-cdk-lib/aws-cognito";
import * as iam from "aws-cdk-lib/aws-iam";

export interface MainStackProps extends cdk.StackProps {
  region: string;
  client: string;
  project: string;
  partner: string;
  stage: string;
  account_num: string;
  userPool: cognito.UserPool;
  MasterRdsServicesLambda: lambda.IFunction;
  ProviderServiceLambda: lambda.IFunction;
  IssuanceMokServicesLambda: lambda.IFunction;
  EventServicesLambda: lambda.IFunction;
  PhoneConsultationServiceLambda: lambda.IFunction;
  VoucherServiceLambda: lambda.IFunction;
  EventStatusSwitchTempServicesLambda: lambda.IFunction;
  userPoolId: string;
  clientFrontendId: string;
  arnLayerVersionJWT: string;
  AssistancesLoggerWriterLambda: lambda.IFunction;
  AssistancesLoggerWriterExternalLambda: lambda.IFunction;
  ListUsersLambda: lambda.IFunction;
}

export class MainStack extends Stack {
  constructor(scope: Construct, id: string, props: MainStackProps) {
    super(scope, id, props);

    const api = new apigateway.RestApi(this, "ApiGateway", {
      restApiName: "MainApiTest",
      description: "API for serverless project",
    });

    const apiKeyMok = api.addApiKey("ApiKey-Mok", {
      apiKeyName: "apikey-mok-asistences",
      description: "Api para clientes del equipo mok",
      value: "BvwKaZw33M6zTiYYVjmxTaVWojUR3qgG1sFCQorB",
    });

    const usagePlanMok = api.addUsagePlan("UsagePlan-Mok", {
      name: "plan-apikey-mok",
      throttle: {
        rateLimit: 10, // 10 requests por segundo
        burstLimit: 20, // máximo de 20 en ráfaga
      },
      quota: {
        limit: 10000, // máximo 10k requests
        period: apigateway.Period.MONTH,
      },
    });

    usagePlanMok.addApiStage({
      stage: api.deploymentStage,
    });

    usagePlanMok.addApiKey(apiKeyMok);

    const apiKeyTw = api.addApiKey("ApiKey-Tw", {
      apiKeyName: "apikey-frontend-asistences",
      description: "Api para el front de asistences",
      value: "2xDjvQVo4X5jNaqbKYIOD8zp4qnuhgvfaxGWsIoe",
    });

    const usagePlanTw = api.addUsagePlan("UsagePlan-Tw", {
      name: "plan-apikey-frontend-asistences",
      throttle: {
        rateLimit: 10, // 10 requests por segundo
        burstLimit: 20, // máximo de 20 en ráfaga
      },
      quota: {
        limit: 10000, // máximo 10k requests
        period: apigateway.Period.MONTH,
      },
    });

    usagePlanTw.addApiStage({
      stage: api.deploymentStage, // Usar la misma etapa del API
    });

    usagePlanTw.addApiKey(apiKeyTw);

    const lambdaAuthorizerPolicy = new iam.PolicyStatement({
      actions: ["kms:*", "sns:Publish", "cognito:*"],
      resources: ["*"],
    });
    const layerJWT = lambda.LayerVersion.fromLayerVersionArn(
      this,
      "layerJWT",
      `arn:aws:lambda:${props.region}:${props.account_num}:layer:asistencia-viajero-jwt:${props.arnLayerVersionJWT}`
    );
    // const layerJWT = lambda.LayerVersion.fromLayerVersionArn(this, 'layerJWT', props.arnLayerVersionJWT)

    const lambdaAuthorizer = new lambda.Function(this, "AuthorizerLambda", {
      runtime: lambda.Runtime.PYTHON_3_13,
      functionName: `${props.project}-Authorizer`,
      handler: "lambda_function.lambda_handler",
      code: lambda.Code.fromAsset("src/lambdas/authorizer"),
      layers: [layerJWT], // Adjuntamos el layer a la función
      environment: {
        ACCOUNT_ID: props.account_num,
        API_ID: api.restApiId,
        CLIENT_ID: props.clientFrontendId,
        STAGE: props.stage,
        USER_POOL_ID: props.userPoolId,
      },
    });

    lambdaAuthorizer.addToRolePolicy(lambdaAuthorizerPolicy);

    const authorizerLambda = new apigateway.RequestAuthorizer(
      this,
      "ApiLambdaAuthorizer",
      {
        handler: lambdaAuthorizer,
        identitySources: [apigateway.IdentitySource.header("Authorization")],
        authorizerName: "lambdaAutorizadorPersonalizado",
      }
    );

    // Crear recurso /temp una sola vez
    const tempResource = api.root.addResource("temp");
    const assistResource = tempResource.addResource("assist");

    // Crear recurso proxy bajo /v1/assistances/{proxy+}
    const assistProxy = assistResource.addResource("{proxy+}");
    assistProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(
        props.EventStatusSwitchTempServicesLambda,
        {
          proxy: true,
        }
      ),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear recurso /v1 una sola vez
    const v1Resource = api.root.addResource("v1");

    // Crear recurso /v1/users
    const usersResource = v1Resource.addResource("users");
    usersResource.addMethod(
      "GET",
      new apigateway.LambdaIntegration(props.ListUsersLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/assistances
    const assistancesResource = v1Resource.addResource("assistances");

    // Crear recurso proxy bajo /v1/assistances/{proxy+}
    const assistancesProxy = assistancesResource.addResource("{proxy+}");
    assistancesProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.EventServicesLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear recurso log bajo /v1/assistances/log v2
    const assistancesLog = assistancesResource.addResource("log");
    assistancesLog.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.AssistancesLoggerWriterLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear recurso log bajo /v1/assistances/log-externals v2
    const assistancesLogexternal =
      assistancesResource.addResource("logs-externals");
    assistancesLogexternal.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(
        props.AssistancesLoggerWriterExternalLambda,
        {
          proxy: true,
        }
      ),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/masters
    const mastersResource = v1Resource.addResource("masters");

    // Crear recurso proxy bajo /v1/masters/{proxy+}
    const mastersProxy = mastersResource.addResource("{proxy+}");
    mastersProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.MasterRdsServicesLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/vouchers
    const vouchersResource = v1Resource.addResource("vouchers");

    // Crear recurso proxy bajo /v1/vouchers/{proxy+}
    const vouchersProxy = vouchersResource.addResource("{proxy+}");
    vouchersProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.VoucherServiceLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/medical-orientation
    const medicalorientationResource = v1Resource.addResource(
      "medical-orientation"
    );

    // Crear o reutilizar recurso /v1/medical-orientation
    const phoneconsultationResource =
      medicalorientationResource.addResource("phone-consultation");

    // Crear recurso proxy bajo /v1/medical-orientation/phone-consultation/{proxy+}
    const phoneconsultationProxy =
      phoneconsultationResource.addResource("{proxy+}");
    phoneconsultationProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.PhoneConsultationServiceLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/medical-orientation
    const searchResource = phoneconsultationResource.addResource("search");

    // Crear recurso proxy bajo /v1/medical-orientation/phone-consultation/{proxy+}
    const searchProxy = searchResource.addResource("{proxy+}");
    searchProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.EventServicesLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/issuances
    const issuancesResource = v1Resource.addResource("issuances");

    // Crear recurso bajo /v1/issuances
    issuancesResource.addMethod(
      "POST",
      new apigateway.LambdaIntegration(props.IssuanceMokServicesLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear recurso proxy bajo /v1/issuances/{filename}
    const issuancesProxy = issuancesResource.addResource("{filename}");
    issuancesProxy.addMethod(
      "GET",
      new apigateway.LambdaIntegration(props.IssuanceMokServicesLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    // Crear o reutilizar recurso /v1/providers
    const providersResource = v1Resource.addResource("providers");
    providersResource.addMethod(
      "POST",
      new apigateway.LambdaIntegration(props.ProviderServiceLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );
    // Crear recurso proxy bajo /v1/providers/{proxy+}
    const providersProxy = providersResource.addResource("{proxy+}");
    providersProxy.addMethod(
      "ANY",
      new apigateway.LambdaIntegration(props.ProviderServiceLambda, {
        proxy: true,
      }),
      {
        authorizer: authorizerLambda,
        authorizationType: apigateway.AuthorizationType.CUSTOM,
        apiKeyRequired: true,
      }
    );

    if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
    if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
    if (props.client) cdk.Tags.of(this).add("Client", props.client);
    if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
    if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
    if (props.project) cdk.Tags.of(this).add("Project", props.project);
  }
}
