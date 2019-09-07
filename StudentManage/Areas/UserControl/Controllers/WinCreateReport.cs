using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System.Linq;

namespace StudentManage.Areas.UserControl.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class WinCreateReportController : ModelWindowsController
    {
        protected int MAXITEMS = 3;
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);            
            win.Resizable = false;
            SetWindowSize(win, 540, 420, sWidth, sHeight);
            return this.Direct();
        }


        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            tblReportInfo model = new tblReportInfo();
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
            CryptManage crypt = new CryptManage();
            string sGxID = crypt.Decrypto(loginUser.Values["UserGxID"]);
            model.GxID = Convert.ToInt64(sGxID);
            form.SetValues(model);
            return this.Direct();
        }

        public virtual ActionResult WinCreateReport_SelectItems(string sOldSelIDs, string sNowSelIDs, string PanelID, string ParentVariable)
        {
            SetTempItemsStore(PanelID);
            string[] arrNowIDs = JSON.Deserialize<string[]>(sNowSelIDs);
            string sSaveIDs = "";
            string[] arrOldIDs = sOldSelIDs.Split(CosValue.SPLITOPERATOR2);
            List<string> listTempIDs = new List<string>();
            if ((arrNowIDs != null) && (arrNowIDs.Length > MAXITEMS))
            {
                X.Msg.Alert("注意", "最多只能选" + MAXITEMS + "大类！").Show();
                var combo = this.GetCmp<ComboBox>(PanelID + "_ComboType");
                string[] newarrIDs = new string[MAXITEMS];
                for (int i = 0; i < MAXITEMS; i++)
                    newarrIDs[i] = arrNowIDs[i];
                combo.SetValue(newarrIDs);
                sSaveIDs = string.Join(CosValue.SPLITOPERATOR2.ToString(), newarrIDs);
            }
            else
                sSaveIDs = string.Join(CosValue.SPLITOPERATOR2.ToString(), arrNowIDs);

            SetHiddenValue(PanelID + "_SelNowItemIDs", sSaveIDs);
            var store1 = this.GetCmp<Store>(PanelID + "_AllItemsStore");
            store1.LoadProxy();
            var store2 = this.GetCmp<Store>(PanelID + "_NowItemsStore");
            store2.LoadProxy();

            SetHiddenValue(PanelID + "_SelOldItemIDs", sSaveIDs);
            return this.Direct();
        }



        public ActionResult WinCreateReport_ItemsAllRead(string ParentVariable, string sGxID, string SelOldItemIDs, string SelNowItemIDs, string SelectedStore)
        {
            //GetParentVariable(ParentVariable);
            string[] arrOldItemIDs = SelOldItemIDs.Split(CosValue.SPLITOPERATOR2);
            string[] arrNowItemIDs = SelNowItemIDs.Split(CosValue.SPLITOPERATOR2);

            if (arrNowItemIDs != null)
            {
                object[][] arrObj = new object[arrNowItemIDs.Length][];
                int num = 0;
                for (int i = 0; i < arrNowItemIDs.Length; i++)
                {
                    switch (arrNowItemIDs[i])
                    {
                        case "Department":
                            DepartmentInfoManage departmentMan = new DepartmentInfoManage();
                            arrObj[i] = departmentMan.GetNameStoreforSelection(sGxID, SelectedStore, false);//获得院系
                            break;
                        case "Province":
                            dqdmTableManage dqMan = new dqdmTableManage();
                            arrObj[i] = dqMan.GetNameStoreforSelection(SelectedStore, false);
                            break;
                        default:
                            OriginTableManage tableMan = new OriginTableManage();
                            arrObj[i] = tableMan.GetNameStoreforSelection(arrNowItemIDs[i], SelectedStore, false);
                            break;
                    }
                    num += arrObj[i].Length;
                }
                object[] returnObj = new object[num];
                num = 0;
                for (int i = 0; i < arrNowItemIDs.Length; i++)
                    for (int j = 0; j < arrObj[i].Length; j++)
                    {
                        returnObj[num] = arrObj[i][j];
                        num++;
                    }
                return this.Store(returnObj);
            }
            else
                return null;
        }
        public ActionResult WinCreateReport_ItemsSelRead(string PanelID, string sGxID, string SelOldItemIDs, string SelNowItemIDs, string SelectedStore)
        {
            string[] arrOldItemIDs = SelOldItemIDs.Split(CosValue.SPLITOPERATOR2);
            string[] arrNowItemIDs = SelNowItemIDs.Split(CosValue.SPLITOPERATOR2);
            int num = 0;
            foreach (string oldItemID in arrOldItemIDs) //如果某个老编号在新编号里面，保留
            {
                if (arrNowItemIDs.Contains(oldItemID))
                    num++;
            }

            object[][] arrObj = new object[num][];
            num = 0;
            int allnum = 0;
            foreach (string oldItemID in arrOldItemIDs) //如果某个老编号在新编号里面，保留
            {
                if (arrNowItemIDs.Contains(oldItemID))
                {
                    switch (oldItemID)
                    {
                        case "Department":
                            DepartmentInfoManage departmentMan = new DepartmentInfoManage();
                            arrObj[num] = departmentMan.GetNameStoreforSelection(sGxID, SelectedStore);//获得院系                            
                            break;
                        case "Province":
                            dqdmTableManage dqMan = new dqdmTableManage();
                            arrObj[num] = dqMan.GetNameStoreforSelection(SelectedStore);
                            break;
                        default:
                            OriginTableManage tableMan = new OriginTableManage();
                            arrObj[num] = tableMan.GetNameStoreforSelection(oldItemID, SelectedStore, true);
                            break;
                    }
                    allnum += arrObj[num].Length;
                    num++;
                }
            }

            object[] returnObj = new object[allnum];
            int k = 0;
            for (int i = 0; i < num; i++)
                for (int j = 0; j < arrObj[i].Length; j++)
                {
                    returnObj[k] = arrObj[i][j];
                    k++;
                }
            return this.Store(returnObj);            
        }
        
        private void SetTempItemsStore(string PanelID)
        {
            string sScripts = "var i = 0; var sSelItems = \"\";";
            sScripts += "while (i < App." + PanelID + "_NowItemsStore" + ".getCount()) { sSelItems = sSelItems + App." + PanelID + "_NowItemsStore" + ".getAt(i).get('TypeID') + \"|\" + App." + PanelID + "_NowItemsStore" + ".getAt(i).get('ID') + \",\"; i++; }";
            sScripts += " sSelItems = sSelItems.slice(0, sSelItems.length - 1);";
            sScripts += "App." + PanelID + "_TempNowItemsStore.setValue(sSelItems);";
            X.AddScript(sScripts);
        }

        public virtual ActionResult WinCreateReport_AddClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            SetTempItemsStore(PanelID);
            return this.Direct();
        }

        public virtual ActionResult WinCreateReport_AddAllClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            SetTempItemsStore(PanelID);
            return this.Direct();
        }

        public virtual ActionResult WinCreateReport_RemoveClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            SetTempItemsStore(PanelID);
            return this.Direct();
        }

        public virtual ActionResult WinCreateReport_RemoveAllClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            SetTempItemsStore(PanelID);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(string PanelID, string ParentVariable,string ChartType)
        {
            tblReportInfo mo = new tblReportInfo();     //mo.Create\Delflag\remark都已经有值了经过这一步
           
            mo.CreateDate = DateTime.Now;
            string[] key = { CookieKeys.ID, CookieKeys.USERGXID };
            List<string> sUserInfo = (new UserInfoManage()).GetLoginUserInfo(key);
            mo.UserID = Convert.ToInt64(sUserInfo[0]);
            mo.GxID = Convert.ToInt64(sUserInfo[1]);
            GetParentVariable(ParentVariable);       //获取传过来的参数ParentVariable
            mo.ReportTypeID = Convert.ToInt16(ParentVar.transValue);



            if (!mo.HandleWay)
                mo.HandleFlag = "尚未处理";
            ReportInfoManage reInfoMan = new ReportInfoManage();
            //ReportManage reportMan = new ReportManage();
            long lNewID = reInfoMan.AddOneRecord(mo);
            //if (mo.HandleWay)
            //    reportMan.InportData(lNewID.ToString());

            SetHiddenValue(PanelID + "_ExitCode", "1");

            GetParentVariable(ParentVariable);
            setParentVariable(ParentVar.areaName, ParentVar.controlName, ParentVar.actionName, lNewID.ToString(), ParentVar.KeyWords);
            SetHiddenValue(PanelID + "_ParentVariable", ParentVar.DefaultVal);

            string sScripts = "var i = 0; var sSelItems = \"\";var sSelTypeIDs = \"\";";
            sScripts += "while (i < App." + PanelID + "_NowItemsStore" + ".getCount()) { sSelTypeIDs = sSelTypeIDs + App." + PanelID + "_NowItemsStore" + ".getAt(i).get('TypeID') + \",\"; sSelItems = sSelItems + App." + PanelID + "_NowItemsStore" + ".getAt(i).get('ID') + \",\"; i++; }";
            sScripts += " sSelItems = sSelItems.slice(0, sSelItems.length - 1); sSelTypeIDs = sSelTypeIDs.slice(0, sSelTypeIDs.length - 1);";
            sScripts += "App.direct.UserControl.WinCreateReport_CreateReportDirectMethod(sSelItems,sSelTypeIDs,\"" + PanelID + "\",App." + PanelID + "_SelGxdm.getValue(),App." + PanelID + "_OperateStartYear.getValue(),App." + PanelID + "_OperateEndYear.getValue(),\"" + ParentVariable + "\",\"" + ChartType + "\",\"" + lNewID.ToString() +"\");";
            X.AddScript(sScripts);
            return this.Direct();

        }        

        [DirectMethod]
        public ActionResult WinCreateReport_CreateReportDirectMethod(string sSelItems, string sSelTypeIDs, string PanelID, string sGxID, string sStartYear, string sEndYear, string ParentVariable,string sChartType,string sID)
        {
            if (sSelItems == "")
            {
                X.Msg.Alert("注意！", "已选项不能为空！").Show();
                return this.Direct();
            }

            ReportInfoManage man = new ReportInfoManage();
            tblReportInfo mo = man.GetOneRecordbyID<tblReportInfo, string>(sID);

            string GxName = "", FileName = "";
            DepartmentInfoManage Deman = new DepartmentInfoManage();
            tblDepartmentInfo model = Deman.GetOneRecordbyID<tblDepartmentInfo, string>(sGxID);     //获取数据库表中的一条记录
            if (model != null)
                GxName = model.Name;
            Expression<Func<tblReportType, bool>> where = (c => c.ID == mo.ReportTypeID && c.DelFlag == false);
            ReportTypeDataListManage reMan = new ReportTypeDataListManage();
            List<tblReportType> list = reMan.GetRecord(where);       //获取数据库表中的多条记录
            if (sEndYear == sStartYear)
                FileName = GxName + sStartYear + "年" + list[0].Name;
            else
                FileName = GxName + sStartYear + "-" + sEndYear + "年" + list[0].Name;
            mo.Name = FileName;

            ReportInfoDataListManage smpDlgMan = new ReportInfoDataListManage();
            string[] EditColumn = { "Name"};
            long nNewID = smpDlgMan.SaveOneSimpleRecord(mo, null, EditColumn);
            SetHiddenValue(PanelID + "_ExitCode", "1");

            switch (mo.ReportTypeID)
            {
                case 1:
                    JHReportManage JHReport = new JHReportManage();
                    JHReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
                case 2:
                    LQReportManage LQReport = new LQReportManage();
                    LQReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
                case 3:
                    if (!sSelTypeIDs.Contains("pcdmTable"))
                    {
                        X.Msg.Alert("注意", "招生批次必须选择，生成报表失败！").Show();
                        return this.Direct();
                    }
                    PCReportManage PCReport = new PCReportManage();
                    PCReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
                case 4:
                    if (!sSelTypeIDs.Contains("Department"))
                    {
                        X.Msg.Alert("注意", "院系必须选择，生成报表失败！").Show();
                        return this.Direct();
                    }
                    DYZYReportManage DYZYReport = new DYZYReportManage();
                    DYZYReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
                //    case "5":
                //FSReportManage FSReport = new FSReportManage();
                //FSReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType);
                //        break;
                case 6:
                    SYDReportManage SYDReport = new SYDReportManage();
                    SYDReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
                case 7:
                    FSReportManage FSReport = new FSReportManage();
                    FSReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, mo.ReportTypeID);
                    break;
            }

            //JHReportManage manReport = new JHReportManage();
            //manReport.CreateReport(sSelItems, sSelTypeIDs, sID, sGxID, sYear);


            //SetHiddenValue(PanelID + "_ExitCode", "1");


            return this.Direct();
        }






    }
}