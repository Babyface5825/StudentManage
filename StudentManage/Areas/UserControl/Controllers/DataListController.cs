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
using System.Data;

namespace StudentManage.Areas.UserControl.Controllers
{    
    public class DataListController : ParentController
    {
        #region 顶部按钮UI相关
        /// <summary>
        /// 通过GetCmp获取当前button
        /// </summary>
        /// <param name="sKeyWord">GridPanel的ID 关键字</param>
        /// <param name="nBtn">button编号</param>
        /// <returns></returns>
        private Button GetButton(string sKeyWord, int nBtn)
        {
            Button btn = null;
            switch (nBtn)
            {
                case DataListButton.btnAdd:  btn = this.GetCmp<Button>(sKeyWord + "_btnAdd"); break;
                case DataListButton.btnEdit: btn = this.GetCmp<Button>(sKeyWord + "_btnEdit"); break;
                case DataListButton.btnDelete: btn = this.GetCmp<Button>(sKeyWord + "_btnDelete"); break;
                case DataListButton.btnSearch: btn = this.GetCmp<Button>(sKeyWord + "_btnSearch"); break;
                case DataListButton.btnMoreSearch: btn = this.GetCmp<Button>(sKeyWord + "_btnMoreSearch"); break;
                case DataListButton.btnAll: btn = this.GetCmp<Button>(sKeyWord + "_btnAll"); break;
                case DataListButton.btnReset: btn = this.GetCmp<Button>(sKeyWord + "_btnReset"); break;
                case DataListButton.btnOrderUp: btn = this.GetCmp<Button>(sKeyWord + "_btnOrderUp"); break;
                case DataListButton.btnOrderDown: btn = this.GetCmp<Button>(sKeyWord + "_btnOrderDown"); break;
                case DataListButton.btnReverse: btn = this.GetCmp<Button>(sKeyWord + "_btnReverse"); break;
                case DataListButton.btnVerify: btn = this.GetCmp<Button>(sKeyWord + "_btnVerify"); break;
                case DataListButton.btnVerifyAll: btn = this.GetCmp<Button>(sKeyWord + "_btnVerifyAll"); break;                
                case DataListButton.btnExport: btn = this.GetCmp<Button>(sKeyWord + "_btnExport"); break;
                case DataListButton.btnRegularExport: btn = this.GetCmp<Button>(sKeyWord + "_btnRegularExport"); break;                
                case DataListButton.btnMore1: btn = this.GetCmp<Button>(sKeyWord + "_btnMore1"); break;
                case DataListButton.btnMore2: btn = this.GetCmp<Button>(sKeyWord + "_btnMore2"); break;
            }
            return btn;
        }

        /// <summary>
        /// 设置当前button的显示/隐藏状态
        /// </summary>
        /// <param name="sKeyWord">GridPanel的ID 关键字</param>
        /// <param name="nBtn">button编号</param>
        /// <param name="flag">true显示/false隐藏</param>
        public void SetButtonVisiable(string sKeyWord, int nBtn, bool flag = true)
        {
            if (nBtn != DataListButton.btnInport) 
            {
                Button btn = GetButton(sKeyWord, nBtn);
                btn.Hidden = !flag;
            }
            else
            {
                var btn = this.GetCmp<FileUploadField>(sKeyWord + "_btnInport");
                btn.Hidden = !flag;
            }
        }
        /// <summary>
        /// 设置当前button的名称
        /// </summary>
        /// <param name="sKeyWord">GridPanel的ID 关键字</param>
        /// <param name="nBtn">button编号</param>
        /// <param name="sName">设置的名称</param>
        public void SetButtonName(string sKeyWord, int nBtn, string sName)
        {
            if (nBtn != DataListButton.btnInport)
            {
                Button btn = GetButton(sKeyWord, nBtn);
                btn.Text = sName;
            }
            else
            {
                var btn = this.GetCmp<FileUploadField>(sKeyWord + "_btnInport");
                btn.ButtonText = sName;
            }
        }
        /// <summary>
        /// 设置当前button的Icon
        /// </summary>
        /// <param name="sKeyWord">GridPanel的ID 关键字</param>
        /// <param name="nBtn">button编号</param>
        /// <param name="icon">icon</param>
        public void SetButtonIcon(string sKeyWord, int nBtn, Icon icon)
        {
            if (nBtn != DataListButton.btnInport)
            {
                Button btn = GetButton(sKeyWord, nBtn);
                btn.Icon = icon;
            }
            else
            {
                var btn = this.GetCmp<FileUploadField>(sKeyWord + "_btnInport");
                btn.Icon = icon;
            }
        }

