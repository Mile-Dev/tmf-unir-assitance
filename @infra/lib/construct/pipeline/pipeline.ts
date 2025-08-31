import * as cdk from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as codepipeline from 'aws-cdk-lib/aws-codepipeline';
import * as codepipeline_actions from 'aws-cdk-lib/aws-codepipeline-actions';
import * as codebuild from 'aws-cdk-lib/aws-codebuild';
import * as iam from 'aws-cdk-lib/aws-iam';
import * as s3 from 'aws-cdk-lib/aws-s3';
import * as codecommit from 'aws-cdk-lib/aws-codecommit';
import * as codestar from 'aws-cdk-lib/aws-codestarconnections';
import { Effect, IRole, ManagedPolicy, PolicyStatement, Role, ServicePrincipal } from "aws-cdk-lib/aws-iam";
import { toLow } from "../../global/helper";
import { aws_codecommit, aws_codebuild, aws_codepipeline, aws_codepipeline_actions, StackProps } from "aws-cdk-lib";
import { Cache, ComputeType, LinuxBuildImage, LocalCacheMode, PipelineProject } from "aws-cdk-lib/aws-codebuild";
import { Vpc } from 'aws-cdk-lib/aws-ec2';
import * as ec2 from 'aws-cdk-lib/aws-ec2';


interface BasePipelineDevOpsProps extends StackProps {
    /**
     * @description - Nombre del proyecto
     * @type {string}
     */
    readonly project: string;

    /**
     * @description - Entorno
     * @example DEV - QA - PROD
     * @type {string}
     */
    readonly stageName: string;

    /**
     * @description - Nombre del repositorio
     * @type {string}
     */
    readonly repoName: string;

    /**
     * @description - Rama del proyecto a desplegar, por defecto va a desplegar main
     * @default main
     * @type {string}
     */
    readonly branchName?: string;

    /**
     * @description - Nombre del archivo buildspec para la ejecución 
     * @default buildspec.yml
     * @type {string}
     */
    readonly buildspecName?: string;

    /**
     * @description - Role para codeBuild, si no se le asigna ninguno creará uno por defecto
     * @default buildspec.yml
     * @type {IRole}
     */
    readonly codeBuildRole?: IRole;

    connectionArn: string;
    repoOwner: string;
    repoBranch: string;
    region: string;
    partner: string;
    account_num: string;
    client: string;
    service: string;
    vpc?: ec2.IVpc;
    securityGroup?: ec2.SecurityGroup;
}

export class CodePipelineConstruct extends Construct {
    constructor(scope: Construct, id: string, props: BasePipelineDevOpsProps) {
        super(scope, id);

        //To lower props
        const project = toLow(props.project)
        const repo = toLow(props.repoName)

        // stage 1 - checkout code
        const sourceOutput = new aws_codepipeline.Artifact();

        // Definir el repositorio de GitHub usando la conexión
        const githubSourceOutput = new codepipeline.Artifact();

        const sourceAction = new codepipeline_actions.CodeStarConnectionsSourceAction({
            actionName: 'GitHub_Source',
            connectionArn: props.connectionArn, // Conexión a GitHub
            output: githubSourceOutput,
            owner: props.repoOwner,  // Tu nombre de usuario o la organización
            repo: props.repoName,  // Nombre del repositorio
            branch: props.repoBranch, // Rama a seguir
            codeBuildCloneOutput: true,
        });

        const pipelineRole = new Role(this, toLow(`${props.project}-${props.service}PipelineRole-${props.stageName}`),
            {
                roleName: toLow(`${props.project}-${props.service}PipelineRole-${props.stageName}`),
                assumedBy: new ServicePrincipal("codepipeline.amazonaws.com"),
                managedPolicies: [
                    ManagedPolicy.fromAwsManagedPolicyName("AWSCloudFormationFullAccess"),
                ],
            }
        );

        pipelineRole.addToPolicy(
            new PolicyStatement({
                effect: Effect.ALLOW,
                actions: ["sts:AssumeRole"],
                resources: ["*"],
            })
        );

        const pipelineName = toLow(`${props.project}-${props.service}Pipeline-${props.stageName}`);

        const pipeline = new aws_codepipeline.Pipeline(this, `${props.project}-${props.service}Pipeline-${props.stageName}`, {
            pipelineName,
            role: pipelineRole,
        });

        // stage 2 - test & cdk deploy

        const codeBuildRole = new Role(this, `${props.service}CodeBuildRole`, {
            assumedBy: new ServicePrincipal("codebuild.amazonaws.com"),
            roleName: toLow(`${props.project}-${props.service}CodeBuildRole-${props.stageName}`),
            managedPolicies: [
                ManagedPolicy.fromAwsManagedPolicyName("AmazonS3FullAccess"),
                ManagedPolicy.fromAwsManagedPolicyName(
                    "service-role/AWSCodeStarServiceRole"
                ),
                ManagedPolicy.fromAwsManagedPolicyName("AmazonSSMFullAccess"),
                ManagedPolicy.fromAwsManagedPolicyName("AWSCloudFormationFullAccess"),
                ManagedPolicy.fromAwsManagedPolicyName("SecretsManagerReadWrite"),
            ],
        });

        codeBuildRole.addToPolicy(
            new PolicyStatement({
                effect: Effect.ALLOW,
                actions: ["sts:AssumeRole"],
                resources: ["*"],
            })
        );

        const sourceFileName = props.buildspecName ? props.buildspecName : "buildspec.yml";

        const buildStage = new aws_codebuild.PipelineProject(this, `${props.service}CodebuildUnitTest`, {
            projectName: toLow(`${props.project}-${props.service}CodeBuild-${props.stageName}`),
            buildSpec: aws_codebuild.BuildSpec.fromSourceFilename(sourceFileName),
            environmentVariables: {
                AWS_ACCOUNT: { value: props.account_num },
                AWS_REGION: { value: props.region },
                PROJECT_NAME: { value: props.project },
                CLIENT_NAME: { value: props.client },
                STACK_ENVIRONMENT: { value: props.stageName },
                PARTNER_NAME: { value: props.partner }
            },
            role: codeBuildRole,
            environment: {
                buildImage: LinuxBuildImage.STANDARD_6_0,
                computeType: ComputeType.MEDIUM,
            },
            vpc:props.vpc,
            securityGroups: props.securityGroup ? [props.securityGroup] : undefined,
            cache: Cache.local(LocalCacheMode.CUSTOM),
        });


        const testBuildOutput = new aws_codepipeline.Artifact();

        const testBuildAction = new aws_codepipeline_actions.CodeBuildAction({
            actionName: 'BuildAndDeploy',
            input: githubSourceOutput,
            outputs: [testBuildOutput],
            project: buildStage
        })

        //Add the stages
        pipeline.addStage({
            stageName: "Source",
            actions: [sourceAction],
        });

        pipeline.addStage({
            stageName: "BuildAndDeploy",
            actions: [testBuildAction],
        });



    }
}
