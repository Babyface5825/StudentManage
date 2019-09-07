using Ext.Net;
using Ext.Net.MVC;
using StudentManage.Controllers;
using StudentManage.Models;
using System.Web.Mvc;

namespace StudentManage.Areas.adminSystemManage.Controllers
{
    [DirectController(AreaName = "adminSystemManage")]
    public class DepartmentInfoController : TreeMasterController
    {
        public DepartmentInfoController()
        {
            sAreaName = "adminSystemManage";
            sPageKeyWord = "DepartmentInfo";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_Form";
            sDataTreeKey = sPageKeyWord;
        }

        public override void InitPage()
        {
            var btn = this.GetCmp<Button>(sMainFormName + "_btnMore");
            btn.Text = "更多内容";
            btn.Hidden = false;
            base.InitPage();
        }

        public override ActionResult TreeMaster_MoreInfoClick(tblTreeInfo model)
        {
            if (model.ID == 0)
            {
                X.Msg.Alert("注意", "请先选中要操作的节点！").Show();
                return this.Direct();
            }

            setParentVariable(null, null, null, model.ID.ToString(), null);
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinDepartmentMore.cshtml";
            pvr.ViewData["CurrentController"] = "WinDepartmentMore";
            pvr.ViewData["WindowID"] = "DepartmentInfo_WinMore";
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }


    }
}