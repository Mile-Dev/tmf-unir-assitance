// lib/stacks/phone-consultation-service-stack.ts
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export interface PhoneConsultationServiceStackProps
  extends CommonLambdaStackProps {
  arnIssuanceMokDocsBucket: string;
  arnAssitsbucketBucket: string;
}

export class PhoneConsultationServiceStack extends cdk.Stack {
  public readonly phoneConsultationServiceLambda: lambda.IFunction;

  constructor(scope: Construct, id: string, props: PhoneConsultationServiceStackProps) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    const sharedServicesPath = path.join(__dirname, "../../../../SharedServices/");
    const storageS3ServicesPath = path.join(
      __dirname,
      "../../../../StorageS3Services/"
    );

    const lambdaS3Policy = new cdk.aws_iam.PolicyStatement({
      actions: [
        "s3:GetObject*",
        "s3:GetBucket*",
        "s3:List*",
        "s3:DeleteObject*",
        "s3:PutObject",
        "s3:PutObjectLegalHold",
        "s3:PutObjectRetention",
        "s3:PutObjectTagging",
        "s3:PutObjectVersionTagging",
        "s3:Abort*",
      ],
      resources: [
        `arn:aws:s3:::tw-global-asistencia-viajero-issuance-mok-docs-${props.stage}-${props.account_num}`,
        `arn:aws:s3:::tw-global-asistencia-viajero-issuance-mok-docs-${props.stage}-${props.account_num}/*`,
        `arn:aws:s3:::tw-global-asistencia-viajero-assitsbucket-${props.stage}-${props.account_num}`,
        `arn:aws:s3:::tw-global-asistencia-viajero-assitsbucket-${props.stage}-${props.account_num}/*`,
      ],
    });

    const phoneConsultationServiceLambda = lambdaCreator.createLambda(
      this,
      "PhoneConsultationServiceFunction",
      props,
      "PhoneConsultationService",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: sharedServicesPath },
          {
            containerPath: "/StorageS3Services/",
            hostPath: storageS3ServicesPath,
          },
        ],
        policyStatements: [
          new cdk.aws_iam.PolicyStatement({
            actions: [
              "kms:*",
              "sns:Publish",
              "dynamodb:*", // Original was very broad, consider narrowing
              "sqs:sendmessage",
            ],
            resources: ["*"],
          }),
          lambdaS3Policy,
        ],
      }
    );
    this.phoneConsultationServiceLambda = phoneConsultationServiceLambda;
  }
}