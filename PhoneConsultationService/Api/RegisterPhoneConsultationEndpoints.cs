 using PhoneConsultationService.Common.Models;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Services;

namespace PhoneConsultationService.Api
{
    public static class RegisterPhoneConsultationEndpoints
    {
        public static void PhoneConsultationEndpoints(this WebApplication app)
        {

            app.MapPost("/v1/medical-orientation/phone-consultation/create", async (PhoneConsultationDto phoneConsultationDto, PhoneConsultationServices phoneConsultationServices) =>
            {
                try
                {
                    await phoneConsultationServices.CreateAsync(phoneConsultationDto);
                    OperationSuccessResponse<PhoneConsultationDto> successResponse = new(phoneConsultationDto);
                    return Results.Created($"/api/phone-consultation/{phoneConsultationDto.IdEvent}", successResponse);
                }
                catch (Exception ex)
                {
                    OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                    return Results.BadRequest(errorDetails);
                }

            });

            app.MapGet("/v1/medical-orientation/phone-consultation/{idEvent}/{idPhoneRecord}", async (string idEvent, string idPhoneRecord, PhoneConsultationServices phoneConsultationServices) =>
            {
                try
                {
                    var phoneConsultationDto = await phoneConsultationServices.GetIdPhoneRecordByIdEventAsync(idEvent, idPhoneRecord);
                    if(phoneConsultationDto == null)
                    {
                        OperationErrorsResponse errorDetails = new("404", "Not Found", "Resource not found");
                        return Results.NotFound(errorDetails);
                    }
                    OperationSuccessResponse<PhoneConsultationDto> successResponse = new(phoneConsultationDto);
                    return Results.Ok(successResponse);
                }
                catch (Exception ex)
                {
                    OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                    return Results.BadRequest(errorDetails);
                }
            });


            app.MapGet("/v1/medical-orientation/phone-consultation/{idEvent}/attachment", async (string idEvent, PhoneConsultationServices phoneConsultationServices) =>
            {
                try
                {
                    var attachmentDto = await phoneConsultationServices.GetAttachmentPhoneConsultationByIdEventAsync(idEvent);
                    OperationSuccessResponse<List<AttachmentDto>> successResponse = new(attachmentDto);
                    return Results.Ok(successResponse);
                }
                catch (Exception ex)
                {
                    OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                    return Results.BadRequest(errorDetails);
                }
            });
        }

    }
}
