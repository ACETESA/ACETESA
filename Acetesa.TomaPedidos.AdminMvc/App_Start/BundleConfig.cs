using System.Web;
using System.Web.Optimization;

namespace Acetesa.TomaPedidos.AdminMvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.13.2.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/datatablejs").Include(
            //          //"~/Scripts/dataTables.select.min.js",
            //          "~/Scripts/dataTables.bootstrap5.min.js",
            //          "~/Scripts/jquery.dataTables.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/toastr.css",
                      "~/Content/app.css",
                      "~/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/ui").Include(
                      "~/Content/jquery-ui-1.11.4.css"));

            //bundles.Add(new StyleBundle("~/Content/datatablecss").Include(
            //          "~/Content/dataTables.bootstrap5.min.css",
            //          "~/Content/datatables.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
