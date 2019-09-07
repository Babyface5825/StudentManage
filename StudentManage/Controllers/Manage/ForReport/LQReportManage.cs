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
    /// 录取，报表生成
    /// </summary>
    public class LQReportManage : ReportManage
    {
        public LQReportManage()
        {
            sTableName = "录取情况分析表";
            DynamicKeys = new string[10];
            DynamicKeys[0] = "Department";
            DynamicKeys[1] = "Province";
            DynamicKeys[2] = "bylbdmTable";
            DynamicKeys[3] = "zzmmdmTable";
            DynamicKeys[4] = "mzdmTable";
            DynamicKeys[5] = "kslbdmTable";
            DynamicKeys[6] = "kslxdmTable";
            DynamicKeys[7] = "byxxdmTable";
            DynamicKeys[8] = "hjlbdmTable";
            DynamicKeys[9] = "xbdmTable";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {
            return _SetWholeNum(sSelTypeIDs, sSelItems, 2);//序号，录取人数
        }

        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            int num = _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            clsExcelApp.SetCellValue(ws, 3, num, "录取人数");
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
        //    SetLinesPre(ws, sSelItems, sSelTypeIDs, out nItemPos, out nRowNumX, out nRowNumY, out ItemKeys, out arrItems);

        //    var tempMan = new tddTableManage();
        //    _DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns,tempMan);
        //}

        //protected override int _DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns,object tempMan)
        //{
        //     return _DFSSetLine1(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns,tempMan);
        //}

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            tddTableManage tddMan = new tddTableManage();
            if (nItemPos == ItemKeys.Count - 1)
            {
                Expression<Func<T_tdd, bool>> where = GetAndLambdaExpression<T_tdd>(arrNowNames, arrNowValues, arrNowSigns);
                int count = tddMan.GetRecordCount(where);
                clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, count);  //录取人数
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

            Range DataRange1 = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols - 1]];
            Range DataRange2 = ws.Range[ws.Cells[4, cols], ws.Cells[rows, cols]];
            Range rng = clsExcelApp.app.Union(DataRange1, DataRange2);//录取人数
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols - 1]).Text, Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text + "" + ((Range)ws.Cells[3, cols - 1]).Text, ws.Cells[3, 4], ws.Cells[3, cols - 1]);
            }
        }
    }
}