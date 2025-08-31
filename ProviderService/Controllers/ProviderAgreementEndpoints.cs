using ProviderService.Domain.Dto.ProviderAgreement.Created;
using ProviderService.Services.Interfaces;
namespace ProviderService.Controllers;

public static class ProviderAgreementEndpoints
{
    public static void MapProviderAgreementCreatedDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/providers").WithTags(nameof(ProviderAgreementCreatedDto));

        group.MapGet("/{id}/agreements/{idagreement}", GetAgreementByIdProvider);
        static async Task<IResult?> GetAgreementByIdProvider(string id, string idagreement, IProviderAgreementServices _providerAgreementServices)
        {
            try
            {
                var result = await _providerAgreementServices.GetProviderAgreementByIdAsync(id, idagreement);
                return result == null ? TypedResults.NotFound()
                                                              : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}/agreements/{idagreement}", UpdatedAgreementByIdProvider);
        static async Task<IResult> UpdatedAgreementByIdProvider(string id, string idagreement, ProviderAgreementCreatedDto input, IProviderAgreementServices _providerAgreementServices)
        {
            try
            {
                var result = await _providerAgreementServices.UpdateProviderAgreementByIdAsync(id, idagreement, input);
                return result == null ? TypedResults.NotFound("The agreement was not found") : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/{id}/agreements", CreatedAgreementByIdProvider);
        static async Task<IResult> CreatedAgreementByIdProvider(string id, ProviderAgreementCreatedDto input, IProviderAgreementServices _providerAgreementServices)
        {
            try
            {
                var resul = await _providerAgreementServices.CreateProviderAgreementAsync(id, input);
                return string.IsNullOrEmpty(resul.IdAgreement) ? TypedResults.NotFound()
                                                              : TypedResults.Created($"/api/provider/agreement/idprovider/{resul.IdProvider}/idagreement/{resul.IdAgreement}", resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}/agreements/{idagreement}", DeleteAgreementByIdProvider);
        static async Task<IResult> DeleteAgreementByIdProvider(string id, string idagreement, IProviderAgreementServices _providerAgreementServices)
        {
            try
            {
                var resul = await _providerAgreementServices.DeleteProviderAgreementByIdAsync(id, idagreement);
                return !resul ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
