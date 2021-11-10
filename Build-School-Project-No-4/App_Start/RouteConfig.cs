using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Build_School_Project_No_4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //    name: "MyDetail",
            //    url: "Detail/{id}",
            //    defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional }
            //);            


            routes.MapRoute(
                name: "MyRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ePals", action = "ePal", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
