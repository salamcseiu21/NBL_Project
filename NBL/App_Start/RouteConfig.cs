﻿using LowercaseRoutesMVC;
using System.Web.Mvc;
using System.Web.Routing;
using NBL.Models.AutoMapper;

namespace NBL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "NBL.Controllers" }
            );
            AutoMapperConfiguration.Configure();
        }
    }
}
