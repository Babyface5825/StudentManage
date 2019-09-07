using System.Web.Mvc;
using Ext.Net.MVC;
using StudentManage.Models;
using StudentManage.Controllers;
using Ext.Net;
using System;
using System.Collections.Generic;
using StudentManage.Manage;

namespace StudentManage.Areas.adminMain.Controllers
{
    [DirectController(AreaName = "adminMain")]
    public class SimpleDlgController : ParentController
    {
        // GET: Parent        
        public SimpleDlgController()
        {
            sAreaName = "adminMain";
            sPageKeyWord = "SimpleDlg";
            sDataListName = sPageKeyWord + "_DataList";
            sMainFormName = sPageKeyWord + "_SearchForm";
        }

        public override void InitPage()
        {
            ViewData["DataList_PanelID"] = sDataListName;
            string key = Request["key"].ToString();

            switch (key)
            {
                case "zydhTable":
                case "tddwTable":
                    SetInitDataAuthor();
                    break;
            }

            setParentVariable(null, null, null, key, null);
            ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;
        }

        #region DataList按钮事件相关
        public override ActionResult DataList_ReturnAddClick(string PanelID, string selection, string keywords, string transvalue)
        {
            var man = GetDataListManage(keywords, transvalue);
            man.InitSelection(selection);
            return OpenMoreWindow(man.sRowNowID, keywords, transvalue);
        }
        protected override ActionResult OpenMoreWindow(string selection, string keywords, string transvalue)
        {
            //注意这里把keywords和transvalue拼起来了
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, keywords + CosValue.SPLITOPERATOR2 + transvalue);
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            switch (transvalue)
            {
                case "kldmTable":  //科类代码
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinKldmSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinKldmSimpleMore";
                    break;
                case "pcdmTable": //批次代码
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinPcdmSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinPcdmSimpleMore";
                    break;
                case "tddwTable": //投档单位
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinTddwSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinTddwSimpleMore";
                    break;
                case "zydhTable": //专业代号
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinZydhSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinZydhSimpleMore";
                    break;
                case "dqdmTable": //地区代码
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinDqdmSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinDqdmSimpleMore";
                    break;
                case "zylbdmTable": //专业类别代码
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinZylbdmSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinZylbdmSimpleMore";
                    break;
                default:
                    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinSimpleMore.cshtml";
                    pvr.ViewData["CurrentController"] = "WinSimpleMore";
                    break;
            }
            pvr.ViewData["WindowID"] = "SimpleDlg_WinSimpleMore";
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }

        public override ActionResult DataList_ReturnResetClick(string PanelID, string selection, string keywords, string transvalue)
        {            
            var man = GetDataListManage(keywords, transvalue);
            var model = man.GetSpecialModelFromCurrentID(0);
            var form = this.GetCmp<FormPanel>(sMainFormName);
            form.SetValues(model);
            SetHiddenValue(sDataListName + "_NowNames", "");
            SetHiddenValue(sDataListName + "_NowValues", "");
            SetHiddenValue(sDataListName + "_NowSigns", "");
            RefreashDataList(sDataListName);
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult SimpleDlg_SearchClickDirectMethod(string sData)
        {
            if (SetSearchWhere(sData))
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        protected override bool SetSearchWhere(string sData)
        {
            tblSimpleInfo model = JSON.Deserialize<tblSimpleInfo>(sData);
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> signs = new List<string>();

            if ((model.Name != null) && (model.Name != ""))
            { names.Add("Name"); values.Add(model.Name); signs.Add(SIGN.Like.ToString()); }
            if ((model.Code != null) && (model.Code != ""))
            { names.Add("Code"); values.Add(model.Code); signs.Add(SIGN.Like.ToString()); }

            SetHiddenValue(sDataListName + "_NowNames", string.Join(CosValue.SPLITOPERATOR1.ToString(), names.ToArray()));
            SetHiddenValue(sDataListName + "_NowValues", string.Join(CosValue.SPLITOPERATOR1.ToString(), values.ToArray()));
            SetHiddenValue(sDataListName + "_NowSigns", string.Join(CosValue.SPLITOPERATOR1.ToString(), signs.ToArray()));

            return true;
        }

        public override ActionResult DataList_ReturnMore1Click(string PanelID, string selection, string keywords, string transvalue)
        {            
            if (selection == null)
            {
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
                return this.Direct();
            }
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, keywords + CosValue.SPLITOPERATOR2 + transvalue);
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewData["WindowID"] = "SimpleDlg_WinJoinMore";
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinJoinMore.cshtml";
            pvr.ViewData["CurrentController"] = "WinJoinMore";
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }

        #endregion

        public ActionResult SimpleDlg_ConfirmSaveMore(long ID)
        {
            if (ID != 0)
                RefreashDataList(sDataListName);
            return this.Direct();
        }


    }
}