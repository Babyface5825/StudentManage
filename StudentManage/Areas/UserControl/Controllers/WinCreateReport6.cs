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
    /// 根据各种选项生成计划人数和实际人数的统计
    /// </summary>
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class WinCreateReport6Controller : WinCreateReportController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "生源地统计表";
            return base.WindowCommonBeforeShow(PanelID, sWidth, sHeight);
        }

    }
}