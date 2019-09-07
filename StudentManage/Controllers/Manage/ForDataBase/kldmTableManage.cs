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
    public class kldmTableManage : OriginTableManage
    {

        public kldmTableManage() 
        {
            Manage_KeyWord = "kldmTable";
        }

        public List<ListCompareTable> InportOriginData(DataTable dt)
        {
            List<ListCompareTable> listReturn = new List<ListCompareTable>();
            try
            {
                Expression<Func<Td_kldm, bool>> where = (c => (c.DelFlag == false));
                List<Td_kldm> list = GetRecord(where);
                foreach (DataRow dr in dt.Rows)
                {
                    long ID = -1;
                    long[] lID = list.Where(c => c.Name == dr["KLMC"].ToString()).Select(c => c.ID).ToArray();
                    if ((lID != null) && (lID.Length > 0))
                        ID = lID[0];

                    if (ID == -1)//目前库表中不存在
                    {
                        Td_kldm model = new Td_kldm();
                        model.Name = dr["KLMC"].ToString();
                        model.Remark = dr["KLDM"].ToString();
                        model.Jbkl = dr["JBKL"].ToString();
                        try
                        {
                            model.Kscjbjxh = Convert.ToInt32(dr["KSCJBJXH"]);
                        }
                        catch
                        {
                            model.Kscjbjxh = 0;
                        }
                        string[] EditColumn = { "Name", "Remark", "Jbkl", "Kscjbjxh" };
                        long lNewID = SaveOneSimpleRecord(model, "kldmTable", EditColumn);
                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = dr[0].ToString();
                        oneTable.lNewID = lNewID;
                        listReturn.Add(oneTable);
                    }
                    else //目前库表中存在
                    {
                        ListCompareTable oneTable = new ListCompareTable();
                        oneTable.sOldID = dr["KLDM"].ToString();
                        oneTable.lNewID = ID;
                        listReturn.Add(oneTable);
                    }
                }
                return listReturn;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}