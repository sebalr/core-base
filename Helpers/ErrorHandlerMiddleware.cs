using System;
using System.Net;
using System.Threading.Tasks;
using Carter.Response;
using Microsoft.AspNetCore.Http;

namespace CoreBase.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                //TODO: Log exception stacktrace
                var httpEx = new HttpException(HttpStatusCode.InternalServerError, ex.Message);
                await HandleExceptionAsync(httpContext, httpEx);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpException exception)
        {
            context.Response.StatusCode = (int) exception.StatusCode;

            return context.Response.AsJson(new { message = exception.Message });
        }
    }
}
