using System;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace StudentManage.Manage
{
    public class oneTableStatus
    {
        public string Name  { set; get; }
        public bool Status { set; get; }

        public oneTableStatus()
        {
            Name = "";
            Status = false;
        }
    }

    public static class AllTableStatus
    {
        public static oneTableStatus[] sta = new oneTableStatus[74];
        public static void InitAllTableStatus()
        {
            for (int i = 0; i < sta.Length; i++)
                sta[i] = new oneTableStatus();

            sta[0].Name = "td_bylbdm";
            sta[1].Name = "td_bylbdm_Origin";
            sta[2].Name = "td_ccdm";
            sta[3].Name = "td_ccdm_Origin";
            sta[4].Name = "td_cjxdm";
            sta[5].Name = "td_cjxdm_Origin";
            sta[6].Name = "td_czlbdm";
            sta[7].Name = "td_czlbdm_Origin";
            sta[8].Name = "td_dqdm";
            sta[9].Name = "td_dqdm_Origin";

            sta[10].Name = "td_hjlbdm";
            sta[11].Name = "td_hjlbdm_Origin";
            sta[12].Name = "td_jhlbdm";
            sta[13].Name = "td_jhlbdm_Origin";
            sta[14].Name = "td_jhxzdm";
            sta[15].Name = "td_jhxzdm_Origin";
            sta[16].Name = "td_kldm";
            sta[17].Name = "td_kldm_Origin";
            sta[18].Name = "td_kmdm";
            sta[19].Name = "td_kmdm_Origin";

            sta[20].Name = "td_kslbdm";
            sta[21].Name = "td_kslbdm_Origin";
            sta[22].Name = "td_kslxdm";
            sta[23].Name = "td_kslxdm_Origin";
            sta[24].Name = "td_kstzdm";
            sta[25].Name = "td_kstzdm_Origin";
            sta[26].Name = "td_kszgdm";
            sta[27].Name = "td_kszgdm_Origin";
            sta[28].Name = "td_lqfsdm";
            sta[29].Name = "td_lqfsdm_Origin";
            sta[30].Name = "td_lqlxdm";
            sta[31].Name = "td_lqlxdm_Origin";

            sta[32].Name = "td_mzdm";
            sta[33].Name = "td_mzdm_Origin";
            sta[34].Name = "td_pcdm";
            sta[35].Name = "td_pcdm_Origin";
            sta[36].Name = "td_tdlxdm";
            sta[37].Name = "td_tdlxdm_Origin";
            sta[38].Name = "td_tdyydm";
            sta[39].Name = "td_tdyydm_Origin";
            sta[40].Name = "td_tjjldm";
            sta[41].Name = "td_tjjldm_Origin";

            sta[42].Name = "td_wyyzdm";
            sta[43].Name = "td_wyyzdm_Origin";
            sta[44].Name = "td_xtdwdm";
            sta[45].Name = "td_xtdwdm_Origin";
            sta[46].Name = "td_xzdm";
            sta[47].Name = "td_xzdm_Origin";
            sta[48].Name = "td_zjlxdm";
            sta[49].Name = "td_zjlxdm_Origin";
            sta[50].Name = "td_zklxdm";
            sta[51].Name = "td_zklxdm_Origin";

            sta[52].Name = "td_zylbdm";
            sta[53].Name = "td_zylbdm_Origin";
            sta[54].Name = "td_zytjlxdm";
            sta[55].Name = "td_zytjlxdm_Origin";
            sta[56].Name = "td_zytzdm";
            sta[57].Name = "td_zytzdm_Origin";
            sta[58].Name = "td_zzmmdm";
            sta[59].Name = "td_zzmmdm_Origin";
            sta[60].Name = "td_byxxdm";
            sta[61].Name = "td_byxxdm_Origin";

            sta[62].Name = "td_xbdm";
            sta[63].Name = "td_xbdm_Origin";


            sta[64].Name = "t_tddw";
            sta[65].Name = "t_tddw_Origin";
            sta[66].Name = "td_zydh";
            sta[67].Name = "td_zydh_Origin";

            sta[68].Name = "t_jhk";
            sta[69].Name = "t_bmk";
            sta[70].Name = "t_tdd";
            sta[71].Name = "t_ksjl";
            sta[72].Name = "t_ksxkcj";
            sta[73].Name = "t_tjxx";
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    public class TableStatusManage : ParentManage
    {       

        public TableStatusManage() 
        {
            
        }


        /// <summary>
        /// 准备修改某张表
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool BeginUpdate<T>() where T: class, new()
        {
            T obj = new T();
            bool flag = true;
            string sName = obj.GetType().Name;            
            lock (locker)
            {
                int i = FindTableStatus(sName);
                if (!AllTableStatus.sta[i].Status)
                    AllTableStatus.sta[i].Status = true;
                else
                    flag = false;
            }
            return flag;
        }

        private static int FindTableStatus(string sName)
        {
            int i = 0;
            bool searchflag = false;
            while (!searchflag)
            {
                if (AllTableStatus.sta[i].Name.ToLower() == sName.ToLower())
                    searchflag = true;
                else
                    i++;
            }
            if (searchflag)
                return i;
            else
                return -1;
        }
        /// <summary>
        /// 结束修改某张表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void EndUpdate<T>() where T : class, new()
        {
            T obj = new T();
            string sName = obj.GetType().Name;            
            lock (locker)
            {
                int i = FindTableStatus(sName);
                AllTableStatus.sta[i].Status = false;
            }
        }
    }
}