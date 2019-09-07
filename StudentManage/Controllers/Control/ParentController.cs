using System;
using System.Web.Mvc;
using StudentManage.Manage;
using Ext.Net;
using Ext.Net.MVC;
using StudentManage.Areas.UserControl.Controllers;
using StudentManage.Models;
using System.Collections;

namespace StudentManage.Controllers
{
    /// <summary>
    /// 用在用户定义控件中，传递父页面相关信息
    /// </summary>
    public class ParentVariable
    {
        /// <summary>
        /// 父页面的Area
        /// </summary>
        public string areaName = "";
        /// <summary>
        /// 父页面的Controller
        /// </summary>
        public string controlName = "";
        /// <summary>
        /// 返回父页面时调用的action
        /// </summary>
        public string actionName = "";
        /// <summary>
        /// 和父页面之间传递的一些参数
        /// </summary>
        public string transValue = "";
        /// <summary>
        /// 获得页面Manage的KeyWords
        /// </summary>
        public string KeyWords = "";
        /// <summary>
        /// 默认传递的合并参数
        /// </summary>
        public string DefaultVal = "";
    }

    public class ParentController : Controller
    {
        protected ParentVariable ParentVar;
        protected string sDataListName;
        protected string sDataTreeName;
        protected string sMainFormName;
        protected string sAreaName;
        protected string sPageKeyWord;
        protected string sDataTreeKey;
        //protected string sDataListKey;

        #region Get/Set ParentVariable
        /// <summary>
        /// 设定当前的parentVar
        /// </summary>
        /// <param name="area">AreaName，默认RouteData.DataTokens["area"]</param>
        /// <param name="control">ControlName，默认RouteData.Values["controller"]</param>
        /// <param name="action">ActionName，默认空</param>
        /// <param name="val">传递的参数，默认空</param>
        /// <param name="key">传递的关键字，默认sPageKeyWord</param>
        public void setParentVariable(string area = null, string control = null, string action = null, string val = null, string key = null)
        { 
            ParentVar = new ParentVariable();
            if (area == null)
                ParentVar.areaName = RouteData.DataTokens["area"].ToString();
            else
                ParentVar.areaName = area;
            if (control == null)
                ParentVar.controlName = RouteData.Values["controller"].ToString();
            else
                ParentVar.controlName = control;
            if (action == null)
                ParentVar.actionName = "";
            else
                ParentVar.actionName = action;
            if (val == null)
                ParentVar.transValue = "";
            else
                ParentVar.transValue = val;
            if (key == null)
                ParentVar.KeyWords = sPageKeyWord;
            else
                ParentVar.KeyWords = key;
            ParentVar.DefaultVal = ParentVar.areaName + CosValue.SPLITOPERATOR1 + ParentVar.controlName + CosValue.SPLITOPERATOR1
                + ParentVar.actionName + CosValue.SPLITOPERATOR1 + ParentVar.transValue + CosValue.SPLITOPERATOR1 + ParentVar.KeyWords;
        }

        /// <summary>
        /// 根据页面传来的字符串，获取当前的ParentVariable
        /// </summary>
        /// <param name="sContainer"></param>
        protected void GetParentVariable(string sContainer)
        {
            string[] ss = sContainer.Split(CosValue.SPLITOPERATOR1);
            ParentVar = new ParentVariable();
            ParentVar.areaName = ss[0];
            ParentVar.controlName = ss[1];
            ParentVar.actionName = ss[2];
            ParentVar.transValue = ss[3];
            ParentVar.KeyWords = ss[4];
        }
        #endregion


        public void SetSearchPanelHeight(int height)
        {
            var panel = this.GetCmp<Panel>("SearchPanelContainer");
            panel.SetHeight(height);
        }

        // GET: Parent
        [MemberValidation]
        public virtual ActionResult Index()
        {
            if (Request["id"] != null)
                ViewData["Center_PageID"] = Request["id"].ToString();
            InitPage();
            return View();
        }



        public virtual void InitPage()
        {
            ViewData["DataList_PanelID"] = sDataListName;
            ViewData["DataTree_PanelID"] = sDataTreeName;
            setParentVariable();
            ViewData["DataList_ParentVariable"] = ParentVar.DefaultVal;
        }

        #region 一些反复调用的小函数        
        /// <summary>
        /// 创建一个指定长度的ArrayList，里面元素都是同样的
        /// </summary>
        /// <param name="obj">相同的元素</param>
        /// <param name="count">数组长度</param>
        /// <returns>创建好的ArrayList</returns>
        public ArrayList NewDuplicateArray(object obj, int count)
        {
            ArrayList arr = new ArrayList();
            for (int i = 0; i < count; i++)
                arr.Add(obj);
            return arr;
        }

