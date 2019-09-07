using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System;

namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinZylbdmSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "详细信息";
            win.Resizable = false;
            SetWindowSize(win, 340, 275, sWidth, sHeight);
            return this.Direct();
        }


        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            zylbdmTableDataListManage manList = new zylbdmTableDataListManage();
            zylbdmTableDataTreeManage manTree = new zylbdmTableDataTreeManage();
            zylbdmTableManage man = new zylbdmTableManage();
            Td_zylbdm model = new Td_zylbdm();
            if (ParentVar.transValue.Contains("Row")) //修改
            {
                manList.InitSelection(ParentVar.transValue);
                model = manTree.GetSpecialModelFromCurrentID(manList.sRowNowID);
            }
            else            
            {
                Td_zylbdm parentModel = manTree.GetParentNode(Convert.ToInt64(ParentVar.transValue));
                model.ParentIndex = parentModel.ID;
                model.ParentName = manTree.GetAllParentName(parentModel.ID, true);
                model.Code = man.GetParentDM(parentModel.ID);
            }
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);
            return this.Direct();
        }

        public ActionResult DqdmCodeChange(string sChangeValue, string sNowID, string PanelID)
        {
            zylbdmTableManage man = new zylbdmTableManage();
            zylbdmTableDataTreeManage manTree = new zylbdmTableDataTreeManage();
            if (man.ExistDMValue(sChangeValue, sNowID))
            {
                X.Msg.Alert("注意", "输入的代码已经存在！").Show();
                SetHiddenValue(PanelID + "_ParentIndex", "-2");
                SetTextField(PanelID + "_ParentName", "");
            }
            else
            {
                Td_zylbdm model = man.GetParentObj(sChangeValue);
                if (model != null)
                {
                    SetHiddenValue(PanelID + "_ParentIndex", model.ID.ToString());
                    SetTextField(PanelID + "_ParentName", manTree.GetAllParentName(model.ID, true));
                }
                else
                {
                    SetHiddenValue(PanelID + "_ParentIndex", "-1");
                    SetTextField(PanelID + "_ParentName", "所有类别");
                }
            }
            return this.Direct();
        }

        public ActionResult WindowSaveClick(Td_zylbdm mo, string PanelID, string ParentVariable)
        {
            if (mo.ParentIndex == -2)
            {
                X.Msg.Alert("注意", "输入的代码有误！").Show();
                return this.Direct();
            }

            GetParentVariable(ParentVariable);
            string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
            zylbdmTableManage man = new zylbdmTableManage();
            man.SaveOneObj(mo);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}