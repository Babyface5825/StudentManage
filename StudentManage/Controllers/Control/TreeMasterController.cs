using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net;
using StudentManage.Manage;
using StudentManage.Models;


namespace StudentManage.Controllers
{


    public class TreeMasterController : ParentController
    {
        // GET: Parent
        public override void InitPage()
        {
            ViewData["TreeMaster_Form"] = sMainFormName;
            base.InitPage();
        }
        /// <summary>
        /// 点击一个树节点后，根据点击的节点ID设置右边的form
        /// </summary>
        /// <param name="PageKeyWord"></param>
        /// <param name="sTreeSelID"></param>
        /// <returns></returns>
        public virtual ActionResult TreeNodeClick(string PageKeyWord, string sTreeSelID, string sNowNames, string sNowValues, string sNowSigns)
        {
            if (sTreeSelID != "-1")
            {
                DataTreeManage treeMan = GetTreeManage(PageKeyWord);
                var model = treeMan.GetSpecialModelFromCurrentID(sTreeSelID);
                var form = this.GetCmp<FormPanel>(sMainFormName);
                form.SetValues(model);
            }
            else
            {
                tblTreeInfo model = new tblTreeInfo();
                model.ID = -1;
                var form = this.GetCmp<FormPanel>(sMainFormName);
                form.SetValues(model);
            }
            return this.Direct();
        }

        public virtual ActionResult TreeMaster_RemarkChange()
        {
            return this.Direct();
        }
        /// <summary>
        /// 准备或取消新增状态
        /// </summary>
        /// <param name="flag">true:准备新增；false:取消新增</param>
        public ActionResult TreeMaster_PrepareAddNextLevel(string PanelID, long id, bool flag)
        {
            string sMainFormName = PanelID;

            if (flag)
                SetHiddenValue(sMainFormName + "_AddorSave", "1");
            else
                SetHiddenValue(sMainFormName + "_AddorSave", "0");

            if (flag)
            {
                tblTreeInfo model = new tblTreeInfo();
                model.ID = id;
                var form = this.GetCmp<FormPanel>(sMainFormName);
                form.SetValues(model);
            }

            if (id == 0)
                X.AddScript("App.direct." + sAreaName + ".TreeMaster_ClearDataTreeSelection('" + sDataTreeName + "');");

            var btnAddNextLevel = this.GetCmp<Button>(sMainFormName + "_btnAddNextLevel");
            var btnDel = this.GetCmp<Button>(sMainFormName + "_btnDelete");
            var btnOrderUp = this.GetCmp<Button>(sMainFormName + "_btnOrderUp");
            var btnOrderDown = this.GetCmp<Button>(sMainFormName + "_btnOrderDown");
            var btnMore = this.GetCmp<Button>(sMainFormName + "_btnMore");
            var btnMore2 = this.GetCmp<Button>(sMainFormName + "_btnMore2");
            btnAddNextLevel.Disabled = flag;
            btnDel.Disabled = flag;
            btnOrderUp.Disabled = flag;
            btnOrderDown.Disabled = flag;
            btnMore.Disabled = flag;
            btnMore2.Disabled = flag;
            if (flag)
                X.AddScript("App.direct." + sAreaName + ".TreeMaster_DisableDataTree('" + sMainFormName.Substring(0, sMainFormName.IndexOf('_')) + "_DataTree" + "',true);");
            else
                X.AddScript("App.direct." + sAreaName + ".TreeMaster_DisableDataTree('" + sMainFormName.Substring(0, sMainFormName.IndexOf('_')) + "_DataTree" + "',false);");
            return this.Direct();
        }

        /// <summary>
        /// 跨action调用Datatree中的DisableDataTree
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [DirectMethod]
        public ActionResult TreeMaster_DisableDataTree(string PanelID, bool flag)
        {
            return RedirectToActionPermanent("DisableDataTree", "DataTree", new { area = "UserControl", PanelID, flag });
        }


        /// <summary>
        /// 跨Action调用DataTree中的ClearDataTreeSelection
        /// </summary>
        /// <param name="PanelID"></param>
        /// <returns></returns>
        [DirectMethod]
        public ActionResult TreeMaster_ClearDataTreeSelection(string PanelID)
        {
            return RedirectToActionPermanent("ClearDataTreeSelection", "DataTree", new { area = "UserControl", PanelID });
        }

