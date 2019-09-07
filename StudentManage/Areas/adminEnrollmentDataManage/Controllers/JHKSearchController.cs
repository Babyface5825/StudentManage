using System.Web.Mvc;
using StudentManage.Controllers;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;

namespace StudentManage.Areas.adminEnrollmentDataManage.Controllers
{
    [DirectController(AreaName = "adminEnrollmentDataManage")]
    public class JHKSearchController : ParentController
    {
        
        public JHKSearchController()
        {
            sAreaName = "adminEnrollmentDataManage";
            sPageKeyWord = "JHKSearch";
            sDataListName = sPageKeyWord + "_DataList";
            sMainFormName = sPageKeyWord + "_SearchForm";            
        }
        
        public override void InitPage()
        {            
            SetSearchPanelHeight(110);
            setParentVariable(null, null, "JHKSearch_SelectPcdmChange", null, "pcdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbPcdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbPcdm", "0", true);

            setParentVariable(null, null, "JHKSearch_SelectKldmChange", null, "kldmTable");
            SetHiddenValue(sPageKeyWord + "_cmbKldm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbKldm", "0", true);

            setParentVariable(null, null, "JHKSearch_SelectJhxzdmChange", null, "jhxzdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbJhxzdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbJhxzdm", "0", true);

            setParentVariable(null, null, "JHKSearch_SelectJhlbdmChange", null, "jhlbdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbJhlbdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbJhlbdm", "0", true);
            
            setParentVariable(null, null, "JHKSearch_SelectDqdmChange", null, "dqdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbDqdm_ParentVariable", ParentVar.DefaultVal);
            SetHiddenValue(sPageKeyWord + "_cmbDqdm_NowNames", "ParentIndex");
            SetHiddenValue(sPageKeyWord + "_cmbDqdm_NowValues", "-1");
            SetHiddenValue(sPageKeyWord + "_cmbDqdm_NowSigns", SIGN.Equal.ToString());
            SetComboValue(sPageKeyWord + "_cmbDqdm", "0", true);

            setParentVariable(null, null, "JHKSearch_SelectXzdmChange", null, "xzdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbXzdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbXzdm", "0", true);


            T_jhk model = new T_jhk();
            var form = this.GetCmp<FormPanel>(sMainFormName);
            form.SetValues(model);

            SetInitDataAuthor();

            base.InitPage();
        }

        #region ItemChange
        public ActionResult JHKSearch_SelectPcdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchPcdm", sSelID);
            return this.Direct();
        }

