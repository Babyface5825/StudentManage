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
    public class WinCreateReport5Controller : WinCreateReportController
    {        

        public WinCreateReport5Controller()
        {
            MAXITEMS = 2;
        }

        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "志愿录取情况表";
            return base.WindowCommonBeforeShow(PanelID, sWidth, sHeight);
        }

        public override ActionResult WinCreateReport_SelectItems(string sOldSelIDs, string sNowSelIDs, string PanelID, string ParentVariable)
        {
            if (!sNowSelIDs.Contains("Department"))
            {
                X.Msg.Alert("注意", "院系必须选择！").Show();
                return this.Direct();
            }
            else
                return base.WinCreateReport_SelectItems(sOldSelIDs, sNowSelIDs, PanelID, ParentVariable);            
        }

    }
}