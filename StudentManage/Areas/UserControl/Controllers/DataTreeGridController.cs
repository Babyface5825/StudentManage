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
    public class DataTreeGridController : ParentController
    {
        protected DataTreeManage GetTreeManager(string sKeyWord = null)
        {
            if (sKeyWord.Contains("RoleAuthorization"))
            {
                RoleAuthorizationTreeGridManage treeMan = new RoleAuthorizationTreeGridManage();
                return treeMan;
            }
            else
            {
                DataTreeManage treeMan = new DataTreeManage();
                return treeMan;
            }
        }

        #region Init

        /// <summary>
        /// 初始化树的节点
        /// </summary>
        /// <returns></returns>
        public virtual StoreResult InitTreeGridNodes(string node,string ParentVariable, string sRelationIDs)
        {
            GetParentVariable(ParentVariable);
            var treeMan = GetTreeManager(ParentVar.controlName);
            return this.Store(treeMan.InitTreeGridNodes(node,ParentVariable, sRelationIDs));
        }

        /// <summary>
        /// 通过LoadProxy方式刷新树节点
        /// </summary>
        /// <param name="type">当前树编号，用于区分一个主视图多个分部视图的情况</param>
        protected void RefreashTreeGridStoreOnce(string sKeyWord)
        {            
            var store = this.GetCmp<Store>(sKeyWord + "_Store");
            store.LoadProxy();
        }
        #endregion

        #region TreeGridNodeClick
        
        public ActionResult DataTreeGridSetAuthorSel(string PanelID, string sAuthorID)
        {
            SetHiddenValue(PanelID + "_RelationID", sAuthorID);            
            RefreashTreeGridStoreOnce(PanelID);
            return this.Direct();
        }

        /// <summary>
        /// 点击TreeGrid的新增行激发的action
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="node">对应当前点击的节点行</param>
        /// <param name="authorizationId">对应当前权限的Id</param>
        /// <returns></returns>
        public virtual ActionResult TreeGridNodeAddClick(string PanelID, string node, string authorizationId)
        {            
            TreeGridModel tg = JSON.Deserialize<TreeGridModel>(node);
            if (VerifyAuthor(PanelID, authorizationId, tg.sID, OperationType.ADD))
            {
                //首先处理同行
                SetGridAll(PanelID, tg.sID);
                //然后处理非叶子节点
                SetAllLeafNode(PanelID, tg, OperationType.ADD);
                //然后处理上级节点
                SetParentNode(PanelID, tg, OperationType.ADD);
            }
            return this.Direct();
        }

        /// <summary>
        /// 点击TreeGrid的修改行激发的action
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="node">对应当前点击的节点行</param>
        /// <param name="authorizationId">对应当前权限的Id</param>
        /// <returns></returns>
        public virtual ActionResult TreeGridNodeEditClick(string PanelID, string node, string authorizationId)
        {
            TreeGridModel tg = JSON.Deserialize<TreeGridModel>(node);
            if (VerifyAuthor(PanelID, authorizationId, tg.sID, OperationType.EDIT))
            {
                //首先处理同行
                SetGridAll(PanelID, tg.sID);
                //然后处理非叶子节点
                SetAllLeafNode(PanelID, tg, OperationType.EDIT);
                //然后处理上级节点
                SetParentNode(PanelID, tg, OperationType.EDIT);
            }
            return this.Direct();
        }

        /// <summary>
        /// 点击TreeGrid的删除行激发的action
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="node">对应当前点击的节点行</param>
        /// <param name="authorizationId">对应当前权限的Id</param>
        /// <returns></returns>
        public virtual ActionResult TreeGridNodeDeleteClick(string PanelID, string node, string authorizationId)
        {
            TreeGridModel tg = JSON.Deserialize<TreeGridModel>(node);
            if (VerifyAuthor(PanelID, authorizationId, tg.sID, OperationType.DELETE))
            {
                //首先处理同行
                SetGridAll(PanelID, tg.sID);
                //然后处理非叶子节点
                SetAllLeafNode(PanelID, tg, OperationType.DELETE);
                //然后处理上级节点
                SetParentNode(PanelID, tg, OperationType.DELETE);
            }
            return this.Direct();
        }

        /// <summary>
        /// 点击TreeGrid的查询行激发的action
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="node">对应当前点击的节点行</param>
        /// <param name="authorizationId">对应当前权限的Id</param>
        /// <returns></returns>
        public virtual ActionResult TreeGridNodeSearchClick(string PanelID, string node, string authorizationId)
        {
            TreeGridModel tg = JSON.Deserialize<TreeGridModel>(node);
            if (VerifyAuthor(PanelID, authorizationId, tg.sID, OperationType.SEARCH))
            {
                //首先处理同行
                SetGridAll(PanelID, tg.sID);
                //然后处理非叶子节点
                SetAllLeafNode(PanelID, tg, OperationType.SEARCH);
                //然后处理上级节点
                SetParentNode(PanelID, tg, OperationType.SEARCH);
            }
            return this.Direct();
        }

        /// <summary>
        /// 点击TreeGrid的全部行激发的action
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="node">对应当前点击的节点行</param>
        /// <param name="authorizationId">对应当前权限的Id</param>
        /// <returns></returns>
        public virtual ActionResult TreeGridNodeAllClick(string PanelID, string node, string authorizationId)
        {            
            TreeGridModel tg = JSON.Deserialize<TreeGridModel>(node);
            if (VerifyAuthor(PanelID, authorizationId, tg.sID, OperationType.ALL))
            {
                if (!tg.IsLeaf)
                {
                    SetAllLeafNode(PanelID, tg, OperationType.ALL);
                    SetParentNode(PanelID, tg, OperationType.ALL);
                }
                else
                {
                    X.AddScript("var node = App." + PanelID + ".getStore().getNodeById(" + tg.sID + ");");
                    if (tg.bAll)
                    {
                        X.AddScript("node.set(\"bAdd\",true);");
                        X.AddScript("node.set(\"bEdit\",true);");
                        X.AddScript("node.set(\"bSearch\",true);");
                        X.AddScript("node.set(\"bDelete\",true);");
                    }
                    else
                    {
                        X.AddScript("node.set(\"bAdd\",false);");
                        X.AddScript("node.set(\"bEdit\",false);");
                        X.AddScript("node.set(\"bSearch\",false);");
                        X.AddScript("node.set(\"bDelete\",false);");
                    }
                    SetParentNode(PanelID, tg, OperationType.ALL);
                }
            }
            return this.Direct();
        }

        #endregion

        #region TreeGridJavaScript

        /// <summary>
        /// 设置当前节点的All状态
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="sID">当前节点ID，如果没有表示js设置的</param>
        private void SetGridAll(string _PanelID, long ID)
        {
            X.AddScript("var node = App." + _PanelID + ".getStore().getNodeById(" + ID + ");");
            SetGridAll();
        }

        private void SetGridAll()
        {
            X.AddScript("var b1=node.get(\"bAdd\");");
            X.AddScript("var b2=node.get(\"bEdit\");");
            X.AddScript("var b3=node.get(\"bDelete\");");
            X.AddScript("var b4=node.get(\"bSearch\");");
            X.AddScript("if (b1 && b2 && b3 && b4)");
            X.AddScript("node.set(\"bAll\",true);");
            X.AddScript("else ");
            X.AddScript("node.set(\"bAll\",false);");
        }

        /// <summary>
        /// 设置所有子节点的状态
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// /// <param name="tg">传过来的model</param>
        /// <param name="type">操作类型：增删改查</param>
        /// <param name="bAll">是否设置all</param>
        private void SetAllLeafNode(string _PanelID, TreeGridModel tg, int type)
        {
            if (!tg.IsLeaf)
            {
                X.AddScript("function SetNodes(node, flag){");
                bool flag = true;
                switch (type)
                {
                    case OperationType.ADD: X.AddScript("node.set(\"bAdd\",flag);"); flag = tg.bAdd; break;
                    case OperationType.EDIT: X.AddScript("node.set(\"bEdit\",flag);"); flag = tg.bEdit; break;
                    case OperationType.DELETE: X.AddScript("node.set(\"bDelete\",flag);"); flag = tg.bDelete; break;
                    case OperationType.SEARCH: X.AddScript("node.set(\"bSearch\",flag);"); flag = tg.bSearch; break;
                    case OperationType.ALL:
                        X.AddScript("node.set(\"bAdd\",flag);");
                        X.AddScript("node.set(\"bEdit\",flag);");
                        X.AddScript("node.set(\"bDelete\",flag);");
                        X.AddScript("node.set(\"bSearch\",flag);");
                        flag = tg.bAll;
                        break;
                }
                SetGridAll();
                X.AddScript("if (!node.isLeaf()) {");
                X.AddScript("for (var i=0;i<node.childNodes.length;i++)");
                X.AddScript("SetNodes(node.childNodes[i], flag);");
                X.AddScript("}}");

                X.AddScript("var node = App." + _PanelID + ".getStore().getNodeById(" + tg.sID + ");");
                if (flag)
                    X.AddScript("SetNodes(node, true);");
                else
                    X.AddScript("SetNodes(node, false);");
            }
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="jsKey">操作类型对应的字符形式，例如bAdd,bEdit……</param>
        private void SetGridParent(int type, string jsKey)
        {
            if (type != OperationType.ALL)
            {
                X.AddScript("node.parentNode.set(\"" + jsKey + "\",flag);");
                X.AddScript("var b1=node.parentNode.get(\"bAdd\");");
                X.AddScript("var b2=node.parentNode.get(\"bEdit\");");
                X.AddScript("var b3=node.parentNode.get(\"bDelete\");");
                X.AddScript("var b4=node.parentNode.get(\"bSearch\");");
                X.AddScript("if (b1 && b2 && b3 && b4) ");
                X.AddScript("node.parentNode.set(\"bAll\",true);");
                X.AddScript("else ");
                X.AddScript("node.parentNode.set(\"bAll\",false);");
            }
            else
            {
                X.AddScript("node.parentNode.set(\"bAdd\",flag);");
                X.AddScript("node.parentNode.set(\"bEdit\",flag);");
                X.AddScript("node.parentNode.set(\"bDelete\",flag);");
                X.AddScript("node.parentNode.set(\"bSearch\",flag);");
                X.AddScript("node.parentNode.set(\"bAll\",flag);");
            }
            X.AddScript("SetParentNodes(node.parentNode.parentNode, flag);");
        }


        /// <summary>
        /// 设置所有父亲节点的状态
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="tg">传过来的model</param>
        /// <param name="type">操作类型：增删改查</param>
        private void SetParentNode(string _PanelID, TreeGridModel tg, int type)
        {
            bool flag = true;
            X.AddScript("function SetParentNodes(node, flag){");
            X.AddScript("if (node != null) {");
            string jsKey = "";
            switch (type)
            {
                case OperationType.ADD: flag = tg.bAdd; jsKey = "bAdd"; break;
                case OperationType.EDIT: flag = tg.bEdit; jsKey = "bEdit"; break;
                case OperationType.DELETE: flag = tg.bDelete; jsKey = "bDelete"; break;
                case OperationType.SEARCH: flag = tg.bSearch; jsKey = "bSearch"; break;
                case OperationType.ALL: flag = tg.bAll; jsKey = "bAll"; break;
            }
            X.AddScript("if (flag) {");//如果是true，需要查看parentnode的所有兄弟节点是否也是true
            X.AddScript("var prenode = node.previousSibling;");
            X.AddScript("var hereflag = true;");
            X.AddScript("while ((hereflag) && (prenode != null)) {");
            X.AddScript(" if (prenode.get(\"" + jsKey + "\")) ");
            X.AddScript("prenode = prenode.previousSibling;");
            X.AddScript(" else hereflag = false");
            X.AddScript(" }");
            X.AddScript("var nextnode = node.nextSibling;");
            X.AddScript("while ((hereflag) && (nextnode != null)) {");
            X.AddScript(" if (nextnode.get(\"" + jsKey + "\")) ");
            X.AddScript("nextnode = nextnode.nextSibling;");
            X.AddScript(" else hereflag = false");
            X.AddScript(" }");
            X.AddScript("if ((hereflag) && (node.parentNode != null)) {");
            SetGridParent(type, jsKey);
            X.AddScript(" }");
            X.AddScript(" }");
            X.AddScript(" else {");
            X.AddScript("if (node.parentNode != null) { ");
            SetGridParent(type, jsKey);
            X.AddScript(" }");
            X.AddScript(" }");
            X.AddScript(" }");
            X.AddScript(" }");
            X.AddScript("var node = App." + _PanelID + ".getStore().getNodeById(" + tg.sID + ");");
            if (flag)
                X.AddScript("SetParentNodes(node, true);");
            else
                X.AddScript("SetParentNodes(node, false);");
        }

        /// <summary>
        /// 验证当前是否选择了角色，如果没有则给出错误提示并还原
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <param name="authorizationId">角色ID</param>
        /// <param name="ID">当前选择的节点行</param>
        /// <param name="type">当前选择的类型：增删改查all</param>
        /// <returns></returns>
        private bool VerifyAuthor(string _PanelID, string authorizationId, long ID, int type)
        {
            if (authorizationId == "0")
            {
                X.AddScript("var node = App." + _PanelID + ".getStore().getNodeById(" + ID + ");");
                switch (type)
                {
                    case OperationType.ADD: X.AddScript("node.set(\"bAdd\",false);"); break;
                    case OperationType.EDIT: X.AddScript("node.set(\"bEdit\",false);"); break;
                    case OperationType.DELETE: X.AddScript("node.set(\"bDelete\",false);"); break;
                    case OperationType.SEARCH: X.AddScript("node.set(\"bSearch\",false);"); break;
                    case OperationType.ALL: X.AddScript("node.set(\"bAll\",false);"); break;
                }
                X.Msg.Alert("注意", "请先选择要配置的角色！").Show();
                return false;
            }
            else
                return true;
        }

        #endregion

        /// <summary>
        /// 提交树
        /// </summary>
        /// <param name="data">提交上来的节点</param>
        /// <returns></returns>
        public ActionResult TreeGridPanelCommit(SubmittedNode data, string ParentVariable, string PanelID)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage treeMan = GetTreeManager(ParentVar.KeyWords);
            X.AddScript("App.direct.UserControl.DataTreeGrid_PanelCommitDirectMethod(" + JSON.Serialize(data) + ",\"" + PanelID + "\",\"" + ParentVariable + "\",App." + PanelID + "_RelationID.getValue());");
            return this.Direct();
        }

        [DirectMethod]
        public ActionResult DataTreeGrid_PanelCommitDirectMethod(string data, string PanelID, string ParentVariable, string sAuthorID)
        {
            GetParentVariable(ParentVariable);
            DataTreeManage treeMan = GetTreeManager(ParentVar.KeyWords);
            SubmittedNode nodes = JSON.Deserialize<SubmittedNode>(data);
            treeMan.SaveAllTreeGridAuthor(sAuthorID, nodes);
            RefreashTreeGridStoreOnce(PanelID);
            return this.Direct();
        }

        /// <summary>
        /// 全部展开树
        /// </summary>
        /// <param name="PanelID">当前Grid的PanelID</param>
        /// <returns></returns>
        public ActionResult ExpandTreeGridPanel(string PanelID)
        {
            X.AddScript("App." + PanelID + ".expandAll();");
            return this.Direct();
        }

    }
}