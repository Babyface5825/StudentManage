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


namespace StudentManage.Areas.UserControl.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DataComboBoxController : ParentController
    {
        
        public virtual ActionResult InitComboBox(string ParentVariable, string sNowNames, string sNowValues, string sNowSigns)
        {
            GetParentVariable(ParentVariable);
            DataComboboxManage man = (DataComboboxManage)GetDataComboboxManage(ParentVar.KeyWords);
            return this.Store((object)man.GetDataComboboxModels(sNowNames, sNowValues, sNowSigns));
        }

        private void _SetComboBoxValue(string sComboName, string sNowID)
        {
            sNowID = sNowID.Remove(sNowID.IndexOf(CosValue.SPLITOPERATOR1));
            var Combobox = this.GetCmp<ComboBox>(sComboName);
            if (sNowID != "0")
            {
                if (sNowID == "-1")
                {
                    X.AddScript("App." + sComboName + ".setValue(App." + sComboName + ".getStore().getAt(0));");
                    X.AddScript("App." + sComboName + "_GetSelID.setValue(App." + sComboName + ".getValue());");
                }
                else
                {
                    Combobox.SetValue(sNowID);
                    SetHiddenValue(sComboName + "_GetSelID", sNowID);
                }
            }
            else
            {
                X.AddScript("App." + sComboName + "_GetSelID.setValue('0');");                
            }
        }

        /// <summary>
        /// 初始化完毕后，设定combobox当前选定项。调用combobox的母版上应该有一个hidden和其对应。
        /// 这个函数由Load调用，因此无法主动调用
        /// </summary>
        /// <param name="_PanelID"></param>
        /// <param name="sNowID">0清空当前combo;-1设定为第一个选项;其他值为相关选项</param>
        /// <returns></returns>
        public virtual ActionResult SetComboBoxValue(string PanelID, string sNowID)
        {            
            _SetComboBoxValue(PanelID, sNowID);
            return this.Direct();
        }

        public virtual ActionResult ChangeComboDisabled(string PanelID,  string disable)
        {
            if (disable == "1")
            {
                var combo = this.GetCmp<ComboBox>(PanelID);
                combo.Disabled = true;
            }
            else
            {
                var combo = this.GetCmp<ComboBox>(PanelID);
                combo.Disabled = false;
            }
            return this.Direct();
        }




        public ActionResult EditDataComboBox(string PanelID, string sSelID, string ParentVariable)
        {
            if ((sSelID == "0") || (sSelID == "-1") || (sSelID == "") || (sSelID == null))
            {
                //X.Msg.Alert("注意", "当前尚未选择任何项或者输入的选项不存在！").Show();
            }
            return this.Direct();
        }


        /// <summary>
        /// 选项改变，自动改变hidden主键
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sSelID"></param>
        /// <param name="sHiddenValue"></param>
        /// <returns></returns>
        public ActionResult SelectDataComboBox(string PanelID, string sSelID, string ParentVariable)
        {
            try
            {
                long lID = Convert.ToInt64(sSelID);
                SetHiddenValue(PanelID + "_GetSelID", sSelID);
            }
            catch
            {
                if ((sSelID != null)&& (sSelID != ""))
                {
                    GetParentVariable(ParentVariable);
                    DataComboboxManage man = (DataComboboxManage)GetDataComboboxManage(ParentVar.KeyWords);
                    man.sRowNowID = man.GetNowIDFromName(sSelID).ToString();
                    SetHiddenValue(PanelID + "_GetSelID", man.sRowNowID);
                }
                else
                    SetHiddenValue(PanelID + "_GetSelID", "-1");
            }
            return this.Direct();
        }

        /// <summary>
        /// 当hidden主键变化时对应此事件
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sSelID"></param>
        /// <param name="sHiddenValue"></param>
        /// <returns></returns>
        public ActionResult ChangeGetSelID(string PanelID, string sSelID, string ParentVariable)
        {
            PanelID = PanelID.Remove(PanelID.LastIndexOf('_'));
            GetParentVariable(ParentVariable);
            return RedirectToActionPermanent(ParentVar.actionName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, sSelID, ParentVar.transValue });
            
        }

        /// <summary>
        /// 当hidden主键变化时对应此事件
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sSelID"></param>
        /// <param name="sInitFlag"></param>
        /// <returns></returns>
        public ActionResult ChangeSetSelID(string PanelID, string sSelID, string sInitFlag)
        {
            if (sInitFlag == "0")
            {
                SetHiddenValue(PanelID + "_InitFlag", "1");
                RefreashDataComboBox(PanelID);
            }
            else
                _SetComboBoxValue(PanelID, sSelID);

            return this.Direct();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PanelID"></param>
        public void RefreashDataComboBox(string PanelID)
        {
            var store = this.GetCmp<Store>(PanelID + "_Store");
            store.LoadProxy();
        }

    }
}