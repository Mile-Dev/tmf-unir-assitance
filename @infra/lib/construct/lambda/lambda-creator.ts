// lib/constructs/lambda-creator.ts
import * as lambda from "aws-cdk-lib/aws-lambda";
import * as cdk from "aws-cdk-lib";
import * as crypto from "node:crypto";
import * as ec2 from "aws-cdk-lib/aws-ec2";
import { Construct } from "constructs";
import * as path from "path";

/**
 * Common properties required for all Lambda stacks.
 */
export interface CommonLambdaStackProps extends cdk.StackProps {
  region: string;
  client: string;
  project: string;
  partner: string;
  stage: string;
  account_num: string;
  vpc: ec2.IVpc;
  lambdaSg: ec2.SecurityGroup;
  // Add other common props needed for all lambdas, e.g., S3 bucket ARNs if applicable to many.
  // For now, these are kept in individual stacks where they are used.
}

/**
 * Options for configuring the Lambda function creation.
 */
export interface CreateLambdaOptions {
  handler?: string;
  volumes?: cdk.DockerVolume[];
  memorySize?: number;
  timeout?: cdk.Duration;
  environment?: { [key: string]: string };
  bundlingCommand?: string[];
  runtime?: lambda.Runtime;
  retryAttempts?: number;
  vpcConfig?: {
    vpc: ec2.IVpc;
    subnets?: ec2.SubnetSelection;
    securityGroups?: ec2.ISecurityGroup[];
    allowAllOutbound?: boolean;
  };
  policyStatements?: cdk.aws_iam.PolicyStatement[];
}

/**
 * A utility class to create AWS Lambda functions with standardized configurations.
 */
export class LambdaCreator {
  private readonly defaultMemorySize = 256;
  private readonly defaultTimeout = cdk.Duration.minutes(3);
  private readonly defaultRuntime = lambda.Runtime.DOTNET_8;

  /**
   * Creates a Lambda function with flexible configuration.
   * @param scope The scope in which to define this construct.
   * @param id The logical ID of the Lambda function.
   * @param props Common Lambda stack properties.
   * @param name The base name for the Lambda function and its asset folder.
   * @param options Optional configuration for the Lambda function.
   * @returns The created Lambda function.
   */
  public createLambda(
    scope: Construct,
    id: string,
    props: CommonLambdaStackProps,
    name: string,
    options?: CreateLambdaOptions
  ): lambda.Function {
    const {
      handler = name,
      volumes = [],
      memorySize = this.defaultMemorySize,
      timeout = this.defaultTimeout,
      environment = {},
      bundlingCommand,
      runtime = this.defaultRuntime,
      retryAttempts,
      vpcConfig,
      policyStatements = [], // New: to add policies directly
    } = options ?? {};

    // Default bundling command (dotnet build + lambda package)
    const defaultCommand = [
      "bash",
      "-c",
      [
        `cp appsettings.${props.stage}.json appsettings.json`,
        "dotnet tool install -g Amazon.Lambda.Tools",
        "dotnet build",
        "dotnet lambda package --output-package /asset-output/function.zip",
      ].join(" && "),
    ];

    // Generate determinist hash based on the name (original logic, though assetHashType.SOURCE is usually preferred for determinism)
    const generateNameHash = (lambdaName: string): string => {
      return crypto
        .createHash("sha256")
        .update(lambdaName)
        .digest("hex")
        .substring(0, 64);
    };

    const nameHash = generateNameHash(name);

    const func = new lambda.Function(scope, id, {
      functionName: `${props.client}-${props.project}-${name}`,
      runtime,
      memorySize,
      timeout,
      handler,
      environment,
      vpc: vpcConfig?.vpc,
      vpcSubnets: vpcConfig?.subnets,
      securityGroups: vpcConfig?.securityGroups,
      allowAllOutbound: vpcConfig?.allowAllOutbound,
      code: lambda.Code.fromAsset(path.join(__dirname, `../../../../${name}/`), {
        assetHashType: cdk.AssetHashType.SOURCE,
        bundling: {
          image: runtime.bundlingImage,
          user: "root",
          outputType: cdk.BundlingOutput.ARCHIVED,
          command: bundlingCommand ?? defaultCommand,
          volumes,
        },
      }),
      ...(retryAttempts !== undefined ? { retryAttempts } : {}),
    });

    // Apply policies
    policyStatements.forEach((policy) => {
      func.addToRolePolicy(policy);
    });

    return func;
  }
}