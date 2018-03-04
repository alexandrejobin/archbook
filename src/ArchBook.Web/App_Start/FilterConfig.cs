using NLog;
using System.Web;
using System.Web.Mvc;

namespace ArchBook.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new NLogExceptionFilter());
        }

        public class NLogExceptionFilter : IExceptionFilter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            public void OnException(ExceptionContext context)
            {
                if (context.ExceptionHandled)
                {
                    logger.Fatal(context.Exception);
                }
            }
        }
    }
}
