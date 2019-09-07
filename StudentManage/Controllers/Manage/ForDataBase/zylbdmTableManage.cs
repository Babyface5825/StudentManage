using System;
using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class zylbdmTableManage : DataTreeManage
    {

        public zylbdmTableManage() 
        {
            Manage_KeyWord = "zylbdmTable";
        }

        /// <summary>
        /// 获得父节点的代码，用在新增节点窗口
        /// </summary>
        /// <param name="lID"></param>
        /// <returns></returns>
        public string GetParentDM(long lID)
        {
            Td_zylbdm model = GetOneRecordbyID<Td_zylbdm, long>(lID);
            string s = model.Code;            
            return s + "01";
        }
        /// <summary>
        /// 根据输入的地区代码，返回对应的ParentObj
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public Td_zylbdm GetParentObj(string sValue)
        {
            string sFirstCode = sValue.Substring(0, 2);
            string sSecondCode = sValue.Substring(2, 2);
            string sThirdCode = sValue.Substring(4, 2);

            if (sSecondCode == "") //创建一级节点
                return null;
            else if (sThirdCode == "")  //创建二级节点，返回一级节点
            {
                string s = sFirstCode + "01";
                Expression<Func<Td_zylbdm, bool>> where = (c => c.Code == s && c.DelFlag == false);
                List<Td_zylbdm> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list[0];
                else
                    return null;
            }
            else //创建三级节点，返回二级节点
            {
                string s = sFirstCode + sSecondCode;
                Expression<Func<Td_zylbdm, bool>> where = (c => c.Code == s && c.DelFlag == false);
                List<Td_zylbdm> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// 查看是否存在对应的地区代码的数据
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public bool ExistDMValue(string sValue, string sNowID)
        {
            long lID = Convert.ToInt64(sNowID);
            Expression<Func<Td_zylbdm, bool>> where = (c => c.Code == sValue && c.ID != lID && c.DelFlag == false);
            List<Td_zylbdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 保存一个地区代码数据
        /// </summary>
        /// <param name="obj"></param>
        public void SaveOneObj(Td_zylbdm obj)
        {
            if (obj.ID == 0) //新增节点
            {
                Td_zylbdm newParent = GetOneRecordbyID<Td_zylbdm, long>(obj.ParentIndex);
                if (newParent != null)
                {
                    newParent.ChildNum++;
                    newParent.IsLeaf = false;
                    SaveOneRecord(newParent);
                    obj.TheLevel = newParent.TheLevel++;
                }
                else
                {
                    obj.ParentIndex = -1;
                    obj.TheLevel = 1;
                }
                obj.ChildNum = 0;
                obj.IsLeaf = true;
                obj.JoinNames = obj.Name;
                AddOneRecord(obj);
            }
            else  //修改节点
            {
                Td_zylbdm oldObj = GetOneRecordbyID<Td_zylbdm, long>(obj.ID);
                if (oldObj.ParentIndex != obj.ParentIndex) //地区上级有修改
                {
                    //老节点的父节点
                    Td_zylbdm oldParent = GetOneRecordbyID<Td_zylbdm, long>(oldObj.ParentIndex);
                    oldParent.ChildNum--;
                    if (oldParent.ChildNum == 0)
                        oldParent.IsLeaf = true;
                    SaveOneRecord(oldParent);
                    //新节点是肯定是新增
                    Td_zylbdm newParent = GetOneRecordbyID<Td_zylbdm, long>(obj.ParentIndex);
                    if (newParent != null)
                    {
                        newParent.ChildNum++;
                        newParent.IsLeaf = false;
                        SaveOneRecord(newParent);
                        obj.TheLevel = newParent.TheLevel++;
                    }
                    else
                    {
                        obj.ParentIndex = -1;
                        obj.TheLevel = 1;
                    }
                    if (!oldObj.JoinNames.Contains(obj.Name))
                        obj.JoinNames = obj.JoinNames + CosValue.SPLITOPERATOR1 + obj.Name;
                    string[] EditColumns = { "Name", "TheOrder", "Remark", "JoinNames", "ParentIndex", "TheLevel", "Code" };
                    SaveOneRecord(obj, EditColumns);
                }
                else
                {
                    if (!oldObj.JoinNames.Contains(obj.Name))
                        obj.JoinNames = obj.JoinNames + CosValue.SPLITOPERATOR1 + obj.Name;
                    string[] EditColumns = { "Name", "TheOrder", "Remark", "JoinNames", "Code" };
                    SaveOneRecord(obj, EditColumns);
                }
            }
        }
        #region 导入数据相关        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        /// <param name="listReturn"></param>
        /// <param name="OriginModel"></param>
        /// <returns></returns>
        private Td_zylbdm IsExistType(Td_zylbdm_Origin OriginModel)
        {
            Expression<Func<Td_zylbdm, bool>> where = (c => c.Code == OriginModel.DM && c.Name == OriginModel.MC);
            List<Td_zylbdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 保存一个一级节点：
        /// 1，如果节点编号有，名称没有，说明以前新增的上级节点，则保存名称；
        /// 2，如果节点编号也没有，就是新建；
        /// 3，如果名称和编号都有，以前建立的节点，直接返回；
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long SaveOneFirstTypeNode(Td_zylbdm model)
        {
            Expression<Func<Td_zylbdm, bool>> where = (c => c.DelFlag == false && c.Code == model.Code);
            List<Td_zylbdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                if ((list[0].Name == "") && (model.Name != ""))
                {
                    string[] EditColumn = { "Name", "JoinNames" };
                    SaveOneRecord(model, EditColumn);
                }
                return list[0].ID;
            }
            else
                return AddOneTreeNode(model);
        }

        /// <summary>
        /// 保存一个二级节点：
        /// 1，如果编号名称都有，以前建立的节点，直接返回；
        /// 2，如果编号有名称没有，以前新增的上级节点，保存名城；
        /// 3，编号也没有，新增节点，同时要看看上级节点怎么处理；
        /// （1）上级节点存在，设定本级节点的ParentIndex然后保存；
        /// （2）上级节点不存在，新增上级节点再新增本级节点；
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long SaveOneSecondTypeNode(Td_zylbdm model, string sFirstCode)
        {
            //Td_zylbdm model = new Td_zylbdm();

            Expression<Func<Td_zylbdm, bool>> where = (c => c.DelFlag == false && c.Code == model.Code);
            List<Td_zylbdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                if ((list[0].Name == "") && (model.Name != ""))
                {
                    string[] EditColumn = { "Name", "JoinNames" };
                    list[0].Name = model.Name;
                    list[0].JoinNames = model.Name;
                    SaveOneRecord(list[0], EditColumn);
                }
                return list[0].ID;
            }
            else
            {
                Expression<Func<Td_zylbdm, bool>> where1 = (c => c.DelFlag == false && c.Code == sFirstCode);
                List<Td_zylbdm> list1 = GetRecord(where1);
                if ((list1 != null) && (list1.Count > 0))
                    model.ParentIndex = list1[0].ID;
                else //上级节点不存在
                {
                    Td_zylbdm ParentModel = new Td_zylbdm();
                    ParentModel.Code = sFirstCode;
                    ParentModel.Name = "";
                    model.ParentIndex = SaveOneFirstTypeNode(ParentModel);
                }
                return AddOneTreeNode(model);
            }
        }
        /// <summary>
        /// 保存一个三级节点，跟二级节点处理类似
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sSecondCode"></param>
        /// <returns></returns>
        private long SaveOneThirdTypeNode(Td_zylbdm model, string sSecondCode, string sFirstCode)
        {
            //Td_zylbdm model = new Td_zylbdm();
            Expression<Func<Td_zylbdm, bool>> where = (c => c.DelFlag == false && c.Code == model.Code);
            List<Td_zylbdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                if ((list[0].Name == "") && (model.Name != ""))
                {
                    string[] EditColumn = { "Name", "JoinNames" };
                    SaveOneRecord(model, EditColumn);
                }
                return list[0].ID;
            }
            else
            {
                Expression<Func<Td_zylbdm, bool>> where1 = (c => c.DelFlag == false && c.Code == sSecondCode);
                List<Td_zylbdm> list1 = GetRecord(where1);
                if ((list1 != null) && (list1.Count > 0))
                    model.ParentIndex = list1[0].ID;
                else //上级节点不存在
                {
                    Td_zylbdm ParentModel = new Td_zylbdm();
                    ParentModel.Code = sFirstCode + sSecondCode;
                    ParentModel.Name = "";
                    model.ParentIndex = SaveOneSecondTypeNode(ParentModel, sFirstCode);
                }
                return AddOneTreeNode(model);
            }
        }        

        public List<ListCompareTable> InportOriginData(DataTable dt, int nNowYear, long lProvinceID)
        {
            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            if (dt != null)
            {
                bool flag = false;
                OriginTableManage manOrigin = new OriginTableManage();
                string[] mainColumns = { "ZYLBDM", "ZYLBMC" };
                List<Td_zylbdm_Origin> OriginModels = manOrigin.InportSomeRowsOriginTable<Td_zylbdm_Origin>(dt, nNowYear, lProvinceID, mainColumns);                
                for (int i = 0; i < OriginModels.Count; i++)
                {   
                    long lNowRelationID = OriginModels[i].RelationID;
                    if (lNowRelationID == 0) //原始表中新加入的字段
                    {
                        flag = true;
                        Td_zylbdm zylbObj = IsExistType(OriginModels[i]);
                        if (zylbObj == null) //如果在数据库中不存在，zylbdm表中需要增加该model
                        {
                            string OriginDM = OriginModels[i].DM;
                            while (OriginDM.Length < 2)
                                OriginDM = "0" + OriginDM;

                            string sFirstCode = GetSubString(OriginDM, 0, 2);
                            string sSecondCode = GetSubString(OriginDM, 2, 2);
                            string sThirdCode = GetSubString(OriginDM, 4, 0);
                            string sTypeName = OriginModels[i].MC;
                            long lNewID = 0;
                            Td_zylbdm model = new Td_zylbdm();
                            model.Code = OriginModels[i].DM;
                            model.Name = sTypeName;
                            model.JoinNames = model.Name;
                            if (sSecondCode == "") //一级节点
                            {
                                model.ParentIndex = -1;
                                lNewID = SaveOneFirstTypeNode(model);
                            }
                            else if (sThirdCode == "")
                                lNewID = SaveOneSecondTypeNode(model, sFirstCode);
                            else
                                lNewID = SaveOneThirdTypeNode(model, sSecondCode, sFirstCode);

                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = OriginModels[i].DM;
                            oneTable.lNewID = lNewID;
                            oneTable.lOriginID = OriginModels[i].ID;
                            listReturn.Add(oneTable);                            
                            //修改原始表中相应的RelationID
                            OriginModels[i].RelationID = lNewID;
                        }
                        else
                        {
                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = OriginModels[i].DM;
                            if (zylbObj.DelFlag)
                            {
                                oneTable.lNewID = zylbObj.JoinID;
                                OriginModels[i].RelationID = zylbObj.JoinID;
                            }
                            else
                            {
                                oneTable.lNewID = zylbObj.ID;
                                OriginModels[i].RelationID = zylbObj.ID;
                            }
                            oneTable.lOriginID = OriginModels[i].ID;
                            listReturn.Add(oneTable);                            
                        }
                    }
                    else
                    {
                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = OriginModels[i].DM;
                        oneTable.lNewID = lNowRelationID;
                        oneTable.lOriginID = OriginModels[i].ID;
                        listReturn.Add(oneTable);
                    }
                }
                if (flag)
                    SaveManyRecords(OriginModels);
            }
            return listReturn;
        }

        #endregion
    }
}