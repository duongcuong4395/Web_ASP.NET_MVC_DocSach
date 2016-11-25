using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteDocSach
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*) BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "DKDN",
                url: "Dang-Ky-Dang-Nhap/{id}",
                defaults: new { controller = "NguoiDung", action = "chon", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SachStore", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
