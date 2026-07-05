using System.Net;
using System.Text.Json;

namespace TupApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
                await EscribirRespuestaErrorAsync(context, ex);
            }
        }

        private static async Task EscribirRespuestaErrorAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, mensaje) = ex switch
            {
                KeyNotFoundException        => (HttpStatusCode.NotFound,             ex.Message),
                ArgumentOutOfRangeException => (HttpStatusCode.BadRequest,           ex.Message),
                ArgumentException           => (HttpStatusCode.BadRequest,           ex.Message),
                InvalidOperationException   => (HttpStatusCode.UnprocessableEntity,  ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized,         "No autorizado."),
                _                           => (HttpStatusCode.InternalServerError,  "Ocurrió un error interno. Por favor intente más tarde.")
            };

            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                status    = (int)statusCode,
                error     = mensaje,
                path      = context.Request.Path.Value,
                timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(respuesta,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(json);
        }
    }
}