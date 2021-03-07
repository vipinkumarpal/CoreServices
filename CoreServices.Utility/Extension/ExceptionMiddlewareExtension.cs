using CoreServices.Utility.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CoreServices.Utility.Extension
{
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
