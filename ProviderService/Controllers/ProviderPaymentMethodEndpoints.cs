using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using ProviderService.Domain.Dto.ProviderPaymentMethod.Created;
using ProviderService.Services.Interfaces;
namespace ProviderService.Controllers;

public static class ProviderPaymentMethodEndpoints
{
    public static void MapProviderPaymentMethodCreatedDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/providers").WithTags(nameof(ProviderPaymentMethodCreatedDto));

        group.MapGet("/{id}/paymentmethods/{idpayment}", GetPaymentMethodByIdProvider);
        static async Task<IResult?> GetPaymentMethodByIdProvider(string id, string idpayment, IProviderPaymentMethodServices _providerPaymentMethodservices)
        {
            try
            {
                var result = await _providerPaymentMethodservices.GetProviderPaymentMethodByIdAsync(id, idpayment);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}/paymentmethods/{idpayment}", UpdatedAgreementByIdProvider);
        static async Task<IResult> UpdatedAgreementByIdProvider(string id, string idpayment, ProviderPaymentMethodCreatedDto input, IProviderPaymentMethodServices _providerPaymentMethodservices)
        {
            try
            {
                var result = await _providerPaymentMethodservices.UpdateProviderPaymentMethodByIdAsync(id, idpayment, input);
                return result == null ? TypedResults.NotFound("The agreement was not found") : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/{id}/paymentmethods", CreatedPaymentMethodByIdProvider);
        static async Task<IResult> CreatedPaymentMethodByIdProvider(string id, ProviderPaymentMethodCreatedDto input, IProviderPaymentMethodServices _providerPaymentMethodservices)
        {
            try
            {
                var resul = await _providerPaymentMethodservices.CreateProviderPaymentMethodAsync(id, input);
                return string.IsNullOrEmpty(resul.IdPayment) ? TypedResults.NotFound()
                                                              : TypedResults.Created($"/api/provider/paymentmethod/idprovider/{resul.IdProvider}/idpaymentmethod/{resul.IdPayment}", resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}/paymentmethods/{idpayment}", DeleteAgreementByIdProvider);
        static async Task<IResult> DeleteAgreementByIdProvider(string id, string idpayment, IProviderPaymentMethodServices _providerPaymentMethodservices)
        {
            try
            {
                var resul = await _providerPaymentMethodservices.DeleteProviderPaymentMethodByIdAsync(id, idpayment);
                return !resul ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
