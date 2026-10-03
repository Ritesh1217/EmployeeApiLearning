using System.Net;
using EmployeeApiLearning.DTO;

namespace EmployeeApiLearning.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, 
             ILogger<GlobalExceptionMiddleware> logger,
             IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Unhandled exception occured : { Message }", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponseDto
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal server Error",
                Detailed = _env.IsDevelopment() ? exception.StackTrace?.ToString() : null
            };

            await context.Response.WriteAsync(response.ToString());
        }
    }
}
