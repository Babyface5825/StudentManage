using System.Linq;
using Ext.Net;
using System.Web.Mvc;
using StudentManage.Models;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Controllers;

/* Used the full class name Ext.Net.MVC.PartialViewResult instead of PartialViewResult. 
Otherwise, it might confuse that System.Web.MVC.PartialViewResult is being used. */


namespace StudentManage.Areas.adminMain.Controllers
{
    public class MainPageController : ParentController
    {
        // GET: adminMain/MainPage
        public override ActionResult Index()
        {
            UserInfoManage userManage = new UserInfoManage();
            ViewBag.Title = "高校招生数据管理与分析系统";
            string[] key = { CookieKeys.USERNAME, CookieKeys.USERDUTY };
            string sName = string.Join("", userManage.GetLoginUserInfo(key));
            ViewData["MainPage_labUserNameText"] = sName;
            var Panel = this.GetCmp<Panel>("Menu");
            switch (AppSettingValues.MENUTYPE)
            {
                case "Default":
                    Panel.Width = 150;
                    break;
                case "Tree":
                    Panel.Width = 180;
                    break;
                case "Image":                    
                    Panel.Width = 120;
                    break;
                default:
                    return null;
            }
            return View();
        }

        public ActionResult MainPage_LoginOutClick()
        {
            UserInfoManage userManage = new UserInfoManage();
            userManage.LoginOut(Response);
            return RedirectToAction("Index", "Login",new { area="adminMain"});            
        }


        /// <summary>
        /// 根据MenuType决定当前的Menu样式
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Ext.Net.MVC.PartialViewResult AutoLoadMenu(string containerId)
        {

            Ext.Net.MVC.PartialViewResult pv = new Ext.Net.MVC.PartialViewResult();            
            pv.ContainerId = containerId;
            pv.WrapByScriptTag = false;
            pv.RenderMode = RenderMode.AddTo;

            switch (AppSettingValues.MENUTYPE)
            {
                case "Default":
                    pv.ViewName = "_AccordionMenu";
                    return pv;
                case "Tree":
                    var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
                    CryptManage crypt = new CryptManage();
                    string sUserRoles =  crypt.Decrypto(loginUser.Values["UserRoles"]);
                    pv.ViewName = "_TreeMenu";
                    pv.ViewData["DataTree_PanelID"] = "MainMenu_DataTree";
                    pv.ViewData["DataTree_RelationID"] = sUserRoles;
                    pv.ViewData["DataTree_AuthorID"] = sUserRoles;
                    return pv;
                //case "Image":
                //    pv.ViewName = "_ImageMenu";
                //    return pv;
                default:
                    return null;
            }
        }


        /// <summary>
        /// 初始化image菜单，不存在三级
        /// </summary>
        /// <returns></returns>
        //public ActionResult InitImageMenu()
        //{
        //    string sWebPath = GetApplicationPath();

        //    string sRelationIDs = System.Web.HttpContext.Current.Request.Cookies.Get("UserRoles").Value;
        //    operationMan.InitAuthorizationArray(sRelationIDs);

        //    Panel motherPanel = new Panel();
        //    motherPanel.Border = false;
        //    motherPanel.Header = false;
        //    motherPanel.Layout = "Accordion";
        //    motherPanel.Region = Region.Center;
        //    motherPanel.Width = 120;            
        //    //首先获取一级菜单
        //    tblTreeInfo[] FirstModels = (tblTreeInfo[])operationMan.GetSomeTreeModels(-1, sRelationIDs);
        //    long nFirstNode = 0;
        //    for (int i = 0; i < FirstModels.Length; i++)
        //    {
        //        Panel menupanel = new Panel();
        //        menupanel.Title = FirstModels[i].Name;
        //        menupanel.Border = false;
        //        menupanel.Header = true;
        //        menupanel.Width = 120;
        //        menupanel.Layout = "VBox";
        //        menupanel.AutoScroll = true;                

