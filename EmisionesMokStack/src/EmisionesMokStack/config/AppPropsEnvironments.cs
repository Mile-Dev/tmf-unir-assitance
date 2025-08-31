
using System.Collections.Generic;
using System.IO;

namespace EmisionesMokStack.config
{
    public class AppPropsEnvironments
    {
        public static IssuanceAppConfig Dev => new()
        {
            Client = "bt",
            Project = "IssuanceMok",
            Stage = "dev",
            Account = "590183946089",
            Region = "us-east-2",
            Tags = new Dictionary<string, string>
            {
                ["project"] = "IssuanceMok",
                ["stage"] = "dev"
            },
            SecretName = "dev/IssuanceMok",
            SecretArn = "",
            SenderEmail = "",
            WebSiteBucket = "",
            AppBucket = "",
            SnapshotArn = "arn:aws:rds:us-east-1:590183946089:cluster-snapshot:dev-snapshot-20-02",
            Database = "",
            RdsPort = 5432,
            Username = "",
            MinCapacity = 1,
            MaxCapacity = 2,
            Cidr = "10.2.0.0/16",
            AvailabilityZones = new List<string> { "us-east-1a", "us-east-1b" },
            PowerToolsLogLevel = "",
            PowerToolsLoggerSampleRate = "1",
            PowerToolsLoggerLogEvent = "true",
            PowerToolsServiceName = "api_backend_dev",
            AlertEmails = [""],

            Directory = "IssuanceMOk",
            LambdaPublishPath = Path.GetFullPath( Path.Combine("..","..", "..", "IssuanceMokServices", "bin", "Release", "net8.0", "publish")),
            LambdaRoleArn = "arn:aws:iam::590183946089:role/lambda-execution-role"
        };

        public static IssuanceAppConfig Prod => new()
        {
            Client = "bt",
            Project = "IssuanceMok",
            Stage = "prod",
            Account = "590183946089",
            Region = "us-east-1",
            Tags = new Dictionary<string, string>
            {
                ["project"] = "gestor",
                ["stage"] = "prod"
            },
            SecretName = "prod/IssuanceMok",
            SecretArn = "",
            SenderEmail = "",
            WebSiteBucket = "t",
            AppBucket = "",
            SnapshotArn = "",
            Database = "",
            RdsPort = 0,
            Username = "",
            MinCapacity = 1,
            MaxCapacity = 2,
            Cidr = "10.2.0.0/16",
            AvailabilityZones = new List<string> { "us-east-1a", "us-east-1b" },
            PowerToolsLogLevel = "",
            PowerToolsLoggerSampleRate = "",
            PowerToolsLoggerLogEvent = "",
            PowerToolsServiceName = "",
            AlertEmails = [""]
        };
    }
}
