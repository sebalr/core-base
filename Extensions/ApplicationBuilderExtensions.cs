using CoreBase.Helpers;
using Microsoft.AspNetCore.Builder;

namespace CoreBase.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDbContextMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbContextMiddleware>();
        }

        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }

    }
}
