using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinTddwSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "投档单位详细信息";
            win.Resizable = false;
            SetWindowSize(win, 300, 300, sWidth, sHeight);
            return this.Direct();
        }

        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            setParentVariable("UserControl", "WinTddwSimpleMore", "WinTddwSimpleMore_SelectPcdmChange", null, "pcdmTable");
            SetHiddenValue(PanelID + "_cmbPcdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinTddwSimpleMore", "WinTddwSimpleMore_SelectKldmChange", null, "kldmTable");
            SetHiddenValue(PanelID + "_cmbKldm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinTddwSimpleMore", "WinTddwSimpleMore_SelectJhxzdmChange", null, "jhxzdmTable");
            SetHiddenValue(PanelID + "_cmbJhxzdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinTddwSimpleMore", "WinTddwSimpleMore_SelectJhlbdmChange", null, "jhlbdmTable");
            SetHiddenValue(PanelID + "_cmbJhlbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinTddwSimpleMore", "WinTddwSimpleMore_SelectGxdmChange", null, "gxdmTable");
            SetHiddenValue(PanelID + "_cmbGxdm_ParentVariable", ParentVar.DefaultVal);
            GetParentVariable(ParentVariable);
            tddwTableDataListManage man = new tddwTableDataListManage();
            man.InitSelection(ParentVar.transValue);
            T_tddw model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            SetComboValue(PanelID + "_cmbGxdm", model.GxID.ToString(), true);
            SetComboValue(PanelID + "_cmbPcdm", model.Pcdm.ToString(), true);
            SetComboValue(PanelID + "_cmbKldm", model.Kldm.ToString(), true);
            SetComboValue(PanelID + "_cmbJhxzdm", model.Jhxzdm.ToString(), true);
            SetComboValue(PanelID + "_cmbJhlbdm", model.Jhlbdm.ToString(), true);
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);
            return this.Direct();
        }
        public ActionResult WinTddwSimpleMore_SelectGxdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelGxdm", sSelID);
            return this.Direct();
        }

        public ActionResult WinTddwSimpleMore_SelectPcdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelPcdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinTddwSimpleMore_SelectKldmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelKldm", sSelID);
            return this.Direct();
        }
        public ActionResult WinTddwSimpleMore_SelectJhxzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelJhxzdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinTddwSimpleMore_SelectJhlbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelJhlbdm", sSelID);
            return this.Direct();
        }


        public ActionResult WindowSaveClick(T_tddw mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            tddwTableDataListManage smpDlgMan = new tddwTableDataListManage();
            string[] EditColumn = { "Code", "Name", "Remark", "GxID" }; //"Pcdm", "Kldm", "Jhxzdm", "Jhlbdm", "Csmbh", "Tag" };
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}