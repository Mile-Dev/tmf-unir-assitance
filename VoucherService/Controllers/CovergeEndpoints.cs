namespace VoucherService.Controllers
{
    using Microsoft.AspNetCore.Http;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using microservice_voucher.Domain.Dto;
    using VoucherService.Services;
    using Microsoft.AspNetCore.Http.HttpResults;

    public static class CovergeEndpoints
    {
        public static void MapCovergeEndpoints(this WebApplication app)
        {
            app.MapGet("/coverage/byvoucher/{voucherNumber}", GetCoveragesByVoucher);
            app.MapGet("/api/voucher/coverage/{voucherNumber}", GetCoveragesbyNumber);

        }
        public static async Task<IResult> GetCoveragesByVoucher(string voucherNumber, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var client = httpClientFactory.CreateClient();
            try
            {
                var PASSWORD = configuration["end_points_settings:get_vouchers_global:password"];
                var URL_BASE = configuration["end_points_settings:get_vouchers_global:url"];
                string FULL_PATH = $"{URL_BASE}?password={PASSWORD}&voucher_number={voucherNumber}";
                HttpResponseMessage response = await client.GetAsync(FULL_PATH);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    if (results == "false")
                    {
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related coverages found with this voucher number {voucherNumber}");
                        ErrorResponse errorResponseNotFound = new ErrorResponse(errorNotFound);
                        return Results.NotFound();
                    }
                    SuccessResponse successResponse = new SuccessResponse(results);
                    return Results.Ok(successResponse);
                }
                ErrorDetails error = new ErrorDetails("500", "Bad Request", $"The request was wrong");
                ErrorResponse errorResponse = new ErrorResponse(error);
                return Results.BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching voucher by id: {ex.Message}");
                return Results.Problem("An error occurred while fetching the coverages.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        public static async Task<IResult> GetCoveragesbyNumber(string voucherNumber, ICoverageServices coverageServices)
        {
            try
            {
                var objectCoveragesbyNumber = await coverageServices.GetEventAsync(voucherNumber);
                SuccessResponse successResponse = new(objectCoveragesbyNumber);
                return objectCoveragesbyNumber != null ? TypedResults.Ok(successResponse) : TypedResults.NotFound();               
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching voucher by id: {ex.Message}");
                return TypedResults.BadRequest("An error occurred while fetching the coverages.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

    }

}
