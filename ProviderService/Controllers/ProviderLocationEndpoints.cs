using Microsoft.AspNetCore.Mvc;
using ProviderService.Domain.Dto.ProviderLocation.Created;
using ProviderService.Services.Interfaces;
using SharedServices.Objects;
namespace ProviderService.Controllers;

public static class ProviderLocationEndpoints
{
    public static void MapProviderLocationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/providers").WithTags("provider location");

        group.MapPost("/{id}/locations/search", GetlocationByIdProvider);
        static async Task<IResult?> GetlocationByIdProvider(string id, [FromBody] Filters parameters, IProviderLocationServices _providerLocationServices)
        {
            try
            {
                var result = await _providerLocationServices.GetLocationByIdProviderAsync(id, parameters);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }


        group.MapGet("/{id}/locations/{idLocation}", GetlocationById);
        static async Task<IResult?> GetlocationById(string id, string idLocation, IProviderLocationServices _providerLocationServices)
        {
            try
            {
                var result = await _providerLocationServices.GetProviderLocationByIdAsync(id, idLocation);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}/locations/{idLocation}", UpdatedLocationByIdProvider);
        static async Task<IResult> UpdatedLocationByIdProvider(string id, string idLocation, ProviderLocationCreatedDto input, IProviderLocationServices _providerLocationServices)
        {
            try
            {
                var result = await _providerLocationServices.UpdateProviderLocationByIdAsync(id, idLocation, input);
                return result == null ? TypedResults.NotFound("The Location was not found") : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/{id}/locations", CreatedLocationByIdProvider);
        static async Task<IResult> CreatedLocationByIdProvider(string id, ProviderLocationCreatedDto input, IProviderLocationServices _providerLocationServices)
        {
            try
            {
                var resul = await _providerLocationServices.CreateProviderLocationAsync(id, input);
                return string.IsNullOrEmpty(resul.IdLocation) ? TypedResults.NotFound()
                                                              : TypedResults.Created($"/v1/providers/{resul.IdProvider}/locations/{resul.IdLocation}", resul);

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}/locations/{idLocation}", DeleteAgreementByIdProvider);
        static async Task<IResult> DeleteAgreementByIdProvider(string id, string idLocation, IProviderLocationServices _providerLocationServices)
        {
            try
            {
                var resul = await _providerLocationServices.DeleteProviderLocationByIdAsync(id, idLocation);
                return !resul ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
