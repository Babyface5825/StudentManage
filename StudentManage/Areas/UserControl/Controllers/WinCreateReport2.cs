using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System.Linq;

namespace StudentManage.Areas.UserControl.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class WinCreateReport2Controller : WinCreateReportController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "创建录取情况分析表";
            return base.WindowCommonBeforeShow(PanelID, sWidth, sHeight);
        }

    }
}