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

namespace StudentManage.Areas.UserControl.Controllers
{
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class WinUserMoreController : ModelWindowsController
    {

        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "详细信息";
            win.Resizable = false;
            SetWindowSize(win, 630, 400, sWidth, sHeight);
            return this.Direct();
        }

        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            setParentVariable("UserControl", "WinUserMore", "WinUserMore_SelectDepartmentChange", null, "DepartmentInfo");
            SetHiddenValue(PanelID + "_cmbDepartmentInfo_ParentVariable", ParentVar.DefaultVal);

            setParentVariable("UserControl", "WinUserMore", "WinUserMore_SelectDutyChange", null, "DutyInfo");
            SetHiddenValue(PanelID + "_cmbDutyInfo_ParentVariable", ParentVar.DefaultVal);

            UserInfoManage userMan = new UserInfoManage();
            GetParentVariable(ParentVariable);
            //如果userId中包含Row，说明是从DataList传来的selection
            //否则就应该只是userinfo的一个主键值
            string sUserID = ParentVar.transValue;
            if (ParentVar.transValue.Contains("Row"))
            {
                UserInfoDataListManage userDataListMan = new UserInfoDataListManage();
                userDataListMan.InitSelection(ParentVar.transValue);
                sUserID = userDataListMan.sRowNowID;
            }
            tblUserInfo model = userMan.GetOneUserModel(sUserID); 

            //如果是新增，model.ID=0，那么获取传过来的departmentId和DutyId
            if (model.ID == 0)
            {
                string[] ss = ParentKey.Split(CosValue.SPLITOPERATOR1);
                model.UserIDepartmentIID = Convert.ToInt64(ss[0]);
                if (ss.Length > 1)
                    model.UserIDutyIID = Convert.ToInt64(ss[1]);
                else
                    model.UserIDutyIID = 0;
            }
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);

            SetHiddenValue(PanelID + "_ExitCode", "0");

            var store = this.GetCmp<Store>(PanelID + "_AllRoleStore");
            store.LoadProxy();
            store = this.GetCmp<Store>(PanelID + "_NowRoleStore");
            store.LoadProxy();

            SetThreeListComboValue(PanelID + "_cmbDepartmentInfo", model.UserIDepartmentIID.ToString());

            if ((model.UserIDutyIID == 0) || (model.UserIDutyIID == null))
                SetComboValue(PanelID + "_cmbDutyInfo", "-1", true);
            else
                SetComboValue(PanelID + "_cmbDutyInfo", model.UserIDutyIID.ToString(), true);

            Paging<object> obj = userMan.GetNowAuthorStore(sUserID);
            SetHiddenValue(PanelID + "_OriginAuthorIDs", string.Join(CosValue.SPLITOPERATOR2.ToString(), userMan.NowAuthorIDs.ToArray()));
            
            return this.Direct();
        }

        public ActionResult WinUserMore_ClearDepartmentClick(string PanelID)
        {
            SetThreeListComboValue(PanelID + "_cmbDepartmentInfo", "0");
            return this.Direct();
        }
        public ActionResult WinUserMore_ClearDutyClick(string PanelID)
        {
            SetComboValue(PanelID + "_cmbDutyInfo", "0", true);
            return this.Direct();
        }

        public ActionResult WinUserMore_SelectDutyChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelDutyID", sSelID);
            return this.Direct();
        }

        public ActionResult WinUserMore_SelectDepartmentChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelDepartmentID", sSelID);
            return this.Direct();
        }

        public ActionResult WinUserMore_UserAuthorAllRead(string ParentVariable, string sNowUserID)
        {
            //权限选择界面只会出现在用户信息管理中，只有管理员能够进入此界面
            UserInfoManage userMan = new UserInfoManage();
            return this.Store(userMan.GetAllAuthorStore(sNowUserID));
        }
        public ActionResult WinUserMore_UserAuthorNowRead(string ParentVariable, string sNowUserID)
        {
            UserInfoManage userMan = new UserInfoManage();
            Paging<object> obj = userMan.GetNowAuthorStore(sNowUserID);            
            return this.Store(obj);
        }
        
        public virtual ActionResult WinUserMore_AddRoleClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllRolePanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowRolePanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            return this.Direct();
        }

        public virtual ActionResult WinUserMore_AddAllRoleClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllRolePanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowRolePanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            return this.Direct();
        }

        public virtual ActionResult WinUserMore_RemoveRoleClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllRolePanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowRolePanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            return this.Direct();
        }

        public virtual ActionResult WinUserMore_RemoveAllRoleClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllRolePanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowRolePanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            return this.Direct();
        }
        public ActionResult WindowSaveClick(tblUserInfo model, string PanelID, string ParentVariable)//, string sSelAuthors, string sSetAuthors, string sOriginAuthors)
        {            
            DirectResult result = new DirectResult();
            result.Success = true;

            UserInfoManage userMan = new UserInfoManage();
            //校验用户名和密码是否填写
            if ((model.UserICode == null) || (model.UserICode == "") || (model.UserIPsd == "") || (model.UserIPsd == null) )
            {
                result.ErrorMessage = "用户名和密码不能为空！";
                result.Success = false;
                return result;
            }
            //校验如果用户名修改了，是否会有重复
            Expression<Func<tblUserInfo, bool>> where = (c => c.UserICode == model.UserICode  && c.DelFlag == false);
            List<tblUserInfo> list = userMan.GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                if (list[0].ID != model.ID)
                {
                    result.ErrorMessage = "用户名已经存在，不能使用！";
                    result.Success = false;
                    return result;
                }
            }

            model.UserIPsd = userMan.GetSecretPass(model.UserIPsd);
            long lUserID = model.ID;

            if (model.UserIDutyIID == 0)
                model.UserIDutyIID = null;
            if (model.UserIDepartmentIID == 0)
            {
                model.UserIDepartmentIID = null;
                model.WholeDepartmentName = "";
            }
            else
            {
                DepartmentInfoManage departmentMan = new DepartmentInfoManage();
                model.WholeDepartmentName = departmentMan.GetWholeDepartmentName(Convert.ToInt64(model.UserIDepartmentIID));
            }

            if (lUserID == 0)
                lUserID = userMan.AddOneRecord(model);
            else
                userMan.SaveOneRecord(model);

            if (lUserID == 0)
            {
                result.ErrorMessage = "保存出错！";
                result.Success = false;
                return result;
            }
            X.AddScript("var i = 0; var sSelAuthors = \"\"; while (i < App." + PanelID + "_NowRoleStore" + ".getCount()) { sSelAuthors = sSelAuthors + App." + PanelID + "_NowRoleStore" + ".getAt(i).get('ID') + \",\"; i++; } sSelAuthors = sSelAuthors.slice(0, sSelAuthors.length - 1);");
            X.AddScript("App.direct.UserControl.WinUserMore_SaveAuthorsDirectMethod(" + lUserID + ",sSelAuthors, App." + PanelID + "_SetAuthorID" + ".getValue(),App." + PanelID + "_OriginAuthorIDs" + ".getValue(),\"" + PanelID + "\");");
            return result;
        }

        [DirectMethod]
        public ActionResult WinUserMore_SaveAuthorsDirectMethod(long lUserID, string sSelAuthors, string sSetAuthors, string sOriginAuthors, string PanelID)
        {
            UserInfoManage userMan = new UserInfoManage();
            if (sSetAuthors == "0")
                userMan.SaveNowAuthor(lUserID, sSelAuthors, sOriginAuthors);
            else
                userMan.SaveNowAuthor(lUserID, sSetAuthors, sOriginAuthors);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }




    }

}