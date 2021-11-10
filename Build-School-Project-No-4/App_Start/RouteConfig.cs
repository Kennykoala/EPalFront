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

            //routes.MapRoute(
            //    name: "homepage",
            //    url: "home/homepage",
            //    defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "editProfile",
            //    url: "members/editprofile/{id}",
            //    defaults: new { controller = "Members", action = "EditProfile", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "profile",
            //    url: "Members/profile/{id}",
            //    defaults: new { controller = "Members", action = "profile", id = UrlParameter.Optional }
            //);



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
