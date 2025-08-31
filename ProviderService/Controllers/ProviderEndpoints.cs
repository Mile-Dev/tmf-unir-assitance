
using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Dto.Provider.Created;
using ProviderService.Services.Interfaces;

namespace ProviderService.Controllers;

public static class ProviderEndpoints
{
    public static void MapProviderGetEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/providers").WithTags("provider");

        group.MapGet("/searchs", GetProvidersAsync);
        static async Task<IResult?> GetProvidersAsync([AsParameters] ProviderFilterCity providerFilterCity, IProviderServices _providerServices)
        {
            try
            {
                var result = await _providerServices.ProviderSearchAsync(providerFilterCity);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/{id}", GetProviderByIdAsync);
        static async Task<IResult?> GetProviderByIdAsync(string id, IProviderServices _providerServices)
        {
            try
            {
                var result = await _providerServices.GetProviderByIdAsync(id);
                return result.Id == null  ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}", UpdateProviderAsync);
        static async Task<IResult?> UpdateProviderAsync(string id, ProviderCreateDto provider, IProviderServices _providerServices)
        {
            try
            {
                var result = await _providerServices.UpdateProviderByIdAsync(id, provider);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/add", CreatedProviderAsync);
        static async Task<IResult?> CreatedProviderAsync(ProviderCreateDto provider, IProviderServices _providerServices)
        {
            try
            {
                var result = await _providerServices.CreateProviderAsync(provider);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}", DeletedProviderAsync);
        static async Task<IResult?> DeletedProviderAsync(string id, IProviderServices _providerServices)
        {
            try
            {
                var result = await _providerServices.DeleteProviderByIdAsync(id);
                return !result  ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
