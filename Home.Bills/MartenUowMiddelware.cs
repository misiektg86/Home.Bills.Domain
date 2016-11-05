using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Home.Bills
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