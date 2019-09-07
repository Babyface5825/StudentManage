using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinOriginDataInportSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "数据导入详细信息";
            win.Resizable = false;
            SetWindowSize(win, 300, 100, sWidth, sHeight);
            return this.Direct();
        }
        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            //ReportInfoDataListManage man = new ReportInfoDataListManage();
            OriginDataPackageDataListManage man = new OriginDataPackageDataListManage();
            man.InitSelection(ParentVar.transValue);
            tblOriginDataPackageInfo model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);           
            return this.Direct();
        }

        public ActionResult WindowSaveClick(tblOriginDataPackageInfo mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            OriginDataPackageDataListManage smpDlgMan = new OriginDataPackageDataListManage();
            string[] EditColumn = {"OriginName"};
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}