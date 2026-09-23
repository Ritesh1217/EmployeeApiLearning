using System.Net;

namespace EmployeeApiLearning.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "something went wrong",
                    error = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