        //        //然后是二级菜单
        //        tblTreeInfo[] SecondModels = (tblTreeInfo[])operationMan.GetSomeTreeModels(FirstModels[i].ID, sRelationIDs);
        //        string sAddress = sWebPath + ConstAddressPath.ICON_PATH;
        //        string sWebAddress = ConstAddressPath.ICON_WEBPATH;
        //        FileManager clsFile = new FileManager();
        //        for (int j = 0; j < SecondModels.Length; j++)
        //        {
        //            ImageButton MenuItem = new ImageButton();
        //            MenuItem.ImageUrl = sAddress + "OperationInfo_" + SecondModels[j].ID + "_1.jpg";
        //            if (clsFile.HasFile(sWebAddress + "OperationInfo_" + SecondModels[j].ID + "_2.jpg"))
        //                MenuItem.OverImageUrl = sAddress + "OperationInfo_" + SecondModels[j].ID + "_2.jpg";
        //            if (clsFile.HasFile(sWebAddress + "OperationInfo_" + SecondModels[j].ID + "_3.jpg"))
        //                MenuItem.PressedImageUrl = sAddress + "OperationInfo_" + SecondModels[j].ID + "_3.jpg";
        //            MenuItem.DirectEvents.Click.Method = HttpMethod.POST;
        //            MenuItem.DirectEvents.Click.Url = sWebPath + "/MainPage/MenuNodeClick";
        //            MenuItem.Width = 100;
        //            MenuItem.Height = 100;
        //            Parameter para = new Parameter();
        //            para.Name = "nodeId";
        //            if (nFirstNode == 0)
        //                nFirstNode = SecondModels[j].ID;
        //            para.Value = SecondModels[j].ID.ToString();
        //            para.Mode = ParameterMode.Raw;
        //            MenuItem.DirectEvents.Click.ExtraParams.Add(para);
        //            ToolTip tooltip = new ToolTip();
        //            if (SecondModels[j].Remark != "")
        //                tooltip.Html = SecondModels[j].Remark;
        //            else
        //                tooltip.Html = SecondModels[j].Name;
        //            MenuItem.ToolTips.Add(tooltip);
        //            menupanel.Items.Add(MenuItem);
        //        }
        //        motherPanel.Items.Add(menupanel);
        //    }
        //    motherPanel.DirectEvents.AfterRender.Method = HttpMethod.POST;
        //    motherPanel.DirectEvents.AfterRender.Url = sWebPath + "/MainPage/InitMenuCenter";
        //    motherPanel.DirectEvents.AfterRender.ExtraParams.Add(new Parameter("sFirstLeafID", nFirstNode.ToString(), ParameterMode.Raw));

        //    return this.ComponentConfig(motherPanel);
        //}

        /// <summary>
        /// 初始化Accordion样式菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult InitAccordionMenu()
        {
            UserInfoManage userMan = new UserInfoManage();
            OperationInfoManage operationMan = new OperationInfoManage();
            string sWebPath = GetApplicationPath();
            string[] key = { CookieKeys.USERROLES };
            string sRelationIDs = string.Join("", userMan.GetLoginUserInfo(key));
            operationMan.InitAuthorizationArray(sRelationIDs);

            Panel motherPanel = new Panel();
            motherPanel.Border = false;
            motherPanel.Header = false;
            motherPanel.Layout = "Accordion";
            motherPanel.Region = Region.Center;
            //首先获取一级菜单
            tblTreeInfo[] FirstModels = (tblTreeInfo[])operationMan.GetSomeTreeModels(-1, sRelationIDs);
            long nFirstNode = 0;
            for (int i = 0; i < FirstModels.Length; i++)
            {
                MenuPanel menupanel = new MenuPanel();
                menupanel.Title = FirstModels[i].Name;
                //然后是二级菜单
                tblTreeInfo[] SecondModels = (tblTreeInfo[])operationMan.GetSomeTreeModels(FirstModels[i].ID, sRelationIDs);
                for (int j = 0; j < SecondModels.Length; j++)
                {
                    MenuItem menuitem = new MenuItem();
                    menuitem.Text = SecondModels[j].Name;
                    if (SecondModels[j].IsLeaf)
                    {
                        menuitem.Icon = Icon.ArrowRight;
                        menuitem.DirectEvents.Click.Method = HttpMethod.POST;
                        menuitem.DirectEvents.Click.Url = sWebPath + "/MainPage/MenuNodeClick";
                        Parameter para = new Parameter();
                        para.Name = "nodeId";
                        if (nFirstNode == 0)
                            nFirstNode = SecondModels[j].ID;
                        para.Value = SecondModels[j].ID.ToString();
                        para.Mode = ParameterMode.Raw;
                        menuitem.DirectEvents.Click.ExtraParams.Add(para);
                    }
                    else
                    {
                        //然后是三级菜单
                        tblTreeInfo[] ThirdModels = (tblTreeInfo[])operationMan.GetSomeTreeModels(SecondModels[j].ID, sRelationIDs);
                        Menu subMenu = new Menu();
                        for (int k = 0; k < ThirdModels.Length; k++)
                        {
                            MenuItem submenuitem = new MenuItem();
                            submenuitem.Text = ThirdModels[k].Name;
                            submenuitem.Icon = Icon.ArrowRight;
                            submenuitem.DirectEvents.Click.Method = HttpMethod.POST;
                            submenuitem.DirectEvents.Click.Url = sWebPath + "/MainPage/MenuNodeClick";
                            Parameter para = new Parameter();
                            para.Name = "nodeId";
                            para.Value = ThirdModels[k].ID.ToString();
                            para.Mode = ParameterMode.Raw;
                            submenuitem.DirectEvents.Click.ExtraParams.Add(para);
                            submenuitem.DirectEvents.Click.EventMask.ShowMask = true;
                            submenuitem.DirectEvents.Click.EventMask.Msg = "正在加载，请稍后...";
                            subMenu.Items.Add(submenuitem);
                        }
                        menuitem.Icon = Icon.PlayGreen;
                        menuitem.Menu.Add(subMenu);

                    }
                    menupanel.Menu.Items.Add(menuitem);
                }
                motherPanel.Items.Add(menupanel);
            }
            motherPanel.DirectEvents.AfterRender.Method = HttpMethod.POST;
            motherPanel.DirectEvents.AfterRender.Url = sWebPath + "/MainPage/InitMenuCenter";
            motherPanel.DirectEvents.AfterRender.ExtraParams.Add(new Parameter("sFirstLeafID", nFirstNode.ToString(), ParameterMode.Raw));
            return this.ComponentConfig(motherPanel);
        }

