using System;
using StudentManage.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections;
using Ext.Net;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace StudentManage.Manage
{
    /// <summary>
    /// 树的根类应该能解决树的大部分功能操作，子类给出具体树名，以及部分特殊操作
    /// </summary>

    public class TreeGridModel
    {
        public long sID { get; set; }
        public string sName { get; set; }
        public bool IsLeaf { get; set; }
        public bool bAdd { get; set; }
        public bool bEdit { get; set; }
        public bool bDelete { get; set; }
        public bool bSearch { get; set; }
        public bool bAll { get; set; }

        public TreeGridModel()
        {
            sID = 0;
            sName = "";
            IsLeaf = false;
            bAdd = false;
            bEdit = false;
            bDelete = false;
            bSearch = false;
            bAll = false;
        }
    }


    /// <summary>
    /// 跟树相关的各种操作
    /// </summary>
    public class DataTreeManage : ParentManage 
    {        
        public int nMaxLevel;
        /// <summary>
        /// 下面部分和权限设定相关
        /// </summary>
        public ArrayList arrNowAuthorization; //当前Authorization的具体情况        
        public string sbNode;  //提交过来的节点编号
        public bool bIsCheck; //是否前面有checkbox，默认false，没有
        public bool bIsExpand; //初始是否展开，默认false，不展开
        public bool bIncludeAuthorLeaf; //权限数组时，是否自动加入所有叶子节点

        public string sFirstLeafNodeID = "0";
        public long nNowID, nNowID1, nNowID2, nNowID3; //用户ThreeListComboBox

        public DataTreeManage()
        {            
            nMaxLevel = -1;
            arrNowAuthorization = new ArrayList();
            bIsCheck = false;
            bIsExpand = false;
            bIncludeAuthorLeaf = false;
        }

        #region 供子类调用的虚类

        private string GetDefaultRootName()
        {
            switch (Manage_KeyWord)
            {
                case "OperationInfo": return "所有菜单";
                case "AuthorizationChannelType": return "所有栏目";
                case "DepartmentInfo": return "所有单位/部门";
                case "dqdmTable": return "所有地区";
                default: return "所有节点";
            }
        }

        /// <summary>
        /// 根据传过来的parentindex获取若干个TreeModel
        /// </summary>
        /// <param name="parentindex"></param>
        /// <returns></returns>
        public virtual object[] GetSomeTreeModels<T>(T ParentIndex, string sRelationIDs = "0", string sNowNames = "", string sNowValues = "", string sNowSigns = "")
        {
            try
            {
                long parentindex = Convert.ToInt64(ParentIndex);
                Type type = GetTypeObj().GetType();
                dynamic obj = type.Assembly.CreateInstance(type.ToString());
                return _GetSomeTreeModels(obj, parentindex, sRelationIDs, true, sNowNames, sNowValues, sNowSigns);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// 树的初始化
        /// </summary>
        /// <param name="sRelationIDs"></param>
        /// <param name="sNowNames"></param>
        /// <param name="sNowValues"></param>
        /// <param name="sNowSigns"></param>
        /// <returns></returns>
        public virtual NodeCollection InitTree(string nodeID, string sRelationIDs, string sNowNames = "", string sNowValues = "", string sNowSigns = "")
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _InitTree(nodeID,obj, sRelationIDs, GetDefaultRootName(), true, sNowNames, sNowValues, sNowSigns);
        } 

        /// <summary>
        /// 以字符串形式返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID，如果为0或-1，则返回所有节点</param>
        /// <returns>包括当前节点的所有子节点组成的字符串</returns>
        public virtual string GetAllLeaf<T>(T id)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetAllLeaf(obj, id);
        }

        /// <summary>
        /// 返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID，如果为0或-1，则返回所有节点</param>
        /// <returns>包括当前节点和所有子节点</returns>
        public virtual List<T> GetAllLeafNodes<T>(long id) where T : class
        {
            return _GetAllLeafNodes<T>(id);
        }

        /// <summary>
        /// 得到当前节点所有的父亲节点（不包括自己）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetAllParent<T>(T id)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetAllParent(obj, id);
        }
        /// <summary>
        /// 得到当前节点所有的父亲节点名称（不包括自己）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetAllParentName<T>(T id, bool own = false)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetAllParentName(obj, id, own);
        }

        /// <summary>
        /// 判断当前节点是否为叶子节点
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public virtual bool IsLeaf<T>(T ID) where T : class
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _IsLeaf(obj, ID);
        }
        /// <summary>
        /// 树节点上移
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public virtual bool OrderUp(long nodeid)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _OrderUp(obj, nodeid);
        }
        /// <summary>
        /// 树节点下移
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public virtual bool OrderDown(long nodeid)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _OrderDown(obj, nodeid);
        }



        /// <summary>
        /// 新增一个树节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual long AddOneTreeNode(tblTreeInfo model)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _AddOneTreeNode(obj, model);
        }
        public virtual void InitSimpleAuthorizationArray(string sRelationIDs)
        {
            //Type type = GetTypeObj().GetType();
            //dynamic obj = type.Assembly.CreateInstance(type.ToString());
            //tblOperationInfo obj = new tblOperationInfo();
            //_InitSimpleAuthorizationArray<tblOperationInfo, tblOperationAuthorRelation>(obj, sRelationIDs, "authorRelations", "AuthorID"); 
        }

        public virtual void InitAuthorizationArray(string sRelationIDs)
        {

        }

        /// <summary>
        /// 和TreeGrid相关的初始化
        /// </summary>
        /// <param name="ParentVariable"></param>
        /// <param name="sRelationIDs"></param>
        /// <returns></returns>
        public virtual NodeCollection InitTreeGridNodes(string nodeID,string ParentVariable, string sRelationIDs = "0")
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            NodeCollection nodes = _InitTreeGrid(nodeID, obj, ParentVariable, sRelationIDs, GetDefaultRootName());
            return nodes;
        }

        /// <summary>
        /// 根据当前的名称获得当前的ID，找不到返回0。主要用在ThreeListBox中
        /// </summary>
        /// <returns></returns>
        public virtual long GetNowIDFromName(string sName, string sParentID)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetNowIDFromName(obj, sName, sParentID);
        }


        /// <summary>
        /// 用在ThreeListBox中，根据当前节点获取上下级相关节点ID
        /// </summary>
        public virtual void GetAllIDs()
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            _GetAllIDs(obj);
        }

        /// <summary>
        /// 得到当前节点的最上层根节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual dynamic GetFirstParent(long id)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetFirstParent(obj, id);
        }
        #endregion

        #region 部分不好归类的private
        /// <summary>
        /// 根据传过来的父节点编号，权限数组IDs，以及查询关键字，返回部分树节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentindex">父节点编号</param>
        /// <param name="sRelationIDs">权限数组IDs</param>
        /// <param name="bGrid">是否grid</param>
        /// <param name="sNowNames">查询关键字名称</param>
        /// <param name="sNowValues">查询关键字内容</param>
        /// <param name="sNowSigns">查询关键字符号</param>
        /// <returns></returns>
        private object[] _GetSomeTreeModels<T>(T obj, long parentindex, string sRelationIDs, bool bGrid, string sNowNames, string sNowValues, string sNowSigns) where T : class
        {
            var where = GetWhereForStore<T>(parentindex, sNowNames, sNowValues, sNowSigns);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> result = GetRecord<T, short>(where, order);
            //下面判断权限
            bool flag = true;
            ArrayList arr = new ArrayList();
            for (int i = 0; i < result.Count; i++)
            {
                flag = true;
                //如果有权限
                if ((sRelationIDs != "") && (sRelationIDs != "0"))
                    flag = VerifyOneNode(obj, Convert.ToInt64(result[i].GetType().GetProperty("ID").GetValue(result[i], null)), bGrid);
                if (flag)
                {
                    tblTreeInfo model = new tblTreeInfo();
                    model.ID = Convert.ToInt64(result[i].GetType().GetProperty("ID").GetValue(result[i], null));
                    model.Code = Convert.ToString(result[i].GetType().GetProperty("Code").GetValue(result[i], null));
                    model.Name = Convert.ToString(result[i].GetType().GetProperty("Name").GetValue(result[i], null));
                    model.Remark = Convert.ToString(result[i].GetType().GetProperty("Remark").GetValue(result[i], null));
                    model.IsLeaf = Convert.ToBoolean(result[i].GetType().GetProperty("IsLeaf").GetValue(result[i], null));
                    arr.Add(model);
                }
            }
            tblTreeInfo[] ret = (tblTreeInfo[])arr.ToArray(typeof(tblTreeInfo));
            return ret;
        }
        /// <summary>
        /// 根据父节点编号以及其他查询条件，构造lambada表达式，返回where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentindex">父节点编号</param>
        /// <param name="sNowNames"></param>
        /// <param name="sNowValues"></param>
        /// <param name="sNowSigns"></param>
        /// <returns></returns>
        private dynamic GetWhereForStore<T>(long parentindex, string sNowNames = "", string sNowValues = "", string sNowSigns = "")
        {
            ArrayList arrNowNames = new ArrayList();
            ArrayList arrNowValues = new ArrayList();
            ArrayList arrNowSigns = new ArrayList();

            if (parentindex != 0)
            {
                arrNowNames.Add("ParentIndex");
                arrNowValues.Add(parentindex);
                arrNowSigns.Add(SIGN.Equal);
            }

            arrNowNames.Add("DelFlag");
            arrNowValues.Add(false);
            arrNowSigns.Add(SIGN.Equal);

            if ((sNowNames != "") && (sNowNames != null))
            {
                arrNowNames.AddRange(sNowNames.Split(CosValue.SPLITOPERATOR1).ToArray());
                arrNowValues.AddRange(sNowValues.Split(CosValue.SPLITOPERATOR1).ToArray());
                if (sNowSigns != "")
                    arrNowSigns.AddRange(sNowSigns.Split(CosValue.SPLITOPERATOR1).ToArray());
                else
                    arrNowSigns.AddRange(NewDuplicateArray(SIGN.Equal, (arrNowNames.Count - arrNowSigns.Count)));
            }
            return GetAndLambdaExpression<T>(arrNowNames, arrNowValues, arrNowSigns);            
        }

        private long _GetNowIDFromName<T>(T obj, string sName, string sParentID) where T : class
        {
            try
            {
                return Convert.ToInt64(sName);
            }
            catch
            {
                ArrayList arrNames = new ArrayList();
                ArrayList arrValues = new ArrayList();

                arrNames.Add("Name");
                arrValues.Add(sName);
                arrNames.Add("DelFlag");
                arrValues.Add(false);
                arrNames.Add("ParentIndex");
                arrValues.Add(sParentID);

                var where = GetAndLambdaExpression<T>(arrNames, arrValues);
                var list = GetRecord(where);
                if (list == null)
                    return 0;
                if (list.Count == 0)
                    return 0;
                return Convert.ToInt64(list[0].GetType().GetProperty("ID").GetValue(list[0], null));
            }
        }

        private void _GetAllIDs<T>(T obj) where T : class
        {
            if (nNowID == 0)
            {
                nNowID1 = 0;
                nNowID2 = 0;
                nNowID3 = 0;
            }
            if (nNowID == -1)
            {
                nNowID1 = -1;
                nNowID2 = -1;
                nNowID3 = -1;
            }
            else
            {
                tblTreeInfo model = GetSpecialModelFromCurrentID(nNowID);
                if (model != null)
                {
                    if (model.TheLevel == 1)
                    {
                        nNowID1 = nNowID;
                        nNowID2 = 0;
                        nNowID3 = 0;
                    }
                    else if (model.TheLevel == 2)
                    {
                        nNowID1 = model.ParentIndex;
                        nNowID2 = nNowID;
                        nNowID3 = 0;
                    }
                    else if (model.TheLevel == 3)
                    {
                        nNowID3 = nNowID;
                        nNowID2 = model.ParentIndex;
                        tblTreeInfo model2 = GetSpecialModelFromCurrentID(nNowID2);
                        if (model2 != null)
                            nNowID1 = model2.ParentIndex;
                    }
                }
            }
        }
        #endregion
        
        #region 和树的初始化相关
        /// <summary>
        /// 权限数组初始化所有可以显示的节点
        /// </summary>
        protected void _InitSimpleAuthorizationArray<T, T1>(T obj, string sRelationIDs, string authorRelationName, string authurIDName) where T : class
        {
            if ((sRelationIDs != "0") && (sRelationIDs != ""))
            {
                string[] allAuthorIDs = sRelationIDs.Split(CosValue.SPLITOPERATOR2);
                arrNowAuthorization.Clear();

                ArrayList names = new ArrayList();
                ArrayList values = new ArrayList();

                names.Add("DelFlag"); 
                values.Add(false); 

                var where = GetAndLambdaExpression<T>(names, values);
                List<T> list = GetRecord(where);

                foreach (T m in list)
                {
                    List<T1> nowAuthorRelations = (List<T1>)m.GetType().GetProperty(authorRelationName).GetValue(m, null);
                    foreach (T1 n in nowAuthorRelations)
                    {                        
                        string authorId = n.GetType().GetProperty(authurIDName).GetValue(n, null).ToString();
                        if (allAuthorIDs.Contains(authorId))
                        {                            
                            if (bIncludeAuthorLeaf)
                            {
                                string id = m.GetType().GetProperty("ID").GetValue(m, null).ToString();                                
                                string ss = _GetAllLeaf(obj, id);
                                List<T> models = GetRecordsByID<T>(ss);
                                foreach (T k in models)
                                    arrNowAuthorization.Add(k);
                            }
                            else
                                arrNowAuthorization.Add(m);
                        }
                    }
                }
            }
        }

        private NodeCollection _InitTree<T>(string NodeID,T obj, string sRelationIDs = "0", string sText = "所有菜单", bool bNeedRoot = true, string sNowNames = "", string sNowValues = "", string sNowSigns = "") where T : class
        {
            var nodes = new NodeCollection(false);
            InitSimpleAuthorizationArray(sRelationIDs);
            StartDFS(NodeID, obj, sRelationIDs, sText, nodes, false, sNowNames, sNowValues, sNowSigns);
            return nodes;
        }
        
        /// <summary>
        /// 准备DFS，进入DFS
        /// </summary>
        /// <param name="bNeedRoot">是否需要根节点</param>
        /// <param name="sText">根节点名称</param>
        /// <param name="nodes">返回的节点集合</param>
        /// <param name="bGrid">是普通tree(false)还是treegrid(true)</param>
        private void StartDFS<T>(string nodeID,T obj, string sRelationIDs, string sText, NodeCollection nodes, bool bGrid, string sNowNames = "", string sNowValues = "", string sNowSigns = "") where T : class
        {
            if (nodeID=="Root")
            {
                Node node = new Node();
                node.Text = sText;
                node.NodeID = "-1";
                node.Leaf = false;
                if (bGrid)
                {
                    TreeGridModel grid = new TreeGridModel();
                    grid.sID = -1;
                    grid.sName = sText;
                    node.AttributesObject = grid;
                }
                if (!DFS(nodeID,obj, sRelationIDs, nodes, node, bGrid, sNowNames, sNowValues, sNowSigns))
                    node.Leaf = true;
                nodes.Add(node);
            }
            else
                DFS(nodeID,obj, sRelationIDs, nodes, null, bGrid, sNowNames, sNowValues, sNowSigns);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="node">单个节点</param>
        /// <param name="bGrid">是普通tree还是treegrid</param>
        /// <returns>true:存在下层节点；flase：不存在下层节点</returns>
        private bool DFS<T>(string nodeID,T obj, string sRelationIDs, NodeCollection nodes, Node node, bool bGrid, string sNowNames = "", string sNowValues = "", string sNowSigns = "") where T : class
        {
            long nodeid = -1;
            if (nodeID!="Root")
                nodeid = Convert.ToInt64(nodeID);

            if (nMaxLevel != -1)
            {
                if (sNowNames == "")
                {
                    sNowNames = "TheLevel";
                    sNowValues = nMaxLevel.ToString();
                    sNowSigns = SIGN.LessThanOrEqual.ToString();
                }
                else
                {
                    sNowNames = sNowNames + CosValue.SPLITOPERATOR1 + "TheLevel";
                    sNowValues = sNowValues + CosValue.SPLITOPERATOR1 + nMaxLevel.ToString();
                    sNowSigns = sNowSigns + CosValue.SPLITOPERATOR1 + SIGN.LessThanOrEqual.ToString();
                }
            }
            var where = GetWhereForStore<T>(nodeid, sNowNames, sNowValues, sNowSigns);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> result = GetRecord<T, short>(where, order);// 分别获得省，市，区的每一层

            int j = 0; //实际显示的节点数目
            if ((result != null) && (result.Count > 0))
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (((sRelationIDs != "0") && (!bIsCheck) && (!bGrid)) && sRelationIDs != "") //首先判断是否为带权限的部分树
                    {
                        if (!VerifyOneNode(obj, Convert.ToInt64(result[i].GetType().GetProperty("ID").GetValue(result[i], null)), bGrid))
                            continue;
                    }
                    var tempNode = new Node();
                    //NodeCollection nodes = new Ext.Net.NodeCollection();
                    tempNode.NodeID = Convert.ToString(result[i].GetType().GetProperty("ID").GetValue(result[i], null));
                    tempNode.Text = Convert.ToString(result[i].GetType().GetProperty("Name").GetValue(result[i], null));
                    tempNode.Leaf = Convert.ToBoolean(result[i].GetType().GetProperty("IsLeaf").GetValue(result[i], null));
                    tempNode.Qtip = Convert.ToString(result[i].GetType().GetProperty("Remark").GetValue(result[i], null));

                    int nNum = GetAuthorizationPos(tempNode.NodeID, bGrid);
                    if (bGrid)
                    {
                        if (nNum != -1)
                        {
                            ((TreeGridModel)arrNowAuthorization[nNum]).sName = Convert.ToString(result[i].GetType().GetProperty("Name").GetValue(result[i], null));
                            ((TreeGridModel)arrNowAuthorization[nNum]).IsLeaf = Convert.ToBoolean(result[i].GetType().GetProperty("IsLeaf").GetValue(result[i], null));
                            tempNode.AttributesObject = (TreeGridModel)arrNowAuthorization[nNum];
                            

                        }
                        else
                        {
                            TreeGridModel grid = new TreeGridModel();
                            grid.sID = Convert.ToInt64(result[i].GetType().GetProperty("ID").GetValue(result[i], null));
                            grid.sName = Convert.ToString(result[i].GetType().GetProperty("Name").GetValue(result[i], null));
                            grid.IsLeaf = Convert.ToBoolean(result[i].GetType().GetProperty("IsLeaf").GetValue(result[i], null));
                            tempNode.AttributesObject = grid;
                        }
                    }
                    else// 如果不是带权限的树
                    {
                        if ((bIsCheck) && (tempNode.Leaf))
                        {
                            if (nNum != -1)
                                tempNode.Checked = true;
                            else
                                tempNode.Checked = false;
                        }
                    }
                    j++;

                    //if (!Convert.ToBoolean(result[i].GetType().GetProperty("IsLeaf").GetValue(result[i], null)))//不是叶子节点,则继续
                    //{
                    //    if (!DFS(obj, sRelationIDs, nodes, tempNode, bGrid, sNowNames, sNowValues, sNowSigns))
                    //        tempNode.Leaf = true;
                    //}
                    //else
                    //{
                    //    tempNode.Leaf = true;
                    //    if (sFirstLeafNodeID == "0")
                    //        sFirstLeafNodeID = tempNode.NodeID;
                    //}
                    if (node != null)
                        node.Children.Add(tempNode);
                    else
                        nodes.Add(tempNode);

                }
            }
            if (j > 0)
                return true;
            else
                return false;               
        }
        

        #endregion

        #region 树的新增
        
        private long _AddOneTreeNode<T>(T obj, tblTreeInfo model) where T : class
        {
            //首先处理Level
            int nLevel = 1;
            if (model.ParentIndex != -1)
            {
                //获取父节点的model
                var ParentModel = GetOneRecordbyID<T, long>(model.ParentIndex);
                ParentModel.ChildNum++;
                ParentModel.IsLeaf = false;
                nLevel = ParentModel.TheLevel + 1;
                string[] EditColumns = { "ChildNum", "IsLeaf" };
                SaveOneRecord<T>(ParentModel, EditColumns);
            }
            //然后获取本层节点的order
            int nOrder = Convert.ToInt16(obj.GetType().GetProperty("TheOrder").GetValue(obj, null));
            if (nOrder == 0)
            {
                ArrayList names = new ArrayList();
                ArrayList values = new ArrayList();
                names.Add("ParentIndex");
                names.Add("DelFlag");
                values.Add(model.ParentIndex);
                values.Add(false);
                var express = GetAndLambdaExpression<T>(names, values);
                var order = BuildOrder<T, short>("TheOrder");
                List<T> result = GetRecord(express, order, false);
                if ((result != null) && (result.Count > 0))
                    nOrder = Convert.ToInt16(result[0].GetType().GetProperty("TheOrder").GetValue(result[0], null)) + 1;
                else
                    nOrder = 1;
            }
            //保存本层节点
            model.TheLevel = (short)nLevel;
            model.TheOrder = (short)nOrder;
            model.IsLeaf = true;
            return AddOneRecord(model);
        }
        #endregion

        #region 树的一些简单操作

        #region Leaf
        /// <summary>
        /// 判断当前节点是否为叶子节点
        /// </summary>
        /// <param name="sID">当前节点ID</param>
        /// <returns>true叶子false非叶子</returns>
        private bool _IsLeaf<T, T1>(T obj, T1 ID) where T : class 
        {
            try
            {
                long lID = Convert.ToInt64(ID);
                tblTreeInfo result = GetOneRecordbyID<T, long>(lID);
                if (result != null)
                    return result.IsLeaf;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// 得到所有的叶子节点，以字符串形式拼接并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string _GetAllLeaf<T, T1>(T obj, T1 ID) where T : class
        {
            try
            {
                long id = Convert.ToInt64(ID);
                if ((id == 0))
                    id = -1;
                string sReturn = id.ToString() + CosValue.SPLITOPERATOR2 + _GetSubLeaf<T>(id);
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
                return sReturn;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 得到所有的叶子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        private List<T> _GetAllLeafNodes<T>(long id) where T : class
        {
            try
            {                
                if (id == 0)
                    return null;
                else if (id == -1)
                    return _GetSubLeafNodes<T>(id);
                else
                {
                    List<T> nodes = new List<T>();
                    nodes.Add(GetOneRecordbyID<T, long>(id));
                    nodes.AddRange(_GetSubLeafNodes<T>(id));
                    return nodes;
                }                
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<T> _GetSubLeafNodes<T>(long id) where T : class
        {
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            names.Add("ParentIndex");
            names.Add("DelFlag");
            values.Add(id);
            values.Add(false);

            var express = GetAndLambdaExpression<T>(names, values);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> result = GetRecord(express, order);
            List<T> returnResults = new List<T>();
            returnResults.AddRange(result);
            foreach (T node in result)
            {
                long nodeid = Convert.ToInt64(node.GetType().GetProperty("ID").GetValue(node, null));                
                if (!Convert.ToBoolean(node.GetType().GetProperty("IsLeaf").GetValue(node, null)))
                    returnResults.AddRange(_GetSubLeafNodes<T>(nodeid));
            }
            return returnResults;
        }

        /// <summary>
        /// 辅助GetAllLeaf递归的函数，获取下层所有子节点。不供外部调用
        /// </summary>
        /// <param name="sID">父节点ID</param>
        /// <returns></returns>
        private string _GetSubLeaf<T>(long id) where T : class
        {
            string sReturn = "";
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            names.Add("ParentIndex");
            names.Add("DelFlag");
            values.Add(id);
            values.Add(false);

            var express = GetAndLambdaExpression<T>(names, values);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> result = GetRecord(express, order);
            foreach (T node in result)
            {
                long nodeid = Convert.ToInt64(node.GetType().GetProperty("ID").GetValue(node, null));
                sReturn = sReturn + nodeid + CosValue.SPLITOPERATOR2;
                if (!Convert.ToBoolean(node.GetType().GetProperty("IsLeaf").GetValue(node, null)))
                    sReturn = sReturn + _GetSubLeaf<T>(nodeid);
            }            
            return sReturn;
        }

        #endregion

        #region Parent


        public T _GetFirstParent<T>(T obj, long id) where T : class
        {
            var node = GetOneRecordbyID<T, long>(id);
            short TheLevel = Convert.ToInt16(node.GetType().GetProperty("TheLevel").GetValue(node, null));
            while (TheLevel > 1)
            {
                long lparent = Convert.ToInt64(node.GetType().GetProperty("ParentIndex").GetValue(node, null));
                node = GetOneRecordbyID<T, long>(lparent);
                TheLevel = Convert.ToInt16(node.GetType().GetProperty("TheLevel").GetValue(node, null));
            }
            return node;
        }


        public string _GetAllParent<T, T1>(T obj, T1 id) where T : class 
        {
            try
            {
                string ss = "";
                var node = GetOneRecordbyID<T, T1>(id);
                while (node != null)
                {
                    string sparent = Convert.ToString(node.GetType().GetProperty("ParentIndex").GetValue(node, null));
                    node = GetOneRecordbyID<T, string>(sparent);
                    if (node != null)
                        ss = ss + CosValue.SPLITOPERATOR2 + node.GetType().GetProperty("ID").GetValue(node, null);
                }
                return ss;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        public string _GetAllParentName<T, T1>(T obj, T1 id, bool own = false) where T : class
        {
            try
            {
                string ss = "";
                var node = GetOneRecordbyID<T, T1>(id);
                if (own)
                    ss = node.GetType().GetProperty("Name").GetValue(node, null);
                while (node != null)
                {
                    string sparent = Convert.ToString(node.GetType().GetProperty("ParentIndex").GetValue(node, null));
                    node = GetOneRecordbyID<T, string>(sparent);
                    if (node != null)
                        ss = node.GetType().GetProperty("Name").GetValue(node, null) + ss;
               }
                return ss;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        /// <summary>
        /// 判断ParentId是否为Id的父亲节点
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public virtual bool IsOneParent(long Id, long ParentId)
        {
            return false;
        }

        public bool _IsOneParent<T>(long Id, long ParentId) where T : class
        {
            string[] list = GetAllParent<long>(Id).Split(CosValue.SPLITOPERATOR2);
            if (list != null)
            {
                foreach (string s in list)
                {
                    long nowId = Convert.ToInt64(s);
                    if ((nowId == ParentId) && (nowId != Id))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断LeafId是否为Id的子节点
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public virtual bool IsOneLeaf(long Id, long LeafId)
        {
            return false;
        }

        public bool _IsOneLeaf<T>(long Id, long LeafId) where T : class
        {
            string[] list = GetAllLeaf(Id).Split(CosValue.SPLITOPERATOR2);
            if (list != null)
            {
                foreach (string s in list)
                {
                    long nowId = Convert.ToInt64(s);
                    if ((nowId == LeafId) && (nowId != Id))
                        return true;
                }
            }
            return false;
        }
        #endregion

        public override dynamic GetSpecialModelFromCurrentID<T>(T ID)
        {
            dynamic obj = base.GetSpecialModelFromCurrentID(ID);
            obj.GetType().GetProperty("ParentName").SetValue(obj, GetAllParentName(ID), null);
            return obj;
        }


        /// <summary>
        /// 验证某个节点是否在某一串节点当中
        /// </summary>
        /// <param name="sAllIDs"></param>
        /// <param name="lID"></param>
        /// <returns></returns>
        public bool VerifyOneNodeIn(string sAllIDs, long? lID)
        {
            string[] arr = sAllIDs.Split(CosValue.SPLITOPERATOR2);
            foreach (string s in arr)
            {
                if (s == lID.ToString())
                    return true;
            }
            return false;
        }
        public virtual dynamic GetAllBrothers(string sParentID)
        {
            return null;
        }

        public dynamic _GetAllBrothers<T>(string sParentID) where T : class
        {
            ArrayList arrNames = new ArrayList();
            ArrayList arrValues = new ArrayList();

            arrNames.Add("ParentIndex");arrValues.Add(sParentID);
            arrNames.Add("DelFlag"); arrValues.Add(false);

            var where = GetAndLambdaExpression<T>(arrNames, arrValues);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> list = GetRecord<T, short>(where, order);
            return list;            
        }
        #endregion

        #region 树节点上下移动
        
        /// <summary>
        /// 改变节点的位置（order），向上移动。
        /// </summary>
        /// <param name="sID">节点ID</param>
        public bool _OrderUp<T>(T obj, long nodeid) where T : class
        {
            tblTreeInfo model = GetOneRecordbyID<T, long>(nodeid);

            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            names.Add("ParentIndex");
            names.Add("DelFlag");
            values.Add(model.ParentIndex);
            values.Add(false);

            var express = GetAndLambdaExpression<T>(names, values);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> list = GetRecord<T, short>(express, order, false);

            if (list.Count > 0)
            { 
                bool flag = false;
                int i = 0;
                //对当前节点定位
                while ((i < list.Count) && (!flag))
                {
                    long id = Convert.ToInt64(list[i].GetType().GetProperty("ID").GetValue(list[i], null));
                    if (id == model.ID)
                        flag = true;
                    else
                        i++;
                }
                //找到了并且不是第一个点
                if ((flag) && i < list.Count - 1)
                {
                    short nTemp1 = Convert.ToInt16(list[i].GetType().GetProperty("TheOrder").GetValue(list[i], null));
                    short nTemp2 = Convert.ToInt16(list[i + 1].GetType().GetProperty("TheOrder").GetValue(list[i + 1], null));
                    string[] EditNames = { "TheOrder"};
                    list[i].GetType().GetProperty("TheOrder").SetValue(list[i], nTemp2);
                    list[i + 1].GetType().GetProperty("TheOrder").SetValue(list[i + 1], nTemp1);
                    SaveOneRecord<T>(list[i], EditNames);
                    SaveOneRecord<T>(list[i + 1], EditNames);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }

        /// <summary>
        /// 改变节点的位置（order），向下移动。
        /// </summary>
        /// <param name="sID">节点ID</param>
        public bool _OrderDown<T>(T obj, long nodeid) where T : class
        {
            tblTreeInfo model = GetOneRecordbyID<T, long>(nodeid);

            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            names.Add("ParentIndex");
            names.Add("DelFlag");
            values.Add(model.ParentIndex);
            values.Add(false);

            var express = GetAndLambdaExpression<T>(names, values);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> list = GetRecord(express, order, false);

            if (list.Count > 0)
            {
                bool flag = false;
                int i = 0;
                //对当前节点定位
                while ((i < list.Count) && (!flag))
                {
                    long id = Convert.ToInt64(list[i].GetType().GetProperty("ID").GetValue(list[i], null));
                    if (id == model.ID)
                        flag = true;
                    else
                        i++;
                }
                //找到了并且不是最后一个点
                if ((flag) && i > 0)
                {
                    short nTemp1 = Convert.ToInt16(list[i].GetType().GetProperty("TheOrder").GetValue(list[i], null));
                    short nTemp2 = Convert.ToInt16(list[i - 1].GetType().GetProperty("TheOrder").GetValue(list[i - 1], null));
                    string[] EditNames = { "TheOrder" };
                    list[i].GetType().GetProperty("TheOrder").SetValue(list[i], nTemp2);
                    list[i - 1].GetType().GetProperty("TheOrder").SetValue(list[i - 1], nTemp1);
                    SaveOneRecord(list[i], EditNames);
                    SaveOneRecord(list[i - 1], EditNames);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }
        #endregion

        #region 和TreeGrid的初始化相关
        
        public NodeCollection _InitTreeGrid<T>(string node,T obj, string ParentVariable, string sRelationIDs, string sText = "全部节点", bool bNeedRoot = true) where T : class
        {
            var nodes = new NodeCollection(false);
            InitAuthorizationArray(sRelationIDs);
            StartDFS(node,obj, sRelationIDs, sText, nodes, true);
            //StartSetNoLeafNodes(sRelationIDs, nodes);
            return nodes;
        }

        /// <summary>
        /// 设置节点是否选中的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        private bool SetNoLeafNodes(Node node, int type, bool finish = false)
        {
            bool flag = false;
            TreeGridModel tg = (TreeGridModel)node.AttributesObject;

            if (tg.IsLeaf)
            {
                switch (type)
                {
                    case OperationType.ADD: return tg.bAdd;
                    case OperationType.EDIT: return tg.bEdit;
                    case OperationType.DELETE: return tg.bDelete;
                    case OperationType.SEARCH: return tg.bSearch;
                }
            }

            for (int i = 0; i < node.Children.Count; i++)
            {
                if (!flag)
                    flag = SetNoLeafNodes(node.Children[i], type, finish);
            }

            if (flag)
            {
                switch (type)
                {
                    case OperationType.ADD: tg.bAdd = true; break;
                    case OperationType.EDIT: tg.bEdit = true; break;
                    case OperationType.DELETE: tg.bDelete = true; break;
                    case OperationType.SEARCH: tg.bSearch = true; break;
                }
                if (finish)
                {
                    if ((tg.bAdd) && (tg.bEdit) && (tg.bDelete) && (tg.bSearch))
                        tg.bAll = true;
                }
                node.AttributesObject = tg;
            }
            return flag;
        }

        /// <summary>
        /// TreeGrid初始化完毕后，再对非叶子结点是否选取初始化
        /// </summary>
        private void StartSetNoLeafNodes(string sRelationIDs, NodeCollection nodes)
        {
            if ((sRelationIDs != "") && (sRelationIDs != "0"))
            {
                //for (int i = 0; i < nodes.Count; i++)
                {
                    SetNoLeafNodes(nodes[0], OperationType.ADD);
                    SetNoLeafNodes(nodes[0], OperationType.EDIT);
                    SetNoLeafNodes(nodes[0], OperationType.DELETE);
                    SetNoLeafNodes(nodes[0], OperationType.SEARCH, true);
                }
            }
        }

        #endregion

        #region 权限数组相关操作
        /// <summary>
        /// 检验当前节点是否在权限数组中
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true:可以用;false：不能用</returns>
        private bool VerifyOneNode<T>(T obj, long id, bool bGrid) where T : class
        {
            bool flag = false;
            if (bIncludeAuthorLeaf)
                flag = VerifyOneNodeforEachNowID(id, bGrid);
            else
            {                
                string[] allIds = _GetAllLeaf(obj, id).Split(CosValue.SPLITOPERATOR2);
                int i = 0;
                while ((!flag) && (i < allIds.Length))
                {
                    flag = VerifyOneNodeforEachNowID(Convert.ToInt64(allIds[i]), bGrid);
                    i++;
                }
            }
            return flag;
        }

        private bool VerifyOneNodeforEachNowID(long id, bool bGrid)
        {
            bool flag = false;
            int i = 0;
            while ((!flag) && (i < arrNowAuthorization.Count))
            {
                string sNowID = "";
                if (bGrid)
                    sNowID = ((TreeGridModel)arrNowAuthorization[i]).sID.ToString();
                else
                    sNowID = Convert.ToString(arrNowAuthorization[i].GetType().GetProperty("ID").GetValue(arrNowAuthorization[i], null));

                if (sNowID == id.ToString())
                    flag = true;
                else
                    i++;
            }
            return flag;
        }

        ///// <summary>
        ///// 判断节点是否属于排除节点
        ///// </summary>
        ///// <param name="node"></param>
        ///// <returns></returns>
        //protected bool VerifyNode(Node node)
        //{
        //    bool flag = true;
        //    //if (arrTransValue.Count > 0)
        //    //{
        //    //    int i = 0;
        //    //    while ((flag) && (i < arrTransValue.Count))
        //    //    {
        //    //        if (node.NodeID == arrTransValue[i].ToString())
        //    //            flag = false;
        //    //        else
        //    //            i++;
        //    //    }
        //    //}
        //    return flag;
        //}


        /// <summary>
        /// 权限数组初始化，只是针对菜单的 
        /// </summary>
        /// <param name="sRelationIDs">UserRole中的所有ID</param>
        protected void _InitAuthorizationArray<T>( string sRelationIDs, string authurIDName, string OperaIDName) where T : class
        {
            arrNowAuthorization.Clear();

            if (sRelationIDs != "")
            {
                ArrayList names = new ArrayList(); ArrayList values = new ArrayList(); ArrayList signs = new ArrayList();
                names.Add(authurIDName); values.Add(sRelationIDs); signs.Add(SIGN.In);
                Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(names, values, signs);
                List<T> list = GetRecord<T, short>(where);
                foreach (T m in list)                
                {
                    TreeGridModel tgm = new TreeGridModel();
                    tgm.sID = Convert.ToInt64(m.GetType().GetProperty(OperaIDName).GetValue(m, null));
                    tgm.bAdd = Convert.ToBoolean(m.GetType().GetProperty("AddFlag").GetValue(m, null)); 
                    tgm.bSearch = Convert.ToBoolean(m.GetType().GetProperty("SearchFlag").GetValue(m, null));
                    tgm.bEdit = Convert.ToBoolean(m.GetType().GetProperty("EditFlag").GetValue(m, null)); 
                    tgm.bDelete = Convert.ToBoolean(m.GetType().GetProperty("DeleteFlag").GetValue(m, null));
                    if ((tgm.bAdd) && (tgm.bSearch) && (tgm.bDelete) && (tgm.bEdit))
                        tgm.bAll = true;
                    arrNowAuthorization.Add(tgm);
                }
            }
        }
        /// <summary>
        /// 根据当前传过来的ID获取在权限数组中的位置
        /// </summary>
        /// <param name="sID">传过来的ID</param>
        /// /// <param name="bGrid">类型：false：DataTree；true：DataTreeGrid。</param>
        /// <returns>-1：没找到，否则返回位置坐标</returns>
        public int GetAuthorizationPos(string sID, bool bGrid)
        {
            int nReturn = -1;
            bool flag = false;
            int i = 0;
            string sNowID = "";
            while ((!flag) && (i < arrNowAuthorization.Count))
            {
                if (bGrid)
                    sNowID = ((TreeGridModel)arrNowAuthorization[i]).sID.ToString();
                else
                    sNowID = Convert.ToString(arrNowAuthorization[i].GetType().GetProperty("ID").GetValue(arrNowAuthorization[i], null));
                if (sNowID == sID)
                    flag = true;
                else
                    i++;
            }

            if (flag)
                nReturn = i;
            return nReturn;
        }
        #endregion

        #region TreeGrid权限保存
        public virtual void SaveAllTreeGridAuthor(string sAuthorizaionNowID, SubmittedNode subNode)
        {
        }

        private void SaveOneTreeGridAuthor<T>( string sAuthorizaionNowID, string RelationName, SubmittedNode node) where T: class, new()
        {
            if (node.Children.Count > 0)
            {
                for (int i = 0; i < node.Children.Count; i++)
                    SaveOneTreeGridAuthor<T>(sAuthorizaionNowID, RelationName, node.Children[i]);
            }
            else
            {
                long AuthorID = Convert.ToInt64(sAuthorizaionNowID);
                TreeGridModel tg = JSON.Deserialize<TreeGridModel>(JSON.Serialize(node.Attributes));
                if ((tg.bAdd) || (tg.bEdit) || (tg.bSearch) || (tg.bDelete))
                {
                    T model = new T();
                    model.GetType().GetProperty("AddFlag").SetValue(model, tg.bAdd, null);
                    model.GetType().GetProperty("EditFlag").SetValue(model, tg.bEdit, null);
                    model.GetType().GetProperty("SearchFlag").SetValue(model, tg.bSearch, null);
                    model.GetType().GetProperty("DeleteFlag").SetValue(model, tg.bDelete, null);
                    model.GetType().GetProperty("AuthorID").SetValue(model, AuthorID, null);
                    model.GetType().GetProperty(RelationName).SetValue(model, tg.sID, null);
                    AddOneRecord(model);
                }
            }
        }

        protected void _SaveAllTreeGridAuthor<T>(string sAuthorizaionNowID, string RelationName, SubmittedNode subNode) where T : class, new()
        {
            if ((sAuthorizaionNowID != "0") && (subNode != null))
            {
                ArrayList names = new ArrayList();
                ArrayList values = new ArrayList();
                names.Add("AuthorID");
                values.Add(sAuthorizaionNowID);
                var express = GetAndLambdaExpression<T>(names, values);
                List<T> list = GetRecord(express);
                DeleteRecord(list); //先把表相关内容清除
                SaveOneTreeGridAuthor<T>(sAuthorizaionNowID, RelationName, subNode);
            }
        }


        #endregion

        #region DataTree权限保存

        private void SaveOneTreeAuthor<T, T1>(string node, string authorID, string TreeName, string AuthorName) where T : class where T1 :class
        {
            if (node.Length > 0)
            {
                string[] no = node.Split(CosValue.SPLITOPERATOR1);
                for (int i = 0; i < no.Length; i++)
                {
                    BaseContext db = new BaseContext();
                    var model = db.Set<T1>().Create();
                    model.GetType().GetProperty(TreeName).SetValue(model, Convert.ToInt64(no[i]), null);
                    model.GetType().GetProperty(AuthorName).SetValue(model, Convert.ToInt64(authorID), null);
                    AddOneRecord(model);
                }
            }
        }
        public virtual void SaveAllTreeCheck(string sAuthorizaionNowID)
        {

        }

        /// <summary>
        /// 获取/转换所有的submittedNode
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bGrid"></param>
        /// <returns></returns>
        public string GetAllSelectNodes(SubmittedNode data, bool bGrid = false)
        {            
            return _GetSelectNodes(data, bGrid);
        }

        private string _GetSelectNodes(SubmittedNode node, bool bGrid)
        {
            string s = "";            
            if (node != null)
            {
                if (node.Children.Count > 0)
                    for (int i = 0; i < node.Children.Count; i++)
                    {
                        string ss = _GetSelectNodes(node.Children[i], bGrid);
                        if (ss.Length > 0)
                            s = s + CosValue.SPLITOPERATOR1 + ss;
                    }
                else
                    if (node.Checked)
                    s = node.NodeID;
            }
            if (s.Length > 0)
                if (s[0] == CosValue.SPLITOPERATOR1)
                    s = s.Remove(0, 1);
            return s;
        }


        public void _SaveAllTreeCheck<T, T1>(string sAuthorizaionNowID, string TreeName, string AuthorName) where T : class where T1 : class
        {
            if ((sAuthorizaionNowID != "0") && (sbNode != null))
            {
                //先删除
                //var where = GetInExpression<T1>(AuthorName, sAuthorizaionNowID);
                //var where = GetOneNotInLambdaExpression<T1>(false, AuthorName, sAuthorizaionNowID);
                //var models = GetRecord<T1>(where);
                //DeleteRecord<T1>(models);
                ////再添加
                //SaveOneTreeAuthor<T, T1>(sbNode, sAuthorizaionNowID, TreeName, AuthorName);
            }
        }

        #endregion

    }
}