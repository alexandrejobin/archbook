using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchBook.Web2
{
    public static class ViewResultExtensions
    {
        public static string Render(this PartialViewResult partialViewResult, HttpContext httpContext)
        {
            if (partialViewResult == null)
            {
                throw new ArgumentNullException(nameof(partialViewResult));
            }

            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var executor = httpContext.RequestServices.GetRequiredService<PartialViewResultExecutor>();
            

            var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            //var viewEngine = partialViewResult.ViewEngine ?? httpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
            //var view = FindView(viewEngine, actionContext, partialViewResult.ViewName);
            var view = executor.FindView(actionContext, partialViewResult).View;
          

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    partialViewResult.ViewData,
                    partialViewResult.TempData,
                    output,
                    htmlHelperOptions);

                view
                    .RenderAsync(viewContext)
                    .GetAwaiter()
                    .GetResult();

                return output.ToString();
            }
        }

        private static IView FindView(IViewEngine viewEngine, ActionContext actionContext, string viewName, bool isMainPage = false)
        {
            var getViewResult = viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: isMainPage);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = viewEngine.FindView(actionContext, viewName, isMainPage: isMainPage);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }

        //public static string ToHtml(this ViewResult result, HttpContext httpContext)
        //{
        //    var feature = httpContext.Features.Get<IRoutingFeature>();
        //    var routeData = feature.RouteData;
        //    var viewName = result.ViewName ?? routeData.Values["action"] as string;
        //    var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
        //    var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
        //    var htmlHelperOptions = options.Value.HtmlHelperOptions;
        //    var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value.ViewEngines.Select(x => x.FindView(actionContext, viewName, true)).FirstOrDefault(x => x != null);
        //    var view = viewEngineResult.View;
        //    var builder = new StringBuilder();

        //    using (var output = new StringWriter(builder))
        //    {
        //        var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output, htmlHelperOptions);

        //        view
        //            .RenderAsync(viewContext)
        //            .GetAwaiter()
        //            .GetResult();
        //    }

        //    return builder.ToString();
        //}
    }
}
