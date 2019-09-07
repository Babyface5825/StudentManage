using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinKldmSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "科类代码详细信息";
            win.Resizable = false;
            SetWindowSize(win, 340, 250, sWidth, sHeight);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(Td_kldm mo, string PanelID, string ParentVariable)
        {
             GetParentVariable(ParentVariable);
            kldmTableDataListManage smpDlgMan = new kldmTableDataListManage();
            string[] EditColumn = {"Code", "Name", "Remark", "Jbkl", "Kscjbjxh" };
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}