using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Controllers;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;

namespace StudentManage.Areas.adminSystemManage.Controllers
{
    [DirectController(AreaName = "adminSystemManage", IDMode = DirectMethodProxyIDMode.None)]
    public class RoleAuthorizationController : ParentController
    {
        public PartialViewResult AutoLoadDataListContainer(string containerId)
        {
            PartialViewResult pvr = new PartialViewResult();
            pvr.ContainerId = containerId;
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\DataList.cshtml";
            pvr.WrapByScriptTag = false;
            pvr.RenderMode = RenderMode.AddTo;
            pvr.ViewData["DataList_PanelID"] = "RoleAuthorization_DataList";
            setParentVariable(null, null, null, null, "RoleAuthorization");
            pvr.ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }

        public PartialViewResult AutoLoadDataTreeGridContainer(string containerId)
        {
            PartialViewResult pvr = new PartialViewResult();
            pvr.ContainerId = containerId;
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\DataTreeGrid.cshtml";
            pvr.WrapByScriptTag = false;
            pvr.RenderMode = RenderMode.AddTo;
            pvr.ViewData["DataTreeGrid_PanelID"] = "RoleAuthorization_DataTreeGrid";
            setParentVariable(null, null, null, null, "RoleAuthorization");
            pvr.ViewData["DataTreeGrid_ParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }
        public PartialViewResult AutoLoadDataTreeContainer(string containerId)
        {
            PartialViewResult pvr = new PartialViewResult();
            pvr.ContainerId = containerId;
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\DataTree.cshtml";
            pvr.WrapByScriptTag = false;
            pvr.RenderMode = RenderMode.AddTo;
            pvr.ViewData["DataTree_PanelID"] = "RoleAuthorization_DataTree";
            setParentVariable(null, null, null, null, "AuthorizationChannelType");
            pvr.ViewData["DataTree_ParentVariable"] = ParentVar.DefaultVal;
            return pvr;
        }

        [DirectMethod]
        public System.Web.Mvc.ActionResult RoleAuthorization_RefreshTreeGrid(string sAuthorID)
        {
            return RedirectToActionPermanent("DataTreeGridSetAuthorSel", "DataTreeGrid", new { area = "UserControl", PanelID = "RoleAuthorization_DataTreeGrid", sAuthorID });
        }

        [DirectMethod]
        public System.Web.Mvc.ActionResult RoleAuthorization_RefreshTree(string sAuthorID)
        {
            return RedirectToActionPermanent("DataTreeSetAuthorSel", "DataTree", new { area = "UserControl", PanelID = "RoleAuthorization_DataTree", sAuthorID });
        }

        public override System.Web.Mvc.ActionResult DataList_ReturnListRowClick(string PanelID, string selection, string keywords, string transvalue)
        {
            try
            {
                var datalistMan = (DataListManage)GetDataListManage(keywords, transvalue);
                var models = datalistMan.GetSpecialModelFromCurrentSelection(selection);
                if (models.Count > 0)
                {
                    X.AddScript("App.direct.RoleAuthorization_RefreshTreeGrid(" + models[0].ID + ");");
                    //X.AddScript("App.direct.RoleAuthorization_RefreshTree(" + models[0].ID + ");");                    
                }
            }
            catch
            {

            }
            return this.Direct();
        }

        public System.Web.Mvc.ActionResult TreeNodeClick(string sTreeSelID)
        {
            return this.Direct();
        }


        public override System.Web.Mvc.ActionResult DataList_ReturnMore1Click(string PanelID, string selection, string keywords, string transvalue)
        {
            if (selection == null)
            {
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
                return this.Direct();
            }

            AuthorizationInfoDataListManage datalistMan = new AuthorizationInfoDataListManage();
            var models = datalistMan.GetSpecialModelFromCurrentSelection(selection);
            if (models != null)
            {
                if (models.Count > 0)
                {
                    X.AddScript("App.RoleAuthorization_DataTreeGrid.submitNodes();");
                    //X.AddScript("App.RoleAuthorization_DataTree.submitNodes();");
                    //X.AddScript("App.RoleAuthorization_DataTree2.submitNodes();");
                    X.Msg.Alert("成功", "保存完毕！").Show();
                }
                else
                    X.Msg.Alert(" 错误", "尚未选择任何行！").Show();
            }
            else
                X.Msg.Alert(" 错误", "尚未选择任何行！").Show();
            return this.Direct();
        }
        
    }
}