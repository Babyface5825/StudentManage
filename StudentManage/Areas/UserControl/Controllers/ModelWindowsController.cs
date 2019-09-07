using System.Web.Mvc;
using Ext.Net.MVC;
using StudentManage.Controllers;
using StudentManage.Models;
using StudentManage.Manage;
using Ext.Net;
using System;

namespace StudentManage.Areas.UserControl.Controllers
{
    public class ModelWindowsController : ParentController
    {
        // GET: Parent
        public virtual ActionResult WindowCommonHide(string ParentVariable, string PanelID, string sExitCode)
        {
            if (sExitCode == "1")
            {
                GetParentVariable(ParentVariable);
                return RedirectToActionPermanent(ParentVar.actionName, ParentVar.controlName, new { area = ParentVar.areaName, PanelID, transValue = ParentVar.transValue });
            }
            else
                return this.Direct();
        }

        protected void SetWindowSize(Window win, int nSetWidth, int nSetHeight, string sPanelWidth, string sPanelHeight)
        {
            win.Width = nSetWidth;
            win.Height = nSetHeight;
            int nWidth = Convert.ToInt32(sPanelWidth);
            int nHeight = Convert.ToInt32(sPanelHeight);            
            win.SetPosition((nWidth - nSetWidth) / 2, (nHeight - nSetHeight) / 2);            
        }

        public virtual ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            return this.Direct();
        }
        public virtual ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
            string[] sss = new string[2];
            sss[0] = ss[0];
            if (ss.Length == 2)
                sss[1] = ss[1];
            else
                sss[1] = null;
            DataListManage man = GetDataListManage(sss[0], sss[1]);
                        
            man.InitSelection(ParentVar.transValue);
            var model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);
            return this.Direct();
        }
    }
}