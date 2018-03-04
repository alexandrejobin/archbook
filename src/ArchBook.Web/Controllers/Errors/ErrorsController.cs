using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArchBook.Web.Controllers.Errors
{
    [RoutePrefix("Errors")]
    public class ErrorsController : Controller
    {
        [Route("HttpError404")]
        public ActionResult HttpError404()
        {
            return View();
        }
    }

    //    [Route("httperror/{httpStatusCode}")]
    //    public ActionResult HttpError(HttpStatusCode? httpStatusCode)
    //    {

    //        Exception exception = null;

    //        if (HttpContext.Session != null)
    //        {
    //            exception = HttpContext.Session["exception"] as Exception;
    //            HttpContext.Session["exception"] = null;
    //        }

    //        if (exception == null)
    //        {
    //            exception = Server.GetLastError();
    //        }

    //        if (httpStatusCode == null)
    //        {
    //            httpStatusCode = HttpStatusCode.InternalServerError;
    //        }

    //        switch (httpStatusCode)
    //        {
    //            case HttpStatusCode.Forbidden:
    //                return HttpError403();
    //            case HttpStatusCode.NotFound:
    //                return HttpError404();
    //            default:
    //                return HttpError500();
    //        }
    //    }

    //    [Route("httperror403")]
    //    public ActionResult HttpError403()
    //    {
    //        AllErrorsCore(HttpStatusCode.Forbidden);

    //        return View("HttpError403");
    //    }

    //    [Route("httperror404")]
    //    public ActionResult HttpError404()
    //    {
    //        AllErrorsCore(HttpStatusCode.NotFound);

    //        return View("HttpError404");
    //    }

    //    /// <summary>
    //    /// Sert à un catch_all route défini dans le fichier RouteConfig.cs
    //    /// </summary>
    //    /// <returns></returns>
    //    public ActionResult HttpError404_2()
    //    {
    //        // bizarrement, si on essaye de définir un catchall route dans le fichier RouteConfig.cs qui pointe directement sur la
    //        // fonction HttpError404, ça ne marche pas à cause qu'il est décoré par RouteAttribute. On doit donc avec 2 versions de
    //        // cette fonction.
    //        return HttpError404();
    //    }

    //    [Route("httperror500")]
    //    public ActionResult HttpError500()
    //    {
    //        AllErrorsCore(HttpStatusCode.InternalServerError);

    //        return View("HttpError500");
    //    }

    //    private void AllErrorsCore(HttpStatusCode httpStatusCode)
    //    {
    //        Response.ClearHeaders();
    //        Response.ClearContent();
    //        Response.StatusCode = (int)httpStatusCode;

    //        Response.TrySkipIisCustomErrors = true;
    //    }

    //    //public ActionResult Http404(string url)
    //    //{
    //    //    Response.StatusCode = (int)HttpStatusCode.NotFound;
    //    //    var model = new NotFoundViewModel();
    //    //    // If the url is relative ('NotFound' route) then replace with Requested path
    //    //    model.RequestedUrl = Request.Url.OriginalString.Contains(url) & Request.Url.OriginalString != url ?
    //    //        Request.Url.OriginalString : url;
    //    //    // Dont get the user stuck in a 'retry loop' by
    //    //    // allowing the Referrer to be the same as the Request
    //    //    model.ReferrerUrl = Request.UrlReferrer != null &&
    //    //        Request.UrlReferrer.OriginalString != model.RequestedUrl ?
    //    //        Request.UrlReferrer.OriginalString : null;

    //    //    // TODO: insert ILogger here

    //    //    return View("NotFound", model);
    //    //}
    //    //public class NotFoundViewModel
    //    //{
    //    //    public string RequestedUrl { get; set; }
    //    //    public string ReferrerUrl { get; set; }
    //    //}

    //    //public ActionResult NotFound()
    //    //{
    //    //    Response.StatusCode = (int)HttpStatusCode.NotFound;

    //    //    // Avoid IIS7 getting in the middle
    //    //    Response.TrySkipIisCustomErrors = true;

    //    //    object model = Request.Url.PathAndQuery;

    //    //    if (!Request.IsAjaxRequest())
    //    //        return View(model);
    //    //    else
    //    //        return PartialView("_NotFound", model);
    //    //}
    //}
}