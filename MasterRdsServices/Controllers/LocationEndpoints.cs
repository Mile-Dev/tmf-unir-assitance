using MasterRdsServices.Services;

namespace MasterRdsServices.Controllers;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("Countries and Cities");

        group.MapGet("/countries", Getcountries);
        static async Task<IResult?> Getcountries(ICountriesAndCitiesServices _countriescitiesServices)
        {
            try
            {
                var result = await _countriescitiesServices.GetCountriesAsync();
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/countries/{id}/cities", Getcitiesbycountry);
        static async Task<IResult?> Getcitiesbycountry(string id, ICountriesAndCitiesServices _countriescitiesServices)
        {
            try
            {
                var result = await _countriescitiesServices.GetCitiesByCountryAsync(id);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
