using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Constructs;
using EmisionesMokStack.config;
using System.Collections.Generic;

namespace EmisionesMokStack
{
    public class IssuanceMokStack : Stack
    {
        internal IssuanceMokStack(Construct scope, string id, IssuanceAppConfig appProps,  IStackProps props = null) : base(scope, id, props)
        {

            #region Crear S3 Bucket
            var bucketName = $"issuance-mok-docs-{appProps.Stage}-{appProps.Region}".ToLower();

            var bucket = new Bucket(this, "IssuanceBucket", new BucketProps
            {
                BucketName = bucketName,
                RemovalPolicy = RemovalPolicy.DESTROY,
                AutoDeleteObjects = true
            });
            #endregion

            #region Crear Tabla DynamoDb
            var table = new Table(this, "IssuanceMok", new TableProps
            {
                TableName = $"IssuanceMok",
                PartitionKey = new Attribute
                {
                    Name = "PK",
                    Type = AttributeType.STRING
                },        
                BillingMode = BillingMode.PAY_PER_REQUEST,
                RemovalPolicy = RemovalPolicy.DESTROY
                
            });
            #endregion

            #region Crear Lambda
            var lambda = new Function(this, "LambdaIssuanceMok", new FunctionProps
            {
                FunctionName = $"LambdaIssuanceMok-{appProps.Stage}",
                Runtime = Runtime.DOTNET_8,
                Handler = "IssuanceMokServices",
                Code = Code.FromAsset(appProps.LambdaPublishPath),
                Timeout = Duration.Seconds(30),
                MemorySize = 1024,
                Architecture = Architecture.X86_64,      
                Environment = new Dictionary<string, string>
                {
                    { "AWS__S3__BucketName", bucket.BucketName },
                    { "AWS__DynamoDB__TableName", table.TableName },
                    { "AWS__Directory", appProps.Directory }
                }
            });

            bucket.GrantReadWrite(lambda);
            table.GrantReadWriteData(lambda);

            Amazon.CDK.Tags.Of(this).Add("project", appProps.Project);
            Amazon.CDK.Tags.Of(this).Add("stage", appProps.Stage);
            #endregion

            lambda.AddPermission("AllowApiGatewayInvoke", new Permission
            {
                Principal = new ServicePrincipal("apigateway.amazonaws.com"),
                Action = "lambda:InvokeFunction",
                SourceArn = $"arn:aws:execute-api:us-east-2:{appProps.Account}:21oc6umimd/*/*/*"
            });

        }
    }
}