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
using System.Linq.Expressions;
using System.Collections;

namespace StudentManage.Areas.UserControl.Controllers
{
    public class ThreeListComboBoxController : ParentController
    {

        #region InitThreeComboBox
        private ActionResult InitThreeComboBox(string PanelID, string ParentVariable, string sParentID, string sNowNames, string sNowValues, string sNowSigns)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage datatreeMan = GetTreeManage(ParentVar.KeyWords);
            if ((sParentID != null) && (sParentID != "0") && (sParentID != ""))
                return this.Store(datatreeMan.GetSomeTreeModels(Convert.ToInt64(sParentID), "0", sNowNames, sNowValues, sNowSigns));
            else
                return null;
        }

        public virtual ActionResult InitThreeComboBox1(string PanelID, string ParentVariable, string sParentID, string sNowNames, string sNowValues, string sNowSigns)
        {
            if (sNowNames != "")
            {
                sNowNames = sNowNames + CosValue.SPLITOPERATOR1 + "TheLevel";
                sNowValues = sNowValues + CosValue.SPLITOPERATOR1 + "1";
                sNowSigns = sNowSigns + CosValue.SPLITOPERATOR1 + SIGN.Equal.ToString();
            }
            else
            {
                sNowNames = "TheLevel";
                sNowValues = "1";
                sNowSigns = SIGN.Equal.ToString();
            }
            return InitThreeComboBox(PanelID, ParentVariable, sParentID, sNowNames, sNowValues, sNowSigns);
        }
        public virtual ActionResult InitThreeComboBox2(string PanelID, string ParentVariable, string sParentID, string sNowNames, string sNowValues, string sNowSigns)
        {
            if (sNowNames != "")
            {
                sNowNames = sNowNames + CosValue.SPLITOPERATOR1 + "TheLevel";
                sNowValues = sNowValues + CosValue.SPLITOPERATOR1 + "2";
                sNowSigns = sNowSigns + CosValue.SPLITOPERATOR1 + SIGN.Equal.ToString();
            }
            else
            {
                sNowNames = "TheLevel";
                sNowValues = "2";
                sNowSigns = SIGN.Equal.ToString();
            }
            return InitThreeComboBox(PanelID, ParentVariable, sParentID, sNowNames, sNowValues, sNowSigns);
        }

        public virtual ActionResult InitThreeComboBox3(string PanelID, string ParentVariable, string sParentID, string sNowNames, string sNowValues, string sNowSigns)
        {
            if (sNowNames != "")
            {
                sNowNames = sNowNames + CosValue.SPLITOPERATOR1 + "TheLevel";
                sNowValues = sNowValues + CosValue.SPLITOPERATOR1 + "3";
                sNowSigns = sNowSigns + CosValue.SPLITOPERATOR1 + SIGN.Equal.ToString();
            }
            else
            {
                sNowNames = "TheLevel";
                sNowValues = "3";
                sNowSigns = SIGN.Equal.ToString();
            }
            return InitThreeComboBox(PanelID, ParentVariable, sParentID, sNowNames, sNowValues, sNowSigns);
        }
        #endregion

        #region SetComboBoxValue
        private void SetComboBoxValue(string sComboName, string sNowID)
        {
            string PanelID = sComboName.Remove(sComboName.LastIndexOf('_'));
            if ((sNowID != "0") && (sNowID != ""))
            {                
                if (sNowID == "-1")
                {                    
                    if (sComboName.Contains("1"))
                    {
                        X.AddScript("App." + sComboName + ".setValue(App." + sComboName + ".getStore().getAt(0));");
                        X.AddScript("App." + PanelID + "_SelID1.setValue(App." + sComboName + ".getValue());");
                        RefreashThreeListStore(PanelID, "_cmbThreeCombo2");
                    }
                    else if (sComboName.Contains("2"))
                    {
                        
                        X.AddScript("if (App." + sComboName + ".getStore().getCount()==0) {");
                        X.AddScript("App." + PanelID + "_GetSelID.setValue(App." + PanelID + "_SelID1.getValue()); ");
                        X.AddScript("App." + sComboName + ".setValue('');}");
                        X.AddScript("else {");
                        X.AddScript("App." + sComboName + ".setValue(App." + sComboName + ".getStore().getAt(0));");
                        X.AddScript("App." + PanelID + "_SelID2.setValue(App." + sComboName + ".getValue());}");
                        RefreashThreeListStore(PanelID, "_cmbThreeCombo3");
                    }
                    else
                    {
                        X.AddScript("if (App." + sComboName + ".getStore().getCount()==0){ ");
                        X.AddScript("App." + sComboName + ".setValue('');");
                        X.AddScript("if (App." + PanelID + "_SelID2.getValue()!='-1') ");
                        X.AddScript("App." + PanelID + "_GetSelID.setValue(App." + PanelID + "_SelID2.getValue());} ");
                        X.AddScript("else {");
                        X.AddScript("App." + sComboName + ".setValue(App." + sComboName + ".getStore().getAt(0));");
                        X.AddScript("App." + PanelID + "_SelID3.setValue(App." + sComboName + ".getValue());");
                        X.AddScript("App." + PanelID + "_GetSelID.setValue(App." + sComboName + ".getValue());}");
                    }
                }
                else
                    X.AddScript("App." + sComboName + ".setValue(" + sNowID + ");");
            }
            else
            {
                var Combobox = this.GetCmp<ComboBox>(sComboName);
                Combobox.ClearValue();
            }
        }


