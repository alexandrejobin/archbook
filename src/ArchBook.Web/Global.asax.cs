using ArchBook.Web.Controllers.Errors;
using Autofac;
using Autofac.Integration.Mvc;
using MTO.Framework.Web.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ArchBook.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static bool _knowTrustLevel = false;
        private static AspNetHostingPermissionLevel _trustLevel;

        protected void Application_Start()
        {
            logger.Info("Application is starting.");

            // take care of unhandled exceptions - there is nothing we can do to
            // prevent the entire w3wp process to go down but at least we can try
            // and log the exception
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                var exception = (Exception)args.ExceptionObject;
                var isTerminating = args.IsTerminating; // always true?

                var msg = "Unhandled exception in AppDomain";

                if (isTerminating)
                {
                    msg += " (terminating)";
                }

                logger.Error(exception, msg);
            };
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MtoRazorViewEngine());

            ServiceLocatorConfig.RegisterServiceLocator();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Application is started.");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // set a unique id to the current request
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }

        //protected void Application_EndRequest()
        //{

        //    if (Context.Response.StatusCode == 404)
        //    {
        //        Response.Clear();

        //        var rd = new RouteData();
        //        rd.DataTokens["area"] = "AreaName"; // In case controller is in another area
        //        rd.Values["controller"] = "Errors";
        //        rd.Values["action"] = "NotFound";

        //        IController c = new ErrorsController();
        //        c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
        //    }
        //}

        protected void Application_End()
        {
            var shutdownReason = GetShutdownReason();
            logger.Info($"Application shutdown. Reason: {shutdownReason}.");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "errors");
            routeData.Values.Add("action", "HttpError404");

            var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            var controller = controllerFactory.CreateController(requestContext, "errors");

            try
            {
                controller.Execute(requestContext);
            }
            catch (Exception ex)
            {
            }

            //if (!HttpContext.Current.IsCustomErrorEnabled)
            //{
            //    // Don't handle the error in this case.
            //    return;
            //}

            //var errorControllerName = "errors";
            //var exception = HttpContext.Current.Server.GetLastError();

            //if (exception is HttpUnhandledException)
            //{
            //    exception = exception.InnerException ?? exception;
            //}

            //var httpException = exception as HttpException;

            //var statusCode = httpException != null
            //    ? httpException.GetHttpCode()
            //    : (int)HttpStatusCode.InternalServerError;

            //if (HttpContext.Current.Request.Path.StartsWith("/" + errorControllerName + "/", StringComparison.OrdinalIgnoreCase))
            //{
            //    HandleErrorHandlingError("The error handling code threw an exception.", HttpContext.Current.Server.GetLastError());

            //    return;
            //}

            ////var errorRoute = "~/" + errorControllerName + "/httperror/" + statusCode;

            //try
            //{
            //    Response.Clear();
            //    var routeData = new RouteData();

            //    routeData.Values.Add("controller", errorControllerName);

            //    switch (statusCode)
            //    {
            //        case 403:
            //            routeData.Values.Add("action", "HttpError403");
            //            break;
            //        case 404:
            //            routeData.Values.Add("action", "HttpError404");
            //            break;
            //        case 500:
            //            routeData.Values.Add("action", "HttpError500");
            //            break;
            //        default:
            //            routeData.Values.Add("action", "HttpError500");
            //            routeData.Values.Add("httpStatusCode", statusCode);
            //            break;
            //    }

            //    routeData.Values.Add("error", exception);

            //    var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
            //    var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            //    var controller = controllerFactory.CreateController(requestContext, errorControllerName);

            //    controller.Execute(requestContext);
            //    HttpContext.Current.Server.ClearError();

            //    //HttpContext.Current.Server.TransferRequest(errorRoute, false, "GET", null);
            //}
            //catch (Exception ex)
            //{
            //    // Log the error here in some way.
            //    TransferToFatalErrorPage();
            //}
        }

        private void TransferToFatalErrorPage()
        {
            Response.ClearHeaders();
            Response.ClearContent();
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Server.ClearError();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            Server.Transfer(urlHelper.Content("~/fatalerrorpage.htm"));
        }

        private static void HandleErrorHandlingError(string reason, Exception exception = null)
        {
            // In this situation, you would most likely log the error then display a static html error page.
            // I'm just writing directly to the response so that I can show the exception details.

            var response = HttpContext.Current.Response;

            response.ClearHeaders();
            response.ClearContent();
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.TrySkipIisCustomErrors = true;
            HttpContext.Current.Server.ClearError();
            response.Write("YOWZAH!! An unhandled error occurred within Application_Error: " + reason ?? string.Empty);
            response.Write(Environment.NewLine);

            if (exception != null)
            {
                response.Write(exception.Message);
                response.Write(exception.StackTrace);
            }

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static string GetShutdownReason()
        {
            if (GetCurrentTrustLevel() == AspNetHostingPermissionLevel.Unrestricted)
            {
                switch (HostingEnvironment.ShutdownReason)
                {
                    case ApplicationShutdownReason.BinDirChangeOrDirectoryRename:
                        return "A change was made to the bin directory or the directory was renamed";
                    case ApplicationShutdownReason.BrowsersDirChangeOrDirectoryRename:
                        return "A change was made to the App_browsers folder or the files contained in it";
                    case ApplicationShutdownReason.ChangeInGlobalAsax:
                        return "A change was made in the global.asax file";
                    case ApplicationShutdownReason.ChangeInSecurityPolicyFile:
                        return "A change was made in the code access security policy file";
                    case ApplicationShutdownReason.CodeDirChangeOrDirectoryRename:
                        return "A change was made in the App_Code folder or the files contained in it";
                    case ApplicationShutdownReason.ConfigurationChange:
                        return "A change was made to the application level configuration";
                    case ApplicationShutdownReason.HostingEnvironment:
                        return "The hosting environment shut down the application";
                    case ApplicationShutdownReason.HttpRuntimeClose:
                        return "A call to Close() was requested";
                    case ApplicationShutdownReason.IdleTimeout:
                        return "The idle time limit was reached";
                    case ApplicationShutdownReason.InitializationError:
                        return "An error in the initialization of the AppDomain";
                    case ApplicationShutdownReason.MaxRecompilationsReached:
                        return "The maximum number of dynamic recompiles of a resource limit was reached";
                    case ApplicationShutdownReason.PhysicalApplicationPathChanged:
                        return "A change was made to the physical path to the application";
                    case ApplicationShutdownReason.ResourcesDirChangeOrDirectoryRename:
                        return "A change was made to the App_GlobalResources foldr or the files contained within it";
                    case ApplicationShutdownReason.UnloadAppDomainCalled:
                        return "A call to UnloadAppDomain() was completed";
                    default:
                        return HostingEnvironment.ShutdownReason.ToString();
                }
            }
            else
            {
                return "Unknown shutdown reason";
            }
        }

        /// <summary>
		/// Get the current trust level of the hosted application
		/// </summary>
		/// <returns></returns>
		protected static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            if (_knowTrustLevel)
            {
                return _trustLevel;
            }

            foreach (var trustLevel in new[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (SecurityException)
                {
                    continue;
                }

                _trustLevel = trustLevel;
                _knowTrustLevel = true;

                return _trustLevel;
            }

            _trustLevel = AspNetHostingPermissionLevel.None;
            _knowTrustLevel = true;

            return _trustLevel;
        }
    }
}
