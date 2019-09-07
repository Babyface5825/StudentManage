using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace StudentManage.Areas.adminFoundDataManage
{
    public class adminFoundDataManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "adminFoundDataManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "adminFoundDataManage_default",
                "adminFoundDataManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}