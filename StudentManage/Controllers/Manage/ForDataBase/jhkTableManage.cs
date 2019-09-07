using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.OleDb;
using System;
using System.Linq;
using System.Collections;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class jhkTableManage : OriginTableManage
    {
        public List<ListCompareTable> PCDM_list;
        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> JHXZDM_list;
        public List<ListCompareTable> JHLBDM_list;
        public List<ListCompareTable> TDDW_list;
        public List<ListCompareTable> ZYDH_list;
        public List<ListCompareTable> ZKLXDM_list;
        public List<ListCompareTable> ZYLBDM_list;
        public List<ListCompareTable> XZDM_list;
        public jhkTableManage()
        {
            Manage_KeyWord = "jhkTable";
        }


        #region 专业代号
        /// <summary>
        /// 处理专业类别信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nYear"></param>
        /// <param name="lProvinceID"></param>
        private void BuildZYLB(DataTable dt, int nYear, long lProvinceID)
        {
            DataView dataView = dt.DefaultView;
            DataTable dt1 = dataView.ToTable(true, "ZYDM", "ZYMC");
            dt1.Columns["ZYDM"].ColumnName = "ZYLBDM"; //和17表保持一致，改名
            dt1.Columns["ZYMC"].ColumnName = "ZYLBMC";
            ZYLBDM_list = (new zylbdmTableManage()).InportOriginData(dt1, nYear, lProvinceID);
        }


        /// <summary>
        /// 专业代号，12库来自TDD，17库有单独的基础表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nYear"></param>
        /// <param name="lProvinceID"></param>
        private void BuildZYDH(DataTable dt, int nYear, long lProvinceID, long lParentDepartmentID, long lGxID, out List<ListCompareTable> old12ZYDH_list)
        {
            //检查是12库还是17库
            old12ZYDH_list = new List<ListCompareTable>();
            bool flag = false;
            int i = 0;
            while ((i < dt.Columns.Count) && !flag)
            {
                if (dt.Columns[i].ColumnName == "ZYMC")  //12库才可能在计划库表中存在ZYMC
                    flag = true;
                else
                    i++;
            }
            if (flag)
            {
                //创建专业类别表
                BuildZYLB(dt, nYear, lProvinceID);
                //根据dt创建专业代号相关的新dt
                DataView dataView = dt.DefaultView;
                DataTable dt1;
                if (dt.Columns.Contains("TJSXBZ"))
                    dt1 = dataView.ToTable(true, "KLDM", "ZYDH", "ZYDM", "ZYMC", "ZYLB", "TJSXBZ", "XZNX", "TDDW");
                else
                    dt1 = dataView.ToTable(true, "KLDM", "ZYDH", "ZYDM", "ZYMC", "ZYLB", "XZNX", "TDDW");

                zydhTableManage zydhMan = new zydhTableManage();
                zydhMan.KLDM_list = KLDM_list;
                zydhMan.ZYLBDM_list = ZYLBDM_list;
                zydhMan.TDDW_list = TDDW_list;
                zydhMan.XZDM_list = XZDM_list;
                dt1.Columns["ZYDM"].ColumnName = "ZYLBDM"; //专业代码对应到专业类别代码，ZYLB不要了

                ZYDH_list = zydhMan.InportOriginData(dt1, nYear, lProvinceID, lParentDepartmentID, lGxID);
                old12ZYDH_list = ZYDH_list;
            }
        }
        #endregion

        /// <summary>
        /// 这个函数可能还有问题
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool ExistJH(DataRow dr, List<T_jhk> list)
        {
            if ((list != null) && (list.Count > 0))
            {
                long Pcdm = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                long Kldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                long Jhxzdm = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                long Jhlbdm = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));
                long Tddwdm = GetOriginListID(TDDW_list, GetOneStrValue(dr, "TDDWDM", CosDataBase.LONGINT_TYPE));
                //long Zydh = GetMainListID(ZYDH_list, GetOneStrValue(dr, "ZYDH", CosDataBase.LONGINT_TYPE));
                //long Zklxdm = GetMainListID(ZKLXDM_list, GetOneStrValue(dr, "ZKLXDM", CosDataBase.LONGINT_TYPE));
                //list.Where(c => c.Name == dr["KLMC"].ToString()).Select(c => c.ID).ToArray()[0]
                var sel = list.Where(c => c.PcdmOrigin == Pcdm && c.JhxzdmOrigin == Jhxzdm && c.JhlbdmOrigin == Jhlbdm && c.KldmOrigin == Kldm && c.TddwdmOrigin == Tddwdm);
                if (sel != null)
                    return true;
                else
                    return false;
            }
            else
                return false;            
        }

        /// <summary>
        /// 投档单位代码不能只从ID上比较，存在投档单位代码相同的记录
        /// </summary>
        /// <returns></returns>
        private long GetOriginTddw(DataRow dr)
        {
            string sPcdm = VerifyStr(GetOneStrValue(dr, "PCDM"));
            string sKldm = VerifyStr(GetOneStrValue(dr, "KLDM"));
            string sJhxzdm = VerifyStr(GetOneStrValue(dr, "JHXZDM"));
            string sJhlbdm = VerifyStr(GetOneStrValue(dr, "JHLBDM"));
            string sTddwdm = VerifyStr(GetOneStrValue(dr, "TDDWDM"));

            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList();
            if (sPcdm != "0") { arrNames.Add("pc.DM"); arrValues.Add(sPcdm); }
            if (sKldm != "0") { arrNames.Add("kl.DM"); arrValues.Add(sKldm); }
            if (sJhxzdm != "0") { arrNames.Add("jhxz.DM"); arrValues.Add(sJhxzdm); }
            if (sJhlbdm != "0") { arrNames.Add("jhlb.DM"); arrValues.Add(sJhlbdm); }
            if (sTddwdm != "0") { arrNames.Add("DM"); arrValues.Add(sTddwdm); }
            Expression<Func<T_tddw_Origin, bool>> where = GetAndLambdaExpression<T_tddw_Origin>(arrNames, arrValues);
            List<T_tddw_Origin> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].ID;
            else
                return 0;            
        }

        private string VerifyStr(string STR)
        {
            if (STR == "")
                return "0";
            else
                return STR;
        }

        /// <summary>
        /// 投档单位代码不能只从ID上比较，存在投档单位代码相同的记录
        /// </summary>
        /// <returns></returns>
        private long GetTddw(DataRow dr)
        {
            string sPcdm = VerifyStr(GetOneStrValue(dr, "PCDM"));
            string sKldm = VerifyStr(GetOneStrValue(dr, "KLDM"));
            string sJhxzdm = VerifyStr(GetOneStrValue(dr, "JHXZDM"));
            string sJhlbdm = VerifyStr(GetOneStrValue(dr, "JHLBDM"));
            string sTddwdm = VerifyStr(GetOneStrValue(dr, "TDDWDM"));

            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList();
            if (sPcdm != "0") { arrNames.Add("pc.DM"); arrValues.Add(sPcdm); }
            if (sKldm != "0") { arrNames.Add("kl.DM"); arrValues.Add(sKldm); }
            if (sJhxzdm != "0") { arrNames.Add("jhxz.DM"); arrValues.Add(sJhxzdm); }
            if (sJhlbdm != "0") { arrNames.Add("jhlb.DM"); arrValues.Add(sJhlbdm); }
            if (sTddwdm != "0") { arrNames.Add("DM"); arrValues.Add(sTddwdm); }
            Expression<Func<T_tddw_Origin, bool>> where = GetAndLambdaExpression<T_tddw_Origin>(arrNames, arrValues);
            List<T_tddw_Origin> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].RelationID;
            else 
                return 0;
        }

        public List<T_jhk> InportOriginData(DataTable dt, int nNowYear, long lProvinceID, long lParentDepartmentID, long lGxID, out List<ListCompareTable> old12ZYDH_list)
        {
            BuildZYDH(dt, nNowYear, lProvinceID, lParentDepartmentID, lGxID, out old12ZYDH_list);
            if (dt.Columns.Contains("JHXZ"))
                dt.Columns["JHXZ"].ColumnName = "JHXZDM"; //和17表保持一致，改名
            if (dt.Columns.Contains("TDDW"))
                dt.Columns["TDDW"].ColumnName = "TDDWDM"; //和17表保持一致，改名
            

            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("DelFlag"); values.Add(false);
            names.Add("ProvinceID"); values.Add(lProvinceID);
            names.Add("YearCode"); values.Add(nNowYear);
            //names.Add("ParentDepartmentID"); values.Add(lParentDepartmentID);
            names.Add("GxID"); values.Add(lGxID);
            Expression<Func<T_jhk, bool>> where = GetAndLambdaExpression<T_jhk>(names, values);
            List<T_jhk> list = GetRecord(where); //取出主表中相关所有字段
            List<T_jhk> listAdd = new List<T_jhk>();
            foreach (DataRow dr in dt.Rows)
            {
                if (!ExistJH(dr, list)) //如果是不存在的计划
                {
                    T_jhk model = new T_jhk();
                    model.ProvinceID = lProvinceID;
                    model.YearCode = nNowYear;
                    model.ParentDepartmentID = lParentDepartmentID;
                    model.GxID = lGxID;
                    model.PcdmOrigin = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                    //model.Pcdm = GetMainListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                    model.KldmOrigin = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                    //model.Jhxzdm = GetMainListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                    model.JhxzdmOrigin = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                    //model.Jhlbdm = GetMainListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));
                    model.JhlbdmOrigin = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));

                    model.TddwdmOrigin = GetOriginTddw(dr);
                    model.Tddwdm = GetTddw(dr);

                    ListCompareTable oneCompare = GetCompareList(ZKLXDM_list, GetOneStrValue(dr, "ZKLX", CosDataBase.LONGINT_TYPE));
                    if (oneCompare != null)
                    {
                        model.ZklxdmOrigin = oneCompare.lOriginID;
                        model.Zklxdm = oneCompare.lNewID;
                    }
                    oneCompare = GetCompareList(ZYDH_list, GetOneStrValue(dr, "ZYDH"));
                    if (oneCompare != null)
                    {
                        model.ZydhOrigin = oneCompare.lOriginID;
                        model.Zydh = oneCompare.lNewID;
                    }
                    model.Jhrs = Convert.ToInt32(GetOneStrValue(dr, "JHRS", CosDataBase.INT_TYPE));
                    model.Jhzxs = Convert.ToInt32(GetOneStrValue(dr, "JHZXS", CosDataBase.INT_TYPE));
                    model.Sfsf = Convert.ToBoolean(GetOneStrValue(dr, "SFSF", CosDataBase.BOOLEAN_TYPE));
                    model.Ytces = Convert.ToInt32(GetOneStrValue(dr, "YTCES", CosDataBase.INT_TYPE));
                    model.Yxxh = Convert.ToInt32(GetOneStrValue(dr, "YXXH", CosDataBase.INT_TYPE));
                    model.Bz = GetOneStrValue(dr, "BZ");
                    listAdd.Add(model);
                    //AddOneRecord(model);
                }
            }
            AddRecords(listAdd);
            list.AddRange(listAdd);
            return list;
        }


        public List<T_jhk> GetJhByZydh(long ZyID)
        {
            zydhTableManage zyMan = new zydhTableManage();
            Expression<Func<T_jhk, bool>> where = (c => c.Zydh == ZyID && c.DelFlag == false);
            List<T_jhk> list = zyMan.GetRecord(where);
            if (list.Count != 0)
                return list;
            else
                return null;
        }
    }
}