        /// <summary>
        /// 获得当前的系统路径
        /// </summary>
        /// <returns></returns>
        protected string GetApplicationPath()
        {            
            string sApplicationPath = Request.ApplicationPath;
            if (sApplicationPath == "/")
                sApplicationPath = "";
            return sApplicationPath;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="PanelID">当前窗口的Win ID</param>
        /// <returns></returns>
        public ActionResult ShowWindow(string PanelID)
        {
            X.AddScript("App." + PanelID + ".show();");
            return this.Direct();
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="PanelID">当前窗口的Win ID</param>
        /// <returns></returns>
        public ActionResult HideWindow(string PanelID)
        {
            X.AddScript("App." + PanelID + ".hide();");
            return this.Direct();
        }

        /// <summary>
        /// 设置View中一个隐藏域的值
        /// </summary>
        /// <param name="sHiddenName"></param>
        /// <param name="sHiddenValue"></param>
        public void SetHiddenValue(string sHiddenName, string sHiddenValue)
        {
            X.AddScript("App." + sHiddenName + ".setValue('" + sHiddenValue + "');");
        }
        /// <summary>
        /// 设置View中一个TextFiled的值
        /// </summary>
        /// <param name="sTextName"></param>
        /// <param name="sTextValue"></param>
        public void SetTextField(string sTextName, string sTextValue)
        {
            X.AddScript("App." + sTextName + ".setValue('" + sTextValue + "');");
        }

        public virtual void SetInitDataAuthor()
        {
            var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
            CryptManage crypt = new CryptManage();
            string sGxID = crypt.Decrypto(loginUser.Values["UserGxID"]);
            SetHiddenValue(sDataListName + "_InitNames", "GxID");
            SetHiddenValue(sDataListName + "_InitValues", sGxID);
            SetHiddenValue(sDataListName + "_InitSigns", SIGN.Equal.ToString());
        }

        protected string ConvertTimeString(string sTime)
        {
            if (sTime.IndexOf(' ') != -1)
            {
                string s = sTime.Remove(sTime.IndexOf(' '));
                if (sTime.Contains("下午"))
                {
                    string[] ss = s.Split(':');
                    if (ss[0] != "12")
                        ss[0] = (Convert.ToInt16(ss[0]) + 12).ToString();
                    s = ss[0] + ":" + ss[1] + ":00";
                }
                else
                    s = s.Replace("12:", "00:") + ":00";
                return s;
            }
            else
                return sTime;
        }


        #endregion

        #region DataList返回相关
        /// <summary>
        /// 返回当前的具体DataListManage
        /// </summary>
        /// <returns></returns>
        public virtual DataListManage GetDataListManage(string sKeyWord = null, string transvalue = null)
        {
            if ((sKeyWord == null) || (sKeyWord == ""))
                sKeyWord = sPageKeyWord;

            switch (sKeyWord)
            {
                case "UserInfo": return new UserInfoDataListManage();
                case "SimpleDlg":
                    if (transvalue == "AuthorizationInfo") return new AuthorizationInfoDataListManage();
                    if (transvalue == "DutyInfo") return new DutyInfoDataListManage();
                    if (transvalue == "ReportType") return new ReportTypeDataListManage();

                    if (transvalue == "dqdmTable") return new dqdmTableDataListManage();

                    if (transvalue == "bylbdmTable") return new bylbdmTableDataListManage();
                    if (transvalue == "ccdmTable") return new ccdmTableDataListManage();
                    if (transvalue == "cjxdmTable") return new cjxdmTableDataListManage();
                    if (transvalue == "czlbdmTable") return new czlbdmTableDataListManage();

                    if (transvalue == "hjlbdmTable") return new hjlbdmTableDataListManage();
                    if (transvalue == "jhlbdmTable") return new jhlbdmTableDataListManage();
                    if (transvalue == "jhxzdmTable") return new jhxzdmTableDataListManage();
                    if (transvalue == "kldmTable") return new kldmTableDataListManage();
                    if (transvalue == "kmdmTable") return new kmdmTableDataListManage();

                    if (transvalue == "kslbdmTable") return new kslbdmTableDataListManage();
                    if (transvalue == "kslxdmTable") return new kslxdmTableDataListManage();
                    if (transvalue == "kstzdmTable") return new kstzdmTableDataListManage();
                    if (transvalue == "kszgdmTable") return new kszgdmTableDataListManage();
                    if (transvalue == "lqfsdmTable") return new lqfsdmTableDataListManage();

                    if (transvalue == "mzdmTable") return new mzdmTableDataListManage();
                    if (transvalue == "pcdmTable") return new pcdmTableDataListManage();
                    if (transvalue == "tdlxdmTable") return new tdlxdmTableDataListManage();
                    if (transvalue == "tdyydmTable") return new tdyydmTableDataListManage();
                    if (transvalue == "tjjldmTable") return new tjjldmTableDataListManage();

                    if (transvalue == "wyyzdmTable") return new wyyzdmTableDataListManage();
                    if (transvalue == "xtdwdmTable") return new xtdwdmTableDataListManage();
                    if (transvalue == "zcdmTable") return new zcdmTableDataListManage();
                    if (transvalue == "xzdmTable") return new xzdmTableDataListManage();
                    if (transvalue == "zjlxdmTable") return new zjlxdmTableDataListManage();

                    if (transvalue == "zklxdmTable") return new zklxdmTableDataListManage();
                    if (transvalue == "zylbdmTable") return new zylbdmTableDataListManage();
                    if (transvalue == "zytjlxdmTable") return new zytjlxdmTableDataListManage();
                    if (transvalue == "zytzdmTable") return new zytzdmTableDataListManage();
                    if (transvalue == "zzmmdmTable") return new zzmmdmTableDataListManage();

                    if (transvalue == "byxxdmTable") return new byxxdmTableDataListManage();
                    if (transvalue == "gxdmTable") return new gxdmTableDataListManage();
                    if (transvalue == "zydhTable") return new zydhTableDataListManage();
                    if (transvalue == "xbdmTable") return new xbdmTableDataListManage();

                    if (transvalue == "tddwTable") return new tddwTableDataListManage();                    

                    break;
                
                case "RoleAuthorization": return new RoleAuthorizationDataListManage();
                case "OriginDataInport": return new OriginDataPackageDataListManage();

                case "JHKSearch": return new jhkTableDataListManage();
                case "BMKOneVerify": return new bmkOneVerifyDataListManage();
                case "BMKTwoVerify": return new bmkTwoVerifyDataListManage();
                case "BMKSearch": return new bmkTableDataListManage();
                case "TDDSearch": return new tddTableDataListManage();
                case "ReportTypeSel": return new ReportTypeSelDataListManage();
                case "ReportCreate": return new ReportInfoDataListManage();
                case "zykTable": return new zykTableDataListManage();
                case "ksjlTable":return new ksjlTableDataListManage();
                case "ksxkcjTable": return new ksxkcjTableDataListManage();
            }
            return null;
        }



        public virtual ActionResult DataList_ReturnAddClick(string PanelID, string selection, string keywords, string transvalue)
        {
            X.AddScript("App.direct." + sAreaName + "." + sPageKeyWord + "_AddClickDirectMethod(App." + sMainFormName + ".getForm().getValues());");
            return this.Direct();
        }

        protected virtual ActionResult OpenMoreWindow(string selection, string keywords, string transvalue)
        {
            return null;
        }

        public virtual ActionResult DataList_ReturnEditClick(string PanelID, string selection, string keywords, string transvalue)
        {
            return OpenMoreWindow(selection, keywords, transvalue);
        }

        public virtual ActionResult MoreWindow_ConfirmSave(string PanelID = null, string transValue = null)
        {            
            //DataListController dlCtrl = new DataListController();
            //dlCtrl.RefreashStore(sDataListName);
            RefreashDataList(sDataListName);
            return this.Direct();
        }
        public virtual ActionResult DataList_ReturnSearchClick(string key, string selection, string PanelID)
        {
            SetHiddenValue(sDataListName + "_VisitStatus", "1");
            X.AddScript("App.direct." + sAreaName + "." + sPageKeyWord + "_SearchClickDirectMethod(App." + sMainFormName + ".getForm().getValues());");
            return this.Direct();
        }
        public void RefreashDataList(string PanelID)
        {
            var store = this.GetCmp<Store>(PanelID + "_Store");
            X.AddScript("App." + PanelID + ".getSelectionMemory().clearMemory();");
            store.LoadProxy();
        }
        
        protected virtual bool SetSearchWhere(string sData)
        {
            return false;
        }

        public virtual ActionResult DataList_ReturnResetClick(string PanelID, string selection, string keywords, string transvalue)
        {
            SetHiddenValue(sDataListName + "_VisitStatus", "0");
            var man = GetDataListManage(keywords, transvalue);
            var model = man.GetSpecialModelFromCurrentID(0);
            var form = this.GetCmp<FormPanel>(sMainFormName);
            form.SetValues(model);
            SetHiddenValue(sDataListName + "_NowNames", "ClearDataList");
            RefreashDataList(sDataListName);
            if (sDataTreeName != "")
                X.AddScript("App.direct." + sAreaName + ".TreeSelMaster_ClearDataTreeSelection('" + sDataTreeName + "');");
            return this.Direct();
        }

        public virtual ActionResult DataList_ReturnInportClick(string key, string selection, string keywords, string transvalue)
        {
            Excel clsExcel = new Excel();
            string sFilePath = ConstAddressPath.TEMP_WEBPATH;
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            string sHint = clsExcel.WholeStep(sFilePath + selection, keywords, transvalue);
            X.AddScript("Ext.Msg.alert('注意', '" + sHint + "');");
            return result;
        }

        public virtual ActionResult DataList_ReturnExportClick(string key, string selection)
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            return result;
        }

        public virtual ActionResult DataList_ReturnOrderUpClick(string PanelID, string selection, string keywords, string transvalue)
        {
            var man = GetDataListManage(keywords, transvalue);
            man.OrderUpRecord(selection);
            RefreashDataList(PanelID);
            return this.Direct();
        }

        public virtual ActionResult DataList_ReturnOrderDownClick(string PanelID, string selection, string keywords, string transvalue)
        {
            var man = GetDataListManage(keywords, transvalue);
            man.OrderDownRecord(selection);
            RefreashDataList(PanelID);
            return this.Direct();
        }
        public virtual ActionResult DataList_ReturnListRowClick(string PanelID, string selection, string keywords, string transvalue)
        {
            return this.Direct();
        }

        public virtual ActionResult DataList_ReturnDeleteClick(string PanelID, string selection, string keywords, string transvalue)
        {
            var man = GetDataListManage(keywords, transvalue);
            var models = man.GetSpecialModelFromCurrentSelection(selection);
            if (man.bHasDelFlag)
                man.DeleteRecordByDelflag(models);
            else
                man.DeleteRecord(models);
            RefreashDataList(PanelID);
            return this.Direct();
        }
        public virtual ActionResult DataList_ReturnVerifyClick(string key, string selection, string PanelID)
        {
            var man = GetDataListManage(key);
            var models = man.GetSpecialModelFromCurrentSelection(selection);
            man.VerifyRecords(models);
            RefreashDataList(PanelID);
            return this.Direct();
        }


        public virtual ActionResult DataList_ReturnMore1Click(string PanelID, string selection, string keywords, string transvalue)
        {
            return this.Direct();
        }

        public virtual ActionResult DataList_ReturnMore2Click(string PanelID, string selection, string keywords, string transvalue)
        {
            return this.Direct();
        }

        #endregion

        #region DataTree相关
        public virtual DataTreeManage GetTreeManage(string sKeyWord = null, string transvalue = null)
        {
            if ((sKeyWord == null) || (sKeyWord == ""))
                sKeyWord = sPageKeyWord;

            switch (sKeyWord)
            {
                case "OperationInfo": return new OperationInfoDataTreeManage();
                case "DepartmentInfo": return new DepartmentInfoDataTreeManage();
                case "dqdmTable": return new dqdmTableDataTreeManage();
                case "zylbdmTable": return new zylbdmTableDataTreeManage();
                case "ReportType": return new ReportTypeDataTreeManage();
            }
            return null;
        }


        private Ext.Net.MVC.PartialViewResult _AutoLoadDataTree(string containerId)
        {
            string[] key = { CookieKeys.DEPARTMENTID, CookieKeys.USERROLES };
            string sUserDepartmentID = (new UserInfoManage()).GetLoginUserInfo(key)[0];
            string sUserRoles = (new UserInfoManage()).GetLoginUserInfo(key)[1];

            Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
            pvr.ContainerId = containerId;
            pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\DataTree.cshtml";
            pvr.WrapByScriptTag = false;
            pvr.RenderMode = RenderMode.AddTo;
            pvr.ViewData["DataTree_PanelID"] = sDataTreeName;
            setParentVariable();
            string parVar = ParentVar.areaName + CosValue.SPLITOPERATOR1 + ParentVar.controlName + CosValue.SPLITOPERATOR1 + CosValue.SPLITOPERATOR1 + CosValue.SPLITOPERATOR1 + sDataTreeKey;
            pvr.ViewData["DataTree_ParentVariable"] = parVar;
            pvr.ViewData["DataTree_RelationID"] = sUserDepartmentID;
            pvr.ViewData["DataTree_AuthorID"] = sUserRoles;
            return pvr;
        }

        public virtual Ext.Net.MVC.PartialViewResult AutoLoadDataTree(string containerId)
        {
            return _AutoLoadDataTree(containerId);
        }


        #endregion

        #region DataComboBox相关

        /// <summary>
        /// 返回当前的具体DataComboboxManage
        /// </summary>
        /// <returns></returns>
        public virtual object GetDataComboboxManage(string sKeyWord = null)
        {
            if ((sKeyWord == null) || (sKeyWord == ""))
                sKeyWord = sPageKeyWord;

            switch (sKeyWord)
            {
                case "AuthorizationInfo": return new AuthorizationInfoDataComboboxManage();
                case "DutyInfo": return new DutyInfoDataComboboxManage();

                case "bylbdmTable": return new bylbdmTableDataComboboxManage();
                case "ccdmTable": return new ccdmTableDataComboboxManage();
                case "cjxdmTable": return new cjxdmTableDataComboboxManage();
                case "czlbdmTable": return new czlbdmTableDataComboboxManage();
                case "dqdmTable": return new dqdmTableDataComboboxManage();

                case "hjlbdmTable": return new hjlbdmTableDataComboboxManage();
                case "jhlbdmTable": return new jhlbdmTableDataComboboxManage();
                case "jhxzdmTable": return new jhxzdmTableDataComboboxManage();
                case "kldmTable": return new kldmTableDataComboboxManage();
                case "kmdmTable": return new kmdmTableDataComboboxManage();

                case "kslbdmTable": return new kslbdmTableDataComboboxManage();
                case "kslxdmTable": return new kslxdmTableDataComboboxManage();
                case "kstzdmTable": return new kstzdmTableDataComboboxManage();
                case "kszgdmTable": return new kszgdmTableDataComboboxManage();
                case "lqfsdmTable": return new lqfsdmTableDataComboboxManage();

                case "mzdmTable": return new mzdmTableDataComboboxManage();
                case "pcdmTable": return new pcdmTableDataComboboxManage();
                case "tdlxdmTable": return new tdlxdmTableDataComboboxManage();
                case "tdyydmTable": return new tdyydmTableDataComboboxManage();
                case "tjjldmTable": return new tjjldmTableDataComboboxManage();

                case "wyyzdmTable": return new wyyzdmTableDataComboboxManage();
                case "xtdwdmTable": return new xtdwdmTableDataComboboxManage();
                case "zcdmTable": return new zcdmTableDataComboboxManage();
                case "xzdmTable": return new xzdmTableDataComboboxManage();
                case "zjlxdmTable": return new zjlxdmTableDataComboboxManage();

                case "zklxdmTable": return new zklxdmTableDataComboboxManage();
                case "zylbdmTable": return new zylbdmTableDataComboboxManage();
                case "zytjlxdmTable": return new zytjlxdmTableDataComboboxManage();
                case "zytzdmTable": return new zytzdmTableDataComboboxManage();
                case "zzmmdmTable": return new zzmmdmTableDataComboboxManage();
                case "byxxdmTable": return new byxxdmTableDataComboboxManage();

                case "xbdmTable": return new xbdmTableDataComboboxManage();

                case "gxdmTable": return new gxdmTableDataComboboxManage();
                case "zydhTable": return new zydhTableDataComboboxManage();
                case "tddwTable": return new tddwTableDataComboboxManage();
            }
            return null;
        }


        /// <summary>
        /// 设置combobox的当前选择值
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sSelID">0：不选则;-1：选择第一个;数值：选择相应内容</param>
        /// <param name="initflag"></param>
        public void SetComboValue(string PanelID, string sSelID, bool initflag = false)
        {
            if ((sSelID != null) && (sSelID != ""))
            {
                if (initflag)
                    SetHiddenValue(PanelID + "_InitFlag", "0");
                else
                    SetHiddenValue(PanelID + "_InitFlag", "1");

                Random rm = new Random();
                SetHiddenValue(PanelID + "_SetSelID", sSelID + CosValue.SPLITOPERATOR1 + rm.Next());
            }
        }

        public void SetThreeListComboValue(string PanelID, string sSelID)
        {
            Random rm = new Random();
            SetHiddenValue(PanelID + "_SetSelID", sSelID + CosValue.SPLITOPERATOR1 + rm.Next());
        }

        #endregion

        #region 不方便归类的函数




        #endregion


    }
}