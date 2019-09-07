using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.OleDb;
using System;
using System.Linq;
using System.Collections;
using System.Threading;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class tddwTableManage : OriginTableManage
    {
        public List<ListCompareTable> PCDM_list;
        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> JHXZDM_list;
        public List<ListCompareTable> JHLBDM_list;

        public tddwTableManage()
        {
            Manage_KeyWord = "tddwTable";
        }

        /// <summary>
        /// 从高校代码表中找到可能的高校代码
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        //private long GetGxID(string sName = null)
        //{
        //    if (sName != null)
        //    {
        //        ArrayList names = new ArrayList();
        //        ArrayList values = new ArrayList();
        //        ArrayList signs = new ArrayList();
        //        names.Add("DelFlag"); values.Add(false); signs.Add(SIGN.Equal);
        //        //names.Add("Name"); values.Add(sName); signs.Add(SIGN.AntiLike);
        //        Expression<Func<Td_gxdm, bool>> where = GetAndLambdaExpression<Td_gxdm>(names, values, signs);
        //        List<Td_gxdm> list = GetRecord(where);
        //        if ((list != null) && (list.Count > 0))
        //        {
        //            foreach (Td_gxdm model in list)
        //            {
        //                if (sName.Contains(model.Name))
        //                    return model.ID;
        //            }
        //        }
        //        return 0;
        //    }
        //    else
        //    {
        //        string[] key = { CookieKeys.USERGXID };
        //        return Convert.ToInt64((new UserInfoManage()).GetLoginUserInfo(key)[0]);
        //    }
        //}


        private List<T_tddw_Origin> InportSomeTddwOriginTable(DataTable dt, int nYear, long lProvinceID)
        {
            List<T_tddw_Origin> returnArr = new List<T_tddw_Origin>();
            List<T_tddw_Origin> addArr = new List<T_tddw_Origin>();
            foreach (DataRow dr in dt.Rows)
            {
                string dm = "", mc = "";
                dm = dr["TDDWDM"].ToString();
                mc = dr["TDDWMC"].ToString();

                ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
                names.Add("DelFlag"); values.Add(false);
                names.Add("YearCode"); values.Add(nYear);
                names.Add("ProvinceID"); values.Add(lProvinceID);
                names.Add("DM"); values.Add(dm);
                names.Add("MC"); values.Add(mc);

                Expression<Func<T_tddw_Origin, bool>> where = GetAndLambdaExpression<T_tddw_Origin>(names, values);
                List<T_tddw_Origin> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    returnArr.Add(list[0]);
                else
                {
                    T_tddw_Origin model = new T_tddw_Origin();
                    model.YearCode = nYear;
                    model.ProvinceID = lProvinceID;
                    model.DM = dm;
                    model.MC = mc;
                    model.RelationID = 0;
                    model.Kldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                    model.Pcdm = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                    model.Jhxzdm = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                    model.Jhlbdm = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));
                    model.Csmbh = GetOneStrValue(dr, "CSMBH");
                    model.Tag = GetOneStrValue(dr, "TAG");
                    addArr.Add(model);
                    returnArr.Add(model);
                }
            }
            if ((addArr != null) && (addArr.Count > 0))
                AddRecords(addArr);
            return returnArr;
        }

        private T_tddw_Origin InportOneTddwOriginTable(DataRow dr, int nYear, long lProvinceID)
        {
            string dm = "", mc = "";
            dm = dr["TDDWDM"].ToString();
            mc = dr["TDDWMC"].ToString();

            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("DelFlag"); values.Add(false);
            names.Add("YearCode"); values.Add(nYear);
            names.Add("ProvinceID"); values.Add(lProvinceID);
            names.Add("DM"); values.Add(dm);
            names.Add("MC"); values.Add(mc);

            Expression<Func<T_tddw_Origin, bool>> where = GetAndLambdaExpression<T_tddw_Origin>(names, values);
            List<T_tddw_Origin> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
            {
                T_tddw_Origin model = new T_tddw_Origin();
                model.YearCode = nYear;
                model.ProvinceID = lProvinceID;
                model.DM = dm;
                model.MC = mc;
                model.RelationID = 0;
                model.Kldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                model.Pcdm = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                model.Jhxzdm = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                model.Jhlbdm = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));
                model.Csmbh = GetOneStrValue(dr, "CSMBH");
                model.Tag = GetOneStrValue(dr, "TAG");
                long lNewID = AddOneRecord(model);
                model.ID = lNewID;                
                return model;
            }
        }

        private T_tddw getOneMainRecord(long lGxID, T_tddw_Origin orgModel)
        {
            Expression<Func<T_tddw, bool>> where = (c=>c.KldmOrigin == orgModel.Kldm && c.PcdmOrigin == orgModel.Pcdm && c.JhxzdmOrigin == orgModel.Jhxzdm && c.JhlbdmOrigin == orgModel.Jhlbdm && c.GxID == lGxID && c.Code == orgModel.DM);
            List<T_tddw> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        public List<ListCompareTable> InportOriginData(DataTable dt, int nNowYear, long lProvinceID, long lGxID)
        {
            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            List<T_tddw_Origin> OriginModels = InportSomeTddwOriginTable(dt, nNowYear, lProvinceID);
            TableStatusManage statusManage = new TableStatusManage();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //T_tddw_Origin OriginModel = InportOneTddwOriginTable(dr, nNowYear, lProvinceID);
                long lNowRelationID = Convert.ToInt64(OriginModels[i].RelationID);
                if (lNowRelationID != 0) //原始表中已经存在，直接取用
                {
                    ListCompareTable oneTable = new ListCompareTable();
                    oneTable.sOldID = dt.Rows[i]["TDDWDM"].ToString();
                    oneTable.lNewID = lNowRelationID;
                    oneTable.lOriginID = OriginModels[i].ID;
                    listReturn.Add(oneTable);
                }
                else
                {
                    long lNewID = 0;
                    T_tddw obj = getOneMainRecord(lGxID, OriginModels[i]);
                    string MC = GetOneStrValue(dt.Rows[i], "TDDWMC");
                    if (obj == null) //目前主表中不存在
                    {
                        while (!statusManage.BeginUpdate<T_tddw>())
                        {
                            Random ra = new Random();
                            Thread.Sleep(ra.Next(100, 1000));
                        }
                        obj = getOneMainRecord(lGxID, OriginModels[i]);
                        if (obj == null)  //还是不存在
                        {
                            T_tddw model = new T_tddw();
                            model.Name = MC;
                            model.Code = GetOneStrValue(dt.Rows[i], "TDDWDM");
                            model.Remark = GetOneStrValue(dt.Rows[i], "BZ");

                            ListCompareTable oneCompare = GetCompareList(PCDM_list, GetOneStrValue(dt.Rows[i], "PCDM", CosDataBase.LONGINT_TYPE));
                            if (oneCompare != null)
                            {
                                model.PcdmOrigin = oneCompare.lOriginID;
                                model.Pcdm = oneCompare.lNewID;
                            }

                            oneCompare = GetCompareList(KLDM_list, GetOneStrValue(dt.Rows[i], "KLDM", CosDataBase.LONGINT_TYPE));
                            if (oneCompare != null)
                            {
                                model.KldmOrigin = oneCompare.lOriginID;
                                model.Kldm = oneCompare.lNewID;
                            }

                            oneCompare = GetCompareList(JHXZDM_list, GetOneStrValue(dt.Rows[i], "JHXZDM", CosDataBase.LONGINT_TYPE));
                            if (oneCompare != null)
                            {
                                model.JhxzdmOrigin = oneCompare.lOriginID;
                                model.Jhxzdm = oneCompare.lNewID;
                            }

                            oneCompare = GetCompareList(JHLBDM_list, GetOneStrValue(dt.Rows[i], "JHLBDM", CosDataBase.LONGINT_TYPE));
                            if (oneCompare != null)
                            {
                                model.JhlbdmOrigin = oneCompare.lOriginID;
                                model.Jhlbdm = oneCompare.lNewID;
                            }

                            model.Csmbh = GetOneStrValue(dt.Rows[i], "CSMBH");
                            model.Tag = GetOneStrValue(dt.Rows[i], "TAG");
                            model.GxID = lGxID;

                            model.JoinNames = model.Name;

                            string[] EditColumns = { "Name", "Code", "Remark", "PcdmOrigin", "Pcdm", "KldmOrigin", "Kldm",
                            "JhxzdmOrigin", "Jhxzdm", "JhlbdmOrigin", "Jhlbdm", "Csmbh", "Tag", "GxID" , "JoinNames"};

                            lNewID = SaveOneSimpleRecord(model, "tddwTable", EditColumns);
                            statusManage.EndUpdate<T_tddw>();
                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = GetOneStrValue(dt.Rows[i], "TDDWDM");
                            oneTable.lNewID = lNewID;
                            oneTable.lOriginID = OriginModels[i].ID;
                            listReturn.Add(oneTable);
                        }
                        else
                        {
                            if (obj.DelFlag)
                                lNewID = obj.JoinID;
                            else
                                lNewID = obj.ID;
                            ListCompareTable oneTable = new ListCompareTable();
                            oneTable.sOldID = GetOneStrValue(dt.Rows[i], "TDDWDM");
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
                        oneTable.sOldID = GetOneStrValue(dt.Rows[i], "TDDWDM");
                        oneTable.lNewID = lNewID;
                        oneTable.lOriginID = OriginModels[i].ID;
                        listReturn.Add(oneTable);
                    }
                    ////不管存在与否，都需修改原始表中相应的RelationID
                    string[] EditColumn = { "RelationID" };
                    OriginModels[i].GetType().GetProperty("RelationID").SetValue(OriginModels[i], lNewID, null);
                    SaveOneRecord(OriginModels[i], EditColumn);
                }
            }
            return listReturn;
        }
    }
}