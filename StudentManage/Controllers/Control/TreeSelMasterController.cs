using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net;
using StudentManage.Manage;
using StudentManage.Models;


namespace StudentManage.Controllers
{
    public class TreeSelMasterController : ParentController
    {
        // GET: Parent
        //public override void InitPage(string keyword = null)
        //{

        //}       

        public virtual ActionResult TreeNodeClick(string sTreeSelID, string sNowNames, string sNowValues, string sNowSigns)
        {
            return this.Direct();
        }


        /// <summary>
        /// 跨Action调用DataTree中的ClearDataTreeSelection
        /// </summary>
        /// <param name="PanelID"></param>
        /// <returns></returns>
        [DirectMethod]
        public ActionResult TreeSelMaster_ClearDataTreeSelection(string PanelID)
        {
            return RedirectToActionPermanent("ClearDataTreeSelection", "DataTree", new { area = "UserControl", PanelID });
        }

    }
}