using Ext.Net.MVC;
using StudentManage.Models;
using StudentManage.Controllers;
using StudentManage.Manage;
using Ext.Net;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Linq;

namespace StudentManage.Areas.adminEnrollmentDataManage.Controllers
{
    [DirectController(AreaName = "adminEnrollmentDataManage")]
    public class ReportCreateController : TreeSelMasterController
    {
        // GET: Parent        
        public ReportCreateController()
        {
            sAreaName = "adminEnrollmentDataManage";
            sPageKeyWord = "ReportCreate";
            sDataListName = sPageKeyWord + "_DataList";
            sMainFormName = sPageKeyWord + "_SearchForm";

        }

        public Ext.Net.MVC.PartialViewResult AutoLoadDataListSel(string containerId)
        {
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ContainerId = containerId;
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\DataList.cshtml";
            pvr.WrapByScriptTag = false;
            pvr.RenderMode = RenderMode.AddTo;
            pvr.ViewData["DataList_PanelID"] = sPageKeyWord + "_DataListSel";
            setParentVariable(null, null, null, null, "ReportTypeSel");
            pvr.ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;
            return pvr;            
        }
        

        public override void InitPage()
        {
            SetSearchPanelHeight(30);
            base.InitPage();

            ViewData["DataList_PanelID"] = sDataListName;
            setParentVariable(null, null, "MoreWindow_ConfirmSave", null, null);
            ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;
            SetInitDataAuthor();
        }


        #region DataList按钮事件相关

