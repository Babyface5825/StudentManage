using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;

namespace StudentManage.Areas.UserControl.Controllers
{
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class WinDepartmentAddZYController : ModelWindowsController
    {
        // GET: UserControl/WinDepartmentAddZY
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "编辑专业信息";
            SetWindowSize(win, 540, 370, sWidth, sHeight);
            return this.Direct();
        }

        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            var store1 = this.GetCmp<Store>(PanelID + "_AllZYStore");
            store1.LoadProxy();
            var store2 = this.GetCmp<Store>(PanelID + "_NowZYStore");
            store2.LoadProxy();
            return this.Direct();
        }
        
        public ActionResult WinDepartmentAddZY_ZYAllRead(string ParentVariable, string sGxID)
        {
            GetParentVariable(ParentVariable);
            zydhTableManage zyMan = new zydhTableManage();
            return this.Store(zyMan.GetAllZyStore(sGxID, ParentVar.transValue));            
        }
        public ActionResult WinDepartmentAddZY_ZYSelRead(string ParentVariable, string sGxID)
        {
            GetParentVariable(ParentVariable);
            zydhTableManage zyMan = new zydhTableManage();
            return this.Store(zyMan.GetSelZyStore(sGxID, ParentVar.transValue));
        }
        public virtual ActionResult WinDepartmentAddZY_AddClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            return this.Direct();
        }

        public virtual ActionResult WinDepartmentAddZY_AddAllClick(string PanelID)
        {
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            return this.Direct();
        }

        public virtual ActionResult WinDepartmentAddZY_RemoveClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("if (source.hasSelection()) {");
            X.AddScript("var records = source.selModel.getSelection();");
            X.AddScript("source.deleteSelected();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);});}");
            return this.Direct();
        }

        public virtual ActionResult WinDepartmentAddZY_RemoveAllClick(string PanelID)
        {
            X.AddScript("var destination = Ext.getCmp('" + PanelID + "_AllPanel');");
            X.AddScript("var source = Ext.getCmp('" + PanelID + "_NowPanel');");
            X.AddScript("var records = source.store.getRange();");
            X.AddScript("source.store.removeAll();");
            X.AddScript("Ext.each(records, function(record){");
            X.AddScript("destination.store.addSorted(record);}); ");
            return this.Direct();
        }


        public ActionResult WindowSaveClick(tblUserInfo model, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            string ss = ParentVar.areaName + CosValue.SPLITOPERATOR1 + ParentVar.controlName + CosValue.SPLITOPERATOR1 + ParentVar.actionName;
            string s = "var i = 0; var sSelZYs = \"\"; var sSelNames = \"\";";
            s = s + "while (i < App." + PanelID + "_NowZYStore" + ".getCount()) { sSelZYs = sSelZYs + App." + PanelID + "_NowZYStore" + ".getAt(i).get('ID') + \",\";";
            s = s + "sSelNames = sSelNames + App." + PanelID + "_NowZYStore" + ".getAt(i).get('Name') + \",\";i++; } ";
            s = s + "sSelZYs = sSelZYs.slice(0, sSelZYs.length - 1);sSelNames = sSelNames.slice(0, sSelNames.length - 1);";
            s = s + " var ss='" + ss + "'+'|'+sSelZYs+'.'+sSelNames+'|';";
            s = s + "App." + PanelID + "_ParentVariable.setValue(ss);";            
            X.AddScript(s);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }


    }
}