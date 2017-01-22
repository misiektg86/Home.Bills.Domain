using System;
using System.Threading.Tasks;
using Frameworks.Light.Ddd;
using Microsoft.AspNetCore.Http;

namespace Home.Bills.Notifications
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

        public async Task Invoke(HttpContext context, IAsyncUnitOfWork unitOfWork)
        {
            await _next.Invoke(context);

            await unitOfWork.CommitAsync();
        }
    }
}