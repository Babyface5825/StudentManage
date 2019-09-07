using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Runtime.CompilerServices;
using Ionic.Zip;
using Ionic.Zlib;
using StudentManage.Models;
using System.Collections;

namespace StudentManage.Manage
{
    //定时压缩实验报告文件
    public class ScanFoundDelTask : ParentManage
    {
        Timer timer;

        public ScanFoundDelTask()
        {
            timer = new Timer(CreateScanTask, null, 0, 1000 * 60 * 60);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CreateScanTask(object obj)
        {
            var hour = DateTime.Now.Hour;
            if (hour == 2) //夜里两点做
                ScanTask();
        }
        

        /// <summary>
        /// 修改思路：修改T表中的ScanFlag，修改T1表中的RelationID（关联到合并后的ID），修改其他表中对应的关联字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="keyWords"></param>
        private void ScanOneTable<T,T1>(string keyWords) where T : class  where T1 : class
        {
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            arrNames.Add("JoinID"); arrNames.Add("ScanFlag");
            arrValues.Add(0); arrValues.Add(false);
            arrSigns.Add(SIGN.NotEqual.ToString()); arrSigns.Add(SIGN.Equal.ToString());

            var where = GetAndLambdaExpression<T>(arrNames, arrValues, arrSigns);
            var list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
            {
                string[] EditColumns = { "ScanFlag" };
                foreach (T model in list)
                {
                    model.GetType().GetProperty("ScanFlag").SetValue(model, true, null);
                    //扫描并修改T1表
                    long lOldID = Convert.ToInt64(model.GetType().GetProperty("ID").GetValue(model, null));
                    long lNewID = Convert.ToInt64(model.GetType().GetProperty("JoinID").GetValue(model, null));
                    UpdateOriginTable<T1>("RelationID", lOldID, lNewID);
                    //扫描并修改其他表
                    switch (keyWords)
                    {
                        case "bylbdmTable": UpdateOriginTable<T_bmk>("Bylbdm", lOldID, lNewID); break;
                        case "ccdmTable": UpdateOriginTable<Td_zydh>("Ccdm", lOldID, lNewID); break;
                        case "cjxdmTable": UpdateOriginTable<T_ksxkcj>("Cjxdm", lOldID, lNewID); break;
                        case "czlbdmTable": UpdateOriginTable<T_bmk>("Czlbdm", lOldID, lNewID); break;
                        case "hjlbdmTable": UpdateOriginTable<T_bmk>("Hjlbdm", lOldID, lNewID); break;

                        case "jhlbdmTable": UpdateOriginTable<T_tddw>("Jhlbdm", lOldID, lNewID); break;
                        case "jhxzdmTable": UpdateOriginTable<T_tddw>("Jhxzdm", lOldID, lNewID); break;
                        case "kldmTable": UpdateOriginTable<T_tddw>("Kldm", lOldID, lNewID); break;
                        case "kmdmTable":
                            UpdateSerialOriginTable<T_bmk>("Xkkm", lOldID, lNewID);
                            UpdateSerialOriginTable<T_bmk>("Skkm", lOldID, lNewID);
                            UpdateOriginTable<T_ksxkcj>("Kmdm", lOldID, lNewID); 
                            break;
                        case "kslbdmTable": UpdateOriginTable<T_bmk>("Kslbdm", lOldID, lNewID); break;

                        case "kslxdmTable": UpdateOriginTable<T_bmk>("Kslxdm", lOldID, lNewID); break;
                        case "kstzdmTable": UpdateSerialOriginTable<T_bmk>("Kstzdm", lOldID, lNewID); break;
                        case "kszgdmTable": UpdateOriginTable<T_bmk>("Kszgdm", lOldID, lNewID); break;
                        case "lqfsdmTable": UpdateOriginTable<T_tdd>("Lqfsdm", lOldID, lNewID); break;
                        case "mzdmTable": UpdateOriginTable<T_bmk>("Mzdm", lOldID, lNewID); break;

                        case "pcdmTable": UpdateOriginTable<T_tddw>("Pcdm", lOldID, lNewID); break;
                        case "tdlxdmTable": UpdateOriginTable<T_tdd>("Tdlxdm", lOldID, lNewID); break;
                        case "tdyydmTable": UpdateOriginTable<T_tdd>("Tdyydm", lOldID, lNewID); break;
                        case "tjjldmTable":
                            UpdateOriginTable<T_tdd>("Tjjldm", lOldID, lNewID);
                            UpdateOriginTable<T_tjxx>("Tjjldm", lOldID, lNewID);
                            break;
                        case "wyyzdmTable": UpdateOriginTable<T_bmk>("Wyyzdm", lOldID, lNewID); break;

                        case "xtdwdmTable": UpdateOriginTable<T_bmk>("Xtdwdm", lOldID, lNewID); break;
                        case "xzdmTable": UpdateOriginTable<Td_zydh>("Xzdm", lOldID, lNewID); break;
                        case "zjlxdmTable": UpdateOriginTable<T_bmk>("Zjlxdm", lOldID, lNewID); break;
                        case "zklxdmTable": UpdateOriginTable<T_jhk>("Zklxdm", lOldID, lNewID); break;
                        case "zytjlxdmTable": UpdateOriginTable<T_tdd>("Zytjlxdm", lOldID, lNewID); break;

                        //case "zytzdmTable": UpdateOriginTable<T_tdd>("Zytzdm", lOldID, lNewID); break;
                        case "zzmmdmTable": UpdateOriginTable<T_bmk>("Zzmmdm", lOldID, lNewID); break;
                        case "byxxdmTable": UpdateOriginTable<T_bmk>("Byxxdm", lOldID, lNewID); break;
                        default: break;
                    }
                }
                SaveSomeRecords(list, EditColumns);
            }
        }

        /// <summary>
        /// 对一系列代码相关的一些数据更新
        /// </summary>
        /// <param name="lOldID"></param>
        /// <param name="lNewID"></param>
        /// <param name="transDB"></param>
        private void UpdateSerialOriginTable<T>(string ColumnName, long lOldID, long lNewID) where T : class
        {
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            arrNames.Add(ColumnName); arrValues.Add(lOldID); arrSigns.Add(SIGN.Contain1);
            var where = GetAndLambdaExpression<T>(arrNames, arrValues);
            var list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                foreach (T model in list)
                {
                    string[] ss = model.GetType().GetProperty(ColumnName).GetValue(model, null).ToString().Split(CosValue.SPLITOPERATOR1);
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (ss[i] == lOldID.ToString())
                            ss[i] = lNewID.ToString();
                    }                    
                    model.GetType().GetProperty(ColumnName).SetValue(model, string.Join(CosValue.SPLITOPERATOR1.ToString(), ss), null);
                }
            string[] EditColumn = { ColumnName };
            SaveSomeRecords(list, EditColumn);

        }

