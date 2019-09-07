using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentManage.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");            
            routes.IgnoreRoute("{exclude}/{extnet}/ext.axd");//important, or the whole frame can not work correctly.
            routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");
            routes.IgnoreRoute("default.htm");

            routes.MapRoute(
                name: "Default",
                url:  "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "StudentManage.Areas.adminMain" } //默认控制器的命名空间
                ).DataTokens.Add("area", "adminMain"); //默认area 的控制器名称
        }
    }
}
