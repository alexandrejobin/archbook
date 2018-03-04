using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ArchBook.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            logger.Fatal(context.Exception);
        }
    }
}