        public virtual ActionResult SetThreeListCombo1(string PanelID, string sNowID)
        {            
            SetComboBoxValue(PanelID + "_cmbThreeCombo1", sNowID);
            return this.Direct();
        }
        public virtual ActionResult SetThreeListCombo2(string PanelID, string sNowID)
        {         
            SetComboBoxValue(PanelID + "_cmbThreeCombo2", sNowID);
            return this.Direct();
        }
        public virtual ActionResult SetThreeListCombo3(string PanelID, string sNowID)
        {            
            SetComboBoxValue(PanelID + "_cmbThreeCombo3", sNowID);
            return this.Direct();
        }
        #endregion
        
        public virtual ActionResult SelectThreeComboBox1(string PanelID, string sSelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage datatreeMan = GetTreeManage(ParentVar.KeyWords);
            datatreeMan.nNowID = datatreeMan.GetNowIDFromName(sSelID, "-1");
            datatreeMan.GetAllIDs();
            SetHiddenValue(PanelID + "_SelID1", datatreeMan.nNowID1.ToString());
            SetHiddenValue(PanelID + "_SelID2", datatreeMan.nNowID2.ToString());
            SetHiddenValue(PanelID + "_SelID3", datatreeMan.nNowID3.ToString());
            SetHiddenValue(PanelID + "_GetSelID", datatreeMan.nNowID.ToString());
            RefreashThreeListStore(PanelID, "_cmbThreeCombo2");
            RefreashThreeListStore(PanelID, "_cmbThreeCombo3");
            return this.Direct();
        }

        public virtual ActionResult SelectThreeComboBox2(string PanelID, string sParentID, string sSelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage datatreeMan = GetTreeManage(ParentVar.KeyWords);
            datatreeMan.nNowID = datatreeMan.GetNowIDFromName(sSelID, sParentID);
            datatreeMan.GetAllIDs();
            SetHiddenValue(PanelID + "_SelID2", datatreeMan.nNowID2.ToString());
            SetHiddenValue(PanelID + "_SelID3", datatreeMan.nNowID3.ToString());
            if (datatreeMan.nNowID != 0)
                SetHiddenValue(PanelID + "_GetSelID", datatreeMan.nNowID.ToString());
            else
                SetHiddenValue(PanelID + "_GetSelID", sParentID);
            var combobox3 = this.GetCmp<ComboBox>(PanelID + "_cmbThreeCombo3");
            combobox3.GetStore().LoadProxy();
            return this.Direct();
        }


        public virtual ActionResult SelectThreeComboBox3(string PanelID, string sParentID, string sSelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage datatreeMan = GetTreeManage(ParentVar.KeyWords);
            datatreeMan.nNowID = datatreeMan.GetNowIDFromName(sSelID, sParentID);
            datatreeMan.GetAllIDs();
            SetHiddenValue(PanelID + "_SelID3", datatreeMan.nNowID3.ToString());
            if (datatreeMan.nNowID != 0)
                SetHiddenValue(PanelID + "_GetSelID", datatreeMan.nNowID.ToString());
            else
                SetHiddenValue(PanelID + "_GetSelID", sParentID);
            return this.Direct();
        }

        public ActionResult ChangeGetSelID(string PanelID, string sSelID, string ParentVariable)
        {
            if ((sSelID != null) && (sSelID != "") && (sSelID != "-1"))
            {
                PanelID = PanelID.Remove(PanelID.LastIndexOf('_'));
                GetParentVariable(ParentVariable);
                return RedirectToActionPermanent(ParentVar.actionName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, sSelID, ParentVar.transValue });
            }
            else
                return this.Direct();
        }
        
        public ActionResult ChangeSetSelID(string PanelID, string sSelID, string ParentVariable)
        {
            sSelID = sSelID.Remove(sSelID.IndexOf(CosValue.SPLITOPERATOR1));
            GetParentVariable(ParentVariable);
            if (sSelID == "")
                sSelID = "0";
            DataTreeManage man = GetTreeManage(ParentVar.KeyWords);
            man.nNowID = Convert.ToInt64(sSelID);
            man.GetAllIDs();
            SetHiddenValue(PanelID + "_SelID1", man.nNowID1.ToString());
            RefreashThreeListStore(PanelID, "_cmbThreeCombo1");
            SetHiddenValue(PanelID + "_SelID2", man.nNowID2.ToString());
            SetHiddenValue(PanelID + "_SelID3", man.nNowID3.ToString());
            if (man.nNowID != -1)
            {
                SetHiddenValue(PanelID + "_GetSelID", man.nNowID.ToString());
                RefreashThreeListStore(PanelID, "_cmbThreeCombo2");
                RefreashThreeListStore(PanelID, "_cmbThreeCombo3");
            }
            return this.Direct();
        }

        public virtual ActionResult ChangeThreeListDisabled(string PanelID, string disable)
        {
            var combo1 = this.GetCmp<ComboBox>(PanelID + "_cmbThreeCombo1");
            var combo2 = this.GetCmp<ComboBox>(PanelID + "_cmbThreeCombo2");
            var combo3 = this.GetCmp<ComboBox>(PanelID + "_cmbThreeCombo3");

            if (disable == "0")
            {
                combo1.Disabled = false;
                combo2.Disabled = false;
                combo3.Disabled = false;
            }
            else if (disable == "1")
            {
                combo1.Disabled = true;
                combo2.Disabled = false;
                combo3.Disabled = false;
            }
            else if (disable == "2")
            {
                combo1.Disabled = true;
                combo2.Disabled = true;
                combo3.Disabled = false;
            }
            else if (disable == "3")
            {
                combo1.Disabled = true;
                combo2.Disabled = true;
                combo3.Disabled = true;
            }
            return this.Direct();
        }
        public void RefreashThreeListStore(string PanelID, string ComboName)
        {
            var combo = this.GetCmp<ComboBox>(PanelID + ComboName);
            combo.GetStore().LoadProxy();
        }



    }
}