using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace StudentManage.Areas.UserControl
{
    public class UserControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserControl_default",
                "UserControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}