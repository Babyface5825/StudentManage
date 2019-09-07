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
    /// 
    /// </summary>
    public class FSReportManage : ReportManage
    {

        public FSReportManage()
        {
            sTableName = "录取分数表";
            DynamicKeys = new string[2];
            DynamicKeys[0] = "Department";
            DynamicKeys[1] = "Province";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {
            return _SetWholeNum(sSelTypeIDs, sSelItems, 5);//序号，录取人数，最高分，最低分，平均分
        }

        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            int num = _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            clsExcelApp.SetCellValue(ws, 3, num, "录取人数"); num++;
            clsExcelApp.SetCellValue(ws, 3, num, "最高分");num++;
            clsExcelApp.SetCellValue(ws, 3, num, "最低分"); num++;
            clsExcelApp.SetCellValue(ws, 3, num, "平均分");

        }



        //protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, string sGxID, string sYear)
        //{
        //    if (nWholeColumnNum == 6)
        //    {
        //        clsExcelApp.SetCellValue(ws, 3, 1, "序号");
        //        clsExcelApp.SetCellValue(ws, 3, 2, "省份");
        //        clsExcelApp.SetCellValue(ws, 3, 3, "录取人数");
        //        clsExcelApp.SetCellValue(ws, 3, 4, "最高分");
        //        clsExcelApp.SetCellValue(ws, 3, 5, "最低分");
        //        clsExcelApp.SetCellValue(ws, 3, 6, "平均分");
        //    }
        //    else if (nWholeColumnNum == 8)
        //    {
        //        clsExcelApp.SetCellValue(ws, 3, 1, "序号");
        //        clsExcelApp.SetCellValue(ws, 3, 2, "院系");
        //        clsExcelApp.SetCellValue(ws, 3, 3, "专业代码");
        //        clsExcelApp.SetCellValue(ws, 3, 4, "专业名称");
        //        clsExcelApp.SetCellValue(ws, 3, 5, "录取人数");
        //        clsExcelApp.SetCellValue(ws, 3, 6, "最高分");
        //        clsExcelApp.SetCellValue(ws, 3, 7, "最低分");
        //        clsExcelApp.SetCellValue(ws, 3, 8, "平均分");
        //    }
        //    else if (nWholeColumnNum == 9)
        //    {
        //        clsExcelApp.SetCellValue(ws, 3, 1, "序号");
        //        clsExcelApp.SetCellValue(ws, 3, 2, "院系");
        //        clsExcelApp.SetCellValue(ws, 3, 3, "专业代码");
        //        clsExcelApp.SetCellValue(ws, 3, 4, "专业名称");
        //        clsExcelApp.SetCellValue(ws, 3, 5, "省份");
        //        clsExcelApp.SetCellValue(ws, 3, 6, "录取人数");
        //        clsExcelApp.SetCellValue(ws, 3, 7, "最高分");
        //        clsExcelApp.SetCellValue(ws, 3, 8, "最低分");
        //        clsExcelApp.SetCellValue(ws, 3, 9, "平均分");
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

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            tddTableManage tddMan = new tddTableManage();
            if (nItemPos == ItemKeys.Count - 1)
            {
                double maxScore = 0; // 最高分
                double minScore = 0; // 最低分
                double sumScore = 0;  // 所有人的总分
                double aveScore = 0; //平均分
                Expression<Func<T_tdd, bool>> where = GetAndLambdaExpression<T_tdd>(arrNowNames, arrNowValues, arrNowSigns);
                int count = tddMan.GetRecordCount(where);
                List<T_tdd> list = tddMan.GetRecord(where);
                clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, count);  //录取人数
                if(list!=null&&list.Count>0)
                {
                    minScore = maxScore = list[0].Tzcj;
                    foreach (T_tdd l in list)
                    {
                        if (l.Tzcj < minScore)
                            minScore = l.Tzcj;
                        if (l.Tzcj > maxScore)
                            maxScore = l.Tzcj;
                        sumScore += l.Tzcj;
                    }
                    aveScore = sumScore / count;
                }
                
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY + 1, maxScore);//最高分
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY + 2, minScore);//最低分
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY + 3, aveScore);//平均分

                nRowNumX++;
                if (nItemPos == 0)
                    nUnitRow1 = nRowNumX;
                else if (nItemPos == 1)
                    nUnitRow2 = nRowNumX;
                else if (nItemPos == 2)
                    nUnitRow3 = nRowNumX;
            }
            else
                nRowNumX = DFSSetLine(nItemPos + 1, nRowNumX, nRowNowNumY, ws, ItemKeys, arrItems, arrNowNames, arrNowValues, arrNowSigns, ReportTypeID);

            UnitCellCore(nItemPos, nRowNumX, ws, ItemKeys);
            return nRowNumX;
        }


        protected override void CreateChart(Worksheet ws, string sSelTypeIDs, int ChartType, long ReportTypeID)
        {
            int rows = ws.UsedRange.Cells.Rows.Count;
            int cols = ws.UsedRange.Cells.Columns.Count;
            List<string> sSelTypes = sSelTypeIDs.Split(CosValue.SPLITOPERATOR2).Distinct().ToList();

            //建立新的表
            Shape shape = ws.Shapes.AddChart(Type.Missing, 300, 1, Type.Missing, Type.Missing);
            Chart chart = shape.Chart;
            chart.HasTitle = false;

            Range DataRange1 = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols - 4]];
            Range DataRange2 = ws.Range[ws.Cells[4, cols], ws.Cells[rows, cols]];
            Range rng = clsExcelApp.app.Union(DataRange1, DataRange2);//平均分
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols]).Text, Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols]).Text, ws.Cells[3, 2], ws.Cells[3, cols]);
            }
        }
        //protected override void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, string sGxID, string sYear)
        //{
        //    //首先分解，获取所有的省份ID和院系ID
        //    ArrayList arrDepart = new ArrayList();
        //    ArrayList arrRegion = new ArrayList();
        //    long[] lIDs = Array.ConvertAll(sSelItems.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //    int[] nTypes = Array.ConvertAll(sSelTypeIDs.Split(CosValue.SPLITOPERATOR2), int.Parse);
        //    for (int i = 0; i < lIDs.Length; i++)
        //    {
        //        if (nTypes[i] == 1) //省份
        //            arrRegion.Add(lIDs[i]);
        //        if (nTypes[i] == 2) //院系
        //            arrDepart.Add(lIDs[i]);
        //    }

        //    long lParentDepartID = GetParentDepartIDfromGxID(sGxID);
        //    int lYearNum = Convert.ToInt32(sYear);


        //    //jhkTableManage jhkMan = new jhkTableManage();
        //    DepartmentInfoManage deMan = new DepartmentInfoManage();
        //    zydhTableManage zyMan = new zydhTableManage();
        //    dqdmTableManage dqMan = new dqdmTableManage();
        //    bmkTableManage bmkMan = new bmkTableManage();
        //    tddTableManage tddMan = new tddTableManage();

        //    if ((arrRegion.Count > 0) && (arrDepart.Count == 0)) //只有省份没有院系
        //    {
        //        for (int j = 0; j < arrRegion.Count; j++)
        //        {
        //            int Lqrs = 0;  //录取人数
        //            double zgf = 0; // 最高分
        //            double zdf = 0; // 最低分
        //            double zf = 0;  // 所有人的总分
        //            double pjf = 0; //平均分
        //            int mark = 0;//标志位，初始化最低分时用
        //            long lNowID = Convert.ToInt64(arrRegion[j]);
        //            string sProvinceName = dqMan.GetNowNamefromID(lNowID);
        //            Expression<Func<Td_dqdm, bool>> where0 = (c =>  c.ParentIndex == lNowID && c.DelFlag == false);
        //            List<Td_dqdm> list0 = dqMan.GetRecord(where0);//报名库的地区代码都是地区代码表中等级为3的
        //            for(int dq = 0; dq < list0.Count; dq++)//这是等级为2的
        //            {
        //                long dqNowID = list0[dq].ID;
        //                Expression<Func<Td_dqdm, bool>> where00 = (c => c.ParentIndex == dqNowID && c.DelFlag == false);
        //                List<Td_dqdm> list00 = dqMan.GetRecord(where00);
        //                for(int dq3 = 0; dq3 < list00.Count; dq3++)
        //                {
        //                    long dq3NowID = list00[dq3].ID;
        //                    Expression<Func<T_bmk, bool>> where = (c => c.Dqdm == dq3NowID && c.YearCode == lYearNum && c.DelFlag == false);
        //                    List<T_bmk> list = bmkMan.GetRecord(where);
        //                    for (int b = 0; b < list.Count; b++)
        //                    {
        //                        //判断该学生有没有被录取,用录取时间和录取类型代码来判断，录取时间不为空且录取类型代码在我们的ID范围之类
        //                        //录取方式的ID范围在1-165,10163-10178之间
        //                        long NowID1 = list[b].ID;
        //                        Expression<Func<T_tdd, bool>> where1 = (c => c.RelationID == NowID1 && c.Lqsj != null && ((c.Lqfs >= 1 && c.Lqfs <= 165) || (c.Lqfs >= 10163 && c.Lqfs <= 10178)));
        //                        List<T_tdd> list1 = tddMan.GetRecord(where1);
        //                        if (list1 != null && list.Count > 0)
        //                        {
        //                            Lqrs += 1;
        //                            if (mark == 0)
        //                            {
        //                                zdf = list1[0].Cj;
        //                                mark = 1;
        //                            }
        //                            if (list1[0].Cj >= zgf)
        //                            {
        //                                zgf = list1[0].Cj;
        //                            }
        //                            else if (list1[0].Cj <= zdf){ zdf = list1[0].Cj; }
        //                            zf += list1[0].Cj;
        //                        }
        //                    }
        //                }
        //            }        
        //            if (Lqrs != 0)
        //            {
        //                pjf = zf / Lqrs;
        //            }
        //            else { pjf = 0; }
        //            clsExcelApp.SetCellValue(ws, 4 + j, 1, j + 1); //序号
        //            clsExcelApp.SetCellValue(ws, 4 + j, 2, sProvinceName); //省份
        //            clsExcelApp.SetCellValue(ws, 4 + j, 3, Lqrs);  //录取人数
        //            clsExcelApp.SetCellValue(ws, 4 + j, 4, zgf);  //最高分
        //            clsExcelApp.SetCellValue(ws, 4 + j, 5, zdf);  //最低分
        //            clsExcelApp.SetCellValue(ws, 4 + j, 6, pjf);  //平均分
        //        }
        //    }
        //    else if ((arrRegion.Count == 0) && (arrDepart.Count > 0)) //只有院系没有省份
        //    {
        //        int k = 0;
        //        for (int j = 0; j < arrDepart.Count; j++)
        //        {
        //            long lNowID = Convert.ToInt64(arrDepart[j]);
        //            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.ID == lNowID && c.DelFlag == false);
        //            List<tblDepartmentInfo> xyList = deMan.GetRecord(where);
        //            long[] ZyIDs = Array.ConvertAll(xyList[0].ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //            clsExcelApp.UniteCells(ws, 4 + k, 1, (4 + k) + ZyIDs.Length - 1, 1);
        //            clsExcelApp.SetCellValue(ws, 4 + k, 1, j + 1);
        //            clsExcelApp.UniteCells(ws, 4 + k, 2, (4 + k) + ZyIDs.Length - 1, 2);
        //            clsExcelApp.SetCellValue(ws, 4 + k, 2, xyList[0].Name);
        //            foreach (long ZyID in ZyIDs)//专业ID
        //            {
        //                int Lqrs = 0;  //录取人数
        //                double zgf = 0; // 最高分
        //                double zdf = 0; // 最低分
        //                double zf = 0;  // 所有人的总分
        //                double pjf = 0; //平均分
        //                int mark = 0;
        //                Expression<Func<Td_zydh, bool>> where2 = (c => c.ID == ZyID && c.DelFlag == false);
        //                List<Td_zydh> list = zyMan.GetRecord(where2);
        //                if ((list != null) && (list.Count > 0))
        //                {
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 3, list[0].Code);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 4, list[0].Name);
        //                }
        //                //先在报名库中筛选相应年份的记录
        //                //Expression<Func<T_bmk, bool>> where4 = (c => c.YearCode == lYearNum && c.DelFlag == false);
        //                //List<T_bmk> listnf = bmkMan.GetRecord(where4);
        //                //for(int nf = 0; nf < listnf.Count; nf++)
        //                //{
        //                    //long llNowID = listnf[nf].ID;
        //                Expression<Func<T_tdd, bool>> where3 = (c =>  c.Lqsj != null && ((c.Lqfs >= 1 && c.Lqfs <= 165) || (c.Lqfs >= 10163 && c.Lqfs <= 10178)) && c.Lqzy == ZyID && c.DelFlag == false);
        //                List<T_tdd> list2 = zyMan.GetRecord(where3);
        //                if ((list2 != null) && (list2.Count > 0))
        //                {
        //                    for(int n = 0; n < list2.Count; n++)
        //                    {
        //                        //要先判断年份，如果投档单里有年份字段更方便
        //                        long llNowID = list2[n].RelationID;
        //                        Expression<Func<T_bmk, bool>> where4 = (c => c.ID == llNowID && c.DelFlag == false);
        //                        List<T_bmk> listnf = bmkMan.GetRecord(where4);
        //                        if (listnf[0].YearCode == lYearNum)
        //                        {
        //                            Lqrs += 1;
        //                            if (mark == 0)
        //                            {
        //                                zdf = list2[n].Cj;
        //                                mark = 1;
        //                            }
        //                            if (list2[n].Cj >= zgf)
        //                            {
        //                                zgf = list2[n].Cj;
        //                            }
        //                            else if (list2[n].Cj <= zdf) { zdf = list2[n].Cj; }
        //                            zf += list2[n].Cj;
        //                        }
        //                    }

        //                }
        //                if (Lqrs != 0)
        //                {
        //                    pjf = zf / Lqrs;
        //                }
        //                else { pjf = 0; }
        //                clsExcelApp.SetCellValue(ws, 4 + k, 5, Lqrs);//录取人数
        //                clsExcelApp.SetCellValue(ws, 4 + k, 6, zgf);//最高分
        //                clsExcelApp.SetCellValue(ws, 4 + k, 7, zdf);//最低分
        //                clsExcelApp.SetCellValue(ws, 4 + k, 8, pjf);//平均分
        //                k++;
        //            }

        //        }
        //    }
        //    else//省份+院系+专业
        //    {
        //        int k = 0;//k是最小的计量单位，代表每一行
        //        for (int j = 0; j < arrDepart.Count; j++)
        //        {
        //            long lNowID = Convert.ToInt64(arrDepart[j]);
        //            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.ID == lNowID && c.DelFlag == false);
        //            List<tblDepartmentInfo> xyList = deMan.GetRecord(where);
        //            long[] ZyIDs = Array.ConvertAll(xyList[0].ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //            //ZyIDs.Length;//该学院有几个专业
        //            //省的个数为arrRegion.Count,所以合并单元格的长度为ZyIDs.Length*arrRegion.Count
        //            clsExcelApp.UniteCells(ws, 4 + k, 1, (4 + k) + ZyIDs.Length * arrRegion.Count - 1, 1);
        //            clsExcelApp.SetCellValue(ws, 4 + k, 1, j + 1);
        //            clsExcelApp.UniteCells(ws, 4 + k, 2, (4 + k) + ZyIDs.Length * arrRegion.Count - 1, 2);
        //            clsExcelApp.SetCellValue(ws, 4 + k, 2, xyList[0].Name);
        //            foreach (long ZyID in ZyIDs)//专业ID
        //            {
        //                Expression<Func<Td_zydh, bool>> where2 = (c => c.ID == ZyID && c.DelFlag == false);
        //                List<Td_zydh> list = zyMan.GetRecord(where2);
        //                if ((list != null) && (list.Count > 0))
        //                {
        //                    clsExcelApp.UniteCells(ws, 4 + k, 3, (4 + k) + arrRegion.Count - 1, 3);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 3, list[0].Code);
        //                    clsExcelApp.UniteCells(ws, 4 + k, 4, (4 + k) + arrRegion.Count - 1, 4);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 4, list[0].Name);
        //                }
        //                //省份，计划人数和实际招生数作为最小的一层循环
        //                for (int l = 0; l < arrRegion.Count; l++)
        //                {

        //                    int Lqrs = 0;  //录取人数
        //                    double zgf = 0; // 最高分
        //                    double zdf = 0; // 最低分
        //                    double zf = 0;  // 所有人的总分
        //                    double pjf = 0; //平均分
        //                    int mark = 0;
        //                    long llNowID = Convert.ToInt64(arrRegion[l]);
        //                    string sProvinceName = dqMan.GetNowNamefromID(llNowID);
        //                    Expression<Func<Td_dqdm, bool>> where0 = (c => c.ParentIndex == llNowID && c.DelFlag == false);
        //                    List<Td_dqdm> list0 = dqMan.GetRecord(where0);//报名库的地区代码都是地区代码表中等级为3的
        //                    for (int dq = 0; dq < list0.Count; dq++)//这是等级为2的
        //                    {
        //                        long dqNowID = list0[dq].ID;
        //                        Expression<Func<Td_dqdm, bool>> where00 = (c => c.ParentIndex == dqNowID && c.DelFlag == false);
        //                        List<Td_dqdm> list00 = dqMan.GetRecord(where00);
        //                        for (int dq3 = 0; dq3 < list00.Count; dq3++)
        //                        {
        //                            long dq3NowID = list00[dq3].ID;
        //                            Expression<Func<T_bmk, bool>> where3 = (c => c.Dqdm == dq3NowID && c.YearCode == lYearNum && c.DelFlag == false);
        //                            List<T_bmk> list1 = bmkMan.GetRecord(where3);
        //                            for (int b = 0; b < list1.Count; b++)
        //                            {
        //                                //判断该学生有没有被录取,用录取时间和录取类型代码来判断，录取时间不为空且录取类型代码在我们的ID范围之类
        //                                //录取方式的ID范围在1-165,10163-10178之间
        //                                long NowID1 = list[b].ID;
        //                                Expression<Func<T_tdd, bool>> where4 = (c => c.RelationID == NowID1 && c.Lqsj != null && ((c.Lqfs >= 1 && c.Lqfs <= 165) || (c.Lqfs >= 10163 && c.Lqfs <= 10178)) && c.Lqzy == ZyID);
        //                                List<T_tdd> list2 = tddMan.GetRecord(where4);
        //                                if (list2 != null && list2.Count > 0)
        //                                {
        //                                    Lqrs += 1;
        //                                    if (mark == 0)
        //                                    {
        //                                        zdf = list2[0].Cj;
        //                                        mark = 1;
        //                                    }

        //                                    if (list2[0].Cj >= zgf)
        //                                    {
        //                                        zgf = list2[0].Cj;
        //                                    }
        //                                    else if (list2[0].Cj <= zdf) { zdf = list2[0].Cj; }
        //                                    zf += list2[0].Cj;
        //                                }
        //                            }
        //                        }
        //                    }                                                    
        //                    if (Lqrs != 0)
        //                    {
        //                        pjf = zf / Lqrs;
        //                    }
        //                    else { pjf = 0; }
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 5, sProvinceName); //省份
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 6, Lqrs); //录取人数
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 7, zgf);  //最高分
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 8, zdf);  //最低分
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 9, pjf);  //平均分
        //                    k++;
        //                }

        //            }
        //        }
        //    }
        //}

    }
}