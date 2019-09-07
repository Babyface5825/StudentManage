using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.OleDb;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using Ext.Net;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class zydhTableManage : OriginTableManage
    {
        public List<ListCompareTable> CCDM_list;
        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> XZDM_list;
        public List<ListCompareTable> ZYLBDM_list;
        public List<ListCompareTable> TDDW_list;

        public zydhTableManage()
        {
            Manage_KeyWord = "zydhTable";
        }

        #region 一些private
        /// <summary>
        /// 根据lParentDepartmentID，获得对应的默认第一个TddwID
        /// </summary>
        /// <param name="lParentDepartmentID"></param>
        /// <returns></returns>
        private long GetNowTddwID(long lParentDepartmentID)
        {
            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.ID == lParentDepartmentID);
            List<tblDepartmentInfo> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                Td_gxdm gx = list[0].gx;
                if (gx != null)
                {
                    if ((gx.tddws != null) && (gx.tddws.Count > 0))
                        return gx.tddws[0].ID;
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据当前传来的dr(2012表中的计划库存在tddw）和当前用户的tddwID，返回tddwID
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="lNowTddwID"></param>
        /// <returns></returns>
        private long GetTddwID(DataRow dr, long lNowTddwID)
        {
            long lReturn = GetMainListID(TDDW_list, GetOneStrValue(dr, "TDDW", CosDataBase.LONGINT_TYPE));
            if (lReturn != 0)
                return lReturn;
            else
                return lNowTddwID;
        }

        /// <summary>
        /// 得到学制代码的实际ID
        /// 17表中应该是直接学制代码的编号，12表看起来是数字？
        /// 认为学制年限表的代码必须是1、2、3、4……这样
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetXzdm(DataRow dr)
        {
            string sReturn = GetOneStrValue(dr, "XZDM", CosDataBase.LONGINT_TYPE);
            if (sReturn == "0")
            {
                sReturn = GetOneStrValue(dr, "XZNX", CosDataBase.INT_TYPE);
                return sReturn;
            }
            else
                return sReturn;
        }
        private Td_zydh getOneMainRecord(long lParentDepartmentID, Td_zydh_Origin orgModel)
        {
            Expression<Func<Td_zydh, bool>> where = (c => c.KldmOrigin == orgModel.Kldm && c.ZylbdmOrigin == orgModel.Zylbdm && c.XzdmOrigin == orgModel.Xzdm && c.ParentDepartmentID == lParentDepartmentID && c.Code == orgModel.DM);
            List<Td_zydh> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        #endregion

        public object GetAllZyStore(string GxID, string ZYIDs, int nTypeID = -1)
        {
            long lGxID = Convert.ToInt64(GxID);
            Expression<Func<Td_zydh, bool>> where = (c => c.GxID == lGxID && c.DelFlag == false);
            List<Td_zydh> list = GetRecord(where);
            long[] lZYIDs;
            if (ZYIDs != "")
                lZYIDs = Array.ConvertAll(ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
            else
                lZYIDs = new long[0];
            if (list != null)
            {
                int countNum = list.Count - lZYIDs.Length;
                if (countNum > 0)
                {
                    object[] data = new object[countNum];
                    int j = 0;
                    foreach (Td_zydh m in list)
                    {
                        if (!lZYIDs.Contains(m.ID))
                        {
                            if (nTypeID == -1)
                            {
                                object[] dataline = new object[2];
                                dataline[0] = m.ID;
                                dataline[1] = m.Name;
                                data[j] = dataline;
                            }
                            else
                            {
                                object[] dataline = new object[3];
                                dataline[0] = nTypeID;
                                dataline[1] = m.ID;
                                dataline[2] = m.Name;
                                data[j] = dataline;
                            }
                            j++;
                        }
                    }
                    //return new Paging<object>(data, countNum);
                    return data;
                }
            }
            object[] data0 = new object[0];
            //return new Paging<object>(data0, 0);
            return data0;
        }


        public Paging<object> GetSelZyStore(string GxID, string ZYIDs)
        {
            long lGxID = Convert.ToInt64(GxID);
            Expression<Func<Td_zydh, bool>> where = (c => c.GxID == lGxID && c.DelFlag == false);
            List<Td_zydh> list = GetRecord(where);
            long[] lZYIDs;
            if (ZYIDs != "")
                lZYIDs = Array.ConvertAll(ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
            else
                lZYIDs = new long[0];
            if (list != null)
            {
                int countNum = lZYIDs.Length;
                if (countNum > 0)
                {
                    object[] data = new object[countNum];
                    int j = 0;
                    foreach (Td_zydh m in list)
                    {
                        if (lZYIDs.Contains(m.ID))
                        {
                            object[] dataline = new object[2];
                            dataline[0] = m.ID;
                            dataline[1] = m.Name;
                            data[j] = dataline;
                            j++;
                        }
                    }
                    return new Paging<object>(data, countNum);
                }
            }
            object[] data0 = new object[0];
            return new Paging<object>(data0, 0);
        }


        public List<Td_zydh_Origin> InportSomeRowsOriginTable(DataTable dt, int nYear, long lProvinceID, long lParentDepartmentID = 0)
        {
            List<Td_zydh_Origin> returnArr = new List<Td_zydh_Origin>();
            List<Td_zydh_Origin> addArr = new List<Td_zydh_Origin>();
            string dm = "", mc = "";
            foreach (DataRow dr in dt.Rows)
            {
                dm = dr["ZYDH"].ToString();
                mc = dr["ZYMC"].ToString();

                ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
                names.Add("DelFlag"); values.Add(false);
                names.Add("YearCode"); values.Add(nYear);
                names.Add("ProvinceID"); values.Add(lProvinceID);
                names.Add("DM"); values.Add(dm);
                names.Add("MC"); values.Add(mc);
                names.Add("ParentDepartmentID"); values.Add(lParentDepartmentID);

                Expression<Func<Td_zydh_Origin, bool>> where = GetAndLambdaExpression<Td_zydh_Origin>(names, values);
                List<Td_zydh_Origin> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    returnArr.Add(list[0]);
                else
                {
                    Td_zydh_Origin model = new Td_zydh_Origin();
                    model.YearCode = nYear;
                    model.ProvinceID = lProvinceID;
                    model.DM = dm;
                    model.MC = mc;
                    model.RelationID = 0;
                    model.ParentDepartmentID = lParentDepartmentID;
                    model.Kldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM"));
                    model.Zkfx = GetOneStrValue(dr, "ZKFX");
                    model.Tjsxbz = GetOneStrValue(dr, "TJSXBZ");
                    model.Xzdm = GetOriginListID(XZDM_list, GetOneStrValue(dr, "XZDM"));
                    model.Sfbz = Convert.ToInt32(GetOneStrValue(dr, "SFBZ", CosDataBase.INT_TYPE));
                    model.Bxdd = GetOneStrValue(dr, "BXDD");
                    model.Zylbdm = GetOriginListID(ZYLBDM_list, GetOneStrValue(dr, "ZYLBDM"));
                    model.Ccdm = GetOriginListID(ZYLBDM_list, GetOneStrValue(dr, "ZYLBDM"));
                    model.Bz = GetOneStrValue(dr, "BZ");
                    addArr.Add(model);
                    returnArr.Add(model);
                }
            }
            if ((addArr != null) && (addArr.Count > 0))
                AddRecords(addArr);
            return returnArr;
        }
        public Td_zydh_Origin InportOneRowOriginTable(DataRow dr, int nYear, long lProvinceID, long lParentDepartmentID = 0)
        {
            string dm = "", mc = "";
            dm = dr["ZYDH"].ToString();
            mc = dr["ZYMC"].ToString();

            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("DelFlag"); values.Add(false);
            names.Add("YearCode"); values.Add(nYear);
            names.Add("ProvinceID"); values.Add(lProvinceID);
            names.Add("DM"); values.Add(dm);
            names.Add("MC"); values.Add(mc);
            names.Add("ParentDepartmentID"); values.Add(lParentDepartmentID); 

            Expression<Func<Td_zydh_Origin, bool>> where = GetAndLambdaExpression<Td_zydh_Origin>(names, values);
            List<Td_zydh_Origin> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
            {
                Td_zydh_Origin model = new Td_zydh_Origin();
                model.YearCode = nYear;
                model.ProvinceID = lProvinceID;
                model.DM = dm;
                model.MC = mc;
                model.RelationID = 0;
                model.ParentDepartmentID = lParentDepartmentID;
                model.Kldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM"));
                model.Zkfx = GetOneStrValue(dr, "ZKFX");
                model.Tjsxbz = GetOneStrValue(dr, "TJSXBZ");
                model.Xzdm = GetOriginListID(XZDM_list, GetOneStrValue(dr, "XZDM"));
                model.Sfbz = Convert.ToInt32(GetOneStrValue(dr, "SFBZ", CosDataBase.INT_TYPE));
                model.Bxdd = GetOneStrValue(dr, "BXDD");
                model.Zylbdm = GetOriginListID(ZYLBDM_list, GetOneStrValue(dr, "ZYLBDM"));
                model.Ccdm = GetOriginListID(ZYLBDM_list, GetOneStrValue(dr, "ZYLBDM"));
                model.Bz = GetOneStrValue(dr, "BZ");
                long lNewID = AddOneRecord(model);
                model.ID = lNewID;
                return model;
            }
        }

       

        public List<ListCompareTable> InportOriginData(DataTable dt, int nNowYear, long lProvinceID, long lParentDepartmentID, long GxID)
        {
            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            if (dt != null)
            {
                List<Td_zydh_Origin> OriginModels = InportSomeRowsOriginTable<Td_zydh_Origin>(dt, nNowYear, lProvinceID);
                TableStatusManage statusManage = new TableStatusManage();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //先导入原始表
                    long lNowRelationID = Convert.ToInt64(OriginModels[i].RelationID);
                    if (lNowRelationID != 0) //原始表中已经存在，直接取用
                    {
                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = dt.Rows[i]["ZYDH"].ToString();
                        oneTable.lNewID = OriginModels[i].RelationID;
                        oneTable.lOriginID = OriginModels[i].ID;
                        listReturn.Add(oneTable);
                    }
                    else
                    {
                        long lNewID = 0;                        
                        Td_zydh obj = getOneMainRecord(lParentDepartmentID, OriginModels[i]);
                        if (obj == null) //目前主表中不存在
                        {
                            while (!statusManage.BeginUpdate<Td_zydh>())
                            {
                                Random ra = new Random();
                                Thread.Sleep(ra.Next(100, 1000));
                            }
                            obj = getOneMainRecord(lParentDepartmentID, OriginModels[i]);
                            if (obj == null)  //还是不存在
                            {
                                Td_zydh model = new Td_zydh();
                                model.Name = GetOneStrValue(dt.Rows[i], "ZYMC");
                                model.Code = GetOneStrValue(dt.Rows[i], "ZYDH");
                                model.Remark = GetOneStrValue(dt.Rows[i], "BZ");

                                ListCompareTable oneCompare = GetCompareList(CCDM_list, GetOneStrValue(dt.Rows[i], "CCDM", CosDataBase.LONGINT_TYPE));
                                if (oneCompare != null)
                                {
                                    model.CcdmOrigin = oneCompare.lOriginID;
                                    model.Ccdm = oneCompare.lNewID;
                                }

                                oneCompare = GetCompareList(KLDM_list, GetOneStrValue(dt.Rows[i], "KLDM", CosDataBase.LONGINT_TYPE));
                                if (oneCompare != null)
                                {
                                    model.KldmOrigin = oneCompare.lOriginID;
                                    model.Kldm = oneCompare.lNewID;
                                }

                                string sXzdm = GetXzdm(dt.Rows[i]);
                                oneCompare = GetCompareList(XZDM_list, sXzdm);
                                if (oneCompare != null)
                                {
                                    model.XzdmOrigin = oneCompare.lOriginID;
                                    model.Xzdm = oneCompare.lNewID;
                                }

                                oneCompare = GetCompareList(ZYLBDM_list, GetOneStrValue(dt.Rows[i], "ZYLBDM", CosDataBase.LONGINT_TYPE));
                                if (oneCompare != null)
                                {
                                    model.ZylbdmOrigin = oneCompare.lOriginID;
                                    model.Zylbdm = oneCompare.lNewID;
                                }

                                model.Zkfx = GetOneStrValue(dt.Rows[i], "ZKFX");
                                model.Tjsxbz = GetOneStrValue(dt.Rows[i], "TJSXBZ");
                                model.Sfks = Convert.ToBoolean(GetOneStrValue(dt.Rows[i], "SFKZ", CosDataBase.BOOLEAN_TYPE));
                                model.Sfbz = Convert.ToInt32(GetOneStrValue(dt.Rows[i], "Sfbz", CosDataBase.INT_TYPE));
                                model.Bxdd = GetOneStrValue(dt.Rows[i], "BXDD");
                                model.Wyyz = GetOneStrValue(dt.Rows[i], "WYYZ");
                                model.ParentDepartmentID = lParentDepartmentID;
                                model.GxID = GxID;
                                model.JoinNames = model.Name;
                                string[] EditColumns = { "Name", "Code", "Remark", "CcdmOrigin", "Ccdm", "KldmOrigin", "Kldm",
                                    "XzdmOrigin", "Xzdm", "ZylbdmOrigin", "Zylbdm", "Zkfx", "Tjsxbz", "Sfks", "Sfbz", "Bxdd", "Wyyz", "ParentDepartmentID", "GxID", "JoinNames" };
                                lNewID = SaveOneSimpleRecord(model, "zydhTable", EditColumns);
                                statusManage.EndUpdate<Td_zydh>();
                                ListCompareTable oneTable = new ListCompareTable();
                                oneTable.sOldID = dt.Rows[i]["ZYDH"].ToString();
                                oneTable.lNewID = lNewID;
                                oneTable.lOriginID = OriginModels[i].ID;
                                listReturn.Add(oneTable);
                            }
                            else //目前主表中存在
                            {
                                if (obj.DelFlag)
                                    lNewID = obj.JoinID;
                                else
                                    lNewID = obj.ID;

                                ListCompareTable oneTable = new ListCompareTable();
                                oneTable.sOldID = dt.Rows[i]["ZYDH"].ToString();
                                oneTable.lNewID = lNewID;
                                oneTable.lOriginID = OriginModels[i].ID;
                                listReturn.Add(oneTable);
                            }
                        }
                        else //目前主表中存在
                        {
                            if (obj.DelFlag)
                                lNewID = obj.JoinID;
                            else
                                lNewID = obj.ID;

                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = dt.Rows[i]["ZYDH"].ToString();
                            oneTable.lNewID = lNewID;
                            oneTable.lOriginID = OriginModels[i].ID;
                            listReturn.Add(oneTable);
                        }
                        ////不管存在与否，都需修改原始表中相应的RelationID
                        string[] EditColumn = { "RelationID" };
                        OriginModels[i].RelationID = lNewID;
                        SaveOneRecord(OriginModels[i], EditColumn);
                    }
                }
            }
            return listReturn;
        }
    }
}