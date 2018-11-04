using System.Web;
using System.Web.Optimization;

namespace EBFP.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));



            //<!-- Bootstrap Core CSS -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/bootstrap/dist/css/bootstrap.min.css")" rel="stylesheet">
            //<!-- Bootstrap Core CSS -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/bootstrap/dist/css/bootstrap.min.css")" rel="stylesheet">
            //<!-- Menu CSS -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.css")" rel="stylesheet">
            //<!-- Animation CSS -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/css/animate.css")" rel="stylesheet">
            //<!-- Custom CSS -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/css/style.css")" rel="stylesheet">
            //<!-- color CSS you can use different color css from css/colors folder -->
            //<!-- We have chosen the skin-blue (blue.css) for this starter page. However, you can choose any other skin from folder css / colors -->
            //<link href="@Url.Content("~/content/bootstrap/eliteadmin/css/colors/blue-dark.css")" id="theme" rel="stylesheet">
            //<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
            //<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->  
            //<link href="@Url.Content("~/content/Site.css")" rel="stylesheet">
            //<!-- Bootstrap Core JavaScript -->
            bundles.Add(new StyleBundle("~/Content/eliteadmin/css").Include(
                      "~/content/bootstrap/eliteadmin/bootstrap/dist/css/bootstrap.min.css",
                      "~/content/bootstrap/eliteadmin/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.css",
                      "~/content/bootstrap/eliteadmin/css/animate.css",
                      "~/content/bootstrap/eliteadmin/css/style.css",
                      "~/content/bootstrap/eliteadmin/css/colors/blue-dark.css",
                      "~/content/Site.css"));
        }
    }
}
