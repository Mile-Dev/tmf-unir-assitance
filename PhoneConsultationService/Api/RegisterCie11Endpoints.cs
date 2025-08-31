using PhoneConsultationService.Common.Models;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Services;

namespace PhoneConsultationService.Api
{
    public static class RegisterCie11Endpoints
    {
        public static void Cie11Endpoints(this WebApplication app)
        {
            app.MapGet("/v1/cie", async (Cid11ApiClientServices cid11ApiClientServices) =>
            {
                try
                {
                    var cid11 = await cid11ApiClientServices.ReadCie11DtosFromJsonFileAsync();
                    OperationSuccessResponse<List<Cie11Dto>> successResponse = new(cid11);
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
