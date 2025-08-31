import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";
import * as s3 from "aws-cdk-lib/aws-s3";
import { RemovalPolicy } from "aws-cdk-lib";
import * as s3deploy from 'aws-cdk-lib/aws-s3-deployment';
import * as path from 'path';



export interface s3StackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;
}

export class S3Stack extends Stack {

    public readonly arnFileBucket: string;
    public readonly arnDatasourcemasterBucket: string;
    public readonly arnAssitsbucketBucket: string;
    public readonly arnIssuanceMokDocsBucket: string;

    constructor(scope: Construct, id: string, props: s3StackProps) {
        super(scope, id, props);

        const fileBucket = new s3.Bucket(this, `file-${props.stage}-bucket`, {
            bucketName: `${props.client}-${props.project}-file-${props.stage}-${props.account_num}`,
            removalPolicy: RemovalPolicy.DESTROY,
            autoDeleteObjects: true,
            blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL,
            encryption: s3.BucketEncryption.S3_MANAGED,
            enforceSSL: true,
        });

        this.arnFileBucket = fileBucket.bucketArn

        const datasourcemasterBucket = new s3.Bucket(this, `datasourcemaster-${props.stage}-bucket`, {
            bucketName: `${props.client}-${props.project}-datasourcemaster-${props.stage}-${props.account_num}`,
            removalPolicy: RemovalPolicy.DESTROY,
            autoDeleteObjects: true,
            blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL,
            encryption: s3.BucketEncryption.S3_MANAGED,
            enforceSSL: true,
        });

        this.arnDatasourcemasterBucket = datasourcemasterBucket.bucketArn

        new s3deploy.BucketDeployment(this, 'DeployICD10CsvToDatasourcemaster', {
            sources: [s3deploy.Source.asset( 'files')],
            destinationBucket: datasourcemasterBucket,
            destinationKeyPrefix: 'phoneconsultation/',
            exclude: ['*'],
            include: ['Origen_Datos_ICD10.csv'],
            prune: false
        });

        new s3deploy.BucketDeployment(this, 'DeployTypeProvider', {
            sources: [s3deploy.Source.asset( 'files')],
            destinationBucket: datasourcemasterBucket,
            destinationKeyPrefix: 'provider/',
            exclude: ['*'],
            include: ['Origen_Datos_Tipo_Proveedor.csv'],
            prune: false
        });

        const assitsbucketBucket = new s3.Bucket(this, `assitsbucket-${props.stage}-bucket`, {
            bucketName: `${props.client}-${props.project}-assitsbucket-${props.stage}-${props.account_num}`,
            removalPolicy: RemovalPolicy.DESTROY,
            autoDeleteObjects: true,
            blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL,
            encryption: s3.BucketEncryption.S3_MANAGED,
            enforceSSL: true,
        });

        this.arnAssitsbucketBucket = assitsbucketBucket.bucketArn

        const issuanceMokDocsBucket = new s3.Bucket(this, `issuance-mok-docs-${props.stage}-bucket`, {
            bucketName: `${props.client}-${props.project}-issuance-mok-docs-${props.stage}-${props.account_num}`,
            removalPolicy: RemovalPolicy.DESTROY,
            autoDeleteObjects: true,
            blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL,
            encryption: s3.BucketEncryption.S3_MANAGED,
            enforceSSL: true,
        });

        this.arnIssuanceMokDocsBucket = issuanceMokDocsBucket.bucketArn
        
        if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
        if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
        if (props.client) cdk.Tags.of(this).add("Client", props.client);
        if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
        if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
        if (props.project) cdk.Tags.of(this).add("Project", props.project);
    }
}