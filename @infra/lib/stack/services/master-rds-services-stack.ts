// lib/stacks/master-rds-services-stack.ts
import * as sqs from "aws-cdk-lib/aws-sqs";
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export interface MasterRdsServicesStackProps extends CommonLambdaStackProps {
  // Specific props for this stack, if any, beyond CommonLambdaStackProps
  // For example, if this stack needs specific bucket ARNs that are not common to all.
}

export class MasterRdsServicesStack extends cdk.Stack {
  public readonly masterRdsServiceLambda: lambda.IFunction;
  public readonly arnMasterRdsServicesLambda: string;

  constructor(
    scope: Construct,
    id: string,
    props: MasterRdsServicesStackProps
  ) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    // General service paths
    const sharedServicesPath = path.join(__dirname, "../../../../SharedServices/");
    const storageS3ServicesPath = path.join(__dirname,"../../../../StorageS3Services/");

    const masterRdsServicesLambda = lambdaCreator.createLambda(
      this,
      "MasterRdsServicesFunction",
      props,
      "MasterRdsServices",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: sharedServicesPath },
          {
            containerPath: "/StorageS3Services/",
            hostPath: storageS3ServicesPath,
          },
        ],
        vpcConfig: {
          vpc: props.vpc,
          securityGroups: [props.lambdaSg],
        },
        policyStatements: [
          // Add specific RDS permissions here if they are not common to all lambdas,
          // or if they require specific resource ARNs not covered by a general policy.
          // The original lambdaPolicy was very broad ("*"). Consider refining.
          // For now, mirroring the original broad policy for demonstration.
          new cdk.aws_iam.PolicyStatement({
            actions: [
              "kms:*",
              "sns:Publish",
              "dynamodb:*", // Original was very broad, consider narrowing
              "s3:GetObject*",
              "s3:GetBucket*",
              "s3:List*",
              "sqs:sendmessage",
            ],
            resources: ["*"],
          }),
        ],
      }
    );

    this.masterRdsServiceLambda = masterRdsServicesLambda;
    this.arnMasterRdsServicesLambda = masterRdsServicesLambda.functionArn;
  }
}