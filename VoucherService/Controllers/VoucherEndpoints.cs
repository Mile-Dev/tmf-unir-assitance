namespace VoucherService.Controllers
{
    using Microsoft.AspNetCore.Http;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using microservice_voucher.Domain.Dto;

    public static class VoucherEndpoints
    {
        public static void MapVoucherEndpoints(this WebApplication app)
        {
            app.MapGet("/v1/vouchers/byid/{id}", GetVoucherById);
            app.MapGet("/v1/vouchers/bylastname/{lastname}", GetVoucherByLastname);
            app.MapGet("/v1/vouchers/byvoucher/{voucherNumber}", GetVoucherByVoucherNumber);
            app.MapGet("/v1/vouchers/voucherinfo/{voucherNumber}", GetVoucherInfo);
            app.MapGet("/v1/vouchers/pdf/byvoucher/{voucherNumber}", GetVoucherPdf);
            app.MapGet("/v1/vouchers/status/byvoucher/{voucherNumber}", GetVoucherStatus);
        }
        public static async Task<IResult> GetVoucherById(string id, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var client = httpClientFactory.CreateClient();
            try
            {
                var PASSWORD = configuration["end_points_settings:get_vouchers_global:password"];
                var URL_BASE = configuration["end_points_settings:get_vouchers_global:url"];
                string FULL_PATH = $"{URL_BASE}?password={PASSWORD}&voucher_number=&passenger_last_name=&passenger_document_number={id}";
                HttpResponseMessage response = await client.GetAsync(FULL_PATH);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    if (results == "false")
                    {
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related vouchers found with this Id {id}");
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
                return Results.Problem("An error occurred while fetching the voucher by id.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        public static async Task<IResult> GetVoucherByLastname(string lastname, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var client = httpClientFactory.CreateClient();
            try
            {
                var PASSWORD = configuration["end_points_settings:get_vouchers_global:password"];
                var URL_BASE = configuration["end_points_settings:get_vouchers_global:url"];
                string FULL_PATH = $"{URL_BASE}?password={PASSWORD}&voucher_number=&passenger_last_name={lastname}";
                HttpResponseMessage response = await client.GetAsync(FULL_PATH);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    if (results == "false")
                    {
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related vouchers found with this Lastname {lastname}");
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
                return Results.Problem("An error occurred while fetching the voucher by lastname.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        public static async Task<IResult> GetVoucherByVoucherNumber(string voucherNumber, IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related vouchers found with this Voucher Number {voucherNumber}");
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
                return Results.Problem("An error occurred while fetching the voucher by Voucher Number.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        public static async Task<IResult> GetVoucherInfo(string voucherNumber, IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related vouchers found with this Voucher Number {voucherNumber}");
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
                return Results.Problem("An error occurred while fetching the voucher info.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        public static async Task<IResult> GetVoucherPdf(string voucherNumber, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var client = httpClientFactory.CreateClient();
            try
            {
                var PASSWORD = configuration["end_points_settings:get_voucher_pdf:password"];
                var URL_BASE = configuration["end_points_settings:get_voucher_pdf:url"];
                string FULL_PATH = $"{URL_BASE}?password={PASSWORD}&voucher_number={voucherNumber}&lang=es";
                HttpResponseMessage response = await client.GetAsync(FULL_PATH);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    if (results == "false")
                    {
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related PDF found with this Voucher Number {voucherNumber}");
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
                return Results.Problem("An error occurred while fetching the voucher PDF.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        public static async Task<IResult> GetVoucherStatus(string voucherNumber, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var client = httpClientFactory.CreateClient();
            try
            {
                var PASSWORD = configuration["end_points_settings:get_voucher_status:password"];
                var URL_BASE = configuration["end_points_settings:get_voucher_status:url"];
                string FULL_PATH = $"{URL_BASE}?password={PASSWORD}&voucher_number={voucherNumber}&lang=pt";
                HttpResponseMessage response = await client.GetAsync(FULL_PATH);

                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    if (results == "false")
                    {
                        ErrorDetails errorNotFound = new ErrorDetails("404", "Not Found", $"No related PDF found with this Voucher Number {voucherNumber}");
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
                return Results.Problem("An error occurred while fetching the voucher PDF.");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }

}
