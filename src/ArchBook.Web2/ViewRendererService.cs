using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArchBook.Web2
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2564
    /// </summary>
    public static class RazorViewToStringRendererExtensions
    {
        public static IServiceCollection AddRazorViewRenderer(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IViewRendererService, ViewRendererService>();
            return services;
        }
    }

    /// <summary>
    /// More info: http://www.dotnettips.info/post/2564
    /// </summary>
    public interface IViewRendererService
    {
        Task<string> RenderViewToStringAsync(string viewNameOrPath, Controller controller);
        Task<string> RenderViewToStringAsync<TModel>(string viewNameOrPath, TModel model, Controller controller);
    }

    /// <summary>
    /// Modified version of: https://github.com/aspnet/Entropy/blob/dev/samples/Mvc.RenderViewToString/RazorViewToStringRenderer.cs
    /// </summary>
    public class ViewRendererService : IViewRendererService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRendererService(
                    IRazorViewEngine viewEngine,
                    ITempDataProvider tempDataProvider,
                    IServiceProvider serviceProvider,
                    IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> RenderViewToStringAsync(string viewNameOrPath, Controller controller)
        {
            return RenderViewToStringAsync(viewNameOrPath, string.Empty, controller);
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewNameOrPath, TModel model, Controller controller)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewNameOrPath);

            using (var output = new StringWriter())
            {
                controller.ViewData.Model = model;
                //var viewDataDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                //{
                //    Model = model
                //};

                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    controller.ViewData,                    
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    output,
                    new HtmlHelperOptions());
                await view.RenderAsync(viewContext);
                return output.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
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

        private ActionContext GetActionContext()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                return new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
            }

            httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