        /// <summary>
        /// 设置当前button的启动/禁用状态
        /// </summary>
        /// <param name="sKeyWord">GridPanel的ID 关键字</param>
        /// <param name="nBtn">button编号</param>
        /// <param name="flag">true禁用/false启用</param>
        public void SetButtonDisabled(string sKeyWord, int nBtn, bool flag = true)
        {
            if (nBtn != DataListButton.btnInport)
            {
                Button btn = GetButton(sKeyWord, nBtn);
                btn.Disabled = flag;
            }
            else
            {
                var btn = this.GetCmp<FileUploadField>(sKeyWord + "_btnInport");
                btn.Disabled = flag;
            }
        }


        #endregion

        #region setPanelColumn
        /// <summary>
        /// 设定/初始化一列。
        /// </summary>
        public void setPanelColumn(string sPanelID, ColumnModel mo, bool sort, int flex = 0, bool visiable = true)
        {
            int rfType = mo.WordType;
            var list = this.GetCmp<GridPanel>(sPanelID);
            if ((rfType == CosDataBase.STRING_TYPE)
                || (rfType == CosDataBase.INT_TYPE)
                || (rfType == CosDataBase.SMALLINT_TYPE)
                || (rfType == CosDataBase.LONGINT_TYPE)
                || (rfType == CosDataBase.STRINGL_TYPE)
                || (rfType == CosDataBase.SELSTR_TYPE)
                || (rfType == CosDataBase.DATETIME_TYPE)
                || (rfType == CosDataBase.DATE_TYPE)
                || (rfType == CosDataBase.TIME_TYPE)
                || (rfType == CosDataBase.BOOLEAN_TYPE)
                || (rfType == CosDataBase.JOIN_TYPE)
                || (rfType == CosDataBase.PARENTNAME_TYPE))
            {
                Column col = new Column();
                col.Text = mo.WordName;
                col.DataIndex = mo.WordValue;                
                col.Width = mo.ColumnWidth;
                col.Sortable = sort;
                //col.Visible = visiable;
                col.Hidden = !visiable;
                col.Flex = flex;
                list.AddColumn(col);
            }
            else if (rfType == CosDataBase.FLOAT_TYPE)
            {
                NumberColumn col = new NumberColumn();
                col.Text = mo.WordName;
                col.DataIndex = mo.WordValue;                
                string s = "0.";
                for (int j = 0; j < AppSettingValues.FLOATDECIMAL; j++)
                    s = s + "0";
                col.Format = s;
                col.Width = mo.ColumnWidth;
                col.Sortable = sort;
                col.Visible = visiable;
                col.Flex = flex;
                list.AddColumn(col);
            }
            else if (rfType == CosDataBase.BOOLEANBOX_TYPE)
            {
                CheckColumn col = new CheckColumn();
                col.Text = mo.WordName;
                col.DataIndex = mo.WordValue;                
                col.Width = 40;
                col.Sortable = sort;
                col.Visible = visiable;
                col.Flex = flex;
                list.AddColumn(col);
            }
        }

        #endregion

        #region setStoreField

        protected void setStoreField(string sKeyWord, ColumnModel model)
        {
            string sFieldName = model.WordValue;
            int rfType = model.WordType;
            setStoreField(sKeyWord, sFieldName, rfType);
        }

        protected void setStoreField(string sKeyWord, string sFieldName)
        {
            setStoreField(sKeyWord, sFieldName, CosDataBase.STRING_TYPE);
        }

        protected void setStoreField(string sKeyWord, string sFieldName, int rfType)
        {
            var store = this.GetCmp<Store>(sKeyWord + "_Store");
            ModelField rField = new ModelField(sFieldName);
            if ((rfType == CosDataBase.STRING_TYPE)
                || (rfType == CosDataBase.STRINGL_TYPE)
                || (rfType == CosDataBase.SELSTR_TYPE)
                || (rfType == CosDataBase.BOOLEAN_TYPE)
                || (rfType == CosDataBase.JOIN_TYPE)
                || (rfType == CosDataBase.PARENTNAME_TYPE))
                rField.Type = ModelFieldType.String;
            else if ((rfType == CosDataBase.DATETIME_TYPE)
                || (rfType == CosDataBase.DATE_TYPE)
                || (rfType == CosDataBase.TIME_TYPE))
                rField.Type = ModelFieldType.String;
            else if ((rfType == CosDataBase.INT_TYPE)
                || (rfType == CosDataBase.SMALLINT_TYPE)
                || (rfType == CosDataBase.LONGINT_TYPE))
                rField.Type = ModelFieldType.Int;
            else if (rfType == CosDataBase.FLOAT_TYPE)
                rField.Type = ModelFieldType.Float;
            else if ((rfType == CosDataBase.BOOLEANBOX_TYPE))
                rField.Type = ModelFieldType.Boolean;
            store.AddField(rField);
        }

