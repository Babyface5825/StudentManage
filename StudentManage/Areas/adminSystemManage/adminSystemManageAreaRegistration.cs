using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace StudentManage.Areas.adminSystemManage
{
    public class adminSystemManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "adminSystemManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "adminSystemManage_default",
                "adminSystemManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}