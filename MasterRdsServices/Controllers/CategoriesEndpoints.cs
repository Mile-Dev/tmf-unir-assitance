using MasterRdsServices.Services;

namespace MasterRdsServices.Controllers;

public static class CategoriesEndpoints
{
    public static void MapCategoriesQueryDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("Categories and Assistances");

        group.MapGet("/categories", GetCategories);
        static IResult GetCategories(ICategoriesServices _ICategoriesServices)
        {
            try
            {
                var result = _ICategoriesServices.GetAllCategories(); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/assistance-types", GetCategoriesTypesAssistance);
        static IResult GetCategoriesTypesAssistance(ICategoriesServices _ICategoriesServices)
        {
            try
            {              
                var result = _ICategoriesServices.GetCategoriesAssits(); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}