using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterRdsServices.Controllers;

public static class GeneralTypesEndpoints
{
    public static void MapGeneralTypesQueryDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("General types and Subtypes");

        group.MapGet("/identification-types", GetDocumenttype);
        static async Task<IResult> GetDocumenttype(IGeneralTypesServices _IGeneralTypesServices)
        {
            try
            {
                var result = await _IGeneralTypesServices.GetGeneralSubType(1);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/identification-types/{id}", GetIdentificationTypeById);
        static async Task<IResult> GetIdentificationTypeById(int id,  IGeneralTypesServices _IGeneralTypesServices)
        {
            try
            {
                var result =  await _IGeneralTypesServices.GetGeneralTypeByIdAsync(id, 1);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/contact-types", GetConctactype);
        static async Task<IResult>  GetConctactype(IGeneralTypesServices _IGeneralTypesServices)
        {
            try
            {
                var result = await _IGeneralTypesServices.GetGeneralSubType(2);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/contact-types/{id}", GetContactTypeById);
        static async Task<IResult> GetContactTypeById(int id, IGeneralTypesServices _IGeneralTypesServices)
        {
            try
            {
                var result = await _IGeneralTypesServices.GetGeneralTypeByIdAsync(id, 2);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/assistance-types/{id}/assistance-subtypes", GetGeneraltypes);
        static async Task<IResult> GetGeneraltypes(int id, IGeneralTypesServices _IGeneralTypesServices)
        {
            try
            {
                var result = await _IGeneralTypesServices.GetAssitanceSubType(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/provider-types", GetpProviderTypesAll);
        static async Task<IResult?> GetpProviderTypesAll(IProviderServices _iproviderServices)
        {
            try
            {
                var result = await _iproviderServices.GetRecords();
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/icd-types", GetPhoneConsultationIcd);
        static async Task<IResult?> GetPhoneConsultationIcd([FromQuery] string? name, IIcdServices _icdServices)
        {
            try
            {
                var result = await _icdServices.GetRecords(name);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
