import * as lambda from "aws-cdk-lib/aws-lambda";
import * as cdk from "aws-cdk-lib";
import * as crypto from "node:crypto";
import * as iam from "aws-cdk-lib/aws-iam";
import * as ec2 from "aws-cdk-lib/aws-ec2";
import * as sqs from "aws-cdk-lib/aws-sqs";
import { SqsEventSource } from "aws-cdk-lib/aws-lambda-event-sources";

import { Construct } from "constructs";
import path = require("path");

export interface LambdaStackProps extends cdk.StackProps {
  region: string;
  client: string;
  project: string;
  partner: string;
  stage: string;
  account_num: String;
  vpc: ec2.IVpc;
  lambdaSg: ec2.SecurityGroup;
  arnDatasourcemasterBucket: string;
  arnAssitsbucketBucket: string;
  arnIssuanceMokDocsBucket: string;
  AssistancesLoggerSqs: sqs.IQueue;
}
export class LambdaStack extends cdk.Stack {
  public readonly MasterRdsServicesLambda: lambda.IFunction;
  public readonly ProviderServiceLambda: lambda.IFunction;
  public readonly IssuanceMokServicesLambda: lambda.IFunction;
  public readonly EventServicesLambda: lambda.IFunction;
  public readonly PhoneConsultationServiceLambda: lambda.IFunction;
  public readonly VoucherServiceLambda: lambda.IFunction;
  public readonly EventStatusSwitchTempServicesLambda: lambda.IFunction;
  public readonly AssistancesLoggerWriterLambda: lambda.IFunction;
  public readonly arnMasterRdsServicesLambda: string;
  public readonly arnEventServicesLambda: string;
  public readonly arnProviderServiceLambda: string;
  public readonly arnIssuanceMokServicesLambda: string;
  public readonly arnPhoneConsultationServiceLambda: string;
  public readonly arnVoucherServiceLambda: string;
  public readonly arnTrackingMokServicesLambda: string;

  private readonly defaultMemorySize = 256;
  private readonly defaultTimeout = cdk.Duration.minutes(3);
  private readonly defaultRuntime = lambda.Runtime.DOTNET_8;

