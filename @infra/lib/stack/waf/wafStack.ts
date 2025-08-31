import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";
import * as wafv2 from "aws-cdk-lib/aws-wafv2";


export interface WafStackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;
}

export class WafStack extends Stack {

    public readonly webAcl: wafv2.CfnWebACL;

    constructor(scope: Construct, id: string, props: WafStackProps) {
        super(scope, id, props);

        const rateLimitThreshold = 1000;

        // IP Set para bloqueo geográfico
        const geoBlockIPSet = new wafv2.CfnIPSet(this, "GeoBlockIPSet", {
            addresses: [],  // Vacío, ya que usaremos GeoMatchStatement
            ipAddressVersion: "IPV4",
            scope: "REGIONAL",
            name: "GeoBlockIPSet",
        });

        const rules: wafv2.CfnWebACL.RuleProperty[] = [
            // 1. AWS Managed Common Rule Set
            {
                name: "AWSManagedRulesCommonRuleSet",
                priority: 1,
                overrideAction: { none: {} },  // Use rule actions
                statement: {
                    managedRuleGroupStatement: {
                        vendorName: "AWS",
                        name: "AWSManagedRulesCommonRuleSet",
                    },
                },
                visibilityConfig: {
                    sampledRequestsEnabled: true,
                    cloudWatchMetricsEnabled: true,
                    metricName: "AWSManagedRulesCommonRuleSet",
                },
            },
            // 2. AWS Managed Known Bad Inputs Rule Set
            {
                name: "AWSManagedRulesKnownBadInputsRuleSet",
                priority: 2,
                overrideAction: { none: {} },
                statement: {
                    managedRuleGroupStatement: {
                        vendorName: "AWS",
                        name: "AWSManagedRulesKnownBadInputsRuleSet",
                    },
                },
                visibilityConfig: {
                    sampledRequestsEnabled: true,
                    cloudWatchMetricsEnabled: true,
                    metricName: "AWSManagedRulesKnownBadInputsRuleSet",
                },
            },
            // 3. Rate Limit Rule (bloquear IPs que excedan threshold)
            {
                name: "IPRateLimit",
                priority: 3,
                action: { block: {} },
                statement: {
                    rateBasedStatement: {
                        limit: rateLimitThreshold,
                        aggregateKeyType: "IP",
                    },
                },
                visibilityConfig: {
                    sampledRequestsEnabled: true,
                    cloudWatchMetricsEnabled: true,
                    metricName: "IPRateLimit",
                },
            },
            // 4. Geo Block Rule (bloquear países en la lista)
            {
                name: "GeoBlockRule",
                priority: 4,
                action: { block: {} },
                statement: {
                    geoMatchStatement: {
                        countryCodes: ["CO","US"],
                    },
                },
                visibilityConfig: {
                    sampledRequestsEnabled: true,
                    cloudWatchMetricsEnabled: true,
                    metricName: "GeoBlockRule",
                },
            },
        ];

        // Crear Web ACL con tráfico tipo ALL (regional)
        this.webAcl = new wafv2.CfnWebACL(this, "WebACL", {
            name: "asistencias-app-waf-web-acl",
            scope: "REGIONAL",
            defaultAction: { allow: {} },  // Por defecto permitir (las reglas bloquean lo definido)
            rules,
            visibilityConfig: {
                cloudWatchMetricsEnabled: true,
                metricName: "MyWebACL",
                sampledRequestsEnabled: true,
            },
            // Optional: trafficType no es directamente configurable en CDK, 
            // pero REGIONAL aplica a ALB, API GW y AppSync (todo el tráfico regional)
        });

        // Asociar la ACL al recurso (ALB, API GW, etc)
        // new wafv2.CfnWebACLAssociation(this, "WebACLAssociation", {
        //     resourceArn: props.resourceArn,
        //     webAclArn: this.webAcl.attrArn,
        // });


        if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
        if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
        if (props.client) cdk.Tags.of(this).add("Client", props.client);
        if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
        if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
        if (props.project) cdk.Tags.of(this).add("Project", props.project);
    }
}
