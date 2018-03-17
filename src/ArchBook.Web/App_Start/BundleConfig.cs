using System.Web;
using System.Web.Optimization;

namespace ArchBook.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/popper-1.13.0.js",
                      "~/Scripts/bootstrap-4.0.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                      "~/Scripts/moment-2.20.1/moment.js",
                      "~/Scripts/moment-2.20.1/locales/fr-ca.js",
                      "~/Scripts/daterangepicker-2.1.27.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/Scripts/fontawesome-5.0.2/fontawesome.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome-icons").Include(
                      "~/Scripts/fontawesome-5.0.2/fontawesome-icons.js"));

            bundles.Add(new StyleBundle("~/content/daterangepicker").Include(
                      "~/Content/daterangepicker-2.1.27.css"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/Content/bootstrap-4.0.0.css",
                      "~/Content/fontawesome-5.0.2.css",
                      "~/Content/site.css"));
        }
    }
}
