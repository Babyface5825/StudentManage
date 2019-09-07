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
    public class ksjlTableManage : OriginTableManage
    {
        public List<BMKTable> BMK_list;
        public ksjlTableManage()
        {
            Manage_KeyWord = "ksjlTable";
        }

        public void InportOriginData(DataTable dt, int nYear, long lProvinceID)
        {
            bmkTableManage bmkMan = new bmkTableManage();
            List<T_ksjl> listAdd = new List<T_ksjl>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //long lbmkID = bmkMan.GetbmkID(dr["KSH"].ToString());
                    long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                    if (lbmkID != 0)
                    {
                        DateTime qsrq = Convert.ToDateTime(GetOneStrValue(dr, "QSRQ", CosDataBase.DATETIME_TYPE));
                        DateTime zjrq = Convert.ToDateTime(GetOneStrValue(dr, "ZJRQ", CosDataBase.DATETIME_TYPE));
                        //Expression<Func<T_ksjl, bool>> where = (c => c.RelationID == lbmkID && c.Qsrq == qsrq && c.Zjrq == zjrq);
                        //List<T_ksjl> list = GetRecord(where);
                        //if ((list == null) || (list.Count == 0))
                        //{
                            T_ksjl ksjl_model = new T_ksjl();
                            ksjl_model.RelationID = lbmkID;
                            ksjl_model.Qsrq = qsrq;
                            ksjl_model.Zjrq = zjrq;
                            ksjl_model.Jl = GetOneStrValue(dr, "JL");
                            ksjl_model.Rhzw = GetOneStrValue(dr, "RHZW");
                            ksjl_model.Zmr = GetOneStrValue(dr, "ZMR");
                            listAdd.Add(ksjl_model);
                            //AddOneRecord(ksjl_model);
                        //}
                    }
                }
                AddRecords(listAdd);
            }
            catch (Exception e)
            {
                
            }
        }

        public List<T_ksjl> GetOneRecordbyRelationID(long lID)
        {
            Expression<Func<T_ksjl, bool>> where = (c => c.RelationID == lID);
            List<T_ksjl> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list;
            else
                return null;
        }
    }
}