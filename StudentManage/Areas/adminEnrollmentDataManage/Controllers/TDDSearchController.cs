using Ext.Net.MVC;
using StudentManage.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManage.Areas.adminEnrollmentDataManage.Controllers
{
    [DirectController(AreaName="adminEnrollmentDataManage")]
    public class TDDSearchController : TreeSelMasterController
    {
        // GET: adminEnrollmentDataManage/TDDSearch
        public TDDSearchController()
        {
            sAreaName = "adminEnrollmentDataManage";
            sPageKeyWord = "TDDSearch";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_SearchForm";
            sDataTreeKey = "dqdmTable";
        }

        public override ActionResult TreeNodeClick(string sTreeSelID, string sNowNames, string sNowValues, string sNowSigns)
        {
            SetHiddenValue(sPageKeyWord + "_SearchDepartmentID", sTreeSelID);
            return this.Direct();
        }

        public override void InitPage()
        {
            SetSearchPanelHeight(60);
            base.InitPage();
        }

    }
}