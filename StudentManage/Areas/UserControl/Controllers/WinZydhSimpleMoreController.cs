using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinZydhSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "专业代号详细信息";
            win.Resizable = false;
            SetWindowSize(win, 280, 305, sWidth, sHeight);
            return this.Direct();
        }
        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            setParentVariable("UserControl", "WinZydhSimpleMore", "WinZydhSimpleMore_SelectKldmChange", null, "kldmTable");
            SetHiddenValue(PanelID + "_cmbKldm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinZydhSimpleMore", "WinZydhSimpleMore_SelectZylbdmChange", null, "zylbdmTable");
            SetHiddenValue(PanelID + "_cmbZylbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinZydhSimpleMore", "WinZydhSimpleMore_SelectXzdmChange", null, "xzdmTable");
            SetHiddenValue(PanelID + "_cmbXzdm_ParentVariable", ParentVar.DefaultVal);

            GetParentVariable(ParentVariable);
            zydhTableDataListManage man = new zydhTableDataListManage();
            man.InitSelection(ParentVar.transValue);
            Td_zydh model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            SetComboValue(PanelID + "_cmbKldm", model.Kldm.ToString(), true);
            SetComboValue(PanelID + "_cmbZylbdm", model.Zylbdm.ToString(), true);
            SetComboValue(PanelID + "_cmbXzdm", model.Xzdm.ToString(), true);

            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);
            return this.Direct();
        }
        public ActionResult WinZydhSimpleMore_SelectKldmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelKldm", sSelID);
            return this.Direct();
        }
        public ActionResult WinZydhSimpleMore_SelectZylbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelZylbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinZydhSimpleMore_SelectXzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelXzdm", sSelID);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(Td_kldm mo, string PanelID, string ParentVariable)
        {
             GetParentVariable(ParentVariable);
            pcdmTableDataListManage smpDlgMan = new pcdmTableDataListManage();
            string[] EditColumn = {"Code", "Name", "Remark", "Kldm", "Zylbdm", "Xzdm", "TheOrder" };
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}