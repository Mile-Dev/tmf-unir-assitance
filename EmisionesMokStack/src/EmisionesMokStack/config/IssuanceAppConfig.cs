
using System.Collections.Generic;

namespace EmisionesMokStack.config
{
    public class IssuanceAppConfig
    {
        // Environment configuration
        public string Client { get; set; }
        public string Project { get; set; }
        public string Stage { get; set; }
        public string Account { get; set; }
        public string Region { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        // Backend configuration
        public string SecretName { get; set; }
        public string SecretArn { get; set; }
        public string SenderEmail { get; set; }
        public string WebSiteBucket { get; set; }
        public string AppBucket { get; set; }

        // Database configuration
        public string SnapshotArn { get; set; }
        public string Database { get; set; }
        public int RdsPort { get; set; }
        public string Username { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }

        // VPC
        public string Cidr { get; set; }
        public List<string> AvailabilityZones { get; set; }

        // Logging
        public string PowerToolsLogLevel { get; set; }
        public string PowerToolsLoggerSampleRate { get; set; }
        public string PowerToolsLoggerLogEvent { get; set; }
        public string PowerToolsServiceName { get; set; }

        // Alerts
        public List<string> AlertEmails { get; set; }

        public string Directory { get; set; }
        public string LambdaPublishPath { get; set; } 
        public string LambdaRoleArn { get; set; }
    }
}
