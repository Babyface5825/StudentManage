using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinJHKMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "计划库详细信息";
            win.Resizable = false;
            SetWindowSize(win, 380, 250, sWidth, sHeight);
            return this.Direct();
        }


        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            setParentVariable("UserControl", "WinJHKMore", "WinJHKMore_SelectTddwdmChange", null, "tddwTable");
            SetHiddenValue(PanelID + "_cmbTddwdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinJHKMore", "WinJHKMore_SelectZydhChange", null, "zydhTable");
            SetHiddenValue(PanelID + "_cmbZydh_ParentVariable", ParentVar.DefaultVal);

            GetParentVariable(ParentVariable);
            jhkTableDataListManage man = new jhkTableDataListManage();
            man.InitSelection(ParentVar.transValue);
            T_jhk model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);

            if (model.Tddwdm == 0)
                SetComboValue(PanelID + "_cmbTddwdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbTddwdm", model.Tddwdm.ToString(), true);

            if (model.Zydh == 0)
                SetComboValue(PanelID + "_cmbZydh", "0", true);
            else
                SetComboValue(PanelID + "_cmbZydh", model.Zydh.ToString(), true);


            return this.Direct();
        }
        
        public ActionResult WindowSaveClick(tblSimpleInfo mo, string PanelID, string ParentVariable)
        {
            //GetParentVariable(ParentVariable);
            //string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
            //SimpleDlgDataListManage smpDlgMan = (SimpleDlgDataListManage)GetDataListManage(ss[0], ss[1]);
            //tblSimpleInfo transModel = (tblSimpleInfo)smpDlgMan.GetSpecialModelFromCurrentModel(mo);
            //long nNewID = smpDlgMan.SaveOneSimpleRecord(transModel);
            //SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }


        #region ItemChange
        public ActionResult WinJHKMore_SelectTddwdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelTddwdm", sSelID);
            return this.Direct();
        }

        public ActionResult WinJHKMore_SelectZydhChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelZydh", sSelID);
            return this.Direct();
        }
        #endregion

    }
}