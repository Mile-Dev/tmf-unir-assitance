import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";
import * as cognito from 'aws-cdk-lib/aws-cognito';


export interface CognitoStackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;
}

export class CognitoStack extends Stack {
    public readonly userPool: cognito.UserPool;
    public readonly userPoolId: string;
    public readonly clientFrontendId: string;
    public readonly clientKomId: string;

    constructor(scope: Construct, id: string, props: CognitoStackProps) {
        super(scope, id, props);

        // User Pool para gestión de usuarios
        const userPool = new cognito.UserPool(this, 'UserPool', {
            userPoolName: 'solution-assist-cognito-pool',
            signInAliases: { email: true },
            autoVerify: { email: true, },
            accountRecovery: cdk.aws_cognito.AccountRecovery.EMAIL_ONLY,
            selfSignUpEnabled: true,
            featurePlan: cdk.aws_cognito.FeaturePlan.LITE,
            keepOriginal: { email: true },
            standardAttributes: {
                email: { required: true, mutable: true }
            }

        });

        // Configurar un dominio Cognito personalizado con el subdominio especificado
        userPool.addDomain('CognitoDomain', {
            cognitoDomain: {
                domainPrefix: `solution-assist-${props.stage}` // subdominio para el dominio hospedado de Cognito
            }
        });

        userPool.addGroup('AssistancesAdminGroup', {
            groupName: 'assistances-admin',
            description: 'Grupo de usuarios para Assistances Admin'
        });

        userPool.addGroup('AssistancesTmoGroup', {
            groupName: 'assistances-tmo',
            description: 'Grupo de usuarios para Assistances TMO'
        });

        // Definir un Resource Server con scopes OAuth2 personalizados (read, write)
        const readScope = new cognito.ResourceServerScope({
            scopeName: 'read',
            scopeDescription: 'Acceso de solo lectura'
        });
        const writeScope = new cognito.ResourceServerScope({
            scopeName: 'write',
            scopeDescription: 'Acceso de escritura'
        });
        
        const AssistencesResourceServerAssistences = userPool.addResourceServer('AssistencesResourceServerAssistences', {
            identifier: 'assistences-services',               // identificador único del recurso
            userPoolResourceServerName: 'Assistences services', // nombre descriptivo
            scopes: [readScope, writeScope]                 // registrar los scopes personalizados
        });

        const AssistencesResourceServerKom = userPool.addResourceServer('AssistencesResourceServerKom', {
            identifier: 'kom-services',               // identificador único del recurso
            userPoolResourceServerName: 'Kom services', // nombre descriptivo
            scopes: [readScope, writeScope]                 // registrar los scopes personalizados
        });

        // Clients para diferentes aplicaciones de clientes
        const clientKom = new cognito.UserPoolClient(this, 'clientKom', {
            userPoolClientName: "Kom-client",
            userPool,
            generateSecret: true,
            authSessionValidity: cdk.Duration.minutes(3),
            refreshTokenValidity: cdk.Duration.days(5),
            accessTokenValidity: cdk.Duration.minutes(10),
            idTokenValidity: cdk.Duration.minutes(5),
            enableTokenRevocation: true,
            preventUserExistenceErrors: false,
            oAuth: {
                flows: {
                    clientCredentials: true
                },
                scopes: [ cognito.OAuthScope.resourceServer(AssistencesResourceServerKom, readScope) ],
            },
    
            
        });

        
        const clientSolutionAssist = new cognito.UserPoolClient(this, 'clientSolutionAssist', {
            userPool,
            userPoolClientName: "solution-assist-client",
            generateSecret: true,
            authFlows: {
                userSrp: true,
                custom: true
            },
            authSessionValidity: cdk.Duration.minutes(3),
            refreshTokenValidity: cdk.Duration.days(30),
            accessTokenValidity: cdk.Duration.minutes(60),
            idTokenValidity: cdk.Duration.minutes(60),
            enableTokenRevocation: true,
            preventUserExistenceErrors: true,
            oAuth: {
                flows: {
                    clientCredentials: true
                },
                scopes: [ 
                    cognito.OAuthScope.resourceServer(AssistencesResourceServerAssistences, readScope),
                    cognito.OAuthScope.resourceServer(AssistencesResourceServerAssistences, writeScope) 
                 ],
            },
        });

        const clientFrontend = new cognito.UserPoolClient(this, 'clientFrontend', {
            userPoolClientName: "frontend-assistences",
            userPool,
            // generateSecret: true,
            authFlows: {
                userSrp: true,
                // custom: true
                user: true
            },
            authSessionValidity: cdk.Duration.minutes(3),
            refreshTokenValidity: cdk.Duration.days(5),
            accessTokenValidity: cdk.Duration.minutes(60),
            idTokenValidity: cdk.Duration.minutes(60),
            enableTokenRevocation: true,
            preventUserExistenceErrors: true,
            oAuth: {
                flows: {
                    implicitCodeGrant:false,
                    authorizationCodeGrant: true
                },
                scopes:[cognito.OAuthScope.EMAIL,cognito.OAuthScope.PHONE,cognito.OAuthScope.OPENID]
            },
        });

        this.userPool = userPool
        this.userPoolId = userPool.userPoolId
        this.clientFrontendId =  clientFrontend.userPoolClientId
        this.clientKomId = clientKom.userPoolClientId;


        if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
        if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
        if (props.client) cdk.Tags.of(this).add("Client", props.client);
        if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
        if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
        if (props.project) cdk.Tags.of(this).add("Project", props.project);
    }
}
