using System.Threading.Tasks;
using CoreBase.Persistance;
using Microsoft.AspNetCore.Http;

namespace CoreBase.Helpers
{
    public class DbContextMiddleware
    {
        private readonly RequestDelegate _next;

        public DbContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, DatabaseContext BaseContext)
        {
            var method = httpContext.Request.Method;
            if (method == "PUT" || method == "POST" || method == "DELETE")
            {
                using(var transaction = BaseContext.Database.BeginTransaction())
                {
                    await _next(httpContext);
                    transaction.Commit();
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

    }

}
