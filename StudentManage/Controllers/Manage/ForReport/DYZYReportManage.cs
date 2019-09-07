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
    public class DYZYReportManage : ReportManage
    {

        public DYZYReportManage()
        {
            sTableName = "第一志愿报考专业表";
            DynamicKeys = new string[2];
            DynamicKeys[0] = "Department";
            DynamicKeys[1] = "Province";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {            
            return _SetWholeNum(sSelTypeIDs, sSelItems, 4);//序号，第一志愿报考人数，招生计划，报考人数占招生计划比例
        }

        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            int num = _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            clsExcelApp.SetCellValue(ws, 3, num, "第一志愿报考人数"); num++;
            clsExcelApp.SetCellValue(ws, 3, num, "招生计划"); num++;
            clsExcelApp.SetCellValue(ws, 3, num, "报考人数占招生计划比例");
        }
        //protected override void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        //{
        //    ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
        //    arrNames.Add("bmk.DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
        //    arrNames.Add("bmk.YearCode"); arrValues.Add(nStartYear); arrSigns.Add(SIGN.GreaterThanOrEqual);
        //    arrNames.Add("bmk.YearCode"); arrValues.Add(nEndYear); arrSigns.Add(SIGN.LessThanOrEqual);
        //    arrNames.Add("bmk.GxID"); arrValues.Add(lGxID); arrSigns.Add(SIGN.Equal);

        //    int nItemPos;
        //    int nRowNumX;
        //    int nRowNumY;
        //    ArrayList ItemKeys;
        //    ArrayList arrItems;
        //    SetLinesPre(ws, sSelItems, sSelTypeIDs,out nItemPos,out nRowNumX,out nRowNumY,out ItemKeys,out arrItems);

        //    var tempMan = new zykTableManage();
        //    _DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns,tempMan);
        //}

        //protected override int _DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, object tempMan)
        //{
        //    return _DFSSetLine1(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns, tempMan);
        //}

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            zykTableManage zykMan = new zykTableManage();
            if (nItemPos == ItemKeys.Count - 1)
            {
                clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                Expression<Func<T_zyk, bool>> where = GetAndLambdaExpression<T_zyk>(arrNowNames, arrNowValues, arrNowSigns);
                List<T_zyk> listModels = zykMan.GetRecord(where);
                if (listModels.Count > 0)
                {                    
                    int count = 0;
                    for (int i = 0; i < listModels.Count; i++)
                    {
                        long lNowRelationID = listModels[i].RelationID1;
                        Expression<Func<T_zyk, bool>> where1 = (c => c.RelationID1 == lNowRelationID);
                        List<T_zyk> listNow = zykMan.GetRecord(where1).OrderBy(c => c.Zyxh).ToList();
                        if (listNow[0].ID == listModels[i].ID)
                            count++;
                    }
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, count); nRowNowNumY++; //第一志愿报考人数
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, listModels[0].jhk.Jhrs); nRowNowNumY++; //招生计划
                    string bili = string.Format("{0:P}", count * 1.0 / listModels[0].jhk.Jhrs);
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, bili);  //报考人数占招生计划比例                    
                }
                else
                {
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, 0); nRowNowNumY++; //第一志愿报考人数
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, 0); nRowNowNumY++; //招生计划
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, "无法计算");  //报考人数占招生计划比例
                }
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


            Range rng = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols-1]];
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text, Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text, Type.Missing, Type.Missing);
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

        //    jhkTableManage jhkMan = new jhkTableManage();
        //    DepartmentInfoManage deMan = new DepartmentInfoManage();
        //    zydhTableManage zyMan = new zydhTableManage();
        //    dqdmTableManage dqMan = new dqdmTableManage();
        //    bmkTableManage bmkMan = new bmkTableManage();
        //    tddTableManage tddMan = new tddTableManage();
        //    zydh_OriginTableManage zyOriginMan = new zydh_OriginTableManage();
        //    zykTableManage zykMan = new zykTableManage();

        //    if ((arrRegion.Count > 0) && (arrDepart.Count == 0)) //只有省份没有院系,这个没实现
        //    {
        //        #region
        //        //int k = 0;
        //        //for (int j = 0; j < arrRegion.Count; j++)
        //        //{
        //        //    long lNowID = Convert.ToInt64(arrRegion[j]);

        //        //    Expression<Func<T_bmk, bool>> where = (c => c.Dqdm == lNowID && c.YearCode == lYearNum  && c.DelFlag == false);
        //        //    List<T_bmk> list = bmkMan.GetRecord(where);
        //        //    List<long> arrZym = new List<long>(); //第一志愿的集合                   
        //        //    for(int r=0;r<list.Count;r++)
        //        //    {
        //        //        long llNowID = list[r].ID;
        //        //        Expression<Func<T_zyk, bool>> where2 = (c => c.RelationID1 == llNowID && c.YearCode == lYearNum );
        //        //        List<T_zyk> list2 = zykMan.GetRecord(where2);
        //        //        long zyID = list2[0].ZydhOrigin;
        //        //        arrZym.Add(zyID);

        //        //    }
        //        //    List<long> arrzym = arrZym.Distinct().ToList();//这些省份的考生报考的第一志愿有哪些，接下来统计人数
        //        //    int zycount = 0;//专业数
        //        //    foreach (long zyID in arrzym)
        //        //    {
        //        //        int bkrs = 0;
        //        //        Expression<Func<Td_zydh_Origin, bool>> where3 = (c => c.ID == zyID && c.YearCode == lYearNum && c.DelFlag == false);
        //        //        List<Td_zydh_Origin> list3 = zyOriginMan.GetRecord(where3);
        //        //        if (list3 != null && list3.Count > 0)
        //        //        {
        //        //            long NowzyID = list3[0].RelationID;//第一志愿专业代号

        //        //            for(int m = 0; m < arrZym.Count; m++)
        //        //            {
        //        //                if (arrZym[m] == zyID) { bkrs += 1; }
        //        //            }
        //        //            Expression<Func<Td_zydh, bool>> where4 = (c => c.ID == NowzyID && c.DelFlag == false);
        //        //            List<Td_zydh> list4 = zyMan.GetRecord(where4);
        //        //            if ((list4 != null) && (list4.Count > 0))
        //        //            {
        //        //                zycount++;
        //        //                clsExcelApp.SetCellValue(ws, 4 + k, 3, list4[0].Name);
        //        //                clsExcelApp.SetCellValue(ws, 4 + k, 4, bkrs);

        //        //                Expression<Func<T_jhk, bool>> where5 = (c => c.Zydh == NowzyID && c.ProvinceID == lNowID && c.YearCode == lYearNum && c.ParentDepartmentID == lParentDepartID && c.DelFlag == false);
        //        //                List<T_jhk> list5 = jhkMan.GetRecord(where5);
        //        //                if ((list5 != null) && (list5.Count > 0))
        //        //                {
        //        //                    clsExcelApp.SetCellValue(ws, 4 + k, 5, list5[0].Jhrs); //招生计划
        //        //                    if (list5[0].Jhrs != 0)
        //        //                    {
        //        //                        string bl = (bkrs / list5[0].Jhrs) * 100 + "%";
        //        //                        clsExcelApp.SetCellValue(ws, 4 + k, 6, bl);
        //        //                    }
        //        //                    else
        //        //                        clsExcelApp.SetCellValue(ws, 4 + k, 6, "无法计算");
        //        //                }
        //        //                else
        //        //                {
        //        //                    clsExcelApp.SetCellValue(ws, 4 + k, 5, 0); //招生计划
        //        //                    clsExcelApp.SetCellValue(ws, 4 + k, 6, "无法计算");
        //        //                }
        //        //            }
        //        //        }
        //        //        k++;
        //        //    }
        //        //    string sProvinceName = dqMan.GetNowNamefromID(lNowID);
        //        //    if (k != 0)
        //        //    {
        //        //        clsExcelApp.UniteCells(ws, 4 + k - zycount, 1, 4 + k - 1, 1);
        //        //        clsExcelApp.UniteCells(ws, 4 + k - zycount, 2, 4 + k - 1, 2);                        
        //        //        clsExcelApp.SetCellValue(ws, 4 + k - zycount, 1, j + 1); //序号
        //        //        clsExcelApp.SetCellValue(ws, 4 + k - zycount, 2, sProvinceName); //省份
        //        //    }
        //        //    else
        //        //    {
        //        //        clsExcelApp.SetCellValue(ws, 4 + k , 1, j + 1); //序号
        //        //        clsExcelApp.SetCellValue(ws, 4 + k , 2, sProvinceName); //省份
        //        //    }

        //        //}
        //        #endregion
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

        //                Expression<Func<Td_zydh, bool>> where2 = (c => c.ID == ZyID && c.DelFlag == false);
        //                List<Td_zydh> list = zyMan.GetRecord(where2);
        //                if ((list != null) && (list.Count > 0))
        //                {
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 3, list[0].Code);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 4, list[0].Name);
        //                }

        //                float bkrs = 0;float jhrs = 0;//方便做除法运算
        //                Expression<Func<Td_zydh_Origin, bool>> where3 = (c => c.RelationID == ZyID && c.YearCode == lYearNum && c.DelFlag == false);
        //                List<Td_zydh_Origin> list2 = zyOriginMan.GetRecord(where3);
        //                if ((list2 != null) && (list2.Count > 0))
        //                {
        //                    long llNowID = list2[0].ID;
        //                    //首先把志愿库里所有考生的该志愿找出来
        //                    Expression<Func<T_zyk, bool>> where4 = (c => c.ZydhOrigin == llNowID && c.bmk.YearCode == lYearNum);//年份的处理？自己添加了个年份
        //                    List<T_zyk> list3 = zykMan.GetRecord(where4);
        //                    for (int t = 0; t < list3.Count; t++)
        //                    {
        //                        if (list3[t].ID == 1) { bkrs += 1; }
        //                        else
        //                        {
        //                            long IID = list3[t].ID - 1;
        //                            Expression<Func<T_zyk, bool>> where5 = (c => c.ID == IID && c.bmk.YearCode == lYearNum);
        //                            List<T_zyk> list4 = zykMan.GetRecord(where5);
        //                            if (list4[0].RelationID1 != list3[t].RelationID1) { bkrs += 1; }//判断是不是第一志愿
        //                        }
        //                    }
        //                }
        //                clsExcelApp.SetCellValue(ws, 4 + k, 5, bkrs);
        //                Expression<Func<T_jhk, bool>> where6 = (c => c.Zydh == ZyID && c.YearCode == lYearNum && c.ParentDepartmentID == lParentDepartID && c.DelFlag == false);
        //                List<T_jhk> list5 = jhkMan.GetRecord(where6);
        //                if ((list5 != null) && (list5.Count > 0))
        //                {
        //                    jhrs = list5[0].Jhrs;
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 6, jhrs);
        //                    if (list5[0].Jhrs != 0)
        //                    {
        //                        float bl = bkrs / jhrs;
        //                        string bls = bl * 100 + "%";
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, bls);
        //                    }
        //                    else
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, "无法计算");
        //                }

        //                else
        //                {
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 6, 0);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 7, "无法计算");
        //                }

        //                k++;
        //            }
        //        }
        //    }
        //    else//省份+院系+专业，数据量大时运行得慢
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

        //                Expression<Func<Td_zydh_Origin, bool>> where3 = (c => c.RelationID == ZyID && c.YearCode == lYearNum && c.DelFlag == false);
        //                List<Td_zydh_Origin> list2 = zyOriginMan.GetRecord(where3);
        //                long lNowID2;         //志愿库中的专业代号Origin
        //                if (list2 != null && list2.Count > 0)
        //                {
        //                     lNowID2 = list2[0].ID;
        //                }
        //                else
        //                     lNowID2 = 0;
        //                //省份，计划人数和实际招生数作为最小的一层循环
        //                for (int l = 0; l < arrRegion.Count; l++)
        //                {
        //                    float bkrs = 0;float jhrs = 0;      //方便做除法
        //                    //先判断省份
        //                    long llNowID = Convert.ToInt64(arrRegion[l]);
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
        //                            Expression<Func<T_bmk, bool>> wheres = (c => c.Dqdm == dq3NowID && c.YearCode == lYearNum && c.DelFlag == false);
        //                            List<T_bmk> lists = bmkMan.GetRecord(wheres);
        //                            foreach (T_bmk s in lists)
        //                            {
        //                                long sID = s.ID;
        //                                Expression<Func<T_zyk, bool>> where4 = (c => c.ZydhOrigin == lNowID2 && c.RelationID1 == sID && c.bmk.YearCode == lYearNum);
        //                                List<T_zyk> list3 = zykMan.GetRecord(where4);
        //                                if (list3 != null && list3.Count > 0)
        //                                {
        //                                    if (list3[0].ID == 1)//判断是不是第一志愿
        //                                    {
        //                                        bkrs += 1;
        //                                    }
        //                                    else
        //                                    {
        //                                        long IID = list3[0].ID - 1;
        //                                        Expression<Func<T_zyk, bool>> where5 = (c => c.ID == IID && c.bmk.YearCode == lYearNum);
        //                                        List<T_zyk> list4 = zykMan.GetRecord(where5);
        //                                        if (list4[0].RelationID1 != sID) { bkrs += 1; }//判断是不是第一志愿
        //                                    }
        //                                }
        //                            }

        //                        }
        //                    }

        //                    string sProvinceName = dqMan.GetNowNamefromID(llNowID);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 5, sProvinceName);
        //                    clsExcelApp.SetCellValue(ws, 4 + k, 6, bkrs);

        //                    Expression<Func<T_jhk, bool>> where6 = (c => c.Zydh == ZyID && c.ProvinceID == llNowID && c.YearCode == lYearNum && c.ParentDepartmentID == lParentDepartID && c.DelFlag == false);
        //                    List<T_jhk> list5 = jhkMan.GetRecord(where6);                
        //                    if ((list5 != null) && (list5.Count > 0))
        //                    {
        //                        jhrs = list5[0].Jhrs;
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, jhrs); //招生计划
        //                        if (list5[0].Jhrs != 0)
        //                        {
        //                            float bl = bkrs / jhrs;
        //                            string bls = bl * 100 + "%";
        //                            clsExcelApp.SetCellValue(ws, 4 + k, 8, bls);
        //                        }
        //                        else
        //                            clsExcelApp.SetCellValue(ws, 4 + k, 8, "无法计算");
        //                    }
        //                    else
        //                    {
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 7, 0); //招生计划
        //                        clsExcelApp.SetCellValue(ws, 4 + k, 8, "无法计算");
        //                    }
        //                    k++;
        //                }

        //            }
        //        }
        //    }
        //}

    }
}