using System.Web.Mvc;
using Ext.Net.MVC;
using StudentManage.Models;
using StudentManage.Controllers;
using StudentManage.Manage;
using Ext.Net;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Collections.Specialized;
using System;
using System.Linq;

namespace StudentManage.Areas.adminEnrollmentDataManage.Controllers
{
    [DirectController(AreaName = "adminEnrollmentDataManage")]
    public class OriginDataInportController : ParentController
    {
        // GET: Parent        
        public OriginDataInportController()
        {
            sAreaName = "adminEnrollmentDataManage";
            sPageKeyWord = "OriginDataInport";
            sDataListName = sPageKeyWord + "_DataList";
        }

        public override void InitPage()
        {
            ViewData["DataList_PanelID"] = sDataListName;
            setParentVariable(null, null, null, null, null);
            ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;

            var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
            CryptManage crypt = new CryptManage();
            string sGxID = crypt.Decrypto(loginUser.Values["UserGxID"]);
            SetHiddenValue(sDataListName + "_InitNames", "GxID");
            SetHiddenValue(sDataListName + "_InitValues", sGxID);
            SetHiddenValue(sDataListName + "_InitSigns", SIGN.Equal.ToString());
        }


        //#region DataList按钮事件相关
        public override ActionResult DataList_ReturnAddClick(string PanelID, string selection, string keywords, string transvalue)
        {
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, keywords);
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinUploadOriginData.cshtml";
            pvr.ViewData["WindowID"] = "OriginDataInport_WinUpload";
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinUploadOriginData";
            return pvr;
        }

        public override ActionResult MoreWindow_ConfirmSave(string PanelID = null, string transValue = null)
        {
            if (PanelID == "OriginDataInport_WinOriginDataInportSimpleMore")
                RefreashDataList(sDataListName);
            else
            {
                RefreashDataList(sDataListName);
                X.AddScript("App.direct.adminEnrollmentDataManage.OriginDataInport_ConfirmSaveDirectMethod(App.OriginDataInport_NowWatchIDs.getValue(),'" + transValue + "');");
            }
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult OriginDataInport_ConfirmSaveDirectMethod(string sNowWatchIDs, string sNewWatchIDs)
        {
            if (sNowWatchIDs == "")
            {
                SetHiddenValue("OriginDataInport_NowWatchIDs", sNewWatchIDs);
                TaskManager task = this.GetCmp<TaskManager>("OriginDataInport_Task");
                task.StartTask("ServerTime");
            }
            else
                SetHiddenValue("OriginDataInport_NowWatchIDs", sNowWatchIDs + "," + sNewWatchIDs);
            return this.Direct();
        }

        public ActionResult OriginDataInport_TaskUpdate(string sWatchIDs)
        {
            OriginDataPackageManage man = new OriginDataPackageManage();
            FileManager filePath = new FileManager();
            List<tblOriginDataPackageInfo> list = man.GetRecordsByID<tblOriginDataPackageInfo>(sWatchIDs);
            bool flag = false;
            string sReturnWatchIDs = sWatchIDs;
            foreach (tblOriginDataPackageInfo model in list)
            {
                //string sPath = ConstAddressPath.TEMP_WEBPATH + model.NowAddress;
                //if (!filePath.HasPath(sPath))
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
                        TaskManager task = this.GetCmp<TaskManager>("OriginDataInport_Task");
                        task.StopTask("ServerTime");
                    }
                    SetHiddenValue("OriginDataInport_NowWatchIDs", sReturnWatchIDs);
                }
            }
            if (flag)
                RefreashDataList(sDataListName);
            return this.Direct();
        }

        public override ActionResult DataList_ReturnEditClick(string PanelID, string selection, string keywords, string transvalue)
        {
            //OriginDataPackageDataListManage manList = new OriginDataPackageDataListManage();
            //OriginDataPackageManage man = new OriginDataPackageManage();
            //manList.InitSelection(selection);
            //if (manList.sRowNowID == "0")
            //    X.Msg.Alert("注意", "请先选中要操作的行！").Show();
            //else
            //    man.InportData(manList.sRowNowID);
            //return this.Direct();
            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinOriginDataInportSimpleMore.cshtml";
            pvr.ViewData["WindowID"] = "OriginDataInport_WinOriginDataInportSimpleMore";
            setParentVariable(null, null, "MoreWindow_ConfirmSave", selection, null);
            pvr.ViewData["WindowParentVariable"] = ParentVar.DefaultVal;
            pvr.ViewData["CurrentController"] = "WinOriginDataInportSimpleMore";
            return pvr;
        }

        public override ActionResult DataList_ReturnDeleteClick(string PanelID, string selection, string keywords, string transvalue)
        {
            OriginDataPackageDataListManage man = new OriginDataPackageDataListManage();
            List<tblOriginDataPackageInfo> model = man.GetSpecialModelFromCurrentSelection(selection);
            if ((model != null) && (model.Count > 0))
            {
                man.DeleteRecord(model[0]);
                FileManager fileMan = new FileManager();
                fileMan.DelOneFile(ConstAddressPath.ORIGINZIP_WEBPATH + model[0].NowAddress);
                RefreashDataList(PanelID);
            }
            return this.Direct();
        }

    }
}