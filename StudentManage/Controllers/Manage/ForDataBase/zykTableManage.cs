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
    public class zykTableManage : OriginTableManage
    {
        public List<ListCompareTable> PCDM_list;
        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> JHXZDM_list;
        public List<ListCompareTable> JHLBDM_list;
        public List<ListCompareTable> TDDW_list;
        public List<ListCompareTable> ZYDH_list;

        public List<BMKTable> BMK_list;
        public List<T_jhk> JHK_list;
        public zykTableManage() 
        {
            Manage_KeyWord = "zykTable";
        }

        public void InportOriginData(DataTable dt, int nYear, long lProvinceID)
        {
            if (dt != null)
            {
                List<T_zyk> listAdd = new List<T_zyk>();
                bmkTableManage bmkMan = new bmkTableManage();
                foreach (DataRow dr in dt.Rows)
                {
                    //long lbmkID = bmkMan.GetbmkID(dr["KSH"].ToString());
                    long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                    if (lbmkID != 0)
                    {
                        //首先找到计划库对应的字段
                        long lPcdm = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                        long lKldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                        long lJhxzdm = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM", CosDataBase.LONGINT_TYPE));
                        long lJhlbdm = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM", CosDataBase.LONGINT_TYPE));
                        long lTddwdm = GetOriginListID(TDDW_list, GetOneStrValue(dr, "TDDWDM", CosDataBase.LONGINT_TYPE));
                        long lZydh = GetOriginListID(ZYDH_list, GetOneStrValue(dr, "ZYDH", CosDataBase.LONGINT_TYPE));
                        //Expression<Func<T_jhk, bool>> where = (c => c.ZydhOrigin == lZydh && c.TddwdmOrigin == lTddwdm);
                        //List<T_jhk> list = GetRecord(where);
                        List<T_jhk> list = JHK_list.Where(c => c.ZydhOrigin == lZydh && c.TddwdmOrigin == lTddwdm).ToList();

                        if ((list != null) && (list.Count > 0))
                        {
                            long lJhkID = list[0].ID;
                            //Expression<Func<T_zyk, bool>> where1 = (c => c.RelationID1 == lbmkID && c.RelationID2 == lJhkID);
                            //List<T_zyk> list1 = GetRecord(where1);
                            //if ((list1 == null) || (list1.Count == 0))
                            //{
                                T_zyk model = new T_zyk();
                                model.RelationID1 = lbmkID;
                                model.RelationID2 = lJhkID;
                                model.PcdmOrigin = lPcdm;
                                model.KldmOrigin = lKldm;
                                model.JhxzdmOrigin = lJhxzdm;
                                model.JhlbdmOrigin = lJhlbdm;
                                model.TddwdmOrigin = lTddwdm;
                                model.ZydhOrigin = lZydh;
                                model.Zyxh = Convert.ToInt16(GetOneStrValue(dr, "ZYXH", CosDataBase.SMALLINT_TYPE));
                                listAdd.Add(model);
                                //AddOneRecord(model);
                            //}
                        }
                    }
                }
                AddRecords(listAdd);
            }
        }
    }
}