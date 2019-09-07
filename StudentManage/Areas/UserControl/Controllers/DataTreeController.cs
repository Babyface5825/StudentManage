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
    [DirectController(AreaName = "UserControl", GenerateProxyForOtherAreas = true)]
    public class DataTreeController : ParentController
    {

        /// <summary>
        /// 初始化树的节点，需要在调用前定义TempData中TreeAuthorNowID
        /// </summary>
        /// <returns></returns>
        public virtual StoreResult InitTreeNodes(string node, string ParentVariable, string PanelID, string sRelationIDs, string sNowNames, string sNowValues, string sNowSigns, string sAuthorIDs)
        {
            try
            {
                GetParentVariable(ParentVariable);
                var treeMan = GetTreeManage(ParentVar.KeyWords);
                if (sAuthorIDs != "0")
                {
                    switch (ParentVar.KeyWords)
                    {
                        case "DepartmentInfo":
                            if (sRelationIDs != "0")
                            {
                                AuthorizationInfoDataListManage manAuthor = new AuthorizationInfoDataListManage();
                                List<tblAuthorizationInfo> authors = manAuthor.GetRecordsByID<tblAuthorizationInfo>(sAuthorIDs);
                                if (!authors.Select(x => x.Name).ToArray().Contains(UserRoleName.SUPER)) //如果不是超级管理员，只显示其所在部门和下级部门
                                {
                                    sNowNames += CosValue.SPLITOPERATOR1 + "ID";
                                    sNowSigns += CosValue.SPLITOPERATOR1 + SIGN.In.ToString();
                                    DepartmentInfoManage manDepart = new DepartmentInfoManage();
                                    sNowValues += manDepart.GetAllParent(sRelationIDs) + CosValue.SPLITOPERATOR2 + manDepart.GetAllLeaf(sRelationIDs);
                                    if (sNowNames[0] == '|')
                                    {
                                        sNowNames = sNowNames.Remove(0, 1);
                                        sNowValues = sNowValues.Remove(0, 1);
                                        sNowSigns = sNowSigns.Remove(0, 1);
                                    }
                                    SetHiddenValue(PanelID + "_NowNames", sNowNames);
                                    SetHiddenValue(PanelID + "_NowValues", sNowValues);
                                    SetHiddenValue(PanelID + "_NowSigns", sNowSigns);
                                }
                            }
                            sAuthorIDs = "0"; //部门和角色表没有关联，所以把角色设为0，只通过上面代码进行分拣
                            break;
                        default: sAuthorIDs = "0"; break;
                    }
                }
                NodeCollection nodes = treeMan.InitTree(node, sAuthorIDs, sNowNames, sNowValues, sNowSigns);
                if (ParentVariable.Contains("MainPage"))
                    TempData["FirstTreeNodeId"] = treeMan.sFirstLeafNodeID;
                return this.Store(nodes);
                
            }
            catch (Exception e)
            {
                return null;
            }
        }


        #region TreeNodeClick
        /// <summary>
        /// 树节点的点击，继承函数去做其他的工作
        /// </summary>
        /// <param name="_PanelID">当前Grid的PanelID></param>
        /// <param name="nodeId">页面传过来点击的id</param>
        /// <returns></returns>
        public ActionResult TreeNodeClick(string nodeId, string ParentVariable, string sNowNames, string sNowValues, string sNowSigns)
        {
            if (ParentVariable != null)
            {
                GetParentVariable(ParentVariable);
                return RedirectToActionPermanent("TreeNodeClick", ParentVar.controlName, new { area = ParentVar.areaName, PageKeyWord = ParentVar.KeyWords, sTreeSelID = nodeId, sNowNames, sNowValues, sNowSigns });
            }
            else
                return this.Direct();
        }

        /// <summary>
        /// checktree时才会用到
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sAuthorID"></param>
        /// <returns></returns>
        public ActionResult DataTreeSetAuthorSel(string PanelID, string sAuthorID)
        {
            SetHiddenValue(PanelID + "_HiddenAuthorizationID", sAuthorID);
            var store = this.GetCmp<Store>(PanelID + "_Store");
            store.LoadProxy();
            return this.Direct();
        }
        #endregion

        public ActionResult RemoteEdit(string id, string field, string newValue, string oldValue)
        {
            return this.RemoteTree(newValue);
        }

        public ActionResult RemoteAppend(string parentId, string text)
        {
            return this.RemoteTree(null, "newId", new { text = text });
        }

        public ActionResult RemoteRemove(string id, string parentId)
        {
            return this.RemoteTree();
        }

        public ActionResult ExpandTreePanel(string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            var treeMan = GetTreeManage(ParentVar.KeyWords);
            if (treeMan.bIsExpand)
                X.AddScript("App." + PanelID + ".expandAll();");
            return this.Direct();
        }

        public ActionResult AfterSaveNode(string PanelID, string ID, string Name)
        {
            X.AddScript("var node = App." + PanelID + ".getStore().getNodeById(" + ID + ");");
            X.AddScript("App." + PanelID + ".editNode(node, 'text', '" + Name + "');");
            return this.Direct();
        }
        
        public ActionResult AfterDeleteNode(string PanelID, string ID)
        {
            X.AddScript("var node = App." + PanelID + ".getStore().getNodeById(" + ID + ").previousSibling;");
            X.AddScript("if (node == null) node = App." + PanelID + ".getStore().getNodeById(" + ID + ").nextSibling;");
            X.AddScript("if (node == null) node = App." + PanelID + ".getStore().getNodeById(" + ID + ").parentNode;");
            X.AddScript("if (node != null) ");
            X.AddScript("App." + PanelID + "_View.getSelectionModel().select(node);");            
            X.AddScript("var node = App." + PanelID + ".getStore().getNodeById(" + ID + ");");
            X.AddScript("App." + PanelID + ".removeNode(node);");
            return this.Direct();
        }
        
        public ActionResult AfterOrderUp(string PanelID, string sID)
        {
            X.AddScript("var Node = App." + PanelID + ".getStore().getNodeById(" + sID + ").previousSibling;");
            X.AddScript("var nNode = App." + PanelID + ".getStore().getNodeById(" + sID + ");");
            X.AddScript("var pNode = nNode.parentNode;");
            X.AddScript("pNode.insertBefore(nNode,Node);");
            return this.Direct();
        }

        public ActionResult AfterOrderDown(string PanelID, string sID)
        {
            X.AddScript("var Node = App." + PanelID + ".getStore().getNodeById(" + sID + ").nextSibling;");
            X.AddScript("var nNode = App." + PanelID + ".getStore().getNodeById(" + sID + ");");
            X.AddScript("var pNode = nNode.parentNode;");
            X.AddScript("pNode.insertBefore(Node,nNode);");
            return this.Direct();
        }

        public ActionResult DataTreePanelCommit(SubmittedNode data, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            var treeMan = GetTreeManage(ParentVar.transValue);
            X.AddScript("App.direct.UserControl.DataTree_PanelCommitDirectMethod(\"" + treeMan.GetAllSelectNodes(data) + "\",\"" + PanelID + "\",\"" + ParentVariable + "\",App." + PanelID + "_HiddenAuthorizationID.getValue());");
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult DataTree_PanelCommitDirectMethod(string sAllSelectNodes, string PanelID, string ParentVariable, string sAuthorID)
        {
            GetParentVariable(ParentVariable);
            var treeMan = GetTreeManage(ParentVar.transValue);
            treeMan.sbNode = sAllSelectNodes;
            treeMan.SaveAllTreeCheck(sAuthorID);
            var store = this.GetCmp<Store>(PanelID + "_Store");
            store.LoadProxy();
            return this.Direct();
        }



        public ActionResult DisableDataTree(string PanelID, bool flag)
        {
            var tree = this.GetCmp<TreePanel>(PanelID);
            tree.Disabled = flag;
            return this.Direct();
        }

        public ActionResult ClearDataTreeSelection(string PanelID)
        {
            X.AddScript("App." + PanelID + "_View.getSelectionModel().deselectAll();");
            return this.Direct();
        }


        public ActionResult InitMenuCenter(string ParentVariable)
        {
            if (ParentVariable.Contains("MainPage"))
            {
                GetParentVariable(ParentVariable);
                return RedirectToActionPermanent("InitMenuCenter", ParentVar.controlName, new { area = ParentVar.areaName, sFirstLeafID = TempData["FirstTreeNodeId"] });
            }
            return this.Direct();
        }


    }
}