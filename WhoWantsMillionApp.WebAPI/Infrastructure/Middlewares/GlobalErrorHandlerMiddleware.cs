using System.Text.Json;
using WhoWantsMillionApp.Services.Exceptions;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Middlewares
{
    public class GlobalErrorHandlerMiddleware
    {
        private RequestDelegate _next;
        public GlobalErrorHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(BaseServiceException baseException)
            {
                Dictionary<string, object> body = new Dictionary<string, object>()
                {
                    {"StatusCode",baseException.StatusCode },
                    {"Message",baseException.Message }
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = baseException.StatusCode;
                string newResponse = JsonSerializer.Serialize(body);
                await context.Response.WriteAsync(newResponse);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);//Delete
                Dictionary<string, object> body = new Dictionary<string, object>()
                {
                    {"StatusCode",500 },
                    {"Message",ex.Message }
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                string newResponse = JsonSerializer.Serialize(body);
                await context.Response.WriteAsync(newResponse);
            }
        }
    }
}