  constructor(scope: Construct, id: string, props: LambdaStackProps) {
    super(scope, id);

    // Permisos de RDS
    const lambdaPolicy = new iam.PolicyStatement({
      actions: [
        "kms:*",
        "sns:Publish",
        "dynamodb:BatchGetItem",
        "dynamodb:GetRecords",
        "dynamodb:GetShardIterator",
        "dynamodb:Query",
        "dynamodb:GetItem",
        "dynamodb:Scan",
        "dynamodb:ConditionCheckItem",
        "dynamodb:BatchWriteItem",
        "dynamodb:PutItem",
        "dynamodb:UpdateItem",
        "dynamodb:DeleteItem",
        "dynamodb:DescribeTable",
        "sqs:sendmessage",
      ],
      resources: ["*"],
    });

    const lambdaS3Policy = new iam.PolicyStatement({
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

    const AllowDynamoAccessProviderDataPolicy = new iam.PolicyStatement({
      actions: ["dynamodb:DescribeTable", "dynamodb:GetItem", "dynamodb:Query"],
      resources: [
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData`,
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/idCountry-index`,
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/geohashPK-index`,
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/idCity-index`,
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/nameProvider-index`,
      ],
    });

    // Rutas generales de servicios necesarios
    const f1 = path.join(__dirname, "../../../SharedServices/");
    const f2 = path.join(__dirname, "../../../StorageS3Services/");
    const f3 = path.join(__dirname, "../../../SQSProducerServices/");

    const MasterRdsServicesLambda = this.createLambda(
      props,
      "MasterRdsServices",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: f1 },
          { containerPath: "/StorageS3Services/", hostPath: f2 },
        ],
        vpcConfig: {
          vpc: props.vpc,
          securityGroups: [props.lambdaSg],
        },
      }
    );

    this.MasterRdsServicesLambda = MasterRdsServicesLambda;

    this.arnMasterRdsServicesLambda = MasterRdsServicesLambda.functionArn;

    const EventServicesLambda = this.createLambda(props, "EventServices", {
      volumes: [
        { containerPath: "/SharedServices/", hostPath: f1 },
        { containerPath: "/SQSProducerServices/", hostPath: f3 },
      ],
      vpcConfig: {
        vpc: props.vpc,
        securityGroups: [props.lambdaSg],
      },
    });

    EventServicesLambda.addToRolePolicy(lambdaPolicy);

    this.EventServicesLambda = EventServicesLambda;
    this.arnEventServicesLambda = EventServicesLambda.functionArn;

    const ProviderServiceLambda = this.createLambda(props, "ProviderService", {
      volumes: [{ containerPath: "/SharedServices/", hostPath: f1 }],
    });

    ProviderServiceLambda.addToRolePolicy(AllowDynamoAccessProviderDataPolicy);

    this.ProviderServiceLambda = ProviderServiceLambda;
    this.arnProviderServiceLambda = ProviderServiceLambda.functionArn;

    const IssuanceMokServicesLambda = this.createLambda(
      props,
      "IssuanceMokServices",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: f1 },
          { containerPath: "/StorageS3Services/", hostPath: f2 },
        ],
      }
    );

    IssuanceMokServicesLambda.addToRolePolicy(lambdaPolicy);
    IssuanceMokServicesLambda.addToRolePolicy(lambdaS3Policy);

    this.arnIssuanceMokServicesLambda = IssuanceMokServicesLambda.functionArn;
    this.IssuanceMokServicesLambda = IssuanceMokServicesLambda;

    const TrackingMokServicesLambda = this.createLambda(
      props,
      "TrackingMokServices",
      {
        volumes: [{ containerPath: "/SharedServices/", hostPath: f1 }],
      }
    );

    this.arnTrackingMokServicesLambda = TrackingMokServicesLambda.functionArn;

    const PhoneConsultationServiceLambda = this.createLambda(
      props,
      "PhoneConsultationService",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: f1 },
          { containerPath: "/StorageS3Services/", hostPath: f2 },
        ],
      }
    );
    PhoneConsultationServiceLambda.addToRolePolicy(lambdaPolicy);
    PhoneConsultationServiceLambda.addToRolePolicy(lambdaS3Policy);

    this.PhoneConsultationServiceLambda = PhoneConsultationServiceLambda;

    const VoucherServiceLambda = this.createLambda(props, "VoucherService");
    this.VoucherServiceLambda = VoucherServiceLambda;

    const EventStatusSwitchTempServicesLambda = this.createLambda(
      props,
      "EventStatusSwitchTempServices",
      {
        vpcConfig: {
          vpc: props.vpc,
          securityGroups: [props.lambdaSg],
        },
      }
    );

    EventStatusSwitchTempServicesLambda.addToRolePolicy(lambdaPolicy);

    this.EventStatusSwitchTempServicesLambda =
      EventStatusSwitchTempServicesLambda;

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
      ],
      resources: [
        `arn:aws:dynamodb:${props.region}:${props.account_num}:table/AssistancesLoggerDB`,
      ],
    });

    const lambdaAssistancesLoggerWriter = new lambda.Function(
      this,
      "AssistancesLoggerWriterLambda",
      {
        runtime: lambda.Runtime.PYTHON_3_13,
        functionName: `${props.project}-AssistancesLoggerWriter`,
        handler: "app.lambda_handler",
        code: lambda.Code.fromAsset(
          path.join(__dirname, `../../../../AssistancesLoggerWriterLambda/`)
        ),
        environment: {
          LOG_LEVEL: "INFO",
        },
      }
    );

    lambdaAssistancesLoggerWriter.addToRolePolicy(
      assistancesLoggerWriterPolicy
    );

    this.AssistancesLoggerWriterLambda = lambdaAssistancesLoggerWriter;
  }

  /**
   * Crea una función Lambda con configuración flexible.
   * @param props - Props con info del proyecto.
   * @param name - Nombre base para carpeta y función.
   * @param options - Opciones para configuración avanzada.
   */
  private createLambda(
    props: LambdaStackProps,
    name: string,
    options?: {
      handler?: string;
      volumes?: cdk.DockerVolume[];
      memorySize?: number;
      timeout?: cdk.Duration;
      environment?: { [key: string]: string };
      bundlingCommand?: string[]; // comando personalizado para bundling
      runtime?: lambda.Runtime;
      retryAttempts?: number;
      vpcConfig?: {
        // Nueva configuración de red
        vpc: ec2.IVpc;
        subnets?: ec2.SubnetSelection;
        securityGroups?: ec2.ISecurityGroup[];
        allowAllOutbound?: boolean;
      };
    }
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
    } = options ?? {};

    // Comando bundling por defecto (dotnet build + lambda package)
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

    // Generar hash determinista basado en el nombre
    const generateNameHash = (lambdaName: string): string => {
      return crypto
        .createHash("sha256")
        .update(lambdaName)
        .digest("hex")
        .substring(0, 64);
    };

    const nameHash = generateNameHash(name);

    return new lambda.Function(this, `Function-${name}`, {
      functionName: `${props.project}-${name}`,
      runtime,
      memorySize,
      timeout,
      handler,
      environment,
      vpc: vpcConfig?.vpc,
      vpcSubnets: vpcConfig?.subnets,
      securityGroups: vpcConfig?.securityGroups,
      allowAllOutbound: vpcConfig?.allowAllOutbound,
      code: lambda.Code.fromAsset(
        path.join(__dirname, `../../../../${name}/`),
        {
          assetHashType: cdk.AssetHashType.SOURCE,
          // assetHash: nameHash,
          bundling: {
            image: runtime.bundlingImage,
            user: "root",
            outputType: cdk.BundlingOutput.ARCHIVED,
            command: bundlingCommand ?? defaultCommand,
            volumes,
          },
        }
      ),
      ...(retryAttempts !== undefined ? { retryAttempts } : {}),
      // Habilitar logs automáticamente (ya viene por defecto en CDK, pero por si quieres agregar otras opciones)
      // logRetention: cdk.aws_logs.RetentionDays.ONE_WEEK,
    });
  }
}
