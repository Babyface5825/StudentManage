using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace StudentManage.Areas.adminMain
{
    public class adminMainAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "adminMain";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "adminMain_default",
                "adminmain/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional }
                //new { controller="MainPage", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}