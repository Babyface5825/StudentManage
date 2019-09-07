using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinReportSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "报表详细信息";
            win.Resizable = false;
            SetWindowSize(win, 300, 180, sWidth, sHeight);
            return this.Direct();
        }
        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            ReportInfoDataListManage man = new ReportInfoDataListManage();
            man.InitSelection(ParentVar.transValue);
            tblReportInfo model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);           
            return this.Direct();
        }

        public ActionResult WindowSaveClick(tblReportInfo mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            ReportInfoDataListManage smpDlgMan = new ReportInfoDataListManage();
            string[] EditColumn = {"Name", "Remark" };
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}