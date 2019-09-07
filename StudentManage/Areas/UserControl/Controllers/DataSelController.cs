using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Controllers;
using StudentManage.Models;
using Ext.Net;
using System.Collections;

namespace StudentManage.Areas.UserControl.Controllers
{    
    public class DataSelController : DataListController
    {

        protected DataListManage GetDataListManager(string sKeyWord = null)
        {            
            if (sKeyWord.Contains("Student"))
            {
                StudentInfoDataSelManage datalistMan = new StudentInfoDataSelManage();
                return datalistMan;
            }
                        
            return null;
        }

        #region Init

        public virtual ActionResult InitComboBox(string PanelID)
        {
            DataListManage datalistMan = (DataListManage)GetDataListManager(PanelID);
            return this.Store(datalistMan.GetModelsForComboSearch());
        }

        public virtual ActionResult SetDataCombo(string PanelID)
        {
            DataListManage datalistMan = (DataListManage)GetDataListManager(PanelID);
            datalistMan.SetarrColumn();
            var combo = this.GetCmp<ComboBox>(PanelID);
            combo.SetValue(((ColumnModel)datalistMan.arrColumn[0]).WordValue);
            return this.Direct();
        }

        public override void InitHeader(string PanelID, DataListManage datalistMan)
        {
            var list = this.GetCmp<GridPanel>(PanelID);
            var store = this.GetCmp<Store>(PanelID + "_Store");
            store.PageSize = AppSettingValues.DATALISTPAGELIMIT;
            store.RemoveFields();
            setStoreField(PanelID, "ID", CosDataBase.INT_TYPE);
            for (int i = 0; i < datalistMan.arrColumn.Count; i++)
            {
                bool visiable = true;
                if (i >= 10)
                    visiable = false;
                if (i < datalistMan.arrColumn.Count - 1)
                    setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 0, visiable);
                else
                    setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 1, visiable);
                setStoreField(PanelID, (ColumnModel)datalistMan.arrColumn[i]);
            }
            list.SortableColumns = true;
        }

        /// <summary>
        /// 对DataList的初始化，由BeforeRender的DirectEvent调用
        /// </summary>
        /// <returns></returns>
        public override ActionResult InitDataList(string ParentVariable, string PanelID, string Center_PageID)
        {
            PanelID = PanelID + "_DataSel";
            DataListManage datalistMan = (DataListManage)GetDataListManager(ParentVariable);            
            InitHeader(PanelID, datalistMan);
            if (datalistMan.bFirstVisit)
                RefreashStore(PanelID);
            return this.Direct();
        }

        /// <summary>
        /// 获取当前Datalist中的数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult DataSelRead(StoreRequestParameters parameters, string ParentVariable, string sInitNames, string sInitValues, string sInitSigns, string sNowNames, string sNowValues, string sNowSigns, string sVisitStatus)
        {
            DataListManage datalistMan = (DataListManage)GetDataListManager(ParentVariable);
            datalistMan.paremeters = new StoreRequestParameters();
            datalistMan.paremeters = parameters;
                        
            string sNames = sInitNames;
            string sValues = sInitValues;
            string sSigns = sInitSigns;

            if (sInitNames != "")
            {
                if (sSigns == "")
                {
                    string[] ss = sNames.Split(CosValue.SPLITOPERATOR1);
                    for (int i = 0; i < ss.Length; i++)
                        sSigns = SIGN.Equal.ToString() + CosValue.SPLITOPERATOR1;
                    sSigns = sSigns.Remove(sSigns.Length - 1, 1);
                }
            }

            if (sNowValues != "")
            {
                if (sNames != "")
                {
                    sNames = sNames + CosValue.SPLITOPERATOR1 + sNowNames;
                    sValues = sValues + CosValue.SPLITOPERATOR1 + sNowValues;
                    sSigns = sSigns + CosValue.SPLITOPERATOR1 + SIGN.Like;
                }
                else
                {
                    sNames = sNowNames;
                    sValues = sNowValues;
                    sSigns = SIGN.Like.ToString();
                }
            }

            return this.Store(datalistMan.InitStore(sNames, sValues, sSigns));            
        }

        #endregion

        #region button

        public ActionResult DataSel_SearchClick(string PanelID, string SearchName, string SearchValue)
        {
            SetHiddenValue(PanelID + "_NowNames", SearchName);
            SetHiddenValue(PanelID + "_NowValues", SearchValue);
            RefreashStore(PanelID + "_DataSel");
            return this.Direct();
        }

        //public ActionResult DataSel_SureClick(string ParentVariable, string selection)
        //{
        //    List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(selection);
        //    if (sel == null)
        //    {
        //        X.Msg.Alert("注意", "尚未选择任何行！").Show();
        //        return this.Direct();
        //    }
        //    string sReturnIDs = "";
        //    foreach (SelectionModel m in sel)
        //        sReturnIDs = sReturnIDs + m.RecordID + CosValue.SPLITOPERATOR2;
        //    if (sReturnIDs.Length > 0)
        //        sReturnIDs = sReturnIDs.Remove(sReturnIDs.Length - 1);
        //    return ClickCore(ParentVariable, sReturnIDs, "DataSel_SureClick");
        //}


        #endregion
    }
}