#!/usr/bin/env node
import * as cdk from "aws-cdk-lib";
import { ConfigEnv } from "../config/config-env";
import { MainStack } from "../lib/stack/endpoints/mainStack";

import { pipelineStack } from "../lib/stack/pipelines/pipelineStack";
import { CognitoStack } from "../lib/stack/cognito/cognitoStack";
import { DynamoStack } from "../lib/stack/dynamo/dynamoStack";
import { NetworkingStack } from "../lib/stack/networking/networkingStack";
import { DatabaseStack } from "../lib/stack/database/databaseStack";
import { WafStack } from "../lib/stack/waf/wafStack";
import { SqsStack } from "../lib/stack/sqs/sqsStack";
import { S3Stack } from "../lib/stack/s3/s3Stack";
import { LambdaStack } from "../lib/stack/services/lambdaStack";
import { LambdaLayersStack } from "../lib/stack/layers/lambdaLayer";

// Import all individual lambda stacks
import { MasterRdsServicesStack } from "../lib/stack/services/master-rds-services-stack";
import { EventServicesStack } from "../lib/stack/services/event-services-stack";
import { ProviderServiceStack } from "../lib/stack/services/provider-service-stack";
import { IssuanceMokServicesStack } from "../lib/stack/services/issuance-mok-services-stack";
import { TrackingMokServicesStack } from "../lib/stack/services/tracking-mok-services-stack";
import { PhoneConsultationServiceStack } from "../lib/stack/services/phone-consultation-service-stack";
import { VoucherServiceStack } from "../lib/stack/services/voucher-service-stack";
import { EventStatusSwitchTempServicesStack } from "../lib/stack/services/event-status-switch-temp-services-stack";
import { PythonLambdasStack } from "../lib/stack/services/python-lambdas-stack";
// import { CommonLambdaStackProps } from "./constructs/lambda-creator";


import { StatemachineStack } from "../lib/stack/statemachines/statemachineStack";

const vars = new ConfigEnv().config;
const app = new cdk.App();

const baseProps = {
  account_num: vars.account,
  region: vars.region,
  client: vars.client,
  stage: vars.environment,
  project: vars.project,
  partner: vars.partner,
  stageName: vars.environment,
};

const completeSecretARN: { [key: string]: string } = {
  dev: "arn:aws:secretsmanager:us-east-1:590183946089:secret:asistencia-viajero/rds/mysql-Xruc1e",
  qa: "arn:aws:secretsmanager:us-east-2:625039860988:secret:asistencia-viajero/rds/mysql-RPf7fM",
  prd: "arn:aws:secretsmanager:us-east-1:834379228655:secret:asistencia-viajero/rds/mysql-pUlFLJ",
};

const vpcCidr: { [key: string]: string } = {
  dev: "10.9.0.0/16",
  qa: "10.9.0.0/16",
  prd: "10.9.0.0/16",
};

const branchName: { [key: string]: string } = {
  dev: "develop",
  qa: "qa",
  prd: "main",
};

const isPubliclyAccessible: { [key: string]: boolean } = {
  dev: false,
  qa: false,
  prd: false,
};

const connectionArn: { [key: string]: string } = {
  dev: "arn:aws:codeconnections:us-east-2:590183946089:connection/4b83bd26-b94b-4bae-82b9-ce2753bfc43f",
  qa: "arn:aws:codeconnections:us-east-1:625039860988:connection/cc7e8a24-1082-4902-a0bc-8d3d39b33bfc",
  prd: "arn:aws:codeconnections:us-east-2:834379228655:connection/6911c564-7dcf-4ffd-94cf-fa3a2ae54ed2",
};

const layerAuthorizerVersion: { [key: string]: string } = {
  dev: "1",
  qa: "4",
  prd: "1",
};

const numberNatGateways: { [key: string]: number } = {
  dev: 0,
  qa: 0,
  prd: 1,
};

const layerTrackingVersion: { [key: string]: string } = {
  dev: "3",
  qa: "4",
  prd: "2",
};

// ✅ SOLUCIÓN: Quitar env de TODOS los stacks para permitir cross-references
const { vpc, dbSg, lambdaSg } = new NetworkingStack(
  app,
  `${vars.project}-NetworkingStack`,
  {
    ...baseProps,
    cidr: vpcCidr[baseProps.stage],
    numberNatGateways: numberNatGateways[baseProps.stage],
  }
);

const dbStack = new DatabaseStack(app, `${vars.project}-DatabaseStack`, {
  ...baseProps,
  vpc: vpc,
  securityGroup: dbSg,
  isPubliclyAccessible: isPubliclyAccessible[baseProps.stage],
  completeSecretARN: completeSecretARN[baseProps.stage],
});

const cognitoStack = new CognitoStack(app, `${vars.project}-CognitoStack`, {
  ...baseProps,
});

const wafStack = new WafStack(app, `${vars.project}-WafStack`, {
  ...baseProps,
});

const s3Stack = new S3Stack(app, `${vars.project}-S3Stack`, {
  ...baseProps,
});

