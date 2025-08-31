using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Services;

namespace MasterRdsServices.Controllers;

public static class VoucherStatusEndpoints
{
    public static void MapVoucherStatusQueryDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("Voucher Statuses");

        group.MapGet("/voucher-statuses", GetVoucherStatuses);
        static IResult GetVoucherStatuses(IVoucherStatusServices _IVoucherStatusServices)
        {
            try
            {
                var result = _IVoucherStatusServices.GetVoucherStatus(); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/voucher-statuses/{id}", GetVoucherStatusById);
        static async Task<IResult> GetVoucherStatusById(int id, IVoucherStatusServices _IVoucherStatusServices)
        {
            try
            {
                var result = await _IVoucherStatusServices.GetVoucherStatusById(id); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