        /// <summary>
        /// 修改Origin表中相关数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lOldID"></param>
        /// <param name="lNewID"></param>
        private void UpdateOriginTable<T>(string ColumnName, long lOldID, long lNewID) where T : class        
        {
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList();
            arrNames.Add(ColumnName); arrValues.Add(lOldID);
            var where = GetAndLambdaExpression<T>(arrNames, arrValues);
            var list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                foreach (T model in list)
                    model.GetType().GetProperty(ColumnName).SetValue(model, lNewID, null);
            string[] EditColumns = { ColumnName };            
            SaveSomeRecords(list, EditColumns);
        }
        public void ScanTask()
        {            
            ScanOneTable<Td_bylbdm, Td_bylbdm_Origin>("bylbdmTable");
            ScanOneTable<Td_ccdm, Td_ccdm_Origin>("ccdmTable");
            ScanOneTable<Td_cjxdm, Td_cjxdm_Origin>("cjxdmTable");
            ScanOneTable<Td_czlbdm, Td_czlbdm_Origin>("czlbdmTable");
            ScanOneTable<Td_hjlbdm, Td_hjlbdm_Origin>("hjlbdmTable");

            ScanOneTable<Td_jhlbdm, Td_jhlbdm_Origin>("jhlbdmTable");
            ScanOneTable<Td_jhxzdm, Td_jhxzdm_Origin>("jhxzdmTable");
            ScanOneTable<Td_kldm, Td_kldm_Origin>("kldmTable");
            ScanOneTable<Td_kmdm, Td_kmdm_Origin>("kmdmTable");
            ScanOneTable<Td_kslbdm, Td_kslbdm_Origin>("kslbdmTable");

            ScanOneTable<Td_kslxdm, Td_kslxdm_Origin>("kslxdmTable");
            ScanOneTable<Td_kstzdm, Td_kstzdm_Origin>("kstzdmTable");
            ScanOneTable<Td_kszgdm, Td_kszgdm_Origin>("kszgdmTable");
            ScanOneTable<Td_lqfsdm, Td_lqfsdm_Origin>("lqfsdmTable");
            ScanOneTable<Td_mzdm, Td_mzdm_Origin>("mzdmTable");

            ScanOneTable<Td_pcdm, Td_pcdm_Origin>("pcdmTable");
            ScanOneTable<Td_tdlxdm, Td_tdlxdm_Origin>("tdlxdmTable");
            ScanOneTable<Td_tdyydm, Td_tdyydm_Origin>("tdyydmTable");
            ScanOneTable<Td_tjjldm, Td_tjjldm_Origin>("tjjldmTable");
            ScanOneTable<Td_wyyzdm, Td_wyyzdm_Origin>("wyyzdmTable");

            ScanOneTable<Td_xtdwdm, Td_xtdwdm_Origin>("xtdwdmTable");
            ScanOneTable<Td_xzdm, Td_xzdm_Origin>("xzdmTable");
            ScanOneTable<Td_zjlxdm, Td_zjlxdm_Origin>("zjlxdmTable");
            ScanOneTable<Td_zklxdm, Td_zklxdm_Origin>("zklxdmTable");
            ScanOneTable<Td_zytjlxdm, Td_zytjlxdm_Origin>("zytjlxdmTable");

            ScanOneTable<Td_zytjlxdm, Td_zytjlxdm_Origin>("zytjlxdmTable");
            ScanOneTable<Td_zytzdm, Td_zytzdm_Origin>("zytzdmTable");
            ScanOneTable<Td_zytzdm, Td_zytzdm_Origin>("zytzdmTable");
            ScanOneTable<Td_zzmmdm, Td_zzmmdm_Origin>("zzmmdmTable");
            ScanOneTable<Td_zzmmdm, Td_zzmmdm_Origin>("zzmmdmTable");

            ScanOneTable<Td_byxxdm, Td_byxxdm_Origin>("byxxdmTable");
        }
    }
}