        public ActionResult JHKSearch_SelectKldmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchKldm", sSelID);
            return this.Direct();
        }
        public ActionResult JHKSearch_SelectJhxzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchJhxzdm", sSelID);
            return this.Direct();
        }
        public ActionResult JHKSearch_SelectJhlbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchJhlbdm", sSelID);
            return this.Direct();
        }

        public ActionResult JHKSearch_SelectXzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchXzdm", sSelID);
            return this.Direct();
        }

        public ActionResult JHKSearch_SelectDqdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_ProvinceID", sSelID);
            return this.Direct();
        }
        #endregion

        #region DataList按钮事件相关
        //[DirectMethod]
        //public ActionResult JHKSearch_AddClickDirectMethod(string sData)
        //{
        //    tblJHKSearch model = JSON.Deserialize<tblJHKSearch>(sData);
        //    if ((model.UserIDepartmentIID == 0) || (model.UserIDepartmentIID == -1) || (model.UserIDepartmentIID == null))
        //    {
        //        X.Msg.Alert("注意", "请先选择部门！").Show();
        //        return this.Direct();
        //    }
        //    return OpenMoreWindow("0", null, model.UserIDepartmentIID.ToString());
        //}

        protected override ActionResult OpenMoreWindow(string selection, string keywords, string transvalue)
        {
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinJHKMore.cshtml";
            pvr.ViewData["WindowID"] = "BMKSearch_WinJHKMore";
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, null);
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinJHKMore";
            return pvr;
        }

        //[DirectMethod]
        //public ActionResult JHKSearch_ClearDutyCombo()
        //{
        //    return RedirectToActionPermanent("SetDataCombo", "DataComboBox", new { area = "UserControl", PanelID = "JHKSearch_cmbDutyInfo", sSelID = "0" });
        //}


        public override ActionResult DataList_ReturnResetClick(string PanelID, string selection, string keywords, string transvalue)
        {
            base.DataList_ReturnResetClick(PanelID, selection, keywords, transvalue);
            SetComboValue(sPageKeyWord + "_cmbPcdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbKldm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbJhxzdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbJhlbdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbDqdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbXzdm", "0", false);
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult JHKSearch_SearchClickDirectMethod(string sData)
        {
            if (SetSearchWhere(sData))
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        protected override bool SetSearchWhere(string sData)
        {
            T_jhk model = JSON.Deserialize<T_jhk>(sData);
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> signs = new List<string>();

            if ((model.GxName != "") && (model.GxName != null))
            { names.Add("ParentDepartment.Name"); values.Add(model.GxName); signs.Add(SIGN.Equal.ToString()); }
            if ((model.SearchZydh != "") && (model.SearchZydh != null))
            { names.Add("zydh.Code"); values.Add(model.SearchZydh); signs.Add(SIGN.Like.ToString()); }
            if ((model.SearchZydm != "") && (model.SearchZydm != null))
            { names.Add("zydh.zylb.Code"); values.Add(model.SearchZydm); signs.Add(SIGN.Like.ToString()); }
            if ((model.SearchZymc != "") && (model.SearchZymc != null))
            { names.Add("zydh.zylb.Name"); values.Add(model.SearchZymc); signs.Add(SIGN.Like.ToString()); }
            if (model.SearchPcdm != 0)
            { names.Add("tddw.Pcdm"); values.Add(model.SearchPcdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if (model.SearchKldm != 0)
            { names.Add("tddw.Kldm"); values.Add(model.SearchKldm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if (model.SearchJhxzdm != 0)
            { names.Add("tddw.Jhxzdm"); values.Add(model.SearchJhxzdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if (model.SearchJhlbdm != 0)
            { names.Add("tddw.Jhlbdm"); values.Add(model.SearchJhlbdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if (model.ProvinceID != 0)
            { names.Add("ProvinceID"); values.Add(model.ProvinceID.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if (model.SearchXzdm != 0)
            { names.Add("zydh.Xzdm"); values.Add(model.SearchXzdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.SearchStartYear != 0) && (model.SearchStartYear != null))
            { names.Add("YearCode"); values.Add(model.SearchStartYear.ToString()); signs.Add(SIGN.GreaterThanOrEqual.ToString()); }
            if ((model.SearchEndYear != 0) && (model.SearchEndYear != null))
            { names.Add("YearCode"); values.Add(model.SearchEndYear.ToString()); signs.Add(SIGN.LessThanOrEqual.ToString()); }
            if ((model.SearchStartJhrs != 0) && (model.SearchStartJhrs != null))
            { names.Add("Jhrs"); values.Add(model.SearchStartJhrs.ToString()); signs.Add(SIGN.GreaterThanOrEqual.ToString()); }
            if ((model.SearchEndJhrs != 0) && (model.SearchEndJhrs != null))
            { names.Add("Jhrs"); values.Add(model.SearchEndJhrs.ToString()); signs.Add(SIGN.LessThanOrEqual.ToString()); }
            if ((model.SearchStartJhzxs != 0) && (model.SearchStartJhzxs != null))
            { names.Add("Jhzxs"); values.Add(model.SearchStartJhzxs.ToString()); signs.Add(SIGN.GreaterThanOrEqual.ToString()); }
            if ((model.SearchEndJhzxs != 0) && (model.SearchEndJhzxs != null))
            { names.Add("Jhzxs"); values.Add(model.SearchEndJhzxs.ToString()); signs.Add(SIGN.LessThanOrEqual.ToString()); }

            SetHiddenValue(sDataListName + "_NowNames", string.Join(CosValue.SPLITOPERATOR1.ToString(), names.ToArray()));
            SetHiddenValue(sDataListName + "_NowValues", string.Join(CosValue.SPLITOPERATOR1.ToString(), values.ToArray()));
            SetHiddenValue(sDataListName + "_NowSigns", string.Join(CosValue.SPLITOPERATOR1.ToString(), signs.ToArray()));

            return true;
        }


        #endregion


    }
}