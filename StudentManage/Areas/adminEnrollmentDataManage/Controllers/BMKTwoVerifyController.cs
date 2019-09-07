using System.Web.Mvc;
using StudentManage.Controllers;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;
using System;

namespace StudentManage.Areas.adminEnrollmentDataManage.Controllers
{
    [DirectController(AreaName = "adminEnrollmentDataManage")]
    public class BMKTwoVerifyController : TreeSelMasterController
    {

        public BMKTwoVerifyController()
        {
            sAreaName = "adminEnrollmentDataManage";
            sPageKeyWord = "BMKTwoVerify";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_SearchForm";
            sDataTreeKey = "dqdmTable";
        }

        public override ActionResult TreeNodeClick(string sTreeSelID, string sNowNames, string sNowValues, string sNowSigns)
        {
            SetHiddenValue(sPageKeyWord + "_SearchDqdmID", sTreeSelID);
            return this.Direct();
        }
        public override void SetInitDataAuthor()
        {
            var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
            CryptManage crypt = new CryptManage();
            string sGxID = crypt.Decrypto(loginUser.Values["UserGxID"]);
            SetHiddenValue(sDataListName + "_InitNames", "GxID|VerifyStatus");
            SetHiddenValue(sDataListName + "_InitValues", sGxID + CosValue.SPLITOPERATOR1.ToString() + "一次审核,二次审核");
            SetHiddenValue(sDataListName + "_InitSigns", SIGN.Equal.ToString() + CosValue.SPLITOPERATOR1.ToString() + SIGN.In.ToString());
        }

