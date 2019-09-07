using System;
using StudentManage.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections;
using Ext.Net;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using System.Data;

namespace StudentManage.Manage
{
    public class ButtonModel
    {
        public int ButtonID { get; set; }
        public string ButtonName { get; set; }
        public Icon ButtonIcon { get; set; }


        public ButtonModel(int ID, string Name = null, Icon icon = Icon.Blank)
        {
            ButtonID = ID;
            ButtonName = Name;
            ButtonIcon = icon;
        }
    }
    public class SearchComboBoxModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }


        public SearchComboBoxModel(int id, string name = null, string value = null)
        {
            ID = id;
            Name = name;
            Value = value;
        }
    }
    public class ColumnModel
    {
        public string WordName { get; set; }
        public string WordValue { get; set; }
        public int WordType { get; set; }
        public int ColumnWidth { get; set; }

        public ColumnModel(string Name, string Value, int Type = CosDataBase.STRING_TYPE, int Width = 80)
        {
            WordName = Name;
            WordValue = Value;
            WordType = Type;
            ColumnWidth = Width;
        }
    }
    public class SelectionModel
    {
        public string RecordID { get; set; }
        public string RowIndex { get; set; }

        public SelectionModel(string id, string index)
        {
            RecordID = id;
            RowIndex = index;
        }
    }
    /// <summary>
    /// 跟DataList相关的各种操作
    /// </summary>
    public class DataListManage : ParentManage
    {
        public ArrayList arrColumn;  //所有列
        public ArrayList arrTopButton; //Topbar上的button列表        

        public bool bHasDelFlag = true;
        public bool bFirstVisit = false;
        public bool bSingleSelect = false;

        public bool bSort = true; //是否允许点击头部后排序
        public bool bNeedAuthor = true; //是否需要关心权限

        public string sOrder = "";  //排序字段名称
        public int nOrderType = 0;  //排序字段类型
        public bool bOrderSequence = true;  //排序字段方向，默认asc

        public string sRowNowID = "0";
        public string sRowAllIDs = "0";
                
        public StoreRequestParameters paremeters;//跟分页有关的参数

        protected ArrayList arrNowNames = new ArrayList();
        protected ArrayList arrNowValues = new ArrayList();
        protected ArrayList arrNowSigns = new ArrayList();
        protected ArrayList arrNowAndors = new ArrayList();

        public DataListManage()
        {
            arrColumn = new ArrayList();
            arrTopButton = new ArrayList();
            InitDataListHeader();
        }

        #region 供子类调用的虚类
        public virtual void SetMainListType()
        {

        }

        public virtual void SetarrColumn()
        {

        }

        public virtual void SetarrTopButton()
        {

        }
        
        /// <summary>
        /// 初始化DataList的store
        /// </summary>
        /// <param name="sNowNames"></param>
        /// <param name="sNowValues"></param>
        /// <param name="sNowSigns"></param>
        /// <param name="sAndOrs"></param>
        /// <returns></returns>
        public virtual Paging<object> InitStore(string sNowNames = "", string sNowValues = "", string sNowSigns = "", string sAndOrs = "")
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _InitStore(obj, sNowNames, sNowValues, sNowSigns);
        }

        public virtual dynamic GetSpecialModelFromCurrentSelection(string selection)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetSpecialModelFromCurrentSelection(obj, selection);
        }
        /// <summary>
        /// 选中条目上移（数据库表中必须有TheOrder字段排序）
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="RelationIDName"></param>
        /// <param name="OtherRelationName"></param>
        public virtual void OrderUpRecord(string selection, string RelationIDName = null, string OtherRelationName = null)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            _OrderUpRecord(obj, selection, RelationIDName);
        }
        /// <summary>
        /// 选中条目下移（数据库表中必须有TheOrder字段排序）
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="RelationIDName"></param>
        /// <param name="OtherRelationName"></param>
        public virtual void OrderDownRecord(string selection, string RelationIDName = null, string OtherRelationName = null)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            _OrderDownRecord(obj, selection, RelationIDName);
        }

        public virtual DataTable GetSomeRecordForExcel(string sNames, string sValues, string sSigns)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetSomeRecordForExcel(obj, sNames, sValues, sSigns);
        }
        #endregion

        public void InitDataListHeader()
        {
            SetMainListType();
            SetarrColumn();
            SetarrTopButton();
        }

        /// <summary>
        /// 根据传过来的字符串，初始化sRowNowID和sRowAllIDs
        /// </summary>
        /// <param name="selection"></param>
        public void InitSelection(string selection)
        {
            sRowNowID = "0";
            sRowAllIDs = "0";
            if (selection != null)
            { 
                if ((selection != "") && (selection != "0"))
                {
                    List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(selection);
                    if ((sel != null) && (sel.Count > 0))
                    {
                        sRowNowID = sel[0].RecordID;
                        sRowAllIDs = string.Join(CosValue.SPLITOPERATOR2.ToString(), sel.Select(x => x.RecordID).ToArray());
                    }
                }
            }
        }
        
        /// <summary>
        /// 查询窗口，初始化查询用的ComboBox
        /// </summary>
        /// <returns></returns>
        public virtual object GetModelsForComboSearch()
        {
            return null;
        }


        /// <summary>
        /// 根据当前选择，从数据库中获取并返回若干个的具体model
        /// </summary>        
        /// <returns></returns>
        private List<T> _GetSpecialModelFromCurrentSelection<T>(T obj, string selection) where T : class
        {
            InitSelection(selection);
            List<T> models = GetRecordsByID<T>(sRowAllIDs);
            return models;
        }
        
        protected dynamic GetNowOrder<T>(string order = "", int ordertype = 0) where T : class
        {
            SetarrColumn();
            bool flag = false;
            int i = 0;
            if (paremeters != null)
            { 
                if (paremeters.SimpleSort != null)
                {
                    while ((!flag) && (i < arrColumn.Count))
                    {
                        if (((ColumnModel)arrColumn[i]).WordValue == paremeters.SimpleSort)
                            flag = true;
                        else
                            i++;
                    }
                }
            }
            if (flag)
            {
                order = paremeters.SimpleSort;
                ordertype = ((ColumnModel)arrColumn[i]).WordType;
            }
            else
            {
                if (sOrder != "")
                {
                    order = sOrder;
                    ordertype = nOrderType;
                }
            }

            if (order != "")
            {
                switch (ordertype)
                {
                    case CosDataBase.BOOLEAN_TYPE:
                    case CosDataBase.BOOLEANBOX_TYPE:
                        return BuildOrder<T, bool>(order);
                    case CosDataBase.INT_TYPE:
                        return BuildOrder<T, int>(order);
                    case CosDataBase.SMALLINT_TYPE:
                        return BuildOrder<T, short>(order);
                    case CosDataBase.FLOAT_TYPE:
                        return BuildOrder<T, double>(order);
                    case CosDataBase.DATETIME_TYPE:
                    case CosDataBase.DATE_TYPE:
                    case CosDataBase.TIME_TYPE:
                        return BuildOrder<T, DateTime>(order);
                    default:
                        return BuildOrder<T, string>(order);
                }
            }
            else
                return BuildOrder<T, long>("ID");
        }




        #region InitStore

        protected Paging<object> GetStoreObject<T>(List<T> list, int nTotalRecord) where T : class
        {
            if (list != null)
            {
                object[] data = new object[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    object[] dataline = new object[arrColumn.Count + 1];
                    dataline[0] = list[i].GetType().GetProperty("ID").GetValue(list[i], null);
                    for (int j = 0; j < arrColumn.Count; j++)
                        dataline[j + 1] = GetPropValue(list[i], ((ColumnModel)arrColumn[j]).WordValue, Convert.ToInt16(((ColumnModel)arrColumn[j]).WordType));
                    data[i] = dataline;
                }
                return new Paging<object>(data, nTotalRecord);                
            }
            else
            {
                object[] data = new object[0];
                return new Paging<object>(data, 0);
            }
        }

        protected object GetPropValue<T>(T list, string WordValue, int WordType) where T : class
        {
            object obj = new object();
            try
            {                
                obj = list;
                if (WordType == CosDataBase.JOIN_TYPE)
                {
                    #region JOIN_TYPE
                    string s = "";
                    string[] arValues = WordValue.Split(CosValue.SPLITOPERATOR2); //首先找到间隔点
                    string[] ColNames = arValues[0].Split('.');
                    int i = 0;
                    while (i < ColNames.Length - 1)
                    {
                        PropertyInfo propertyInfo1 = obj.GetType().GetProperty(ColNames[i]);
                        if (i == 0)
                            obj = propertyInfo1.GetValue(list, null);
                        else
                            obj = propertyInfo1.GetValue(obj, null);
                        i++;
                    }
                    string sListTableName = ColNames[ColNames.Length - 1];
                    string[] arrValues = arValues[1].Split(CosValue.SPLITOPERATOR1);
                    string sListTableColumn = arrValues[0];
                    string sSearchName = "", sSearchValue = "";
                    if (arrValues.Length > 1)
                    {
                        string[] sSearch = arrValues[1].Split('=');
                        sSearchName = sSearch[0];
                        sSearchValue = sSearch[1];
                    }
                    IEnumerable<object> listTable = obj.GetType().GetProperty(sListTableName).GetValue(obj) as IEnumerable<object>;
                    ArrayList arrStr = new ArrayList();
                    ArrayList arrOrder = new ArrayList();
                    foreach (object ll in listTable)
                    {
                        string[] ColumnNames = sListTableColumn.Split('.');
                        i = 0;
                        obj = ll;
                        short nOrder = 0;
                        PropertyInfo propertyInfo4 = obj.GetType().GetProperty("TheOrder");
                        if (propertyInfo4 != null)
                            nOrder = Convert.ToInt16(propertyInfo4.GetValue(obj, null));
                        while (i < ColumnNames.Length - 1)
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(ColumnNames[i]);
                            if (i == 0)
                                obj = propertyInfo.GetValue(ll, null);
                            else
                                obj = propertyInfo.GetValue(obj, null);
                            i++;
                        }
                        if (obj != null)
                        {
                            PropertyInfo propertyInfo1 = obj.GetType().GetProperty(ColumnNames[i]);
                            PropertyInfo propertyInfo3 = obj.GetType().GetProperty("DelFlag");
                            bool delflag = false;
                            if (propertyInfo3 != null)
                                delflag = Convert.ToBoolean(propertyInfo3.GetValue(obj, null));

                            if (!delflag)
                            {
                                if (sSearchValue != "")
                                {
                                    PropertyInfo propertyInfo2 = ll.GetType().GetProperty(sSearchName);
                                    if (propertyInfo2 != null)
                                    {
                                        string obt = Convert.ToString(propertyInfo2.GetValue(ll, null));
                                        if (obt == sSearchValue)
                                        {
                                            int ii = 0;
                                            bool bhereflag = false;
                                            while ((!bhereflag) && (ii < arrOrder.Count))
                                            {
                                                if (Convert.ToInt16(arrOrder[ii]) > nOrder)
                                                    bhereflag = true;
                                                else
                                                    ii++;
                                            }
                                            arrOrder.Insert(ii, nOrder);
                                            arrStr.Insert(ii, propertyInfo1.GetValue(obj, null).ToString());
                                        }
                                        //s = s + propertyInfo1.GetValue(obj, null).ToString() + CosValue.SPLITOPERATOR2;
                                    }
                                }
                                else
                                {
                                    int ii = 0;
                                    bool bhereflag = false;
                                    while ((!bhereflag) && (ii < arrOrder.Count))
                                    {
                                        if (Convert.ToInt16(arrOrder[ii]) > nOrder)
                                        {
                                            bhereflag = true;
                                            ii--;
                                        }
                                        else
                                            ii++;
                                    }
                                    arrOrder.Insert(ii, nOrder);
                                    arrStr.Insert(ii, propertyInfo1.GetValue(obj, null).ToString());
                                }
                                //    s = s + propertyInfo1.GetValue(obj, null).ToString() + CosValue.SPLITOPERATOR2;
                            }
                        }
                    }
                    if (arrStr.Count > 0)
                        s = string.Join(CosValue.SPLITOPERATOR2.ToString(), (string[])arrStr.ToArray(typeof(string)));
                    return s;
                    #endregion
                }
                else if (WordType == CosDataBase.PARENTNAME_TYPE)
                {
                    #region PARENTNAME_TYPE
                    string s = "";
                    if (obj != null)
                    {
                        long lParentIndex = Convert.ToInt64(obj.GetType().GetProperty(WordValue).GetValue(obj, null));
                        while (lParentIndex != -1)
                        {
                            obj = GetOneRecordbyID<T, long>(lParentIndex);
                            s = obj.GetType().GetProperty("Name").GetValue(obj, null).ToString() + s;
                            lParentIndex = Convert.ToInt64(obj.GetType().GetProperty(WordValue).GetValue(obj, null));
                        }                        
                    }
                    return s;
                    #endregion
                }
                else
                {
                    #region OtherType
                    string[] ColumnNames = WordValue.Split('.');
                    int i = 0;
                    while (i < ColumnNames.Length - 1)
                    {
                        PropertyInfo propertyInfo1 = obj.GetType().GetProperty(ColumnNames[i]);
                        if (i == 0)
                            obj = propertyInfo1.GetValue(list, null);
                        else
                            obj = propertyInfo1.GetValue(obj, null);
                        i++;
                    }
                    if (obj != null)
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(ColumnNames[i]);
                        if (propertyInfo.GetValue(obj, null) != null)
                        {
                            switch (WordType)
                            {
                                case CosDataBase.DATETIME_TYPE:
                                    obj = Convert.ToDateTime(propertyInfo.GetValue(obj, null)).ToString("yyyy-MM-dd HH:mm:ss");
                                    break;
                                case CosDataBase.DATE_TYPE:
                                    obj = Convert.ToDateTime(propertyInfo.GetValue(obj, null)).ToString("yyyy-MM-dd");
                                    break;
                                case CosDataBase.TIME_TYPE:
                                    obj = Convert.ToDateTime(propertyInfo.GetValue(obj, null)).ToString("HH:mm:ss");
                                    break;
                                case CosDataBase.BOOLEAN_TYPE:
                                    if (Convert.ToBoolean(propertyInfo.GetValue(obj, null)))
                                        obj = "是";
                                    else
                                        obj = "否";
                                    break;
                                case CosDataBase.BOOLEANBOX_TYPE:
                                    if (Convert.ToBoolean(propertyInfo.GetValue(obj, null)))
                                        obj = true;
                                    else
                                        obj = false;
                                    break;
                                case CosDataBase.SMALLINT_TYPE:
                                    obj = Convert.ToInt16(propertyInfo.GetValue(obj, null));
                                    break;
                                case CosDataBase.LONGINT_TYPE:
                                    obj = Convert.ToInt64(propertyInfo.GetValue(obj, null));
                                    break;
                                case CosDataBase.INT_TYPE:
                                    obj = Convert.ToInt32(propertyInfo.GetValue(obj, null));
                                    break;
                                case CosDataBase.FLOAT_TYPE:
                                    obj = Convert.ToDouble(propertyInfo.GetValue(obj, null));
                                    break;
                                default:
                                    obj = (string)propertyInfo.GetValue(obj, null);
                                    break;
                            }
                        }
                        else
                            obj = null;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                obj = null;
            }
            return obj;
        }


        #endregion       

        protected dynamic GetWhereForStore<T>(string sNowNames = "", string sNowValues = "", string sNowSigns = "")
        {
            SetMainListType();
            arrNowNames.Clear(); arrNowValues.Clear(); arrNowSigns.Clear();
            if (bHasDelFlag)
            {
                arrNowNames.Add("DelFlag");
                arrNowValues.Add(false);
                arrNowSigns.Add(SIGN.Equal);
            }

            if ((sNowNames != "") && (sNowNames != null))
            {
                arrNowNames.AddRange(sNowNames.Split(CosValue.SPLITOPERATOR1).ToArray());
                arrNowValues.AddRange(sNowValues.Split(CosValue.SPLITOPERATOR1).ToArray());
                if (sNowSigns == "")
                    arrNowSigns.AddRange(NewDuplicateArray(SIGN.Equal, sNowNames.Split(CosValue.SPLITOPERATOR1).Length));
                else
                    arrNowSigns.AddRange(sNowSigns.Split(CosValue.SPLITOPERATOR1).ToArray());
            }
            Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(arrNowNames, arrNowValues, arrNowSigns);
            return where;
        }
        private Paging<object> _InitStore<T>(T obj, string sNowNames = "", string sNowValues = "", string sNowSigns = "") where T : class
        {
            if (sNowNames.Contains("ClearDataList"))
            {
                List<T> list1 = null;
                return GetStoreObject(list1, 0);
            }
            else
            {
                //else if (sNowNames != "")
                //    sNowNames = sNowNames.Remove(sNowNames.LastIndexOf(CosValue.SPLITOPERATOR1));
                Expression<Func<T, bool>> where = GetWhereForStore<T>(sNowNames, sNowValues, sNowSigns);
                int nTotalRecord = 0;
                SetarrColumn();
                var order = GetNowOrder<T>();

                bool SortSeq = true;
                if (paremeters.SimpleSort == null)
                    SortSeq = bOrderSequence;
                else if (paremeters.SimpleSortDirection == SortDirection.DESC)
                    SortSeq = false;
                else
                    SortSeq = true;

                List<T> list = GetRecord(out nTotalRecord, where, order, SortSeq, paremeters.Page, paremeters.Limit);
                return GetStoreObject(list, nTotalRecord);
            }
        }

        #region orderdown and orderup
        private void _OrderUpRecord<T>(T obj, string selection, string RelationIDName = null, string OtherRelationName = null) where T : class
        {
            OrderRecord<T>(selection, RelationIDName, OtherRelationName, false);
        }
        private void _OrderDownRecord<T>(T obj, string selection, string RelationIDName = null, string OtherRelationName = null) where T : class
        {
            OrderRecord<T>(selection, RelationIDName, OtherRelationName, true);
        }
        private void OrderRecord<T>(string selection, string RelationIDName, string OtherRelationName, bool asc) where T : class
        {
            InitSelection(selection);
            var model1 = (T)GetSpecialModelFromCurrentID(sRowNowID);
            short model1order = Convert.ToInt16(model1.GetType().GetProperty("TheOrder").GetValue(model1, null));
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            ArrayList signs = new ArrayList();
            names.Add("TheOrder");
            values.Add(model1order);
            if (asc)
                signs.Add(SIGN.GreaterThan);
            else
                signs.Add(SIGN.LessThan);
            if (bHasDelFlag)
            {
                names.Add("DelFlag");
                values.Add(false);
                signs.Add(SIGN.Equal);
            }
            if (OtherRelationName != null)
            {
                names.Add(OtherRelationName);
                values.Add(Convert.ToString(model1.GetType().GetProperty(OtherRelationName).GetValue(model1, null)));
                signs.Add(SIGN.Equal);
            }
            if (RelationIDName != null)
            {
                names.Add(RelationIDName);
                values.Add(Convert.ToInt64(model1.GetType().GetProperty(RelationIDName).GetValue(model1, null)));
                signs.Add(SIGN.Equal);
            }
            var express = GetAndLambdaExpression<T>(names, values, signs);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> list = GetRecord(express, order, asc);
            if (list.Count > 0)
            {
                string[] EditNames = { "TheOrder" };
                short nTemp = Convert.ToInt16(list[0].GetType().GetProperty("TheOrder").GetValue(list[0], null));
                list[0].GetType().GetProperty("TheOrder").SetValue(list[0], model1order);
                model1.GetType().GetProperty("TheOrder").SetValue(model1, nTemp);
                SaveOneRecord(list[0], EditNames);
                SaveOneRecord(model1, EditNames);
            }
        }
        #endregion

        protected DataTable _GetSomeRecordForExcel<T>(T obj, string sNames, string sValues, string sSigns) where T : class
        {
            ArrayList arrNames = new ArrayList();
            ArrayList arrValues = new ArrayList();
            ArrayList arrSigns = new ArrayList();

            if (bHasDelFlag)
            {
                arrNames.Add("DelFlag");
                arrValues.Add(false);
                arrSigns.Add(SIGN.Equal);
            }

            if (sNames != "")
            {
                string[] allnames = sNames.Split(CosValue.SPLITOPERATOR1);
                string[] allvalues = sValues.Split(CosValue.SPLITOPERATOR1);
                string[] allsigns = sSigns.Split(CosValue.SPLITOPERATOR1);
                for (int i = 0; i < allnames.Length; i++)
                {
                    arrNames.Add(allnames[i]);
                    arrValues.Add(allvalues[i]);
                    arrSigns.Add(allsigns[i]);
                }
            }
            var where = GetAndLambdaExpression<T>(arrNames, arrValues, arrSigns);
            var list = GetRecord(where);
            DataTable dt = ListToDataTable(list);
            return dt;
        }

        public DataTable ListToDataTable<T>(List<T> entitys) where T: class
        {
            SetarrColumn();

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)                
                return null;
            
            DataTable dt = new DataTable();
            //生成Header
            object[] header = new object[arrColumn.Count];
            for (int i = 0; i < arrColumn.Count; i++)
            {
                dt.Columns.Add(((ColumnModel)arrColumn[i]).WordName);
                header[i] = ((ColumnModel)arrColumn[i]).WordName;
            }
            dt.Rows.Add(header);

            if (entitys != null)
            {                
                for (int i = 0; i < entitys.Count; i++)
                {
                    object[] dataline = new object[arrColumn.Count];
                    for (int j = 0; j < arrColumn.Count; j++)
                        dataline[j] = GetPropValue(entitys[i], ((ColumnModel)arrColumn[j]).WordValue, Convert.ToInt16(((ColumnModel)arrColumn[j]).WordType));
                    dt.Rows.Add(dataline);
                }
            }
            
            return dt;
        }

        
    }
}