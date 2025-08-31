namespace EventServices.Common
{
    public class Constans
    {
        public const string URLFirstContact = "https://vpce-0a0e6f965235dce8c-1h1d6bse.lambda.us-east-2.vpce.amazonaws.com/2015-03-31/functions/arn:aws:lambda:us-east-2:590183946089:function:LambdaEventFirstContact/invocations";

        public const string ClientMok = "2";

        public const string ClientTerrawind = "1";

        public const string ClientDefault = "DEFAULT";

        public const string PathEventServicesBucketName = "eventservices";

        //cambiar 
        public const string StatusScheduled = "Scheduled";

        //cambiar 
        public const string RequestStatusSuccess = "success";

        public const string RequestStatusError = "error";

        public const string StatusCompleted = "Completed";

        public const string SeparatorExternalRequest = "|";

        public const string MessageRole = "User_System";

        public const string MessageUserName = "Admin_User";

        public const int CanceledGuaranteePayment = 7;

        public const string STATUS_EP_NO_PROVIDER_ASSIGNED = "EP_NO_PROVIDER_ASSIGNED";
      
        public const string STATUS_EP_APPOINTMENT_SCHEDULED = "EP_APPOINTMENT_SCHEDULED";
        
        public const string STATUS_EP_SERVICE_CANCELED = "EP_SERVICE_CANCELED";
        
        public const string STATUS_EP_SERVICE_COMPLETED = "EP_SERVICE_COMPLETED";
        
        public const string STATUS_EP_BILLED_EVENT = "EP_BILLED_EVENT";
        
        public const string STATUS_EP_PAYMENT_IN_PROCESS = "EP_PAYMENT_IN_PROCESS";
        
        public const string STATUS_EP_PAID_EVENT = "EP_PAID_EVENT";

        public const string STATUS_EVENT_CLOSED = "EVENT_CLOSED";

        public const string STATUS_EVENT_CANCEL = "EVENT_CANCEL";

        public const string STATUS_EVENT_IN_ASSISTENCE_TEAM = "EVENT_IN_ASSISTENCE_TEAM";

        public const string STATUS_EVENT_IN_TMO = "EVENT_IN_TMO";

        public const string STATUS_EVENT_IN_PROVIDER = "EVENT_IN_PROVIDER";

        public const string STATUS_EVENT_CREATED = "EVENT_CREATED";

        //mensajes 

        public const bool MESSAGE_SEND = true;

        public const string MESSAGE_EVENT_PROVIDER_CREATE =
          "BYTS - Un proveedor de servicios ha sido asignado exitosamente con el ID {0} a su solicitud de asistencia (ID del evento: {1}, Voucher: {2}). " +
          "La solicitud corresponde a un servicio de tipo {3} y actualmente se encuentra en estado: {4}.";

        public const string MESSAGE_GUARANTEE_PAYMENT_CREATE =
          "BYTS - Se ha creado exitosamente una garantía de pago con el ID {0} para el evento de asistencia (ID del evento: {1}), " +
          "vinculada al voucher: {2}. Con base en esta garantía, el servicio será iniciado.";

        public const string MESSAGE_EVENT_PROVIDER_RESOLVE =
          "BYTS - El proveedor completó exitosamente el servicio para el evento con ID {0}. " +
          "Tipo de asistencia: {1}. El caso de asistencia ha sido resuelto. El servicio ha sido finalizado con el diagnóstico: {2}.";

        public const string MESSAGE_EVENT_CREATE =
          "BYTS - El evento fue creado exitosamente con el ID: {0} y está asociado al voucher: {1}.";

        public const string MESSAGE_EVENT_UPDATE =
          "BYTS - El evento con ID {0} ha sido actualizado exitosamente. Está asociado al voucher {1}.";

        public const string MESSAGE_EVENT_PROVIDER_RESCHEDULE =
          "BYTS - El servicio del proveedor fue reprogramado exitosamente para el evento con ID {0}.";

        public const string MESSAGE_EVENT_PROVIDER_UPDATE =
          "BYTS - El proveedor fue actualizado exitosamente para el evento con ID {0}.";

        public const string MESSAGE_EVENT_PROVIDER_UPDATE_DIAGNOSTIC =
          "BYTS - El proveedor fue actualizado exitosamente para el evento con ID {0} y diagnostico {1}.";

        public const string MESSAGE_EVENT_PROVIDER_CANCELED =
          "BYTS - El proveedor fue cancelado exitosamente del evento con ID {0}.";

        public const string MESSAGE_EVENT_CANCELED =
          "BYTS - El evento con ID {0}, asignado al voucher {1}, fue cancelado exitosamente.";

        public const string MESSAGE_EVENT_CLOSE =
          "BYTS - El evento con ID {0}, asignado al voucher {1}, fue cerrado exitosamente.";

        public const string MESSAGE_EVENT_REOPEN =
          "BYTS - El evento con ID {0}, asignado al voucher {1}, fue reabierto exitosamente.";

        public const string MESSAGE_GUARANTEE_PAYMENT_UPDATE =
          "BYTS - La garantía de pago fue actualizada exitosamente para el evento con ID {0}, asignado al voucher {1}.";

        public const string MESSAGE_EVENT_PHONECONSULTATION_CANCELED =
          "BYTS - La consulta telefónica asociada al evento con ID {0} asignada al voucher {1} ha sido cancelada exitosamente.";

        public const string MESSAGE_EVENT_PHONECONSULTATION_RESOLVE =
          "BYTS - La consulta telefónica asociada al evento con ID {0} asignada al voucher {1} fue completada exitosamente.";

        public const string MESSAGE_EVENT_PHONECONSULTATION_CREATE =
          "BYTS - La consulta telefónica asociada al evento con ID  {0}, asignada al voucher {1}, fue creada exitosamente.";

        public const string MESSAGE_EVENT_PHONECONSULTATION_RESCHEDULE =
          "BYTS - La consulta telefónica asociada al evento con ID  {0} fue reprogramada exitosamente.";

    }
}
