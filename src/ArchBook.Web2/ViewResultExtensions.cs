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
        public static string Render(this ViewResult viewResult, HttpContext httpContext)
        {
            if (viewResult == null)
            {
                throw new ArgumentNullException(nameof(viewResult));
            }

            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
            var viewEngine = viewResult.ViewEngine ?? httpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
            var view = FindView(viewEngine, actionContext, viewResult.ViewName, true);

            return RenderToString(actionContext, view, viewResult.ViewData, viewResult.TempData, httpContext);
        }

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

            var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());            
            var viewEngine = partialViewResult.ViewEngine ?? httpContext.RequestServices.GetRequiredService<IRazorViewEngine>();
            var view = FindView(viewEngine, actionContext, partialViewResult.ViewName);

            return RenderToString(actionContext, view, partialViewResult.ViewData, partialViewResult.TempData, httpContext);            
        }

        private static string RenderToString(ActionContext actionContext, IView view, ViewDataDictionary viewData, ITempDataDictionary tempData, HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewData,
                    tempData,
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
    }
}