        public override ActionResult DataList_ReturnListRowClick(string PanelID, string selection, string keywords, string transvalue)
        {
            if (keywords == "ReportTypeSel")
            {
                ReportTypeDataListManage man = new ReportTypeDataListManage();
                man.InitSelection(selection);
                SetHiddenValue(sPageKeyWord + "_SelTypeID", man.sRowNowID);
            }
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult ReportCreate_AddClickDirectMethod(string sSelTypeID)
        {
            //sSelTypeID的值是上面InitSelection之后传过来的
            if (sSelTypeID == "0")
            {
                X.Msg.Alert("注意", "请先选择报表类型！").Show();
                return this.Direct();
            }
            else
            {
                Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
                setParentVariable(null, null, "MoreWindow_ConfirmSave", sSelTypeID, null);//设置了要传递的参数的值
                pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;   //把参数传递到了下一个页面

                switch (sSelTypeID)
                {
                    case "1"://招生计划及录取情况表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport1.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport1";
                        pvr.ViewData["CurrentController"] = "WinCreateReport1";
                        return pvr;
                    case "2"://录取情况分析表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport2.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport2";
                        pvr.ViewData["CurrentController"] = "WinCreateReport2";
                        return pvr;
                    case "3"://招生批次表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport3.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport3";
                        pvr.ViewData["CurrentController"] = "WinCreateReport3";
                        return pvr;
                    case "4"://第一志愿报考专业表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport4.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport4";
                        pvr.ViewData["CurrentController"] = "WinCreateReport4";
                        return pvr;
                    case "5"://志愿录取情况表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport5.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport5";
                        pvr.ViewData["CurrentController"] = "WinCreateReport5";
                        return pvr;
                    case "6"://生源地统计表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport6.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport6";
                        pvr.ViewData["CurrentController"] = "WinCreateReport6";
                        return pvr;
                    case "7"://录取分数表
                        pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinCreateReport7.cshtml";
                        pvr.ViewData["WindowID"] = "ReportCreate_WinCreateReport7";
                        pvr.ViewData["CurrentController"] = "WinCreateReport7";
                        return pvr;
                }
            }
            return null;
        }


        public override ActionResult DataList_ReturnAddClick(string PanelID, string selection, string keywords, string transvalue)
        {
            X.AddScript("var s=App.ReportCreate_SelTypeID.getValue();");
            X.AddScript("App.direct." + sAreaName + ".ReportCreate_AddClickDirectMethod(s);");
            return this.Direct();
        }

        public override ActionResult MoreWindow_ConfirmSave(string PanelID = null, string transValue = null)
        {
            RefreashDataList(sDataListName);
            string s = "App.direct.adminEnrollmentDataManage.ReportCreate_ConfirmSaveDirectMethod(App.ReportCreate_NowWatchIDs.getValue(),'" + transValue + "');";
            X.AddScript(s);

            return this.Direct();
        }

        [DirectMethod]
        public ActionResult ReportCreate_ConfirmSaveDirectMethod(string sNowWatchIDs, string sNewWatchIDs)
        {
            if (sNowWatchIDs == "")
            {
                SetHiddenValue("ReportCreate_NowWatchIDs", sNewWatchIDs);
                TaskManager task = this.GetCmp<TaskManager>("ReportCreate_Task");
                task.StartTask("ServerTime");
            }
            else
                SetHiddenValue("ReportCreate_NowWatchIDs", sNowWatchIDs + "," + sNewWatchIDs);
            return this.Direct();
        }

        public ActionResult ReportCreate_TaskCreate(string sWatchIDs)
        {
            ReportManage man = new ReportManage();
            List<tblReportInfo> list = man.GetRecordsByID<tblReportInfo>(sWatchIDs);
            bool flag = false;
            string sReturnWatchIDs = sWatchIDs;
            foreach (tblReportInfo model in list)
            {
                if (model.HandleFlag == "处理完成")
                {
                    flag = true;
                    List<string> t = sReturnWatchIDs.Split(CosValue.SPLITOPERATOR2).ToList();
                    t.Remove(model.ID.ToString());
                    if (t.Count > 0)
                        sReturnWatchIDs = string.Join(CosValue.SPLITOPERATOR2.ToString(), t.ToArray());
                    else
                    {
                        sReturnWatchIDs = "";
                        TaskManager task = this.GetCmp<TaskManager>("ReportCreate_Task");
                        task.StopTask("ServerTime");
                    }
                    SetHiddenValue("ReportCreate_NowWatchIDs", sReturnWatchIDs);
                }
            }
            if (flag)
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        public override ActionResult DataList_ReturnEditClick(string PanelID, string selection, string keywords, string transvalue)
        {
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinReportSimpleMore.cshtml";
            pvr.ViewData["WindowID"] = "ReportCreate_WinReportSimpleMore";
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, null);
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinReportSimpleMore";
            return pvr;
        }

        public override ActionResult DataList_ReturnExportClick(string PanelID, string selection = "")
        {
            if (selection == "")
            {
                X.Msg.Alert("注意", "请先选择需要导出的报表！").Show();
                return this.Direct();
            }
            else
            {
                ReportInfoDataListManage man = new ReportInfoDataListManage();
                List<tblReportInfo> model = man.GetSpecialModelFromCurrentSelection(selection);

                if ((model != null) && (model.Count > 0))
                {
                    string sAddress = ConstAddressPath.REPORT_WEBPATH;
                    string sWholeFileName = sAddress + model[0].ID + ".xls";
                    FileManager clsFile = new FileManager();
                    if (clsFile.HasFile(sWholeFileName))
                        clsFile.DownloadFile(sWholeFileName, model[0].Name + ".xls");
                    else
                    {
                        X.Msg.Alert("注意", "该报表未正确生成，请重新生成后再下载...").Show() ;
                        DataList_ReturnDeleteClick(PanelID, selection, null, null);
                        return this.Direct();
                    }
                }
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
            }
        }

        public override ActionResult DataList_ReturnDeleteClick(string PanelID, string selection, string keywords, string transvalue)
        {
            ReportInfoDataListManage man = new ReportInfoDataListManage();
            List<tblReportInfo> model = man.GetSpecialModelFromCurrentSelection(selection);
            if ((model != null) && (model.Count > 0))
            {
                for(int c = 0; c < model.Count; c++)
                {
                    man.DeleteRecord(model[c]);
                    FileManager fileMan = new FileManager();
                    //fileMan.DelOneFile(ConstAddressPath.REPORT_WEBPATH + model[0].Name+"("+model[0].ID+")"+".xlsx");
                    fileMan.DelOneFile(ConstAddressPath.REPORT_WEBPATH + model[c].ID + ".xls");
                    RefreashDataList(PanelID);
                }
                
            }
            return this.Direct();
        }

        #endregion
    }
}