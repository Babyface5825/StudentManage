using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinPcdmSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "批次代码详细信息";
            win.Resizable = false;
            SetWindowSize(win, 340, 250, sWidth, sHeight);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(Td_kldm mo, string PanelID, string ParentVariable)
        {
             GetParentVariable(ParentVariable);
            pcdmTableDataListManage smpDlgMan = new pcdmTableDataListManage();
            string[] EditColumn = {"Code", "Name", "Remark", "Gbpcdm", "Gbccdm" };
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}