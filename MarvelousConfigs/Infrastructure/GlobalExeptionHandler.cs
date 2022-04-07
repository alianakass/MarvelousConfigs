using MarvelousConfigs.BLL.Exeptions;
using System.Net;
using System.Text.Json;

namespace MarvelousConfigs.API.Infrastructure
{
    public class GlobalExeptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExeptionHandler> _logger;

        public GlobalExeptionHandler(RequestDelegate next, ILogger<GlobalExeptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, "Не возможно связаться с сервером и обработать запрос");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
        {
            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            _logger.LogError($"Eror {code} : {message}");
            await context.Response.WriteAsync(result);
        }
    }
}
