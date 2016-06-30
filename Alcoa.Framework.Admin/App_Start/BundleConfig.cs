using System;
using System.Web;
using System.Web.Optimization;

namespace Alcoa.Framework.Admin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Removes *.min.js and *.min.css from ignore list in Debug mode
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/kendo/2014.1.318/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/2014.1.318/kendo.web.min.js",
                        "~/Scripts/kendo/2014.1.318/cultures/*.min.js",
                        "~/Scripts/kendoExcelGrid.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/alcoa-blue-1.0.0.css",
                        "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                        "~/Content/kendo/2014.1.318/kendo.common.min.css",
                        "~/Content/kendo/2014.1.318/kendo.silver.min.css"));
        }

        /// <summary>
        /// //Removes *.min.js and *.min.css from ignore list in Debug mode
        /// </summary>
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}