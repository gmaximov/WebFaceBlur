using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebFaceBlur
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AdressFormHandler",
                url: "AdressForm/Post",
                defaults: new { controller = "AdressForm", action = "Post" }
            );

            routes.MapRoute(
                name: "AdressForm",
                url: "AdressForm/GetForm",
                defaults: new { controller = "AdressForm", action = "GetForm" }
            );

            routes.MapRoute(
                name: "Image",
                url: "Image/{*path}",
                defaults: new { controller = "Image", action = "Get", path = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*path}",
                defaults: new { controller = "Default", action = "Index", path = UrlParameter.Optional }
            );
        }
    }
}
