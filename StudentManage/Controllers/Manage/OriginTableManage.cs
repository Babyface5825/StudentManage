using System;
using System.Collections.Generic;
using System.Linq;
using StudentManage.Models;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

namespace StudentManage.Manage
{

    public class JoinModel
    {
        public long ID { get; set; }
        public string AllJoinIDs { get; set; }
        public string AllJoinNames { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OriginTableManage : ParentManage
    {
        public OriginTableManage()
        {

        }

        #region 一些private
        /// <summary>
        /// 根据规定顺序整理当前的所有column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moreColumns"></param>
        /// <param name="moreColumnTypes"></param>
        /// <param name="dr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private T ArrangeMoreColumns<T>(string[] moreColumns, int[] moreColumnTypes, DataRow dr, T model) where T : class, new()
        {
            if (moreColumns != null)
            {
                for (int j = 0; j < moreColumns.Length; j++)
                {
                    string nowColumnName = moreColumns[j].ToLower();
                    nowColumnName = nowColumnName[0].ToString().ToUpper() + nowColumnName.Substring(1);
                    try
                    {
                        if (moreColumnTypes == null)
                            model.GetType().GetProperty(nowColumnName).SetValue(model, GetOneStrValue(dr, moreColumns[j].ToUpper()), null);
                        else
                        {
                            switch (moreColumnTypes[j])
                            {
                                case CosDataBase.SMALLINT_TYPE:
                                case CosDataBase.INT_TYPE:
                                case CosDataBase.LONGINT_TYPE:
                                    model.GetType().GetProperty(nowColumnName).SetValue(model, Convert.ToInt64(GetOneStrValue(dr, moreColumns[j].ToUpper(), moreColumnTypes[j])), null);
                                    break;
                                case CosDataBase.DATE_TYPE:
                                case CosDataBase.DATETIME_TYPE:
                                    model.GetType().GetProperty(nowColumnName).SetValue(model, Convert.ToDateTime(GetOneStrValue(dr, moreColumns[j].ToUpper(), moreColumnTypes[j])), null);
                                    break;
                                case CosDataBase.FLOAT_TYPE:
                                    model.GetType().GetProperty(nowColumnName).SetValue(model, Convert.ToDouble(GetOneStrValue(dr, moreColumns[j].ToUpper(), moreColumnTypes[j])), null);
                                    break;
                                default:
                                    model.GetType().GetProperty(nowColumnName).SetValue(model, GetOneStrValue(dr, moreColumns[j].ToUpper(), moreColumnTypes[j]), null);
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //model.GetType().GetProperty(s).SetValue(model, "0", null);
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 根据dm和mc从 List-Compare 找到一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dm"></param>
        /// <param name="mc"></param>
        /// <returns></returns>
        private T FindRecord<T>(List<T> list, string dm, string mc) where T : class
        {
            foreach (T m in list)
            {
                string nowDM = m.GetType().GetProperty("DM").GetValue(m, null).ToString();
                string nowMC = m.GetType().GetProperty("MC").GetValue(m, null).ToString();
                if ((nowDM == dm) && (nowMC == mc))
                    return m;
            }
            return null;
        }
        #endregion

        #region 导入中常用函数

        protected ListCompareTable GetCompareList(List<ListCompareTable> list, string dm)
        {
            try
            {
                if ((list != null) && (list.Count > 0) && (dm != null) && (dm != ""))
                {
                    ListCompareTable[] arr = list.Where(c => c.sOldID == dm).ToArray();
                    if ((arr != null) && (arr.Length > 0))
                        return arr[0];
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        /// <summary>
        /// 根据考生号，从报名库list中获取ID
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sKSH"></param>
        /// <returns></returns>
        protected long GetBMKIDFromList(List<BMKTable> list, string sKSH)
        {
            long lbmkID = 0;
            long[] ll = list.Where(c => c.KSH == sKSH || c.smallKSH == sKSH).Select(c => c.ID).ToArray();
            if ((ll != null) && (ll.Length > 0))
                lbmkID = ll[0];
            return lbmkID;
        }
        protected long GetMainListID(List<ListCompareTable> list, string dm)
        {
            try
            {
                if ((list != null) && (list.Count > 0) && (dm != null) && (dm != ""))
                {
                    long[] arr = list.Where(c => c.sOldID == dm).Select(c => c.lNewID).ToArray();
                    if ((arr != null) && (arr.Length > 0))
                        return arr[0];
                    else
                        return 0;
                }
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        protected long GetOriginListID(List<ListCompareTable> list, string dm)
        {
            try
            {
                if ((list != null) && (list.Count > 0) && (dm != null) && (dm != ""))                    
                {
                    long[] arr = list.Where(c => c.sOldID == dm).Select(c => c.lOriginID).ToArray();
                    if ((arr != null) && (arr.Length > 0))
                        return arr[0];
                    else
                        return 0;
                }
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        protected string GetOneStrValue(DataRow dr, string name, int type = CosDataBase.STRING_TYPE)
        {
            try
            {
                if (dr.Table.Columns.Contains(name) && dr[name] != DBNull.Value)
                {
                    switch (type)
                    {
                        case CosDataBase.DATE_TYPE:
                        case CosDataBase.DATETIME_TYPE:
                        case CosDataBase.TIME_TYPE:
                            DateTime dateT = Convert.ToDateTime(dr[name]);
                            if (dateT < Convert.ToDateTime("1905-03-14 00:00:00"))
                                return "1905-03-14 00:00:00";
                            else
                                return dateT.ToString("yyyy-MM-dd HH:mm:ss");
                        case CosDataBase.FLOAT_TYPE: return Convert.ToDouble(dr[name]).ToString();
                        case CosDataBase.BOOLEAN_TYPE:
                            if (dr[name].ToString() == "1")
                                return "true";
                            else
                                return "false";
                        case CosDataBase.SMALLINT_TYPE:
                        case CosDataBase.INT_TYPE:
                        case CosDataBase.LONGINT_TYPE:
                            if (dr[name].ToString() != "")
                                return dr[name].ToString().Trim();
                            else
                                return "0";
                        default: return dr[name].ToString().Trim();
                    }
                }
                else
                {
                    switch (type)
                    {
                        case CosDataBase.DATE_TYPE:
                        case CosDataBase.DATETIME_TYPE:
                        case CosDataBase.TIME_TYPE: return "1905-03-14 00:00:00";
                        case CosDataBase.FLOAT_TYPE: return "0.0";
                        case CosDataBase.BOOLEAN_TYPE: return "false";
                        case CosDataBase.SMALLINT_TYPE:
                        case CosDataBase.INT_TYPE:
                        case CosDataBase.LONGINT_TYPE: return "0";
                        default: return "";
                    }
                }
            }
            catch
            {
                switch (type)
                {
                    case CosDataBase.DATE_TYPE: 
                    case CosDataBase.DATETIME_TYPE: 
                    case CosDataBase.TIME_TYPE: return "1905-03-14 00:00:00";
                    case CosDataBase.FLOAT_TYPE: return "0.0";
                    case CosDataBase.BOOLEAN_TYPE: return "false";
                    case CosDataBase.SMALLINT_TYPE:                     
                    case CosDataBase.INT_TYPE: 
                    case CosDataBase.LONGINT_TYPE: return "0";
                    default: return "";
                }
            }
        }
        #endregion

        #region 导入表相关
        /// <summary>
        /// 导入基础数据表的公共函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="nYear"></param>
        /// <param name="lProvinceID"></param>
        /// <param name="keyword"></param>
        /// <param name="mainColumns"></param>
        /// <param name="moreColumns"></param>
        /// <param name="moreColumnTypes"></param>
        /// <returns></returns>
        public List<ListCompareTable> InportSimpleTableOriginData<T, T1>(DataTable dataTable, int nYear, long lProvinceID, string keyword, string[] mainColumns = null, string[] moreColumns = null, int[] moreColumnTypes = null) where T : class, new() where T1 : class, new()
        {
            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            try
            {
                if (dataTable != null)
                {
                    DataView dataView = dataTable.DefaultView;
                    if (mainColumns == null)
                    {
                        mainColumns = new string[2];
                        mainColumns[0] = dataTable.Columns[0].ColumnName;
                        mainColumns[1] = dataTable.Columns[1].ColumnName;
                    }
                    DataTable dt = dataView.ToTable(true, mainColumns[0], mainColumns[1]);
                    List<T1> OriginModels = InportSomeRowsOriginTable<T1>(dt, nYear, lProvinceID, mainColumns, moreColumns, moreColumnTypes);
                    TableStatusManage statusManage = new TableStatusManage();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        long lNowRelationID = Convert.ToInt64(OriginModels[i].GetType().GetProperty("RelationID").GetValue(OriginModels[i], null));
                        if (lNowRelationID == 0) //原始表中新加入的字段
                        {
                            long lNewID = 0;
                            T obj = getOneMainRecord<T>(dt.Rows[i][1].ToString());
                            if (obj == null) //目前主表中不存在
                            {
                                while (!statusManage.BeginUpdate<T>())
                                {
                                    Random ra = new Random();
                                    Thread.Sleep(ra.Next(100, 1000));
                                }
                                obj = getOneMainRecord<T>(dt.Rows[i][1].ToString());
                                if (obj == null)  //还是不存在
                                {
                                    int num = 4;
                                    if (moreColumns != null)
                                        num = num + moreColumns.Length;
                                    string[] EditColumns = new string[num];
                                    EditColumns[0] = "Name";
                                    EditColumns[1] = "Remark";
                                    EditColumns[2] = "Code";
                                    EditColumns[3] = "JoinNames";
                                    if (moreColumns != null)
                                        for (int j = 0; j < moreColumns.Length; j++)
                                            EditColumns[j + 4] = moreColumns[j];

                                    T model = new T();
                                    model.GetType().GetProperty("Name").SetValue(model, dt.Rows[i][1].ToString(), null);
                                    model.GetType().GetProperty("Code").SetValue(model, dt.Rows[i][0].ToString(), null);
                                    model.GetType().GetProperty("JoinNames").SetValue(model, dt.Rows[i][1].ToString(), null);

                                    if ((dt.Columns.Count > 2) && (dt.Columns[2].ColumnName == "BZ"))
                                        model.GetType().GetProperty("Remark").SetValue(model, dt.Rows[i][2].ToString(), null);
                                    model = ArrangeMoreColumns(moreColumns, moreColumnTypes, dt.Rows[i], model);

                                    lNewID = SaveOneSimpleRecord(model, keyword, EditColumns);
                                    statusManage.EndUpdate<T>();
                                    ListCompareTable oneTable = new ListCompareTable();
                                    oneTable.sOldID = dt.Rows[i][0].ToString();
                                    oneTable.lNewID = lNewID;
                                    oneTable.lOriginID = Convert.ToInt64(OriginModels[i].GetType().GetProperty("ID").GetValue(OriginModels[i], null));
                                    listReturn.Add(oneTable);
                                }
                                else //主表中存在了
                                {
                                    statusManage.EndUpdate<T>();
                                    lNewID = Convert.ToInt64(obj.GetType().GetProperty("ID").GetValue(obj, null));
                                    ListCompareTable oneTable = new ListCompareTable();
                                    oneTable.sOldID = dt.Rows[i][0].ToString();
                                    oneTable.lNewID = lNewID;
                                    oneTable.lOriginID = Convert.ToInt64(OriginModels[i].GetType().GetProperty("ID").GetValue(OriginModels[i], null));
                                    listReturn.Add(oneTable);
                                }
                            }
                            else //目前主表中存在
                            {
                                lNewID = Convert.ToInt64(obj.GetType().GetProperty("ID").GetValue(obj, null));
                                ListCompareTable oneTable = new ListCompareTable();
                                oneTable.sOldID = dt.Rows[i][0].ToString();
                                oneTable.lNewID = lNewID;
                                oneTable.lOriginID = Convert.ToInt64(OriginModels[i].GetType().GetProperty("ID").GetValue(OriginModels[i], null));
                                listReturn.Add(oneTable);
                            }
                            //不管存在与否，都需修改原始表中相应的RelationID
                            //string[] EditColumn = { "RelationID" };
                            OriginModels[i].GetType().GetProperty("RelationID").SetValue(OriginModels[i], lNewID, null);
                            //SaveOneRecord(OriginModels[i], EditColumn);
                        }
                        else //原始表中已有相关字段，直接取用
                        {
                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = dt.Rows[i][0].ToString();
                            oneTable.lNewID = lNowRelationID;
                            oneTable.lOriginID = Convert.ToInt64(OriginModels[i].GetType().GetProperty("ID").GetValue(OriginModels[i], null));
                            listReturn.Add(oneTable);
                        }
                    }
                    SaveManyRecords(OriginModels);
                }
                return listReturn;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 导入高考成绩，从17表的报名库中或12表的投档单中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <param name="lbmkID"></param>
        protected void InportGkcj(DataTable dt, List<BMKTable> BMK_list, List<ListCompareTable> CJXDM_list)
        {
            List<T_ksxkcj> listAll = new List<T_ksxkcj>();
            foreach (DataRow dr in dt.Rows)
            {
                long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                if (lbmkID > 0)
                {
                    for (int i = 0; i < dr.ItemArray.Length; i++)
                    {
                        string ColumnName = dt.Columns[i].ColumnName;
                        if (ColumnName.Contains("GKCJX"))
                        {
                            T_ksxkcj model = new T_ksxkcj();
                            if (dr.ItemArray[i] != DBNull.Value)
                            {
                                try
                                {
                                    model.Ddcj = Convert.ToDouble(dr.ItemArray[i]);
                                }
                                catch { }
                            }
                            if (model.Ddcj > 0)
                            {
                                string cjxdm = ColumnName.Replace("GKCJX", "");
                                model.RelationID = lbmkID;
                                model.Kmdm = 0;
                                model.KmdmOrigin = 0;
                                ListCompareTable oneCompare = GetCompareList(CJXDM_list, cjxdm);
                                if (oneCompare != null)
                                {
                                    model.CjxdmOrigin = oneCompare.lOriginID;
                                    model.Cjxdm = oneCompare.lNewID;
                                }
                                model.Djff = 0;
                                model.Cjlx = 2;
                                listAll.Add(model);
                            }
                        }
                    }
                }
            }
            AddRecords(listAll);
        }

        /// <summary>
        /// 原始表中新增（或者获取）若干条记录
        /// </summary>
        /// <typeparam name="T">原始表类型</typeparam>
        /// <param name="dr">传过来的DataRow</param>
        /// <param name="nYear">当前年份</param>
        /// <param name="lProvinceID">当前省份</param>
        /// <param name="mainColumns">主Column名称，默认null表示第0列和第1列</param>
        /// <param name="moreColumns">更多Column名称，默认null表示不需要，否则可以操作较复杂基础表的更多列</param>
        /// <param name="lParentDepartmentID">当前操作账号所在单位（高校），默认0表示不需要</param>
        /// <returns></returns>
        public List<T> InportSomeRowsOriginTable<T>(DataTable dt, int nYear, long lProvinceID, string[] mainColumns = null, string[] moreColumns = null, int[] moreColumnTypes = null, long lParentDepartmentID = 0) where T : class, new()
        {
            List<T> returnArr = new List<T>();
            List<T> addArr = new List<T>();

            //首先取出当年当省相关数据
            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("DelFlag"); values.Add(false);
            names.Add("YearCode"); values.Add(nYear);
            names.Add("ProvinceID"); values.Add(lProvinceID);
            if (lParentDepartmentID != 0)
            { names.Add("ParentDepartmentID"); values.Add(lParentDepartmentID); }
            Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(names, values);
            List<T> listOrigin = GetRecord(where);

            string dm = "", mc = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (mainColumns == null)
                {
                    dm = dr[0].ToString();
                    mc = dr[1].ToString();
                }
                else
                {
                    dm = dr[mainColumns[0]].ToString();
                    mc = dr[mainColumns[1]].ToString();
                }

                T record = FindRecord(listOrigin, dm, mc);
                if (record != null) //存在
                {
                    returnArr.Add(record);
                    listOrigin.Remove(record);
                }
                else //不存在
                {
                    T model = new T();
                    model.GetType().GetProperty("YearCode").SetValue(model, nYear, null);
                    model.GetType().GetProperty("ProvinceID").SetValue(model, lProvinceID, null);
                    model.GetType().GetProperty("DM").SetValue(model, dm, null);
                    model.GetType().GetProperty("MC").SetValue(model, mc, null);
                    model.GetType().GetProperty("RelationID").SetValue(model, 0, null);
                    model = ArrangeMoreColumns(moreColumns, moreColumnTypes, dr, model);

                    addArr.Add(model);
                    returnArr.Add(model);
                }
            }
            if ((addArr != null) && (addArr.Count > 0))
                AddRecords(addArr);

            return returnArr;
        }




        /// <summary>
        /// 原始表中新增（或者获取）一条记录
        /// </summary>
        /// <typeparam name="T">原始表类型</typeparam>
        /// <param name="dr">传过来的DataRow</param>
        /// <param name="nYear">当前年份</param>
        /// <param name="lProvinceID">当前省份</param>
        /// <param name="mainColumns">主Column名称，默认null表示第0列和第1列</param>
        /// <param name="moreColumns">更多Column名称，默认null表示不需要，否则可以操作较复杂基础表的更多列</param>
        /// <param name="lParentDepartmentID">当前操作账号所在单位（高校），默认0表示不需要</param>
        /// <returns></returns>
        public T InportOneRowOriginTable<T>(DataRow dr, int nYear, long lProvinceID, string[] mainColumns = null, string[] moreColumns = null, int[] moreColumnTypes = null, long lParentDepartmentID = 0) where T : class, new()
        {
            string dm = "", mc = "";
            if (mainColumns == null)
            {
                dm = dr[0].ToString();
                mc = dr[1].ToString();
            }
            else
            {
                dm = dr[mainColumns[0]].ToString();
                mc = dr[mainColumns[1]].ToString();
            }

            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("DelFlag"); values.Add(false);
            names.Add("YearCode"); values.Add(nYear);
            names.Add("ProvinceID"); values.Add(lProvinceID);
            names.Add("DM"); values.Add(dm);
            names.Add("MC"); values.Add(mc);

            if (lParentDepartmentID != 0)
            { names.Add("ParentDepartmentID"); values.Add(lParentDepartmentID); }

            Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(names, values);
            List<T> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
            {
                T model = new T();
                model.GetType().GetProperty("YearCode").SetValue(model, nYear, null);
                model.GetType().GetProperty("ProvinceID").SetValue(model, lProvinceID, null);
                model.GetType().GetProperty("DM").SetValue(model, dm, null);
                model.GetType().GetProperty("MC").SetValue(model, mc, null);
                model.GetType().GetProperty("RelationID").SetValue(model, 0, null);
                model = ArrangeMoreColumns(moreColumns, moreColumnTypes, dr, model);

                long lNewID = AddOneRecord(model);
                model.GetType().GetProperty("ID").SetValue(model, lNewID, null);
                return model;
            }
        }

        #endregion

        #region 生成报表中常用函数

        public string GetKeyWords(string sKey)
        {
            switch (sKey)
            {
                case "Province": return "省份";
                case "bylbdmTable": return "毕业类别";
                case "xbdmTable": return "性别";
                case "zzmmdmTable": return "政治面貌";
                case "mzdmTable": return "民族";
                case "kslbdmTable": return "考生类别";
                case "kslxdmTable": return "考试类型";
                case "byxxdmTable": return "毕业学校";
                case "hjlbdmTable": return "户籍类别";
                case "pcdmTable": return "招生批次";                
            }
            return "";
        }


        public object[] GetNameStoreforSelection(string sKey, string SelectedStore, bool useFlag)
        {
            Type type = GetTypeObj(sKey).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            string sKeyWords = GetKeyWords(sKey);
            return _GetNameStoreforSelection(obj, SelectedStore, useFlag, sKey, sKeyWords);
        }

        private object[] _GetNameStoreforSelection<T>(T obj, string SelectedStore, bool useFlag, string sTypeID, string sKeyWords) where T : class
        {
            string sUsedIDs = GetUsedIDs(SelectedStore, sTypeID);
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            arrNames.Add("DelFlag"); arrNames.Add("ID");
            arrValues.Add(false); arrValues.Add(sUsedIDs);
            arrSigns.Add(SIGN.Equal);
            if (useFlag)
                arrSigns.Add(SIGN.In);
            else
                arrSigns.Add(SIGN.NotIn);
            var where = GetAndLambdaExpression<T>(arrNames, arrValues, arrSigns);

            object[] data = GetReturnData(sTypeID, where, sKeyWords + "-");
            if (data != null)
                return data;

            object[] data0 = new object[0];
            return data0;
        }

        #endregion

        #region 处理JoinName相关函数

        /// <summary>
        /// 根据选中的多条记录，返回这些记录的名称
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetSomeNames(List<SelectionModel> sel, string keyword)
        {
            Type type = GetTypeObj(keyword).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetSomeNames(obj, sel);
        }

        private string _GetSomeNames<T>(T obj, List<SelectionModel> sel) where T : class
        {
            string s = "";
            List<T> list = GetRecordsByID<T>(string.Join(CosValue.SPLITOPERATOR2.ToString(), sel.Select(x => x.RecordID).ToArray()));
            if ((list != null) && (list.Count > 0))
            {
                foreach (T m in list)
                    s = s + m.GetType().GetProperty("JoinNames").GetValue(m, null) + CosValue.SPLITOPERATOR1;
            }
            if (s.Length > 1)
                s = s.Remove(s.Length - 1);
            return s;
        }

        public void JoinRecords(JoinModel model, string keyword)
        {
            Type type = GetTypeObj(keyword).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            _JoinRecords(obj, model);
        }

        private void _JoinRecords<T>(T obj, JoinModel model) where T : class, new()
        {
            //首先修改合并的记录的JoinNames
            T mo = new T();
            mo.GetType().GetProperty("ID").SetValue(mo, model.ID, null);
            mo.GetType().GetProperty("JoinNames").SetValue(mo, model.AllJoinNames, null);
            string[] EditColumn = { "JoinNames" };
            SaveOneRecord(mo, EditColumn);
            //然后修改被合并的记录的信息
            List<T> list = GetRecordsByID<T>(model.AllJoinIDs);
            if ((list != null) && (list.Count > 0))
            {
                string[] EditColumns2 = { "DelFlag", "JoinID" };
                foreach (T m in list)
                {
                    long nowID = Convert.ToInt64(m.GetType().GetProperty("ID").GetValue(m, null));
                    if (nowID != model.ID)
                    {
                        m.GetType().GetProperty("DelFlag").SetValue(m, true, null);
                        m.GetType().GetProperty("JoinID").SetValue(m, model.ID, null);
                    }
                }
                SaveSomeRecords(list, EditColumns2);
            }
        }        

        public T getOneMainRecord<T>(string sJoinName) where T : class, new()
        {
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            ArrayList signs = new ArrayList();
            names.Add("JoinNames"); names.Add("DelFlag");
            values.Add(sJoinName); values.Add(false);
            signs.Add(SIGN.Contain1.ToString()); signs.Add(SIGN.Equal.ToString());
            Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(names, values, signs);
            List<T> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        #endregion
    }
}
