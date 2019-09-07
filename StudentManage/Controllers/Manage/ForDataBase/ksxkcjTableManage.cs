using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class ksxkcjTableManage : OriginTableManage
    {
        public List<ListCompareTable> KMDM_list;
        public List<ListCompareTable> CJXDM_list;
        public List<BMKTable> BMK_list;
        public ksxkcjTableManage()
        {
            Manage_KeyWord = "ksxkcjTable";
        }


        /// <summary>
        /// 增加记录
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public void AddNewCj(DataTable dt, bool IsOldData)
        {
            List<T_ksxkcj> listAdd = new List<T_ksxkcj>();
            foreach (DataRow dr in dt.Rows)
            { 
                double dDdcj = Convert.ToDouble(GetOneStrValue(dr, "DDCJ", CosDataBase.FLOAT_TYPE));
                if (dDdcj > 0)
                {                    
                    bmkTableManage bmkMan = new bmkTableManage();
                    //long lbmkID = bmkMan.GetbmkID(dr["KSH"].ToString());
                    long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                    if (lbmkID != 0)
                    {
                        long kmdmOrigin = GetOriginListID(KMDM_list, GetOneStrValue(dr, "KMDM", CosDataBase.LONGINT_TYPE));
                        long cjxdmOrigin = GetOriginListID(CJXDM_list, GetOneStrValue(dr, "CJXDM", CosDataBase.LONGINT_TYPE));
                        short cjlx = 1;
                        if (!IsOldData)
                            cjlx = Convert.ToInt16(GetOneStrValue(dr, "CJLX", CosDataBase.SMALLINT_TYPE));
                        //Expression<Func<T_ksxkcj, bool>> where = (c => c.RelationID == lbmkID && c.KmdmOrigin == kmdmOrigin && c.CjxdmOrigin == cjxdmOrigin && c.Cjlx == cjlx);
                        //List<T_ksxkcj> list = GetRecord(where);
                        //if ((list == null) || (list.Count == 0))
                        //{
                            T_ksxkcj ksxkcj_model = new T_ksxkcj();
                            ksxkcj_model.RelationID = lbmkID;
                            ksxkcj_model.Xkbmh = GetOneStrValue(dr, "XKBMH");
                            ksxkcj_model.Kmdm = GetMainListID(KMDM_list, GetOneStrValue(dr, "KMDM", CosDataBase.LONGINT_TYPE));
                            ksxkcj_model.KmdmOrigin = kmdmOrigin;
                            ksxkcj_model.Cjxdm = GetMainListID(CJXDM_list, GetOneStrValue(dr, "CJXDM", CosDataBase.LONGINT_TYPE));
                            ksxkcj_model.CjxdmOrigin = cjxdmOrigin;
                            ksxkcj_model.Cjlx = cjlx;
                            ksxkcj_model.Ksny = Convert.ToDateTime(GetOneStrValue(dr, "KSNY", CosDataBase.DATETIME_TYPE));
                            ksxkcj_model.Ddcj = dDdcj;
                            ksxkcj_model.Djff = Convert.ToDouble(GetOneStrValue(dr, "DJFF", CosDataBase.FLOAT_TYPE));
                            listAdd.Add(ksxkcj_model);
                        //}
                    }
                }
            }
            AddRecords(listAdd);
        }

        public List<T_ksxkcj> GetOneRecordbyRelationID(long lID, short type = -1)
        {
            if (type != -1)
            {
                Expression<Func<T_ksxkcj, bool>> where = (c => c.RelationID == lID && c.Cjlx == type);
                List<T_ksxkcj> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list;
            }
            else
            {
                Expression<Func<T_ksxkcj, bool>> where = (c => c.RelationID == lID);
                List<T_ksxkcj> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    return list;
            }
            return null;
        }

        /// <summary>
        /// 检查当前库是否为2012库
        /// </summary>
        /// <param name="dr"></param>
        private bool VeiryOldData(DataTable dt)
        {
            //检查是12库还是17库
            bool flag = false;
            int i = 0;
            while ((i < dt.Columns.Count) && !flag)
            {
                if (dt.Columns[i].ColumnName == "CJ")  //12库才可能有CJ字段
                    flag = true;
                else
                    i++;
            }
            return flag;
        }
        /// <summary>
        /// 直接导入，来自会考/学考成绩项表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nYear"></param>
        /// <param name="lProvinceID"></param>
        public void InportOriginData(DataTable dt, int nYear, long lProvinceID)
        {
            if (dt != null)
            {
                bool IsOldData = VeiryOldData(dt); //检查是不是12库
                if (IsOldData)
                    dt.Columns["CJ"].ColumnName = "DQCJ"; //和17表保持一致，改名

                //foreach (DataRow dr in dt.Rows)
                AddNewCj(dt, IsOldData);
            }            
        }
    }
}