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
    /// 生源地统计表，报表生成
    /// </summary>
    public class SYDReportManage: ReportManage
    {
        public SYDReportManage()
        {
            sTableName = "生源地统计表";
            DynamicKeys = new string[1];
            DynamicKeys[0] = "Province";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {            
            return _SetWholeNum(sSelTypeIDs, sSelItems, 3);//序号，区市，录取人数
        }


        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            int num = _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            clsExcelApp.SetCellValue(ws, 3, num, "区市"); num++;
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
        //    _DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns, tempMan);
        //}

        //protected override int _DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, object tempMan)
        //{
        //    return _DFSSetLine1(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns,tempMan);
        //}

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            tddTableManage tddMan = new tddTableManage();
            dqdmTableManage dqMan = new dqdmTableManage();
                        
            Expression<Func<T_tdd, bool>> where = GetAndLambdaExpression<T_tdd>(arrNowNames, arrNowValues, arrNowSigns);
            List<T_tdd> listModels = tddMan.GetRecord(where);
            string[] arr = new string[listModels.Count];
            if (listModels.Count > 0)
            {                    
                for (int i = 0; i < listModels.Count; i++)
                {
                    if (listModels[i].bmk.dq != null)
                    {
                        string cityCode = listModels[i].bmk.dq.Code.Substring(0, 4) + "00";//获得市的行政代码
                        arr[i] = cityCode;
                    }
                }
                var list = from a in arr
                            group a by a into G
                            orderby G.Key
                            select new
                            {
                                sCode = G.Key,
                                count = G.Count()
                            };
                foreach (var l in list)
                {
                    nRowNowNumY = 3;
                    string cityName = dqMan.GetNowNamefromCode(l.sCode);
                    clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, cityName); nRowNowNumY++;
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, l.count); 
                    nRowNumX++;
                }
                nRowNumX--;
            }
            else
            {
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, "无"); nRowNowNumY++;
                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, 0); nRowNowNumY++;
            }
            nRowNumX++;

            clsExcelApp.UniteCells(ws, nUnitRow1, 2, nRowNumX - 1, 2);

            //if (nItemPos == 0)
                nUnitRow1 = nRowNumX;
            //else if (nItemPos == 1)
            //    nUnitRow2 = nRowNumX;
            //else if (nItemPos == 2)
            //    nUnitRow3 = nRowNumX;
            
            //UnitCellCore(nItemPos, nRowNumX, ws, ItemKeys);
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


            Range rng = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols]];
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text, Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text, Type.Missing, Type.Missing);
            }
        }


    }
}