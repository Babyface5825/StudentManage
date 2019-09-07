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
    /// 计划库除了省份和院系/专业，没有其他类型
    /// </summary>
    public class JHReportManage : ReportManage
    {
        public JHReportManage()
        {
            sTableName = "招生计划及录取情况表";
            DynamicKeys = new string[2];
            DynamicKeys[0] = "Department";
            DynamicKeys[1] = "Province";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {
            return _SetWholeNum(sSelTypeIDs, sSelItems, 3);//序号，计划招生数，实际录取数
        }

        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            int num = _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            clsExcelApp.SetCellValue(ws, 3, num, "计划招生数"); num++;
            clsExcelApp.SetCellValue(ws, 3, num, "计划执行数");
        }

        //protected override void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear, long ReportTypeID)
        //{            
        //    ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
        //    arrNames.Add("DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
        //    arrNames.Add("YearCode"); arrValues.Add(nStartYear); arrSigns.Add(SIGN.GreaterThanOrEqual);
        //    arrNames.Add("YearCode"); arrValues.Add(nEndYear); arrSigns.Add(SIGN.LessThanOrEqual);            
        //    arrNames.Add("GxID"); arrValues.Add(lGxID); arrSigns.Add(SIGN.Equal);

        //    int nItemPos;
        //    int nRowNumX;
        //    int nRowNumY;
        //    ArrayList ItemKeys = new ArrayList(); 
        //    ArrayList arrItems = new ArrayList(); 

        //    SetLinesPre(ws, sSelItems, sSelTypeIDs, out nItemPos, out nRowNumX, out nRowNumY, ItemKeys, arrItems);
        //    var tempMan = new jhkTableManage();
        //    _DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns, tempMan);
        //}


        //protected override int _DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, object tempMan)
        //{
        //    return _DFSSetLine1(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns, tempMan);
        //}

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            jhkTableManage jhkMan = new jhkTableManage();
            if (nItemPos == ItemKeys.Count - 1)
            {
                Expression<Func<T_jhk, bool>> where = GetAndLambdaExpression<T_jhk>(arrNowNames, arrNowValues, arrNowSigns);
                List<T_jhk> list = jhkMan.GetRecord(where);
                int Jhrs = 0, Jhzxs = 0;
                if (list != null && list.Count > 0)
                {
                    foreach (T_jhk l in list)
                    {
                        Jhrs += l.Jhrs;
                        Jhzxs += l.Jhzxs;
                    }
                }
                clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, Jhrs);  //计划人数
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY + 1, Jhzxs);  //计划执行数
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
            Shape shape1 = ws.Shapes.AddChart(Type.Missing, 300, 260, Type.Missing, Type.Missing);
            Chart chart = shape.Chart;
            Chart chart1 = shape1.Chart;
            chart.HasTitle = false;
            chart1.HasTitle = false;

            Range DataRange1 = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols - 2]];
            Range DataRange2 = ws.Range[ws.Cells[4, cols - 1], ws.Cells[rows, cols - 1]];
            Range DataRange3 = ws.Range[ws.Cells[4, cols], ws.Cells[rows, cols]];
            Range rng = clsExcelApp.app.Union(DataRange1, DataRange2);//专业计划招生数
            Range rng1 = clsExcelApp.app.Union(DataRange1, DataRange3);//计划执行数
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols - 1]).Text, Type.Missing, Type.Missing);
                chart1.ChartWizard(rng1, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols]).Text, Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols - 1]).Text, ws.Cells[3, 4], ws.Cells[3, cols - 1]);
                chart1.ChartWizard(rng1, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols]).Text, ws.Cells[3, 4], ws.Cells[3, cols]);
            }
        }
    }
}