        #endregion

        #region Init
        /// <summary>
        /// 初始化DataList顶部工具条的Button
        /// </summary>
        private void InitTopButton(string PanelID, DataListManage datalistMan, string Center_PageID)
        {
            foreach (ButtonModel mo in datalistMan.arrTopButton)
            {
                SetButtonVisiable(PanelID, mo.ButtonID);
                if (mo.ButtonName != null)
                    SetButtonName(PanelID, mo.ButtonID, mo.ButtonName);
                if (mo.ButtonIcon != Icon.Blank)
                    SetButtonIcon(PanelID, mo.ButtonID, mo.ButtonIcon);
            }

            if (datalistMan.bNeedAuthor)
            { 
                UserInfoManage userMan = new UserInfoManage();
                string[] key = { CookieKeys.USERROLES };
                string sUserRoles = string.Join("", userMan.GetLoginUserInfo(key));
                var authormodel = userMan.GetAccountOperationModel(sUserRoles, Center_PageID);
                if (!authormodel.AddFlag)
                {
                    SetButtonVisiable(PanelID, DataListButton.btnAdd, false);
                    SetButtonVisiable(PanelID, DataListButton.btnInport, false);
                }
                if (!authormodel.EditFlag)
                    SetButtonVisiable(PanelID, DataListButton.btnEdit, false);
                if (!authormodel.DeleteFlag)
                    SetButtonVisiable(PanelID, DataListButton.btnDelete, false);
                if (!authormodel.SearchFlag)
                {
                    SetButtonVisiable(PanelID, DataListButton.btnSearch, false);                    
                    SetButtonVisiable(PanelID, DataListButton.btnReset, false);
                    SetButtonVisiable(PanelID, DataListButton.btnExport, false);
                }
            }
        }

