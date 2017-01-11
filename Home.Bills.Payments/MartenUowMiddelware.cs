using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Http;

namespace Home.Bills.Payments
{
    public class MartenUowMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public MartenUowMiddelware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context, IDocumentSession documentSession)
        {
            await _next.Invoke(context);

            await documentSession.SaveChangesAsync();
        }
    }
}