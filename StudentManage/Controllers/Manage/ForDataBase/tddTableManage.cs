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
    /// 对应考生号在报名库中的主码
    /// </summary>
    public class BMKTable
    {
        public long ID { set; get; }
        public string KSH { set; get; }
        public string smallKSH { set; get; }


        public BMKTable()
        {
            ID = 0;
            KSH = "";
            smallKSH = "";
        }

    }
    
    /// <summary>
    /// 报名库  2017的表中分成了这两个表 2012只有一个投档单  主要参照2017写的
    /// </summary>
    public class tddTableManage : OriginTableManage
    {
        public List<BMKTable> BMK_list;
        public List<T_jhk> JHK_list;

        public List<ListCompareTable> BYLBDM_list;
        public List<ListCompareTable> CCDM_list;
        public List<ListCompareTable> CJXDM_list;
        public List<ListCompareTable> CZLBDM_list;
        public List<ListCompareTable> DQDM_list;

        public List<ListCompareTable> HJLBDM_list;
        public List<ListCompareTable> JHLBDM_list;
        public List<ListCompareTable> JHXZDM_list;
        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> KMDM_list;

        public List<ListCompareTable> KSLBDM_list;
        public List<ListCompareTable> KSLXDM_list;
        public List<ListCompareTable> KSTZDM_list;
        public List<ListCompareTable> KSZGDM_list;
        public List<ListCompareTable> LQFSDM_list;

        public List<ListCompareTable> MZDM_list;
        public List<ListCompareTable> PCDM_list;
        public List<ListCompareTable> TDLXDM_list;
        public List<ListCompareTable> TDYYDM_list;
        public List<ListCompareTable> TJJLDM_list;

        public List<ListCompareTable> WYYZDM_list;
        public List<ListCompareTable> XTDWDM_list;
        //public List<ListCompareTable> ZCDM_list;
        public List<ListCompareTable> XZDM_list;
        public List<ListCompareTable> ZJLXDM_list;

        public List<ListCompareTable> ZKLXDM_list;
        public List<ListCompareTable> ZYLBDM_list;
        public List<ListCompareTable> ZYTJLXDM_list;
        public List<ListCompareTable> ZYTZDM_list;
        public List<ListCompareTable> ZZMMDM_list;


        public List<ListCompareTable> BYXXDM_list;
        public List<ListCompareTable> XBDM_list;
        public List<ListCompareTable> ZYDH_list;
        public List<ListCompareTable> TDDW_list;
                
                


        public tddTableManage() 
        {
            Manage_KeyWord = "tddTable";
        }


        public T_tdd GetOneRecordbyRelationID(long lID)
        {
            Expression<Func<T_tdd, bool>> where = (c => c.RelationID == lID && c.DelFlag == false);
            List<T_tdd> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
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
                if (dt.Columns[i].ColumnName == "ZKZH")  //12库才可能在计划库表中存在准考证号
                    flag = true;
                else
                    i++;
            }
            return flag;
        }


        private void InportZyk(DataTable dt, List<BMKTable> BMK_list)
        {
            short num = 1;
            List<T_zyk> listAll = new List<T_zyk>();
            //首先确定所有位置
            List<int> pos = new List<int>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if ((dt.Columns[i].ColumnName.Contains("TDDWDM")) && (dt.Columns[i].ColumnName.Length >= 7))
                    pos.Add(i);
            }

            foreach (DataRow dr in dt.Rows)
            {
                long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                if (lbmkID > 0)
                {
                    for (int j = 0; j < pos.Count; j++)
                    {
                        long lPcdm = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM", CosDataBase.LONGINT_TYPE));
                        long lKldm = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM", CosDataBase.LONGINT_TYPE));
                        long lJhxzdm = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZ", CosDataBase.LONGINT_TYPE));
                        long lTddwdm = GetOriginListID(TDDW_list, dr.ItemArray[pos[j]].ToString());
                        long lZydh = GetOriginListID(ZYDH_list, dr.ItemArray[pos[j] + 1].ToString());
                        //Expression<Func<T_jhk, bool>> where = (c => c.ZydhOrigin == lZydh && c.TddwdmOrigin == lTddwdm);
                        //List<T_jhk> list = GetRecord(where);
                        List<T_jhk> list = JHK_list.Where(c => c.ZydhOrigin == lZydh && c.TddwdmOrigin == lTddwdm).ToList();

                        if ((list != null) && (list.Count > 0))
                        {
                            T_zyk model = new T_zyk();
                            model.RelationID1 = lbmkID;
                            model.RelationID2 = list[0].ID;
                            model.PcdmOrigin = lPcdm;
                            model.KldmOrigin = lKldm;
                            model.JhxzdmOrigin = lJhxzdm;
                            model.JhlbdmOrigin = 0;
                            model.TddwdmOrigin = lTddwdm;
                            model.ZydhOrigin = lZydh;
                            model.Zyxh = num;
                            listAll.Add(model);
                            num++;
                        }
                    }
                }
            }
            AddRecords(listAll);
        }

        public List<BMKTable> InportOriginData(DataTable dt, OleDbConnection con, string sNowPath, int nYear, long lProvinceID, long GxID)
        {            
            bool IsOldData = VeiryOldData(dt); //检查是不是12库
            bmkTableManage bmkMan = new bmkTableManage();
            if (IsOldData)
            {
                dt.Columns["ZXDM"].ColumnName = "BYXXDM"; //和17表保持一致，改名
                dt.Columns["ZXMC"].ColumnName = "BYXXMC"; 
                dt.Columns["TDDW"].ColumnName = "TDDWDM";
                dt.Columns["SFZH"].ColumnName = "ZJHM";  //身份证号->证件号码
                dt.Columns["JTDZ"].ColumnName = "TXDZ";  //家庭地址->通讯地址
                dt.Columns["YXDRCJ"].ColumnName = "YXDYCJ";  //院校导入成绩->院校定义成绩
                dt.Columns["LQFS"].ColumnName = "LQLXDM";  //录取方式->录取类型代码
                dt.Columns["BH"].ColumnName = "TDDBH";  //编号->投档单编号


                bmkMan.InitAllFoundDataList(con, sNowPath, dt, nYear, lProvinceID);

                bmkMan.BYLBDM_list = BYLBDM_list;
                bmkMan.CJXDM_list = CJXDM_list;
                bmkMan.CZLBDM_list = CZLBDM_list;
                bmkMan.DQDM_list = DQDM_list;
                bmkMan.HJLBDM_list = HJLBDM_list;

                bmkMan.KLDM_list = KLDM_list;
                bmkMan.KMDM_list = KMDM_list;
                bmkMan.KSLBDM_list = KSLBDM_list;
                bmkMan.KSLXDM_list = KSLXDM_list;
                bmkMan.KSTZDM_list = KSTZDM_list;

                bmkMan.KSZGDM_list = KSZGDM_list;
                bmkMan.MZDM_list = MZDM_list;
                bmkMan.WYYZDM_list = WYYZDM_list;
                bmkMan.ZJLXDM_list = ZJLXDM_list;
                bmkMan.XTDWDM_list = XTDWDM_list;

                bmkMan.ZZMMDM_list = ZZMMDM_list;
                bmkMan.XBDM_list = XBDM_list;
                //bmkMan.BYXXDM_list = BYXXDM_list;

                BMK_list = bmkMan.AddSomeNewStudents(dt, nYear, GxID, lProvinceID);
            }


            List<T_tdd> tddList = new List<T_tdd>();
            foreach (DataRow dr in dt.Rows)
            {
                long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                if (lbmkID != 0)
                {
                    //Expression<Func<T_tdd, bool>> where = (c => c.RelationID == lbmkID && c.DelFlag == false);
                    //long lID = ExistOneRecord(where);
                    //List<T_tdd> list = GetRecord(where);
                    //if ((list == null) || (list.Count == 0)) //一条报名库记录对应一条投档单记录
                    //if (lID == 0)
                    {
                        T_tdd tdd_model = new T_tdd();
                        tdd_model.RelationID = lbmkID;
                        tdd_model.Ljzyh = GetOneStrValue(dr, "LJZYH");
                        tdd_model.TddwOrigin = GetOriginListID(TDDW_list, GetOneStrValue(dr, "TDDWDM"));
                        tdd_model.PcdmOrigin = GetOriginListID(PCDM_list, GetOneStrValue(dr, "PCDM"));
                        tdd_model.KldmOrigin = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM"));
                        tdd_model.JhxzdmOrigin = GetOriginListID(JHXZDM_list, GetOneStrValue(dr, "JHXZDM"));
                        tdd_model.JhlbdmOrigin = GetOriginListID(JHLBDM_list, GetOneStrValue(dr, "JHLBDM"));
                        
                        ListCompareTable oneCompare = GetCompareList(TDLXDM_list, GetOneStrValue(dr, "TDLXDM"));
                        if (oneCompare != null)
                        {
                            tdd_model.TdlxdmOrigin = oneCompare.lOriginID;
                            tdd_model.Tdlxdm = oneCompare.lNewID;
                        }

                        tdd_model.Tdsj = Convert.ToDateTime(GetOneStrValue(dr, "TDSJ", CosDataBase.DATETIME_TYPE));
                        tdd_model.Cj = Convert.ToDouble(GetOneStrValue(dr, "CJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Tzcj = Convert.ToDouble(GetOneStrValue(dr, "TZCJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Tdcj = Convert.ToDouble(GetOneStrValue(dr, "TDCJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Zgf = Convert.ToDouble(GetOneStrValue(dr, "ZGF", CosDataBase.FLOAT_TYPE));
                        tdd_model.Yxdycj = Convert.ToDouble(GetOneStrValue(dr, "YXDYCJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Zyytjzcj = Convert.ToDouble(GetOneStrValue(dr, "ZYYTJZCJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Zyytfjcj = Convert.ToDouble(GetOneStrValue(dr, "ZYYTFJCJ", CosDataBase.FLOAT_TYPE));
                        tdd_model.Ytzy = GetMainListID(ZYDH_list, GetOneStrValue(dr, "YTZY"));
                        tdd_model.Lqzy = GetMainListID(ZYDH_list, GetOneStrValue(dr, "LQZY"));
                        tdd_model.Lqsj = Convert.ToDateTime(GetOneStrValue(dr, "LQSJ", CosDataBase.DATE_TYPE));
                        oneCompare = GetCompareList(TDYYDM_list, GetOneStrValue(dr, "TDYYDM"));
                        if (oneCompare != null)
                        {
                            tdd_model.TdyydmOrigin = oneCompare.lOriginID;
                            tdd_model.Tdyydm = oneCompare.lNewID;
                        }
                        tdd_model.Zyhg = Convert.ToBoolean(GetOneStrValue(dr, "ZYHG", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Tjhg = Convert.ToBoolean(GetOneStrValue(dr, "TJHG", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Xkkmhg = Convert.ToBoolean(GetOneStrValue(dr, "XKKMHG", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Skkmhg = Convert.ToBoolean(GetOneStrValue(dr, "SKKMHG", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Lqxh = GetOneStrValue(dr, "LQXH");
                        tdd_model.Tddbh = GetOneStrValue(dr, "TDDBH");
                        tdd_model.Xzbh = GetOneStrValue(dr, "XZBH");
                        tdd_model.Tddzt = GetOneStrValue(dr, "TDDZT");
                        tdd_model.Sdbz = GetOneStrValue(dr, "SDBZ");
                        tdd_model.Remark = GetOneStrValue(dr, "BZ");
                        oneCompare = GetCompareList(ZYTZDM_list, GetOneStrValue(dr, "ZYTZDM"));
                        if (oneCompare != null)
                        {
                            tdd_model.ZytzOrigin = oneCompare.lOriginID;
                            tdd_model.Zytz = oneCompare.lNewID;
                        }
                        tdd_model.Zycj = Convert.ToDouble(GetOneStrValue(dr, "ZYCJ",CosDataBase.FLOAT_TYPE));
                        tdd_model.Zycjtfpw = Convert.ToInt16(GetOneStrValue(dr, "ZYCJTFPW", CosDataBase.INT_TYPE));
                        oneCompare = GetCompareList(ZYTJLXDM_list, GetOneStrValue(dr, "ZYTJLXDM"));
                        if (oneCompare != null)
                        {
                            tdd_model.ZytjlxdmOrigin = oneCompare.lOriginID;
                            tdd_model.Zytjlxdm = oneCompare.lNewID;
                        }
                        tdd_model.Kslsczbz = GetOneStrValue(dr, "KSLSCZBZ");
                        oneCompare = GetCompareList(LQFSDM_list, GetOneStrValue(dr, "LQLXDM"));
                        if (oneCompare != null)
                        {
                            tdd_model.LqfsOrigin = oneCompare.lOriginID;
                            tdd_model.Lqfs = oneCompare.lNewID;
                        }
                        tdd_model.Kszt = GetOneStrValue(dr, "KSZT");
                        tdd_model.Zyzytj = Convert.ToBoolean(GetOneStrValue(dr, "ZYZYTJ", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Dxtj = Convert.ToBoolean(GetOneStrValue(dr, "DXTJ", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.Zytjfc = Convert.ToBoolean(GetOneStrValue(dr, "Zytjfc", CosDataBase.BOOLEAN_TYPE));
                        tdd_model.TjjldmOrigin = GetOriginListID(TJJLDM_list, GetOneStrValue(dr, "TJJLDM"));
                        tddList.Add(tdd_model);
                    }
                }                
            }
            AddRecords(tddList);

            if (IsOldData)
            {
                //下面处理高考成绩(12库）
                InportGkcj(dt, BMK_list, CJXDM_list);
                //下面处理志愿信息（12库）
                InportZyk(dt, BMK_list);
            }

            return BMK_list;
        }

    }
}