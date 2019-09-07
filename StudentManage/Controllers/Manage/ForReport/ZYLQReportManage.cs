using Microsoft.Office.Interop.Excel;
using StudentManage.Controllers;
using StudentManage.Models;
using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace StudentManage.Manage
{
    /// <summary>
    /// 招生计划相关，报表生成
    /// </summary>
    public class ZYLQReportManage : ReportManage
    {

        public ZYLQReportManage()
        {
            sTableName = "志愿录取情况表";
        }

        private void SetWholeNum(string sSelTypeIDs)
        {
            //List<string> ss = new List<string>(sSelTypeIDs.Split(CosValue.SPLITOPERATOR2));
            //List<string> sss = ss.Distinct().ToList();
            //nWholeColumnNum = 2; //序号，计划招生数
            //foreach (string s in sss)
            //{
            //    if (s == "1") //省份
            //        nWholeColumnNum += 1;  //省份
            //    else if (s == "2") //院系
            //        nWholeColumnNum += 3; //院系，专业代码, 专业名称
            //}
            nWholeColumnNum = 8;
        }


        //protected override void AddTitleHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, string sGxID, string sYear)
        //{
        //    SetWholeNum(sSelTypeIDs);
        //    clsExcelApp.UniteCells(ws, 1, 1, 1, nWholeColumnNum);
        //    clsExcelApp.SetCellProperty(ws, 1, 1, 1, nWholeColumnNum, 16, "楷体_GB2312", true, Constants.xlCenter);
        //    clsExcelApp.SetCellValue(ws, 1, 1, GxName + sYear + "年" + sTableName);

        //    clsExcelApp.SetCellProperty(ws, 2, 3, 2, nWholeColumnNum, 12, "宋体", false, Constants.xlRight);
        //    clsExcelApp.UniteCells(ws, 2, 3, 2, nWholeColumnNum);
        //    string datestr = sYear + "年";
        //    clsExcelApp.SetCellValue(ws, 2, 3, datestr);
        //}

        //protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, string sGxID, string sYear)
        //{
        //     if (nWholeColumnNum == 8)
        //    {
        //        clsExcelApp.SetCellValue(ws, 3, 1, "序号");
        //        clsExcelApp.SetCellValue(ws, 3, 2, "院系");
        //        clsExcelApp.SetCellValue(ws, 3, 3, "专业代码");
        //        clsExcelApp.SetCellValue(ws, 3, 4, "专业名称");
        //        clsExcelApp.SetCellValue(ws, 3, 5, "志愿录取情况");
        //        clsExcelApp.SetCellValue(ws, 3, 6, "录取人数");
        //        clsExcelApp.SetCellValue(ws, 3, 7, "录取人数占总人数比例");
        //        clsExcelApp.SetCellValue(ws, 3, 8, "该专业录取总人数");
        //    }
        //}

        /// <summary>
        /// 根据高校ID获取ParentDepartmentID
        /// </summary>
        /// <param name="sGxID"></param>
        /// <returns></returns>
        private long GetParentDepartIDfromGxID(string sGxID)
        {
            long lGxID = Convert.ToInt64(sGxID);
            DepartmentInfoManage man = new DepartmentInfoManage();
            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.GxID == lGxID && c.DelFlag == false);
            List<tblDepartmentInfo> list = man.GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].ID;
            else
                return 0;
        }

        //protected override void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, string sGxID, string sYear)
        //{
        //    //首先分解，获取所有的省份ID和院系ID
        //    //ArrayList arrDepart = new ArrayList();
        //    //ArrayList arrRegion = new ArrayList();
        //    //long[] lIDs = Array.ConvertAll(sSelItems.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //    //int[] nTypes = Array.ConvertAll(sSelTypeIDs.Split(CosValue.SPLITOPERATOR2), int.Parse);
        //    //for (int i = 0; i < lIDs.Length; i++)
        //    //{
        //    //    if (nTypes[i] == 1) //省份
        //    //        arrRegion.Add(lIDs[i]);
        //    //    if (nTypes[i] == 2) //院系
        //    //        arrDepart.Add(lIDs[i]);
        //    //}
        //    ArrayList arrDepart = new ArrayList();
        //    long[] lIDs = Array.ConvertAll(sSelItems.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //    for(int i = 0; i < lIDs.Length; i++)
        //    {
        //        arrDepart.Add(lIDs[i]);
        //    }
           
        //    long lParentDepartID = GetParentDepartIDfromGxID(sGxID);
        //    int lYearNum = Convert.ToInt32(sYear);

        //    jhkTableManage jhkMan = new jhkTableManage();
        //    DepartmentInfoManage deMan = new DepartmentInfoManage();
        //    zydhTableManage zyMan = new zydhTableManage();
        //    //dqdmTableManage dqMan = new dqdmTableManage();
        //    bmkTableManage bmkMan = new bmkTableManage();
        //    zykTableManage zykMan = new zykTableManage();
        //    tddTableManage tddMan = new tddTableManage();
        //    zydh_OriginTableManage zyOriginMan = new zydh_OriginTableManage();

        //    if  (arrDepart.Count > 0) //只有院系没有省份
        //    {
        //        int k = 0;
        //        for (int j = 0; j < arrDepart.Count; j++)
        //        {
        //            long lNowID = Convert.ToInt64(arrDepart[j]);
        //            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.ID == lNowID && c.DelFlag == false);
        //            List<tblDepartmentInfo> xyList = deMan.GetRecord(where);
        //            long[] ZyIDs = Array.ConvertAll(xyList[0].ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //            clsExcelApp.UniteCells(ws, 4 + k, 1, (4 + k) + (ZyIDs.Length * 2) - 1, 1);//每个专业占两行
        //            clsExcelApp.SetCellValue(ws, 4 + k, 1, j + 1);
        //            clsExcelApp.UniteCells(ws, 4 + k, 2, (4 + k) + (ZyIDs.Length * 2) - 1, 2);
        //            clsExcelApp.SetCellValue(ws, 4 + k, 2, xyList[0].Name);
        //            foreach (long ZyID in ZyIDs)//专业ID
        //            {
        //                float lqrs1 = 0;float lqrs2 = 0;float lqrsz = 0;
        //                Expression<Func<Td_zydh, bool>> where2 = (c => c.ID == ZyID && c.DelFlag == false);
        //                List<Td_zydh> list = zyMan.GetRecord(where2);
        //                if ((list != null) && (list.Count > 0))
        //                {
        //                    clsExcelApp.UniteCells(ws, 4 + k, 3, 4 + k + 1, 3);
        //                    clsExcelApp.UniteCells(ws, 4 + k, 4, 4 + k + 1, 4);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 3, list[0].Code);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 4, list[0].Name);

        //                }
        //                clsExcelApp.SetCellValue(ws, 4 + k, 5, "服从志愿调剂录取");
        //                clsExcelApp.SetCellValue(ws, 4 + k + 1, 5, "第一志愿报考该专业录取");
        //                //第六列
        //                Expression<Func<Td_zydh_Origin, bool>> where7 = (c => c.RelationID == ZyID && c.YearCode == lYearNum && c.DelFlag == false);
        //                List<Td_zydh_Origin> list7 = zyOriginMan.GetRecord(where7);
        //                long zyIDOrigin;
        //                if (list7 != null && list7.Count > 0)
        //                {
        //                    zyIDOrigin = list7[0].ID;
        //                }
        //                else zyIDOrigin = 0;

        //                Expression<Func<T_tdd, bool>> where4 = (c => c.Lqzy==ZyID && c.DelFlag == false);
        //                List<T_tdd> list3 = tddMan.GetRecord(where4);
        //                for(int l = 0; l < list3.Count; l++)
        //                {
        //                    //年份的判断,这个有待改进
        //                    long llNowID = list3[l].RelationID;
        //                    Expression<Func<T_bmk, bool>> where6 = (c => c.ID == llNowID && c.DelFlag == false);
        //                    List<T_bmk> listnf = bmkMan.GetRecord(where6);
        //                    if (listnf[0].YearCode == lYearNum)
        //                    {
        //                        Expression<Func<T_zyk, bool>> where5 = (c => c.RelationID1 == llNowID&&c.ZydhOrigin == zyIDOrigin);
        //                        List<T_zyk> list4 = zykMan.GetRecord(where5);
        //                        if (list4 != null && list4.Count > 0)
        //                        {
        //                            if (list4[0].ID == 1) lqrs2 += 1;
        //                            else
        //                            {
        //                                long IID2 = list4[0].ID - 1;
        //                                Expression<Func<T_zyk, bool>> wheref = (c => c.ID == IID2 && c.bmk.YearCode == lYearNum);
        //                                List<T_zyk> listf = zykMan.GetRecord(wheref);
        //                                if (listf[0].RelationID1 != list4[0].RelationID1) { lqrs2 += 1; }//判断是不是第一志愿
        //                            }
        //                        }
        //                        else { lqrs1 += 1; }
        //                    }
                                
        //                }
        //                clsExcelApp.SetCellValue(ws, 4 + k, 6, lqrs1);
        //                clsExcelApp.SetCellValue(ws, 4 + k + 1, 6, lqrs2);
        //                //第七列和第八列
        //                Expression<Func<T_jhk, bool>> where3 = (c => c.Zydh == ZyID && c.YearCode == lYearNum && c.ParentDepartmentID == lParentDepartID && c.DelFlag == false);
        //                List<T_jhk> list2 = jhkMan.GetRecord(where3);
        //                clsExcelApp.UniteCells(ws, 4 + k, 8, 4 + k + 1, 8);
        //                if ((list2 != null) && (list2.Count > 0))
        //                {
        //                    lqrsz = list2[0].Jhzxs;
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 8, lqrsz);
        //                    if (lqrsz != 0)
        //                    {
        //                        float b1 = lqrs1 / lqrsz;float b2 = lqrs2 / lqrsz;
        //                        string b1s = b1 * 100 + "%";
        //                        string b2s = b2 * 100 + "%";
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, b1s);
        //                        clsExcelApp.SetCellValue(ws, 4 + k + 1, 7, b2s);
        //                    }
        //                    else
        //                    {
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, "无法计算");
        //                        clsExcelApp.SetCellValue(ws, 4 + k + 1, 7, "无法计算");
        //                    }
        //                }
                            
        //                else
        //                {
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 8, 0);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 7, "无法计算");
        //                    clsExcelApp.SetCellValue(ws, 4 + k + 1, 7, "无法计算");
        //                }                          
        //                k = k + 2;
        //            }
        //        }
        //    }
 
        //}

    }
}