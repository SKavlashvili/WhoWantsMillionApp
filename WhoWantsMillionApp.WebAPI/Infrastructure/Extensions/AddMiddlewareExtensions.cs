using WhoWantsMillionApp.WebAPI.Infrastructure.Middlewares;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Extensions
{
    public static class AddMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalErrorHandlerMiddleware>();
        }
    }
}
