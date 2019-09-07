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
    public class WinNeedConfirmController : ParentController
    {
        public ActionResult WinNeedConfirm_Show(string PanelID, string HideVariable, string sHint= "确定删除选定内容吗？")
        {
            var lab = this.GetCmp<Label>(PanelID + "_labHint");            
            lab.Text = sHint;
            SetHiddenValue(PanelID + "_HiddenVariable", HideVariable);
            return ShowWindow(PanelID);            
        }

        public ActionResult WinNeedConfirm_Sure(string PanelID, string HideVariable)
        {            
            GetParentVariable(HideVariable);            
            return RedirectToActionPermanent(ParentVar.actionName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, ID = ParentVar.transValue, SureType = true });
        }
        public ActionResult WinNeedConfirm_Cancel(string PanelID, string HideVariable)
        {
            GetParentVariable(HideVariable);
            return RedirectToActionPermanent(ParentVar.actionName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, ID = ParentVar.transValue, SureType = false });
        }

    }
}