        /// <summary>
        /// 点击修改按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_SaveClick(tblTreeInfo model, string newFlag)
        {
            if (model.Name == "")
            {
                X.Msg.Alert("注意", "名称不能为空！").Show();
                return this.Direct();
            }

            DataTreeManage treeMan = GetTreeManage(sDataTreeKey);
            if (newFlag == "0") //修改
            {
                if (model.ID > 0)
                {
                    string[] EditNames = { "Name", "Remark", "Code" };
                    var SpecModel = treeMan.GetSpecialModelFromCurrentModel(model);
                    if (treeMan.SaveOneRecord(SpecModel, EditNames))
                        return RedirectToActionPermanent("AfterSaveNode", "DataTree", new { area = "UserControl", PanelID = sDataTreeName, ID = ((tblTreeInfo)SpecModel).ID, Name = ((tblTreeInfo)SpecModel).Name });
                }
                else
                    X.Msg.Alert("注意", "请先选中要修改的节点！").Show();
            }
            else //新增
            {
                model.ParentIndex = model.ID;
                model.ID = 0;
                var SpecModel = (tblTreeInfo)treeMan.GetSpecialModelFromCurrentModel(model);
                long nNewID = treeMan.AddOneTreeNode(SpecModel);
                if (nNewID != 0)
                {
                    X.AddScript("var node = App." + sDataTreeName + ".getStore().getNodeById(" + SpecModel.ParentIndex + ");");
                    X.AddScript("App." + sDataTreeName + ".appendChild(node, {id:\"" + nNewID + "\",text:\"" + SpecModel.Name + "\",leaf:true});");
                    X.AddScript("App.direct." + sAreaName + ".TreeMaster_AskAddMoreNode('" + sMainFormName + "_WinNeedConfirm" + "'," + SpecModel.ParentIndex + ");");
                }
            }
            return this.Direct();
        }

        /// <summary>
        /// 新增过后，是否再次新增
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        [DirectMethod]
        public ActionResult TreeMaster_AskAddMoreNode(string sConfirmName, string sID)
        {            
            setParentVariable(null, null, "TreeMaster_ConfirmAddMore", sID, null);
            return RedirectToActionPermanent("WinNeedConfirm_Show", "WinNeedConfirm", new { area = "UserControl", PanelID = sConfirmName, HideVariable = ParentVar.DefaultVal, sHint = "新增成功！继续新增吗？" });
        }

        /// <summary>
        /// 确定继续新增
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="ID"></param>
        /// <param name="SureType"></param>
        /// <returns></returns>
        public ActionResult TreeMaster_ConfirmAddMore(string PanelID, string ID, bool SureType)
        {
            if (SureType)
                TreeMaster_PrepareAddNextLevel(PanelID.Substring(0, PanelID.LastIndexOf('_')), Convert.ToInt64(ID), true);
            else
                TreeMaster_PrepareAddNextLevel(PanelID.Substring(0, PanelID.LastIndexOf('_')), Convert.ToInt64(ID), false);
            return this.Direct();
        }


        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_DeleteClick(tblTreeInfo model)
        {
            if (model.ID > 0)
            {
                var treeMan = GetTreeManage(sDataTreeKey);
                var SpecModel = treeMan.GetSpecialModelFromCurrentID(model.ID);
                if (treeMan.DeleteRecordByDelflag(SpecModel))
                {
                    if (((tblTreeInfo)SpecModel).ParentIndex != -1) //修改上级节点
                    {
                        var ParentModel = treeMan.GetSpecialModelFromCurrentID(((tblTreeInfo)SpecModel).ParentIndex);
                        ParentModel.ChildNum--;
                        if (ParentModel.ChildNum == 0)
                            ParentModel.IsLeaf = true;
                        string[] EditNames = { "ChildNum", "IsLeaf" };
                        if (treeMan.SaveOneRecord(ParentModel, EditNames))
                            return RedirectToActionPermanent("AfterDeleteNode", "DataTree", new { area = "UserControl", PanelID = sDataTreeName, model.ID });
                    }
                    else
                        return RedirectToActionPermanent("AfterDeleteNode", "DataTree", new { area = "UserControl", PanelID = sDataTreeName, model.ID });
                }
            }
            else
                X.Msg.Alert("注意", "请先选中要删除的节点！").Show();
            return this.Direct();
        }
        
