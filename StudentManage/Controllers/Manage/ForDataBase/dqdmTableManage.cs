using System;
using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Ext.Net;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class dqdmTableManage : DataTreeManage
    {
        public dqdmTableManage() 
        {
            Manage_KeyWord = "dqdmTable";
        }

        public string GetNowNamefromID(long ID)
        {
            Td_dqdm model = GetOneRecordbyID<Td_dqdm, long>(ID);
            if (model != null)
                return model.Name;
            else
                return "";
        }

        public string GetNowNamefromCode(string sCode)
        {
            Expression<Func<Td_dqdm, bool>> where = (c => c.Code == sCode && c.DelFlag == false);
            List<Td_dqdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].Name;
            else
                return null;
        }

        /// <summary>
        /// 获得相关省份的名字，用于报表创建窗口的选择中
        /// </summary>
        /// <param name="SelectedStore">已选省份</param>
        /// <param name="useFlag">是否只已选省份,true是false否</param>
        /// <param name="sTypeID"></param>
        /// <returns></returns>
        public object[] GetNameStoreforSelection(string SelectedStore, bool useFlag = true, string sTypeID = "Province")
        {
            string sUsedIDs = GetUsedIDs(SelectedStore, sTypeID);

            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            arrNames.Add("DelFlag"); arrNames.Add("ParentIndex"); arrNames.Add("ID");
            arrValues.Add(false); arrValues.Add(-1); arrValues.Add(sUsedIDs);
            arrSigns.Add(SIGN.Equal); arrSigns.Add(SIGN.Equal);
            if (useFlag)
                arrSigns.Add(SIGN.In);
            else
                arrSigns.Add(SIGN.NotIn);
            var where = GetAndLambdaExpression<Td_dqdm>(arrNames, arrValues, arrSigns);

            object[] data = GetReturnData(sTypeID, where, "省份-");
            if (data != null)
                return data;

            object[] data0 = new object[0];
            return data0;
        }

        


        /// <summary>
        /// 根据当前的名称获得当前的地区model(保证系统中有完整的地区列表，不新增了)
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public long GetCurrentRegionbyName(string sName)
        {
            Expression<Func<Td_dqdm, bool>> where = (c => c.Name.Contains(sName) && c.DelFlag == false);
            List<Td_dqdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].ID;
            //else
            //{
            //    Td_dqdm model = new Td_dqdm();
            //    model.Name = sName;
            //    if (sName.Remove(0, sName.Length - 2) != "省")
            //    {
            //        if (sName.Contains("上海") || sName.Contains("北京") || sName.Contains("天津") || sName.Contains("重庆"))
            //            model.Name = model.Name + "市";
            //        else
            //            model.Name = model.Name + "省"; 
            //    }
            //    model.JoinNames = model.Name;
            //    return AddOneTreeNode(model);
            //}
            return 0;
        }

        /// <summary>
        /// 获得父节点的代码，用在新增节点窗口
        /// </summary>
        /// <param name="lID"></param>
        /// <returns></returns>
        public string GetParentDM(long lID)
        {
            Td_dqdm model = GetOneRecordbyID<Td_dqdm, long>(lID);
            string s = model.Code;
            if (s.Length > 4)
                return s.Remove(4) + "00";
            else
                return s;
        }
        
        /// <summary>
        /// 查看是否存在对应的地区代码的数据
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public bool ExistDMValue(string sValue, string sNowID)
        {
            long lID = Convert.ToInt64(sNowID);
            Expression<Func<Td_dqdm, bool>> where = (c => c.Code == sValue && c.ID != lID && c.DelFlag == false);
            List<Td_dqdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 根据输入的地区代码，返回对应的ParentObj
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public Td_dqdm GetParentObj(string sValue)
        {
            string sFirstCode = sValue.Substring(0, 2);
            string sSecondCode = sValue.Substring(2, 2);
            string sThirdCode = sValue.Substring(4, 2);

            if (sSecondCode == "00") //创建一级节点
                return null;
            else if (sThirdCode == "00")  //创建二级节点，返回一级节点
            {
                string s = sFirstCode + "0000";
                Expression<Func<Td_dqdm, bool>> where = (c => c.Code == s && c.DelFlag == false);
                List<Td_dqdm> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list[0];
                else
                    return null;
            }
            else //创建三级节点，返回二级节点
            {
                string s = sFirstCode + sSecondCode + "00";
                Expression<Func<Td_dqdm, bool>> where = (c => c.Code == s && c.DelFlag == false);
                List<Td_dqdm> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// 保存一个地区代码数据
        /// </summary>
        /// <param name="obj"></param>
        public void SaveOneObj(Td_dqdm obj)
        {
            if (obj.ID == 0) //新增节点
            {
                Td_dqdm newParent = GetOneRecordbyID<Td_dqdm, long>(obj.ParentIndex);
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
                Td_dqdm oldObj = GetOneRecordbyID<Td_dqdm, long>(obj.ID);
                if (oldObj.ParentIndex != obj.ParentIndex) //地区上级有修改
                {
                    //老节点的父节点
                    Td_dqdm oldParent = GetOneRecordbyID<Td_dqdm, long>(oldObj.ParentIndex);
                    oldParent.ChildNum--;
                    if (oldParent.ChildNum == 0)
                        oldParent.IsLeaf = true;
                    SaveOneRecord(oldParent);                    
                    //新节点是肯定是新增
                    Td_dqdm newParent = GetOneRecordbyID<Td_dqdm, long>(obj.ParentIndex);
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
                    if (!oldObj.JoinNames.Split(CosValue.SPLITOPERATOR1).Contains(oldObj.Name))
                    {
                        if ((obj.JoinNames != "")&& (obj.JoinNames != null))
                            obj.JoinNames = obj.JoinNames + CosValue.SPLITOPERATOR1 + oldObj.Name;
                        else
                            obj.JoinNames = oldObj.Name;
                    }
                }

                if (!oldObj.JoinNames.Split(CosValue.SPLITOPERATOR1).Contains(oldObj.Name))
                {
                    if ((obj.JoinNames != "") && (obj.JoinNames != null))
                        obj.JoinNames = obj.JoinNames + CosValue.SPLITOPERATOR1 + oldObj.Name;
                    else
                        obj.JoinNames = oldObj.Name;
                }

                if (oldObj.ParentIndex != obj.ParentIndex) //地区上级有修改
                {
                    string[] EditColumns = { "Name", "TheOrder", "Remark", "JoinNames", "ParentIndex", "TheLevel", "Code" };
                    SaveOneRecord(obj, EditColumns);
                }
                else
                {
                    string[] EditColumns = { "Name", "TheOrder", "Remark", "JoinNames", "Code" };
                    SaveOneRecord(obj, EditColumns);
                }

            }
        }


        #region 导入数据相关
        /// <summary>
        /// 根据当前的FistCode找到对应的Province，或者可能就是现在传过来的ProvinceID
        /// </summary>
        /// <param name="lProvinceID"></param>
        /// <param name="sFirstCode"></param>
        /// <returns></returns>
        private Td_dqdm GetRealProvince(long lProvinceID, string sFirstCode = null, BaseContext transDB = null)
        {
            if (sFirstCode != null)
            {
                Expression<Func<Td_dqdm, bool>> where = (c => c.Code == sFirstCode && c.DelFlag == false);
                List<Td_dqdm> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list[0];
            }
            
            Td_dqdm nowProvince = GetOneRecordbyID<Td_dqdm, long>(lProvinceID);
            if (nowProvince != null)
            {
                if ((nowProvince.Code == null) || (nowProvince.Code == ""))
                {
                    string[] EditColumn = { "Code" };
                    nowProvince.Code = sFirstCode;
                    SaveOneRecord(nowProvince, EditColumn);
                    return nowProvince;
                }
                else 
                    return nowProvince;
            }
            return null;
        }
        /// <summary>
        /// 得到当前的市model
        /// </summary>
        /// <param name="nowProvince"></param>
        /// <param name="sSecondCode"></param>
        /// <param name="sThirdCode"></param>
        /// <param name="sRegionName"></param>
        /// <returns></returns>
        private Td_dqdm GetRealCity(Td_dqdm nowProvince, string sSecondCode, string sThirdCode, string sRegionName)
        {
            long lParentIndex = nowProvince.ID;
            Expression<Func<Td_dqdm, bool>> where = (c => c.Code == sSecondCode && c.DelFlag == false && c.ParentIndex == lParentIndex);
            List<Td_dqdm> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
            {
                Td_dqdm model = new Td_dqdm();
                model.ParentIndex = nowProvince.ID;
                model.Code = sSecondCode;

                if (sThirdCode == "00") //直接新增当前节点
                    model.Name = sRegionName;
                else
                    model.Name = "市辖区/县";
                long lNewModelID = AddOneTreeNode(model);
                return GetOneRecordbyID<Td_dqdm, long>(lNewModelID);
            }
        }
        private bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>
        /// 看当前行对应的区域是否存在
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Td_dqdm IsExistRegion(Td_dqdm_Origin OriginModel, List<Td_dqdm> Models)
        {
            string sCode = OriginModel.DM;
            string sName = OriginModel.MC;
            List<Td_dqdm> list = new List<Td_dqdm>();
            if (IsNumeric(sCode))
                list = Models.Where(c => ((c.Code == sCode) || (c.Code == null && c.Name == sName))).ToList();
            else
                list = Models.Where(c => c.Name == sName).ToList();
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 用于系统初始化时导入地区数据
        /// </summary>
        /// <param name="dt"></param>
        public void InportInitData(DataTable dt)
        {
            //Expression<Func<Td_dqdm, bool>> where = (c => (c.ID > 0));
            //List<Td_dqdm> list = GetRecord(where);
            //if ((list != null) && (list.Count > 0))
            //    DeleteRecord(list);
            foreach (DataRow dr in dt.Rows)
            {
                string sFirstCode = dr[0].ToString().Substring(0, 2);
                string sSecondCode = dr[0].ToString().Substring(2, 2);
                string sThirdCode = dr[0].ToString().Substring(4, 2);
                string sRegionName = dr[1].ToString().Trim();
                long lNewID = 0;
                Td_dqdm model = new Td_dqdm();
                if ((sSecondCode == "00") && (sThirdCode == "00")) //省
                {
                    model.ParentIndex = -1;
                    model.TheOrder = Convert.ToInt16(sFirstCode);
                }
                else if ((sSecondCode != "00") && (sThirdCode == "00")) //市
                {
                    Td_dqdm nowProvince = GetRealProvince(0, sFirstCode + "0000");
                    model.ParentIndex = nowProvince.ID;
                    model.TheOrder = Convert.ToInt16(sSecondCode);
                }
                else //县
                {
                    Td_dqdm nowProvince = GetRealProvince(0, sFirstCode + "0000");
                    Td_dqdm nowCity = GetRealCity(nowProvince, sFirstCode + sSecondCode + "00", sThirdCode, sRegionName);
                    model.ParentIndex = nowCity.ID;
                    model.TheOrder = Convert.ToInt16(sThirdCode);
                }
                model.JoinNames = model.Name;
                model.Code = dr[0].ToString().Trim();
                model.Name = sRegionName;
                lNewID = AddOneTreeNode(model);
            }
        }

        /// <summary>
        /// 对传入的DataTable预处理下，只保留本省的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable ShrinkDataTable(DataTable dt, long lProvinceID)
        {
            Td_dqdm model = GetOneRecordbyID<Td_dqdm, long>(lProvinceID);            
            if (model != null)
            {
                string code = model.Code.Substring(0, 2);
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    string sNowFirstDM = dt.Rows[i]["DQDM"].ToString().Substring(0, 2);
                    if (sNowFirstDM != code)
                        dt.Rows.Remove(dt.Rows[i]);
                }
            }
            return dt;
        }


        public List<ListCompareTable> InportOriginData(DataTable dt, int nYear, long lProvinceID, string[] mainColumns)
        {
            DataTable datatable = ShrinkDataTable(dt, lProvinceID); //先把其他省份的去掉
            System.Data.DataView dv = datatable.DefaultView;
            dv.Sort = "DQDM Asc";
            DataTable dt2 = dv.ToTable(true);

            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            OriginTableManage man = new OriginTableManage();
            List<Td_dqdm_Origin> OriginModels = man.InportSomeRowsOriginTable<Td_dqdm_Origin>(dt, nYear, lProvinceID, mainColumns);
            List<Td_dqdm> Models = GetAllLeafNodes<Td_dqdm>(lProvinceID);
            //TableStatusManage statusManage = new TableStatusManage(); 地区代码，认为每个省份的数据库无重复
            bool flag = true; //记录有无修改
            for (int i = 0; i < OriginModels.Count; i++)
            {
                long lNowRelationID = Convert.ToInt64(OriginModels[i].RelationID);
                if (lNowRelationID == 0) //原始表中新加入的字段
                {
                    flag = false;
                    Td_dqdm dqObj = IsExistRegion(OriginModels[i], Models);
                    if (dqObj == null) //如果区域在数据库中不存在，dqdm表中需要增加该区域
                    {
                        string sFirstCode, sSecondCode, sThirdCode;
                        if ((OriginModels[i].DM.Length >= 2) && (IsNumeric(OriginModels[i].DM)))
                            sFirstCode = OriginModels[i].DM.Substring(0, 2);
                        else
                            sFirstCode = "00";
                        if ((OriginModels[i].DM.Length >= 4) && (IsNumeric(OriginModels[i].DM)))
                            sSecondCode = OriginModels[i].DM.Substring(2, 2);
                        else
                            sSecondCode = "00";
                        if ((OriginModels[i].DM.Length >= 6) && (IsNumeric(OriginModels[i].DM)))
                            sThirdCode = OriginModels[i].DM.Substring(4, 2);
                        else
                            sThirdCode = "00";

                        long lNewID = 0;
                        Td_dqdm model = new Td_dqdm();
                        model.Code = OriginModels[i].DM;
                        model.Name = OriginModels[i].MC;                        
                        model.JoinNames = OriginModels[i].MC;

                        if (sThirdCode == "00") //二级节点 （不可能增加一级节点）
                        {
                            model.TheOrder = Convert.ToInt16(sSecondCode);
                            model.ParentIndex = lProvinceID;
                        }
                        else  //三级节点
                        {
                            string sParentCode = sFirstCode + sSecondCode + "00";
                            Td_dqdm nowCity = null;
                            List<Td_dqdm> listHere = Models.Where(c => c.Code == sParentCode).ToList();
                            if ((listHere == null) || (listHere.Count == 0)) //二级节点不存在
                            {
                                nowCity = new Td_dqdm();
                                nowCity.ParentIndex = lProvinceID;
                                nowCity.Code = sParentCode;
                                nowCity.TheOrder = Convert.ToInt16(sSecondCode);
                                nowCity.Name = "市辖区/县";
                                AddOneTreeNode(nowCity);
                            }
                            else
                                nowCity = listHere[0];
                            model.TheOrder = Convert.ToInt16(sThirdCode);
                            model.ParentIndex = nowCity.ID;
                        }
                        lNewID = AddOneTreeNode(model);

                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = OriginModels[i].DM;
                        oneTable.lNewID = lNewID;
                        oneTable.lOriginID = OriginModels[i].ID;
                        listReturn.Add(oneTable);
                        OriginModels[i].RelationID = lNewID;
                    }
                    else
                    {
                        string sRegionName = OriginModels[i].MC;
                        if (dqObj.Code == null)
                        {
                            dqObj.Code = OriginModels[i].DM;
                            string[] EditColumn = { "Code" };
                            SaveOneRecord(dqObj, EditColumn);
                        }
                        if (sRegionName != dqObj.Name)
                        {
                            if (!dqObj.JoinNames.Split(CosValue.SPLITOPERATOR1).Contains(sRegionName))
                            {
                                dqObj.JoinNames = dqObj.JoinNames + CosValue.SPLITOPERATOR1 + sRegionName;
                                string[] EditColumn = { "JoinNames" };
                                SaveOneRecord(dqObj, EditColumn);
                            }
                        }
                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = OriginModels[i].DM;
                        if (dqObj.DelFlag)
                        {
                            oneTable.lNewID = dqObj.JoinID;
                            OriginModels[i].RelationID = dqObj.JoinID;
                        }
                        else
                        {
                            oneTable.lNewID = dqObj.ID;
                            OriginModels[i].RelationID = dqObj.ID;
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
            if (!flag)
                SaveManyRecords(OriginModels);
            return listReturn;
        }

        #endregion
    }
}