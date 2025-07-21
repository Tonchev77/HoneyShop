namespace HoneyShop.Web.Infrastructure.Extensions
{
    using HoneyShop.Web.Infrastructure.Middlewares;
    using Microsoft.AspNetCore.Builder;
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseManagerAccessRestriction(this IApplicationBuilder app)
        {
            app.UseMiddleware<ManagerAccessRestrictionMiddleware>();

            return app;
        }
    }
}
