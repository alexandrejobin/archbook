using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchBook.Web2
{
    public class UserNameEnricherMiddleware
    {
        private const string UserNamePropertyName = "UserName";
        readonly RequestDelegate _next;

        public UserNameEnricherMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var userName = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "(anonymous)";

            using (LogContext.PushProperty(UserNamePropertyName, userName))
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class EnricherExtensions
    {
        public static IApplicationBuilder UseSerilogUserNameEnricher(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<UserNameEnricherMiddleware>();
        }
    }
}
