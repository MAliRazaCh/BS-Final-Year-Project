﻿using System.Web;
using System.Web.Optimization;

namespace FypNew
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.pjax.js",
                
                "~/Content/lib1/widgster/widgster.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"
                        ));
            bundles.Add(new StyleBundle("~/AllJS").Include(
                        "~/assets/js/jquery.backstretch.js",
                        "~/assets/js/scripts.js"
                        ));
            bundles.Add(new StyleBundle("~/MyStyles").Include(
                        "~/assets/bootstrap/css/bootstrap.css",
                        "~/assets/font-awesome/css/font-awesome.css",
                        "~/assets/css/form-elements.css",
                        "~/assets/css/style.css",
                        "~/Content/Site.css"
                        ));
            bundles.Add(new ScriptBundle("~/AllScripts").Include(
                        "~/Scripts/js1/settings.js",
                        "~/Content/lib1/underscore/underscore.js"
                        , "~/Scripts/js1/app.js"
                        ));
        }
    }
}