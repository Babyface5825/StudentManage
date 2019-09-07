using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class kslbdmTableManage : OriginTableManage
    {

        public kslbdmTableManage() 
        {
            Manage_KeyWord = "kslbdmTable";
        }

        //public List<ListCompareTable> InportOriginData(DataTable dt)
        //{
        //    foreach (DataRow dr in dt.Rows)
        //        dr[1] = dr[1].ToString().Replace("城市", "城镇");
        //    return InportSimpleTableOriginData<Td_kslbdm>(dt, "kslbdmTable");
        //}

    }
}