        /// <summary>
        /// 点击重置按钮
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_ResetClick()
        {
            SetHiddenValue(sMainFormName + "_AddorSave", "0");

            tblTreeInfo model = new tblTreeInfo();
            model.ID = 0;
            var form = this.GetCmp<FormPanel>(sMainFormName);
            form.SetValues(model);

            X.AddScript("App.direct." + sAreaName + ".TreeMaster_ClearDataTreeSelection('" + sDataTreeName + "');");

            var btnAddNextLevel = this.GetCmp<Button>(sMainFormName + "_btnAddNextLevel");
            var btnDel = this.GetCmp<Button>(sMainFormName + "_btnDelete");
            var btnOrderUp = this.GetCmp<Button>(sMainFormName + "_btnOrderUp");
            var btnOrderDown = this.GetCmp<Button>(sMainFormName + "_btnOrderDown");
            var btnMore = this.GetCmp<Button>(sMainFormName + "_btnMore");
            var btnMore2 = this.GetCmp<Button>(sMainFormName + "_btnMore2");
            btnAddNextLevel.Disabled = false;
            btnDel.Disabled = false;
            btnOrderUp.Disabled = false;
            btnOrderDown.Disabled = false;
            btnMore.Disabled = false;
            btnMore2.Disabled = false;
            X.AddScript("App.direct." + sAreaName + ".TreeMaster_DisableDataTree('" + sDataTreeName + "',false);");
            return this.Direct();
        }

        /// <summary>
        /// 点击新增按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_AddNextLevelClick(tblTreeInfo model)
        {
            if (model.ID != 0)
            {
                var loginUser = System.Web.HttpContext.Current.Request.Cookies.Get("LoginUser");
                CryptManage crypt = new CryptManage();
                string sAuthorIDs = crypt.Decrypto(loginUser.Values["UserRoles"]);
                string sUserDepartmentID = crypt.Decrypto(loginUser.Values["DepartmentID"]);
                AuthorizationInfoDataListManage manAuthor = new AuthorizationInfoDataListManage();
                List<tblAuthorizationInfo> authors = manAuthor.GetRecordsByID<tblAuthorizationInfo>(sAuthorIDs);
                if (!authors.Select(x => x.Name).ToArray().Contains(UserRoleName.SUPER)) //如果不是超级管理员，不能新增顶层节点
                {
                    if (model.ID == -1)
                    {
                        X.Msg.Alert("注意", "当前权限不能在顶层节点下新增！").Show();
                        return this.Direct();
                    }
                }                
                return TreeMaster_PrepareAddNextLevel(sMainFormName, model.ID, true);
            }
            else
                X.Msg.Alert("注意", "请先选择上级节点！").Show();
            return this.Direct();
        }

        /// <summary>
        /// 点击上移按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_OrderUpClick(tblTreeInfo model)
        {
            if (model.ID > 0)
            {
                DataTreeManage treeMan = GetTreeManage(sDataTreeKey);
                if (treeMan.OrderUp(model.ID))
                    return RedirectToActionPermanent("AfterOrderUp", "DataTree", new { area = "UserControl", PanelID = sDataTreeName, sID = model.ID });
            }
            else
                X.Msg.Alert("注意", "请先选中要移动的节点！").Show();
            return this.Direct();
        }

        /// <summary>
        /// 点击下移按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult TreeMaster_OrderDownClick(tblTreeInfo model)
        {
            if (model.ID > 0)
            {
                DataTreeManage treeMan = GetTreeManage(sDataTreeKey);
                if (treeMan.OrderDown(model.ID))
                    return RedirectToActionPermanent("AfterOrderDown", "DataTree", new { area = "UserControl", PanelID = sDataTreeName, sID = model.ID });
            }
            else
                X.Msg.Alert("注意", "请先选中要移动的节点！").Show();
            return this.Direct();
        }

        public virtual ActionResult TreeMaster_MoreInfoClick(tblTreeInfo model)
        {
            return this.Direct();
        }
        public virtual ActionResult TreeMaster_More2InfoClick(tblTreeInfo model)
        {
            return this.Direct();
        }
        public virtual ActionResult TreeMaster_InportClick()
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            //首先把文件临时copy到服务器上            
            var upFile = this.GetCmp<FileUploadField>(sMainFormName + "_btnInport");
            if (upFile.HasFile)
            {
                FileManager clsFile = new FileManager();
                string sFilePath = ConstAddressPath.TEMP_WEBPATH;
                string sAddress = DateTime.Now.ToFileTimeUtc().ToString() + upFile.FileName.Remove(0, upFile.FileName.LastIndexOf('.'));
                if (clsFile.UploadFile(upFile, sFilePath + sAddress))
                {
                    SetHiddenValue(sMainFormName + "_txtInportFileName", sAddress);
                    return result;
                }
            }
            result.IsUpload = false;
            return result;
        }

        public virtual ActionResult TreeMaster_AfterInportClick(string sAddress)
        {
            Excel clsExcel = new Excel();
            string sFilePath = ConstAddressPath.TEMP_WEBPATH;
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            string sHint = clsExcel.WholeStep(sFilePath + sAddress, sDataTreeKey);
            X.AddScript("Ext.Msg.alert('注意', '" + sHint + "');");
            return result;
        }

    }
}