        public override void InitPage()
        {
            SetSearchPanelHeight(110);
            setParentVariable(null, null, "BMKTwoVerify_SelectZzmmdmChange", null, "zzmmdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbZzmmdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbZzmmdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectMzdmChange", null, "mzdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbMzdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbMzdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectXbdmChange", null, "xbdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbXbdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbXbdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectKslbdmChange", null, "kslbdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbKslbdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbKslbdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectBylbdmChange", null, "bylbdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbBylbdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbBylbdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectKslxdmChange", null, "kslxdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbKslxdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbKslxdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectWyyzdmChange", null, "wyyzdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbWyyzdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbWyyzdm", "0", true);

            setParentVariable(null, null, "BMKTwoVerify_SelectByxxdmChange", null, "byxxdmTable");
            SetHiddenValue(sPageKeyWord + "_cmbByxxdm_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbByxxdm", "0", true);
            
            T_bmk model = new T_bmk();
            var form = this.GetCmp<FormPanel>(sMainFormName);
            form.SetValues(model);
            SetInitDataAuthor();
            base.InitPage();
        }
        public ActionResult BMKTwoVerify_SelectMzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchMzdmID", sSelID);
            return this.Direct();
        }
        public ActionResult BMKTwoVerify_SelectXbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchXbdmID", sSelID);
            return this.Direct();
        }
        public ActionResult BMKTwoVerify_SelectZzmmdmChange(string panelID,string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchZzmmdmID", sSelID);
            return this.Direct();
        }
        public ActionResult BMKTwoVerify_SelectKslbdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchKslbdmID", sSelID);
            return this.Direct();
        }

        public ActionResult BMKTwoVerify_SelectBylbdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchBylbdmID", sSelID);
            return this.Direct();
        }
        public ActionResult BMKTwoVerify_SelectKslxdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchKslxdmID", sSelID);
            return this.Direct();
        }

        public ActionResult BMKTwoVerify_SelectWyyzdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchWyyzdmID", sSelID);
            return this.Direct();
        }

        public ActionResult BMKTwoVerify_SelectByxxdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchByxxdmID", sSelID);
            return this.Direct();
        }
        public ActionResult BMKTwoVerify_SelectDqdmChange(string panelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchDqdmID", sSelID);
            return this.Direct();
        }

        #region DataList按钮事件相关
        //[DirectMethod]
        //public ActionResult BMKTwoVerify_AddClickDirectMethod(string sData)
        //{
        //    tblBMKTwoVerify model = JSON.Deserialize<tblBMKTwoVerify>(sData);
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
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinStudentMore.cshtml";
            pvr.ViewData["WindowID"] = "BMKTwoVerify_WinStudentMore";
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, "2");
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinStudentMore";
            return pvr;                        
        }

        public override ActionResult DataList_ReturnResetClick(string PanelID, string selection, string keywords, string transvalue)
        {
            base.DataList_ReturnResetClick(PanelID, selection, keywords, transvalue);
            SetComboValue(sPageKeyWord + "_cmbMzdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbXbdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbZzmmdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbKslbdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbBylbdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbKslxdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbWyyzdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbByxxdm", "0", false);
            SetComboValue(sPageKeyWord + "_cmbDqdm", "0", false);
            return this.Direct();
        }

        public override ActionResult DataList_ReturnAddClick(string PanelID, string selection, string keywords, string transvalue)
        {
            bmkTableDataListManage manDataList = new bmkTableDataListManage();
            manDataList.InitSelection(selection);
            bmkTableManage man = new bmkTableManage();
            man.SecondVerifyRecords(manDataList.sRowAllIDs);
            RefreashDataList(sDataListName);
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult BMKTwoVerify_SearchClickDirectMethod(string sData)
        {
            if (SetSearchWhere(sData))
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        protected override bool SetSearchWhere(string sData)
        {
            T_bmk model = JSON.Deserialize<T_bmk>(sData);
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> signs = new List<string>();

            if ((model.Ksh != "") && (model.Ksh != null))
            { names.Add("Ksh"); values.Add(model.Ksh); signs.Add(SIGN.Like.ToString()); }
            if ((model.Zkzh != "") && (model.Zkzh != null))
            { names.Add("Zkzh"); values.Add(model.Zkzh.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Xm != "") && (model.Xm != null))
            { names.Add("Xm"); values.Add(model.Xm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Dqdm != 0) && (model.Dqdm != -1))
            {
                names.Add("Dqdm");
                dqdmTableDataTreeManage tree = new dqdmTableDataTreeManage();
                values.Add(tree.GetAllLeaf(model.Dqdm));
                signs.Add(SIGN.In.ToString());
            }
            if ((model.Zzmmdm != 0) && (model.Zzmmdm != -1))
            { names.Add("Zzmmdm"); values.Add(model.Zzmmdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Xbdm != 0) && (model.Xbdm != -1))
            { names.Add("Xbdm"); values.Add(model.Xbdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Mzdm != 0) && (model.Mzdm != -1))
            { names.Add("Mzdm"); values.Add(model.Mzdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Hkkh != "") && (model.Hkkh != null))
            { names.Add("Hkkh"); values.Add(model.Hkkh); signs.Add(SIGN.Like.ToString()); }
            if ((model.Kslbdm != 0) && (model.Kslbdm != -1))
            { names.Add("Kslbdm"); values.Add(model.Kslbdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Bylbdm != 0) && (model.Bylbdm != -1))
            { names.Add("Bylbdm"); values.Add(model.Bylbdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Kslxdm != 0) && (model.Kslxdm != -1))
            { names.Add("Kslxdm"); values.Add(model.Kslxdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Wyyzdm != 0) && (model.Wyyzdm != -1))
            { names.Add("Wyyzdm"); values.Add(model.Wyyzdm.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.Byxxdm != 0) && (model.Byxxdm != -1))
            { names.Add("Byxxdm"); values.Add(model.Byxxdm.ToString()); signs.Add(SIGN.Equal.ToString()); }


            if ((model.SearchStartYear != 0) && (model.SearchStartYear != null))
            { names.Add("YearCode"); values.Add(model.SearchStartYear.ToString()); signs.Add(SIGN.GreaterThanOrEqual.ToString()); }
            if ((model.SearchEndYear != 0) && (model.SearchEndYear != null))
            { names.Add("YearCode"); values.Add(model.SearchEndYear.ToString()); signs.Add(SIGN.LessThanOrEqual.ToString()); }
            if (model.SearchStartCsrq != null)
            { names.Add("Csrq"); values.Add(model.SearchStartCsrq.ToString()); signs.Add(SIGN.GreaterThanOrEqual.ToString()); }
            if (model.SearchEndCsrq != null)
            { names.Add("Csrq"); values.Add(model.SearchEndCsrq.ToString()); signs.Add(SIGN.LessThanOrEqual.ToString()); }
            if (model.SearchVerifyStatus != "")
            { names.Add("VerifyStatus"); values.Add(model.SearchVerifyStatus.ToString()); signs.Add(SIGN.Equal.ToString()); }

            //if ((model.UserIDepartmentIID != 0) && (model.UserIDepartmentIID != null) && (model.UserIDepartmentIID != -1))
            //{ names.Add("UserIDepartmentIID"); values.Add(GetTreeManage("DepartmentInfo").GetAllLeaf(model.UserIDepartmentIID)); signs.Add(SIGN.In.ToString()); }
            //if ((model.UserIMobilePhone != "") && (model.UserIMobilePhone != null))
            //{ names.Add("UserIMobilePhone"); values.Add(model.UserIMobilePhone); signs.Add(SIGN.Equal.ToString()); }

            SetHiddenValue(sDataListName + "_NowNames", string.Join(CosValue.SPLITOPERATOR1.ToString(), names.ToArray()));
            SetHiddenValue(sDataListName + "_NowValues", string.Join(CosValue.SPLITOPERATOR1.ToString(), values.ToArray()));
            SetHiddenValue(sDataListName + "_NowSigns", string.Join(CosValue.SPLITOPERATOR1.ToString(), signs.ToArray()));

            return true;
        }


        #endregion


    }
}