using System.Web.Mvc;
using StudentManage.Controllers;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;

namespace StudentManage.Areas.adminSystemManage.Controllers
{
    [DirectController(AreaName = "adminSystemManage")]
    public class UserInfoController : TreeSelMasterController
    {
        // GET: adminSystemManage/UserInfo


        public UserInfoController()
        {
            sAreaName = "adminSystemManage";
            sPageKeyWord = "UserInfo";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_SearchForm";
            sDataTreeKey = "DepartmentInfo";
        }

        public override ActionResult TreeNodeClick(string sTreeSelID, string sNowNames, string sNowValues, string sNowSigns)
        {
            if ((sTreeSelID != "-1") && (sTreeSelID != "0"))
            {
                DepartmentInfoDataTreeManage treeMan = new DepartmentInfoDataTreeManage();
                SetHiddenValue(sPageKeyWord + "_TreeNodeID", sTreeSelID);
            }
            return this.Direct();
        }
        
        public override void InitPage()
        {            
            SetSearchPanelHeight(60);
            setParentVariable(null, null, "UserInfo_SelectDutyChange", null, "DutyInfo");
            SetHiddenValue(sPageKeyWord + "_cmbDutyInfo_ParentVariable", ParentVar.DefaultVal);
            SetComboValue(sPageKeyWord + "_cmbDutyInfo", "0", true);

            string[] key = { CookieKeys.DEPARTMENTID, CookieKeys.USERROLES };
            string sDepartmentID = (new UserInfoManage()).GetLoginUserInfo(key)[0];
            string sAuthorIDs = (new UserInfoManage()).GetLoginUserInfo(key)[1];            
            DepartmentInfoManage man = new DepartmentInfoManage();
            SetHiddenValue(sPageKeyWord + "_SearchDepartmentID", man.GetSpecialIDsFromAuthors(sAuthorIDs, sDepartmentID));

            base.InitPage();
        }

        public ActionResult UserInfo_SelectDutyChange(string PanelID, string sSelID)
        {
            SetHiddenValue(sPageKeyWord + "_SearchDutyID", sSelID);
            return this.Direct();
        }

        #region DataList按钮事件相关
        [DirectMethod]
        public ActionResult UserInfo_AddClickDirectMethod(string sData)
        {
            tblUserInfo model = JSON.Deserialize<tblUserInfo>(sData);
            if ((model.UserIDepartmentIID == 0) || (model.UserIDepartmentIID == -1) || (model.UserIDepartmentIID == null))
            {
                X.Msg.Alert("注意", "请先选择部门！").Show();
                return this.Direct();
            }
            return OpenMoreWindow("0", null, model.UserIDepartmentIID.ToString());
        }

        protected override ActionResult OpenMoreWindow(string selection, string keywords, string transvalue)
        {
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinUserMore.cshtml";
            pvr.ViewData["WindowID"] = "UserInfo_WinUserMore";            
            pvr.ViewData["WinUserMore_SetAuthorID"] = "0";
            pvr.ViewData["WindowParentKey"] = transvalue;
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, null);            
            pvr.ViewData["WinUserMore_AuthorPanelHidden"] = false;
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinUserMore";
            return pvr;
        }

        [DirectMethod]
        public ActionResult UserInfo_ClearDutyCombo()
        {
            return RedirectToActionPermanent("SetDataCombo", "DataComboBox", new { area = "UserControl", PanelID = "UserInfo_cmbDutyInfo", sSelID = "0" });
        }


        public override ActionResult DataList_ReturnResetClick(string PanelID, string selection, string keywords, string transvalue)
        {
            base.DataList_ReturnResetClick(PanelID, selection, keywords, transvalue);
            SetComboValue(sPageKeyWord + "_cmbDutyInfo", "0");
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult UserInfo_SearchClickDirectMethod(string sData)
        {
            if (SetSearchWhere(sData))
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        protected override bool SetSearchWhere(string sData)
        {
            tblUserInfo model = JSON.Deserialize<tblUserInfo>(sData);
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> signs = new List<string>();

            if ((model.Name != "") && (model.Name != null))
            { names.Add("Name"); values.Add(model.Name); signs.Add(SIGN.Like.ToString()); }
            if ((model.SearchVerifyFlag != null))
            { names.Add("VerifyFlag"); values.Add(model.SearchVerifyFlag.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.UserIDutyIID != 0) && (model.UserIDutyIID != null))
            { names.Add("UserIDutyIID"); values.Add(model.UserIDutyIID.ToString()); signs.Add(SIGN.Equal.ToString()); }
            if ((model.SearchDepartmentIDs != "0") && (model.SearchDepartmentIDs != null) && (model.SearchDepartmentIDs != "-1"))
            { names.Add("UserIDepartmentIID"); values.Add(model.SearchDepartmentIDs); signs.Add(SIGN.In.ToString()); }
            if ((model.UserIDepartmentIID != 0) && (model.UserIDepartmentIID != null) && (model.UserIDepartmentIID != -1))
            { names.Add("UserIDepartmentIID"); values.Add(GetTreeManage("DepartmentInfo").GetAllLeaf(model.UserIDepartmentIID)); signs.Add(SIGN.In.ToString()); }

                if ((model.UserIMobilePhone != "")&& (model.UserIMobilePhone != null))
            { names.Add("UserIMobilePhone"); values.Add(model.UserIMobilePhone); signs.Add(SIGN.Equal.ToString()); }

            SetHiddenValue(sDataListName + "_NowNames", string.Join(CosValue.SPLITOPERATOR1.ToString(),names.ToArray()));
            SetHiddenValue(sDataListName + "_NowValues", string.Join(CosValue.SPLITOPERATOR1.ToString(), values.ToArray()));
            SetHiddenValue(sDataListName + "_NowSigns", string.Join(CosValue.SPLITOPERATOR1.ToString(), signs.ToArray()));

            return true;
        }


        #endregion


    }
}