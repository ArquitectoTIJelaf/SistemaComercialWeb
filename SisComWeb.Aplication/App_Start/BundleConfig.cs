using System.Web.Optimization;

namespace SisComWeb.Aplication
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new StyleBundle("~/bundles/css").Include(
            //        "~/wwwroot/statics/bootstrap/dist/css/bootstrap.min.css",
            //        "~/wwwroot/plugins/bower_components/toast-master/css/jquery.toast.css",
            //        "~/wwwroot/statics/css/animate.css",
            //        "~/wwwroot/statics/css/style.css",
            //        "~/wwwroot/statics/css/colors/blue-dark.css",
            //        "~/wwwroot/statics/css/custom-style.css",
            //        "~/wwwroot/plugins/bower_components/bootrstrap-datetimepicker/bootstrap-datetimepicker.min.css",
            //        "~/wwwroot/plugins/bower_components/sweetalert/sweetalert.css",
            //        "~/wwwroot/plugins/bower_components/animatedborder/css/animatedborder.css"
            //    ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                    "~/wwwroot/plugins/bower_components/jquery/dist/jquery.min.js",
                    "~/wwwroot/statics/bootstrap/dist/js/bootstrap.min.js",
                    "~/wwwroot/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.js",
                    "~/wwwroot/statics/js/jquery.slimscroll.js",
                    "~/wwwroot/statics/js/waves.js",
                    "~/wwwroot/plugins/bower_components/toast-master/js/jquery.toast.js",
                    "~/wwwroot/statics/js/toastr.js",
                    "~/wwwroot/statics/js/custom.min.js",
                    "~/wwwroot/plugins/bower_components/styleswitcher/jQuery.style.switcher.js",
                    "~/wwwroot/statics/js/cbpFWTabs.js",
                    "~/wwwroot/statics/vue/handlebars.min.js",
                    "~/wwwroot/statics/vue/vue.js",
                    "~/wwwroot/statics/vue/lodash.js",
                    "~/wwwroot/statics/vue/axios.min.js",
                    "~/wwwroot/statics/vue/vee-validate.min.js",
                    "~/wwwroot/statics/vue/lang.js",
                    "~/wwwroot/statics/vue/vue-select.js",
                    "~/wwwroot/statics/vue/vue-paginate.js",
                    "~/wwwroot/statics/vue/v-mask.min.js",
                    "~/wwwroot/statics/vue/base.js",
                    "~/wwwroot/plugins/bower_components/bootrstrap-datetimepicker/moment.js",
                    "~/wwwroot/plugins/bower_components/bootrstrap-datetimepicker/moment_2.10.6_locale_es.js",
                    "~/wwwroot/plugins/bower_components/bootrstrap-datetimepicker/bootstrap-datetimepicker.min.js",
                    //"~/wwwroot/plugins/bower_components/bootrstrap-datetimepicker/bootstrap-datetimepicker-4.7.14.min.js",
                    "~/wwwroot/plugins/bower_components/sweetalert/sweetalert2.all.min.js"
                ));
        }
    }
}