        public virtual void InitHeader(string PanelID, DataListManage datalistMan)
        {
            var list = this.GetCmp<GridPanel>(PanelID);
            var store = this.GetCmp<Store>(PanelID + "_Store");
            store.PageSize = AppSettingValues.DATALISTPAGELIMIT;
            
            store.RemoveFields();
            RowNumbererColumn col = new RowNumbererColumn();
            col.Width = 40;
            list.AddColumn(col);
            setStoreField(PanelID, "ID", CosDataBase.INT_TYPE);
            if (PanelID == "BMKSearch_DataList")
            {
                for (int i = 0; i < datalistMan.arrColumn.Count; i++)
                {
                    bool visiable = true;
                    if (((i >= 9) && (i != datalistMan.arrColumn.Count - 1))||(i==0))
                        visiable = false;
                    if (i < datalistMan.arrColumn.Count - 1)
                        setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 0, visiable);
                    else
                        setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 1, visiable);
                    setStoreField(PanelID, (ColumnModel)datalistMan.arrColumn[i]);
                }
            }else
            {
                for (int i = 0; i < datalistMan.arrColumn.Count; i++)
                {
                    bool visiable = true;
                    if ((i >= 9) && (i != datalistMan.arrColumn.Count - 1))
                        visiable = false;
                    if (i < datalistMan.arrColumn.Count - 1)
                        setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 0, visiable);
                    else
                        setPanelColumn(PanelID, (ColumnModel)datalistMan.arrColumn[i], datalistMan.bSort, 1, visiable);
                    setStoreField(PanelID, (ColumnModel)datalistMan.arrColumn[i]);
                }
            }

            list.SortableColumns = true;
        }

        /// <summary>
        /// 设置单选/多选方式
        /// </summary>
        /// <param name="clsDLManager"></param>
        private void SetSelectionModel(DataListManage datalistMan, string PanelID)
        {
            var selModel = this.GetCmp<RowSelectionModel>(PanelID + "_SelectedModel");
            if (datalistMan.bSingleSelect)
                selModel.Mode = SelectionMode.Single;
            else
                selModel.Mode = SelectionMode.Multi;
        }

        /// <summary>
        /// 对DataList的初始化，由BeforeRender的DirectEvent调用
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult InitDataList(string ParentVariable, string PanelID, string Center_PageID)
        {
            GetParentVariable(ParentVariable);
            DataListManage man = GetDataListManage(ParentVar.KeyWords, ParentVar.transValue);
            if (man != null)
            {
                InitTopButton(PanelID, man, Center_PageID);
                InitHeader(PanelID, man);
                SetSelectionModel(man, PanelID);
                if (man.bFirstVisit)
                {
                    SetHiddenValue(PanelID + "_VisitStatus", "1");
                    RefreashStore(PanelID);
                }
            }
            return this.Direct();
        }

        public void RefreashStore(string PanelID)
        {
            var store = this.GetCmp<Store>(PanelID + "_Store");
            X.AddScript("App." + PanelID + ".getSelectionMemory().clearMemory();");
            store.LoadProxy();
        }
        /// <summary>
        /// 获取当前Datalist中的数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual ActionResult DataListRead(StoreRequestParameters parameters, string ParentVariable, string sInitNames, string sInitValues, string sInitSigns, string sInitAndOrs, string sNowNames, string sNowValues, string sNowSigns, string sNowAndOrs, string sVisitStatus)
        {
            GetParentVariable(ParentVariable);
            DataListManage datalistMan = (DataListManage)GetDataListManage(ParentVar.KeyWords, ParentVar.transValue);
            if ((sVisitStatus == "1") || datalistMan.bFirstVisit)
            {
                datalistMan.paremeters = new StoreRequestParameters();
                datalistMan.paremeters = parameters;

                string sNames = sInitNames;
                string sValues = sInitValues;
                string sSigns = sInitSigns;
                string sAndOrs = sInitAndOrs;

                if (sInitNames != "")
                {
                    if (sSigns == "")
                    {
                        string[] ss = sNames.Split(CosValue.SPLITOPERATOR1);
                        sSigns = string.Join(CosValue.SPLITOPERATOR1.ToString(), (string[])NewDuplicateArray(SIGN.Equal, ss.Length).ToArray(typeof(string)));
                    }
                }

                if (sNowNames != "")
                {
                    if (sNames != "")
                    {
                        sNames = sNames + CosValue.SPLITOPERATOR1 + sNowNames;
                        sValues = sValues + CosValue.SPLITOPERATOR1 + sNowValues;
                        sSigns = sSigns + CosValue.SPLITOPERATOR1 + sNowSigns;
                        sAndOrs = sAndOrs + CosValue.SPLITOPERATOR1 + sNowAndOrs;
                    }
                    else
                    {
                        sNames = sNowNames;
                        sValues = sNowValues;
                        sSigns = sNowSigns;
                        sAndOrs = sNowAndOrs;
                    }
                }
                return this.Store(datalistMan.InitStore(sNames, sValues, sSigns));
            }
            else
                return null;
        }

        #endregion

        #region 按钮事件相关
        public ActionResult PageChange(string PanelID)
        {
            X.AddScript("App." + PanelID + ".getSelectionMemory().clearMemory();"); 
            return this.Direct();
        }

        protected ActionResult ClickCore(string ParentVariable, string selection, string PanelID, string ClickName)
        {
            if (ParentVariable != null)
            {
                GetParentVariable(ParentVariable);
                return RedirectToActionPermanent(ClickName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, selection, keywords = ParentVar.KeyWords, transvalue = ParentVar.transValue});
            }
            else
                return this.Direct();
        }

        public ActionResult DataList_Select()
        {
            X.Msg.Alert("11", "11").Show();
            return this.Direct();
        }

        /// <summary>
        /// 点击DataList的topbar上的新增按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_AddClick(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnAddClick");
        }

        /// <summary>
        /// 点击DataList的topbar上的修改按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_EditClick(string ParentVariable, string selection, string PanelID)
        {
            List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(selection);
            if (sel == null)
            {
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
                return this.Direct();
            }
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnEditClick");
        }

        /// <summary>
        /// 点击DataList的topbar上的删除按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_DeleteClick(string ParentVariable, string selection, string PanelID)
        {
            List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(selection);
            if (sel == null)
            {
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
                return this.Direct();
            }
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnDeleteClick");
        }

        /// <summary>
        /// 点击DataList的topbar上的上移按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_OrderUpClick(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnOrderUpClick");
        }

        /// <summary>
        /// 点击DataList的topbar上的下移按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_OrderDownClick(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnOrderDownClick");
        }

        /// <summary>
        /// 点击DataList的topbar上的查询按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_SearchClick(string ParentVariable, string PanelID)
        {            
            return ClickCore(ParentVariable, "", PanelID, "DataList_ReturnSearchClick");
        }
        
        /// <summary>
        /// 点击DataList的topbar上的重置按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_ResetClick(string ParentVariable, string PanelID)
        {               
            return ClickCore(ParentVariable, "", PanelID, "DataList_ReturnResetClick");
        }
        /// <summary>
        /// 单击DataList上的某一行
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <param name="selection"></param>
        /// <returns></returns>
        public ActionResult DataList_ListRowClick(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnListRowClick");
        }

        public ActionResult DataList_More1Click(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnMore1Click");
        }
        public ActionResult DataList_More2Click(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnMore2Click");
        }

        public ActionResult DataList_InportClick(string ParentVariable)
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;

            //首先把文件临时copy到服务器上
            GetParentVariable(ParentVariable);
            var upFile = this.GetCmp<FileUploadField>(ParentVar.controlName + "_DataList_btnInport");
            if (upFile.HasFile)
            {
                FileManager clsFile = new FileManager();
                string sFilePath = ConstAddressPath.TEMP_WEBPATH;
                string sAddress = DateTime.Now.ToFileTimeUtc().ToString() + upFile.FileName.Remove(0, upFile.FileName.LastIndexOf('.'));
                if (clsFile.UploadFile(upFile, sFilePath + sAddress))
                {
                    SetHiddenValue(ParentVar.controlName + "_DataList_txtInportFileName", sAddress);
                    return result;
                } 
            }
            result.IsUpload = false;            
            return result;
        }

        public ActionResult DataList_AfterInportClick(string ParentVariable, string sAddress, string PanelID)
        {            
            return ClickCore(ParentVariable, sAddress, PanelID, "DataList_ReturnInportClick");            
        }

        public ActionResult DataList_ExportClick(string ParentVariable, string selection, string PanelID)
        {
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnExportClick");
        }

        public ActionResult DataList_RegularExportClick(string ParentVariable, string sInitNames, string sInitValues, string sInitSigns, string sNowNames, string sNowValues, string sNowSigns)
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;

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
                    sSigns = sSigns + CosValue.SPLITOPERATOR1 + sNowSigns;
                }
                else
                {
                    sNames = sNowNames;
                    sValues = sNowValues;
                    sSigns = sNowSigns;
                }
            }
            GetParentVariable(ParentVariable);
            DataListManage man = GetDataListManage(ParentVar.KeyWords, ParentVar.transValue);
            DataTable dt = man.GetSomeRecordForExcel(sNames, sValues, sSigns);
            ExcelOut ex = new ExcelOut();
            if (!ex.ExcelCreateAndDownload(dt))
            {
                X.Msg.Alert("错误", "导出文件出错！").Show();
            }
            return result;
        }
        /// <summary>
        /// 点击DataList的topbar上的审核按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_VerifyClick(string ParentVariable, string selection, string PanelID)
        {
            List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(selection);
            if (sel == null)
            {
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
                return this.Direct();
            }
            return ClickCore(ParentVariable, selection, PanelID, "DataList_ReturnVerifyClick");
        }
        /// <summary>
        /// 点击DataList的topbar上的审核全部按钮事件
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        public ActionResult DataList_VerifyAllClick(string ParentVariable, string sInitNames, string sInitValues, string sInitSigns, string sNowNames, string sNowValues, string sNowSigns)
        {
            //string sNames = sInitNames;
            //string sValues = sInitValues;
            //string sSigns = sInitSigns;

            //if (sInitNames != "")
            //{
            //    if (sSigns == "")
            //    {
            //        string[] ss = sNames.Split(CosValue.SPLITOPERATOR1);
            //        for (int i = 0; i < ss.Length; i++)
            //            sSigns = SIGN.Equal.ToString() + CosValue.SPLITOPERATOR1;
            //        sSigns = sSigns.Remove(sSigns.Length - 1, 1);
            //    }
            //}
            //if (sNowValues != "")
            //{
            //    if (sNames != "")
            //    {
            //        sNames = sNames + CosValue.SPLITOPERATOR1 + sNowNames;
            //        sValues = sValues + CosValue.SPLITOPERATOR1 + sNowValues;
            //        sSigns = sSigns + CosValue.SPLITOPERATOR1 + sNowSigns;
            //    }
            //    else
            //    {
            //        sNames = sNowNames;
            //        sValues = sNowValues;
            //        sSigns = sNowSigns;
            //    }
            //}
            //GetParentVariable(ParentVariable);
            //var man = (DataListManage)GetDataListManage(ParentVar.transValue);
            //man.VerifyAllRecords(man.GetAllModels(sNames, sValues, sSigns));
            //RefreashDataList(ParentVar.actionName);
            return this.Direct();
        }
        #endregion

    }
}