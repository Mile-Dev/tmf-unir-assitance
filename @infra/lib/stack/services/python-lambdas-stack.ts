// lib/stacks/assistances-logger-writer-stack.ts
import * as sqs from "aws-cdk-lib/aws-sqs";
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import * as iam from "aws-cdk-lib/aws-iam";
import * as cognito from "aws-cdk-lib/aws-cognito";
import * as ec2 from "aws-cdk-lib/aws-ec2";
import * as sns from "aws-cdk-lib/aws-sns";

import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export interface PythonLambdasStackProps extends CommonLambdaStackProps {
  userPoolId: string;
  arnLayerTrackingMok: string;
  clientKomId: string;
}

export class PythonLambdasStack extends cdk.Stack {
  public readonly assistancesLoggerWriterLambda: lambda.IFunction;
  public readonly ListUsersLambda: lambda.IFunction;
  public readonly SnsNotificationLambda: lambda.IFunction;
  public readonly trackingMokServicesLambda: lambda.IFunction;
  public readonly assistancesLoggerWriterExternalLambda: lambda.IFunction;
  constructor(scope: Construct, id: string, props: PythonLambdasStackProps) {
    super(scope, id);

    const assistancesLoggerWriterPolicy = new iam.PolicyStatement({
      actions: [
        "dynamodb:Query",
        "dynamodb:GetItem",
        "dynamodb:PutItem",
        "dynamodb:UpdateItem",
        "sqs:sendmessage",
        "sqs:ChangeMessageVisibility",
        "sqs:DeleteMessage",
        "sqs:ReceiveMessage",
        "sqs:GetQueueAttributes",
        "sqs:GetQueueUrl",
        "sqs:SendMessage",
        "states:StartExecution",
      ],
      resources: [
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/AssistancesLoggerDB`,
        `arn:aws:sqs:${props.region}:${props.account_num}:AssistancesLoggerSQS`, // Add SQS queue ARN to resources
        `arn:aws:states:${props.region}:${props.account_num}:stateMachine:trackingMokExpressStateMachine`,
      ],
    });

    const lambdaAssistancesLoggerWriter = new lambda.Function(
      this,
      "AssistancesLoggerWriterLambda",
      {
        runtime: lambda.Runtime.PYTHON_3_13,
        functionName: `${props.client}-${props.project}-AssistancesLoggerWriter`,
        handler: "app.lambda_handler",
        code: lambda.Code.fromAsset(
          path.join(__dirname, `../../../../AssistancesLoggerWriterLambda/`)
        ),
        environment: {
          LOG_LEVEL: "INFO",
          SF_TRACKING_MOK_ARN: `arn:aws:states:${props.region}:${props.account_num}:stateMachine:trackingMokExpressStateMachine`,
        },
      }
    );
    lambdaAssistancesLoggerWriter.addToRolePolicy(
      assistancesLoggerWriterPolicy
    );
    this.assistancesLoggerWriterLambda = lambdaAssistancesLoggerWriter;

    const lambdaAssistancesLoggerWriterExternal = new lambda.Function(
      this,
      "AssistancesLoggerWriterExternalLambda",
      {
        runtime: lambda.Runtime.PYTHON_3_13,
        functionName: `${props.client}-${props.project}-AssistancesLoggerWriterExternal`,
        handler: "app.lambda_handler",
        code: lambda.Code.fromAsset(
          path.join(
            __dirname,
            `../../../../AssistancesLoggerWritterExternalLambda/`
          )
        ),
        environment: {
          LOG_LEVEL: "INFO",
          CLIENTS_MAP: JSON.stringify({
            [props.clientKomId]: "MOK",
          }),
        },
      }
    );

    lambdaAssistancesLoggerWriterExternal.addToRolePolicy(
      assistancesLoggerWriterPolicy
    );

    this.assistancesLoggerWriterExternalLambda =
      lambdaAssistancesLoggerWriterExternal;

    const listUsersPolicy = new iam.PolicyStatement({
      actions: ["cognito-idp:ListUsers", "cognito-idp:ListUsersInGroup"],
      resources: ["*"],
    });

    const listUsersLambda = new lambda.Function(this, "ListUsersLambda", {
      runtime: lambda.Runtime.PYTHON_3_13,
      functionName: `${props.project}-ListUsers`,
      handler: "app.lambda_handler",
      code: lambda.Code.fromAsset(
        path.join(__dirname, `../../../../ListUsersLambda/`)
      ),
      environment: {
        LOG_LEVEL: "INFO",
        USER_POOL_ID: props.userPoolId,
      },
    });

    listUsersLambda.addToRolePolicy(listUsersPolicy);

    this.ListUsersLambda = listUsersLambda;

    const trackingMokServicesPolicy = new iam.PolicyStatement({
      actions: [
        "cognito-idp:AdminGetUser",
        "dynamodb:Query",
        "dynamodb:GetItem",
        "dynamodb:PutItem",
        "dynamodb:UpdateItem",
        "dynamodb:DescribeTable",
        "secretsmanager:GetSecretValue",
      ],
      resources: ["*"],
    });

    const layerTrackingMok = lambda.LayerVersion.fromLayerVersionArn(
      this,
      "layerTrackingMok",
      `arn:aws:lambda:${props.region}:${props.account_num}:layer:asistencia-viajero-trackingMok:${props.arnLayerTrackingMok}`
    );

    const layerAwsPowertools = lambda.LayerVersion.fromLayerVersionArn(
      this,
      "layerAwsPowertools",
      `arn:aws:lambda:us-east-1:017000801446:layer:AWSLambdaPowertoolsPythonV3-python313-x86_64:14`
    );

    const trackingMokServicesLambda = new lambda.Function(
      this,
      "TrackingMokServicesLambda",
      {
        runtime: lambda.Runtime.PYTHON_3_13,
        functionName: `${props.client}-${props.project}-TrackingMok`,
        handler: "lambda_function.lambda_handler",
        timeout: cdk.Duration.minutes(15),
        memorySize: 512,
        code: lambda.Code.fromAsset(
          path.join(__dirname, `../../../../TrackingMokLambda/`)
        ),
        layers: [layerTrackingMok], // Adjuntamos el layer a la función
        environment: {
          LOG_LEVEL: "INFO",
          MOK_SECRET_NAME: "asistencia-viajero/mok/enpoint",
          DB_SECRET_NAME: "asistencia-viajero/rds/mysql",
        },
        vpc: props.vpc,
        vpcSubnets: {
          subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS,
        },
        securityGroups: [props.lambdaSg],
      }
    );

    trackingMokServicesLambda.addToRolePolicy(trackingMokServicesPolicy);

    this.trackingMokServicesLambda = trackingMokServicesLambda;

    const snsNotificationPolicy = new iam.PolicyStatement({
      actions: [
        "sns:*",
        "dynamodb:Query",
        "dynamodb:GetItem",
        "dynamodb:PutItem",
        "dynamodb:UpdateItem",
      ],
      resources: ["*"],
    });

    // SNS Topic
    const topicTrackingMok = new sns.Topic(this, "Topic", {
      displayName: `${props.project} TrackingMok Notifications `,
      topicName: `${props.project}-TrackingMokNotification`,
    });

    const snsNotificationLambda = new lambda.Function(
      this,
      "SnsNotificationLambda",
      {
        runtime: lambda.Runtime.PYTHON_3_13,
        functionName: `${props.client}-${props.project}-SnsNotification`,
        handler: "lambda_function.lambda_handler",
        code: lambda.Code.fromAsset("src/lambdas/sns_notification/"),
        environment: {
          LOG_LEVEL: "INFO",
          SNS_TOPIC_ARN: topicTrackingMok.topicArn,
        },
      }
    );

    snsNotificationLambda.addToRolePolicy(snsNotificationPolicy);

    this.SnsNotificationLambda = snsNotificationLambda;
  }
}
