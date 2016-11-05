using Microsoft.AspNetCore.Builder;

namespace Home.Bills
{
    public static class UnitOfWorkExtensions
    {
        public static IApplicationBuilder UseMartenUnitOfWork(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MartenUowMiddelware>();
        }
    }
}