using ProviderService.Domain.Dto.ProviderContact.Created;
using ProviderService.Domain.Dto.ProviderContact.Query;
using ProviderService.Services.Interfaces;

namespace ProviderService.Controllers;

public static class ProviderContactEndpoints
{
    public static void MapProviderContactGetDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/providers").WithTags(nameof(ProviderContactGetDto));

        group.MapGet("/{id}/contacts", GetContactsAllByIdProvider);
        static async Task<IResult> GetContactsAllByIdProvider(string id, IProviderContactServices _providerContactServices)
        {
            try
            {
                var result = await _providerContactServices.GetProviderContactAllByIdAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/{id}/contacts/{idcontact}", GetContactByIdProvider);
        static async Task<IResult> GetContactByIdProvider(string id, string idcontact, IProviderContactServices _providerContactServices)
        {
            try
            {
                var result = await _providerContactServices.GetProviderContactByIdAsync(id, idcontact);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}/contacts/{idcontact}", UpdatedContactByIdProvider);
        static async Task<IResult> UpdatedContactByIdProvider(string id, string idcontact, ProviderContactCreatedDto input, IProviderContactServices _providerContactServices)
        {
            try
            {
                var result = await _providerContactServices.UpdateProviderContactByIdAsync(id, idcontact, input);
                return result == null ? TypedResults.NotFound("The contact was not found") : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/{id}/contacts", CreatedContactByIdProvider);
        static async Task<IResult> CreatedContactByIdProvider(string id, ProviderContactCreatedDto input, IProviderContactServices _providerContactServices)
        {
            try
            {
                var resul = await _providerContactServices.CreateProviderContactAsync(id, input);
                return string.IsNullOrEmpty(resul.IdContact) ? TypedResults.NotFound()
                                                              : TypedResults.Created($"/api/provider/contact/idprovider/{resul.IdProvider}/idcontact/{resul.IdContact}", resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}/contacts/{idcontact}", DeleteAgreementByIdProvider);
        static async Task<IResult> DeleteAgreementByIdProvider(string id, string idcontact, IProviderContactServices _providerContactServices)
        {
            try
            {
                var resul = await _providerContactServices.DeleteProviderContactByIdAsync(id, idcontact);
                return !resul ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