        public ActionResult InitMenuCenter(string sFirstLeafID)
        {
            MenuNodeClick(sFirstLeafID);
            if (AppSettingValues.MENUTYPE == "Tree")
                X.AddScript("App.MainMenu_DataTree.expandAll();");
            return this.Direct();
        }

        public ActionResult MenuNodeClick(string nodeId)
       {
            MenuNodeClickForMainPage(nodeId);
            return this.Direct();
        }

        public ActionResult TreeNodeClick(string sTreeSelID)
        {
            MenuNodeClickForMainPage(sTreeSelID);
            return this.Direct();
        }

        /// <summary>
        /// 菜单节点点击后打开相应页面
        /// </summary>
        /// <param name="nodeId"></param>
        private void MenuNodeClickForMainPage(string nodeId)
        {
            OperationInfoManage operationMan = new OperationInfoManage();
            OperationInfoDataTreeManage operationTree = new OperationInfoDataTreeManage();
            if (operationTree.IsLeaf(nodeId))
            {
                string s = operationMan.GetNameandWholeURL(nodeId);
                if (s != CosValue.SPLITOPERATOR1.ToString())
                {
                    string[] sTrans = s.Split(CosValue.SPLITOPERATOR1);
                    LoadCenterPage(GetApplicationPath() + "/" + sTrans[1], nodeId, sTrans[0]);
                }
            }
        }

        /// <summary>
        /// 打开主体页面
        /// </summary>
        /// <param name="sURL">页面URL</param>
        /// <param name="sID">页面ID</param>
        /// <param name="sName">页面名称</param>
        private void LoadCenterPage(string sURL, string sID, string sName)
        {
            X.AddScript("var tab=App.CenterTabs.getComponent(" + sID + ");");
            if (AppSettingValues.TABTYPE == "Single")
            {
                X.AddScript("if (!tab) {");
                X.AddScript("App.CenterTabs.remove(0);");
            }
            else
            {
                X.AddScript("if (tab) {App.CenterTabs.setActiveTab(tab);}");
                X.AddScript("else {");
            }
                        
            if (sURL.Contains('?'))
                X.AddScript("var hostName = window.location.protocol + \"//\" + window.location.host+ \"/\" + '" + sURL + "'+'&id=" + sID + "';");
            else
                X.AddScript("var hostName = window.location.protocol + \"//\" + window.location.host+ \"/\" + '" + sURL + "'+'?id=" + sID + "';");
            X.AddScript("App.CenterTabs.add(new Ext.panel.Panel({");
            X.AddScript("id:\"" + sID + "\",");
            X.AddScript("title:\"" + sName + "\",");
            X.AddScript("hideMode : \"offsets\",");
            if (AppSettingValues.TABTYPE == "Single")
                X.AddScript("closable : false,");
            else
                X.AddScript("closable : true,");
            X.AddScript("loader : {");
            X.AddScript("renderer : \"frame\",");
            X.AddScript("url : hostName,");
            X.AddScript("listeners : {");
            X.AddScript("beforeload: function () {this.target.body.mask('加载中...');},");
            X.AddScript("load: function (loader) {this.target.body.unmask();}}}");
            X.AddScript("}));");
            X.AddScript("tab=App.CenterTabs.getComponent(" + sID + ");");
            X.AddScript("App.CenterTabs.setActiveTab(tab);}");
        }

    }
}