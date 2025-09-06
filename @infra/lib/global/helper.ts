import { aws_sns as sns, CfnOutput, CfnOutputProps } from "aws-cdk-lib";
import { Template } from "aws-cdk-lib/assertions";
import { ISecret } from "aws-cdk-lib/aws-secretsmanager";
import { Construct } from "constructs";
const fs = require('fs');

/**
 * @description - Funcion para abstraer CfnOutput
 * 
 */
export function output(context: Construct, id: string, value: CfnOutputProps) {
    new CfnOutput(context, id, value) //NOSONAR
}


/**
 * @description - Funcion que deja texto ingresado a lowercase
 * @param text - string to lowercase
 * @returns {string}
 */
export function toLow(text?: string) {
  return (text ?? '').toLowerCase();
}



/**
 * @description - Funcion para abstraer proceso de obtener valor de string
 * 
 */
export function getSecretValueAsString(secret: ISecret, value: string) {
    return secret.secretValueFromJson(value).unsafeUnwrap()
}




/**
 * @description - Namespace con funcion para crear topic SNS
 * 
 * @namespace
 */
export namespace TopicNotifications {

    /**
     * 
     * @param topicName - Nombre del Topic a crear
     * @param stage - Stage de Despliegue del stack
     * @param context - Construct
     * 
     * @returns {sns.CfnTopic} - Nuevo Topico SNS 
     */
    export function CfnNewTopic(topicName: string, stage: string, context: Construct) {
        return new sns.CfnTopic(context, `${topicName}-${stage}-notificationTopic`, {
            displayName: topicName,
            topicName: topicName
        })
    }

}


/**
 * 
 * Sintetiza el template en forma de archivo físico (JSON), recibe opcionalmente un nombre 
 * 
 * Útil para casos de debug 
 * @param template: Template
 * 
 * @param name: string
 */
export function synthTemplate(template: Template, name?: string) {
    try {
        fs.writeFileSync(name || 'template.json', JSON.stringify(template));
        // file written successfully
    } catch (err) {
        console.error(err);
    }
}