import { Effect, IPrincipal, PolicyStatement } from "aws-cdk-lib/aws-iam";


export function default_policy(notPrincipals: IPrincipal[], ids: string[], userNames:string[], resources: string[]) {

    return new PolicyStatement({
        effect: Effect.DENY,
        //principals: [new AnyPrincipal()],
        notPrincipals: [
            ...notPrincipals
        ],
        actions: [
            "s3:GetObject",
            "s3:PutObject",
            "s3:DeleteObject",
            "s3:ListBucket",
            "s3:PutObjectAcl"
        ],
        resources: [
            ...resources
        ],
        conditions:{
            StringNotLike:{
                "aws:userId":[
                    ...ids
                ],
                "aws:username":[
                    ...userNames
                ]
            }
        }
    })
}