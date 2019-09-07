using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace StudentManage.Areas.adminEnrollmentDataManage
{
    public class adminEnrollmentDataManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "adminEnrollmentDataManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "adminEnrollmentDataManage_default",
                "adminEnrollmentDataManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}