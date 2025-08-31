import boto3
import os
import json

def lambda_handler(event, context):
    try:

        sns = boto3.client('sns')
        topic_arn = os.environ['SNS_TOPIC_ARN']
        action = event.get('action', 'Unknown')
        current_status = event.get('current_status',"desconocido")
        errorInfo = event.get("errorInfo")

        statusCode = event.get('statusCode')
        body = json.loads(event.get("body"))

        error = body.get("error")
        details = body.get("details")


        subject = f"Tracking Mok Lambda {action}"

        message = []
        
        # if errorInfo: 
        #     try:
        #         parsed = json.loads(errorInfo.get("Cause", "{}"))
        #         shutdown_msg = parsed.get("errorMessage", "Error desconocido.")
        #         message.append("\n⚠️ Proceso fallido:")
        #         message.append(f"⚠️ {shutdown_msg}")
        #     except Exception as parse_error:
        #         message.append("\n⚠️ Proceso fallido (error al parsear causa)")
        # else:
        #     message.append(f"Proceso terminado correctamente, status: {current_status}")

        if statusCode == 200:
            return {
                **event,
                "status_notification": "proceso exitoso, no se notifico"
            }
        else: 
            message.append("\n⚠️ Proceso fallido:")
            message.append(f"⚠️ {error}")
            message.append(f"⚠️ {details}")
    
            sns.publish(
                TopicArn=topic_arn,
                Message="\n".join(message),
                Subject=subject
            )

            return {
                **event,
                "status_notification": "envio_exitoso"
            }

        

    except Exception as e:
        print(e)
        return {
            **event,
            "status_notification": "envio_fallido"
        }