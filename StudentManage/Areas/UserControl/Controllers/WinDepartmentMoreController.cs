using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinDepartmentMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "单位部门详细信息";            
            SetWindowSize(win, 340, 245, sWidth, sHeight);
            return this.Direct();
        }


        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            DepartmentInfoManage man = new DepartmentInfoManage();
            tblDepartmentInfo model = man.GetSpecialModelFromCurrentID(ParentVar.transValue);
            if (model.ID != 0)
            {
                setParentVariable("UserControl", "WinDepartmentMore", "WinDepartmentMore_SelectGxdmChange", null, "gxdmTable");
                SetHiddenValue(PanelID + "_cmbGxdm_ParentVariable", ParentVar.DefaultVal);
                tblDepartmentInfo parentModel = man.GetFirstParent(model.ID);
                SetComboValue(PanelID + "_cmbGxdm", parentModel.GxID.ToString(),true);
                if (model.ParentIndex != -1)
                {
                    var panel = this.GetCmp<Panel>(PanelID + "_comboPanel");
                    panel.Disable();
                }
                var form = this.GetCmp<FormPanel>(PanelID + "_Form");
                form.SetValues(model);
            }
            return this.Direct();
        }

        public ActionResult WinDepartmentMore_SelectGxdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelGxdm", sSelID);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(tblDepartmentInfo mo, string PanelID, string ParentVariable)
        {
            string[] EditColumns = { "DepartmentIADD", "DepartmentIPostalCode", "DepartmentIPhone", "DepartmentIFax", "DepartmentIeMail", "GxID", "ZYIDs" };
            DepartmentInfoManage man = new DepartmentInfoManage();
            man.SaveOneRecord(mo, EditColumns);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

        public  ActionResult WinDepartmentMore_OpenAddZYWindow(string PanelID, string ID, string ZYIDs, string GxID)
        {
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinDepartmentAddZY.cshtml";
            pvr.ViewData["WindowID"] = PanelID + "_WinDepartmentAddZY";
            setParentVariable(null, null, "MoreWindow_ConfirmSave", ZYIDs, ID);
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["WindowParentKey"] = GxID;
            pvr.ViewData["CurrentController"] = "WinDepartmentAddZY";
            return pvr;
        }

        public override ActionResult MoreWindow_ConfirmSave(string PanelID = null, string transValue = null)
        {
            string[] ss = transValue.Split(CosValue.SPLITOPERATOR3);
            SetHiddenValue(PanelID.Remove(PanelID.LastIndexOf('_')) + "_ZYIDs", ss[0]);
            SetTextField(PanelID.Remove(PanelID.LastIndexOf('_')) + "_ZYNames", ss[1]);
            return this.Direct();
        }

    }
}