const dynamoStack = new DynamoStack(app, `${vars.project}-DynamoStack`, {
  ...baseProps,
});

const lambdaLayersStack = new LambdaLayersStack(
  app,
  `${vars.project}-LambdaLayersStack`,
  {
    ...baseProps,
  }
);

const sqsStack = new SqsStack(app, `${vars.project}-SqsStack`, {
  ...baseProps,
});

// ✅ SOLUCIÓN: Quitar 'env' de TODOS los stacks con cross-references
const masterRdsServicesStack = new MasterRdsServicesStack(app, `${vars.project}-lambda-MasterRdsServicesStack`, {
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const eventServicesStack = new EventServicesStack(app, `${vars.project}-lambda-EventServicesStack`, {
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const providerServiceStack = new ProviderServiceStack(app, `${vars.project}-lambda-ProviderServiceStack`,  {
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const issuanceMokServicesStack = new IssuanceMokServicesStack( app, `${vars.project}-lambda-IssuanceMokServicesStack`,{
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
  arnAssitsbucketBucket: s3Stack.arnAssitsbucketBucket,
  arnIssuanceMokDocsBucket: s3Stack.arnIssuanceMokDocsBucket,
});

const trackingMokServicesStack = new TrackingMokServicesStack(app,`${vars.project}-lambda-TrackingMokServicesStack`,{
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const phoneConsultationServiceStack = new PhoneConsultationServiceStack( app, `${vars.project}-lambda-PhoneConsultationServiceStack`,{
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
  arnAssitsbucketBucket: s3Stack.arnAssitsbucketBucket,
  arnIssuanceMokDocsBucket: s3Stack.arnIssuanceMokDocsBucket,
});

const voucherServiceStack = new VoucherServiceStack(app,`${vars.project}-lambda-VoucherServiceStack`,{
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const eventStatusSwitchTempServicesStack =  new EventStatusSwitchTempServicesStack(app,`${vars.project}-lambda-EventStatusSwitchTempServicesStack`,{
  ...baseProps,
  vpc: vpc,
  lambdaSg: lambdaSg,
});

const pythonLambdasStack = new PythonLambdasStack(app,`${vars.project}-lambda-PythonLambdasStack`, {
  ...baseProps,
  // ✅ Usar vpc y lambdaSg directamente (más simple)
  vpc: vpc,
  lambdaSg: lambdaSg,
  userPoolId: cognitoStack.userPoolId,
  arnLayerTrackingMok: layerTrackingVersion[baseProps.stage],
  clientKomId: cognitoStack.clientKomId,
});

new MainStack(app, `${vars.project}-MainStack`, {
  userPool: cognitoStack.userPool,
  ...baseProps,
  userPoolId: cognitoStack.userPoolId,
  clientFrontendId: cognitoStack.clientFrontendId,
  MasterRdsServicesLambda: masterRdsServicesStack.masterRdsServiceLambda,
  ProviderServiceLambda: providerServiceStack.providerServiceLambda,
  IssuanceMokServicesLambda: issuanceMokServicesStack.issuanceMokServicesLambda,
  EventServicesLambda: eventServicesStack.eventServicesLambda,
  PhoneConsultationServiceLambda: phoneConsultationServiceStack.phoneConsultationServiceLambda,
  VoucherServiceLambda: voucherServiceStack.voucherServiceLambda,
  EventStatusSwitchTempServicesLambda: eventStatusSwitchTempServicesStack.eventStatusSwitchTempServicesLambda,
  AssistancesLoggerWriterLambda: pythonLambdasStack.assistancesLoggerWriterLambda,
  AssistancesLoggerWriterExternalLambda: pythonLambdasStack.assistancesLoggerWriterExternalLambda,
  ListUsersLambda: pythonLambdasStack.ListUsersLambda,
  arnLayerVersionJWT: layerAuthorizerVersion[baseProps.stage],
});

const statemachineStack = new StatemachineStack(
  app,
  `${vars.project}-StatemachineStack`,
  {
    ...baseProps,
    trackingMokServicesLambda: pythonLambdasStack.trackingMokServicesLambda,
    SnsNotificationLambda: pythonLambdasStack.SnsNotificationLambda
  }
);

// ✅ Los pipelines pueden mantener env ya que no tienen cross-references
new pipelineStack(app, `${vars.project}-pipelineStack`, {
  ...baseProps,
  repoName: "bigtree-asistencias-backend",
  connectionArn: connectionArn[baseProps.stage],
  repoOwner: 'TERRAWIND-DEVELOP',
  repoBranch: branchName[baseProps.stage],
  service: 'backend'
});

new pipelineStack(app, `${vars.project}-databasePipelineStack`, {
  ...baseProps,
  repoName: "bigtree-asistencias-database",
  connectionArn: connectionArn[baseProps.stage],
  repoOwner: 'TERRAWIND-DEVELOP',
  repoBranch: branchName[baseProps.stage],
  service: 'database',
  vpc: vpc,
  securityGroup: dbSg,
});