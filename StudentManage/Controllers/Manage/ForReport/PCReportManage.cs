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
    public class PCReportManage: ReportManage
    {
        
        public PCReportManage()
        {
            sTableName = "招生批次表";            
            DynamicKeys = new string[7];
            DynamicKeys[0] = "Department";
            DynamicKeys[1] = "Province";            
            DynamicKeys[2] = "zzmmdmTable";
            DynamicKeys[3] = "mzdmTable";
            DynamicKeys[4] = "byxxdmTable";            
            DynamicKeys[5] = "xbdmTable";
            DynamicKeys[6] = "pcdmTable";
        }

        protected override int SetWholeNum(string sSelTypeIDs, string sSelItems)
        {
            return _SetWholeNum(sSelTypeIDs, sSelItems, 1);//序号
        }

        protected override void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            _AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
        }

        //protected override void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        //{
        //    ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
        //    arrNames.Add("bmk.DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
        //    arrNames.Add("bmk.YearCode"); arrValues.Add(nStartYear); arrSigns.Add(SIGN.GreaterThanOrEqual);
        //    arrNames.Add("bmk.YearCode"); arrValues.Add(nEndYear); arrSigns.Add(SIGN.LessThanOrEqual);
        //    arrNames.Add("bmk.GxID"); arrValues.Add(lGxID); arrSigns.Add(SIGN.Equal);

        //    long[] lIDs = Array.ConvertAll(sSelItems.Split(CosValue.SPLITOPERATOR2), long.Parse);
        //    string[] sTypes = sSelTypeIDs.Split(CosValue.SPLITOPERATOR2);

        //    int nItemPos;
        //    int nRowNumX;
        //    int nRowNumY;
        //    ArrayList ItemKeys;
        //    ArrayList arrItems;
        //    SetLinesPre(ws, sSelItems, sSelTypeIDs, out nItemPos, out nRowNumX, out nRowNumY, out ItemKeys, out arrItems);

        //    var tempMan = new zykTableManage();
        //    _DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns, tempMan);
        //}

        protected override int DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, long ReportTypeID)
        {
            zykTableManage zykMan = new zykTableManage();
            dqdmTableManage dqMan = new dqdmTableManage();
            OriginTableManage tableMan = new OriginTableManage();
            for (int i = 0; i < ((ArrayList)arrItems[nItemPos]).Count; i++)
            {
                if (ItemKeys[nItemPos].ToString() == "pcdmTable")
                    continue;

                ArrayList arrNowNames = new ArrayList(); ArrayList arrNowValues = new ArrayList(); ArrayList arrNowSigns = new ArrayList();
                for (int j = 0; j < arrNames.Count; j++)
                {
                    arrNowNames.Add(arrNames[j]);
                    arrNowValues.Add(arrValues[j]);
                    arrNowSigns.Add(arrSigns[j]);
                }

                long lNowID = Convert.ToInt64(((ArrayList)arrItems[nItemPos])[i]);
                int nRowNowNumY = nRowNumY;
                string sColumnValue = "";
                switch (ItemKeys[nItemPos].ToString())
                {
                    case "Department":
                        zydhTableManage zyMan = new zydhTableManage();
                        tblDepartmentInfo depart = GetOneRecordbyID<tblDepartmentInfo, long>(lNowID);
                        if (depart.ZYIDs != null && depart.ZYIDs != "" && depart.ZYIDs != " ")
                        {
                            long[] ZyIDs = Array.ConvertAll(depart.ZYIDs.Split(CosValue.SPLITOPERATOR2), long.Parse);
                            if (ZyIDs.Length > 0)
                            {
                                clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, depart.Name);
                                nRowNowNumY++;
                                int nRowNumXX = nRowNumX;
                                arrNowNames.Add("jhk.Zydh"); arrNowValues.Add(0); arrNowSigns.Add(SIGN.Equal);
                                for (int j = 0; j < ZyIDs.Length; j++)
                                {
                                    arrNowValues[arrNowValues.Count - 1] = ZyIDs[j];
                                    Td_zydh zymodel = GetOneRecordbyID<Td_zydh, long>(ZyIDs[j]);
                                    int nRowNowNumYY = nRowNowNumY;
                                    if (zymodel != null)
                                    {
                                        //clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumYY, zymodel.Code); nRowNowNumYY++;
                                        clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumYY, zymodel.Name); nRowNowNumYY++;
                                        nRowNumX = _SetLineCore(nItemPos, nRowNumX, ws, ItemKeys, arrItems, arrNowNames, arrNowValues, arrNowSigns, nRowNowNumYY, ReportTypeID);
                                    }
                                }
                                clsExcelApp.UniteCells(ws, nRowNumXX, nRowNowNumY - 1, nRowNumX - 1, nRowNowNumY - 1);
                            }
                        }
                        break;
                    case "Province":
                        arrNowNames.Add("bmk.ProvinceID"); arrNowValues.Add(lNowID); arrNowSigns.Add(SIGN.Equal);
                        string sProvinceName = dqMan.GetNowNamefromID(lNowID);
                        clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, sProvinceName); //省份
                        nRowNowNumY++;
                        nRowNumX = _SetLineCore(nItemPos, nRowNumX, ws, ItemKeys, arrItems, arrNowNames, arrNowValues, arrNowSigns, nRowNowNumY, ReportTypeID);
                        break;
                    default:
                        string sNowTableName = ItemKeys[nItemPos].ToString().Replace("Table", "");
                        sNowTableName = "bmk." + sNowTableName.Substring(0, 1).ToUpper() + sNowTableName.Substring(1, sNowTableName.Length - 1);
                        arrNowNames.Add(sNowTableName); arrNowValues.Add(lNowID); arrNowSigns.Add(SIGN.Equal);
                        sColumnValue = tableMan.GetNowNamefromID(lNowID, ItemKeys[nItemPos].ToString());
                        clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, sColumnValue);
                        nRowNowNumY++;
                        nRowNumX = _SetLineCore(nItemPos, nRowNumX, ws, ItemKeys, arrItems, arrNowNames, arrNowValues, arrNowSigns, nRowNowNumY, ReportTypeID);
                        break;
                }
            }
            return nRowNumX;
        }

        protected override int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            zykTableManage zykMan = new zykTableManage();
            if (nItemPos == ItemKeys.Count - 2)
            {
                arrNowNames.Add("jhk.tddw.Pcdm");arrNowValues.Add(0);arrNowSigns.Add(SIGN.Equal);
                clsExcelApp.SetCellValue(ws, nRowNumX, 1, nRowNumX - 3); //序号
                for (int i = 0; i < ((ArrayList)arrItems[arrItems.Count - 1]).Count; i++)
                {
                    arrNowValues[arrNowValues.Count - 1] = ((ArrayList)arrItems[arrItems.Count - 1])[i];
                    Expression<Func<T_zyk, bool>> where = GetAndLambdaExpression<T_zyk>(arrNowNames, arrNowValues, arrNowSigns);
                    List<T_zyk> listModels = zykMan.GetRecord(where);
                    var ssss = listModels.Select(c => c.RelationID1).Distinct();
                    clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumY, ssss.Count());  //录取人数
                    nRowNowNumY++;
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

            //Range DataRange1 = ws.Range[ws.Cells[4, 2], ws.Cells[rows, ]];
            //Range DataRange2 = ws.Range[ws.Cells[4, cols], ws.Cells[rows, cols]];
            //Range rng = clsExcelApp.app.Union(DataRange1, DataRange2);//录取人数
            Range rng = ws.Range[ws.Cells[4, 2], ws.Cells[rows, cols]];
            if (ChartType == -4102)
            {
                chart.ChartWizard(rng, XlChartType.xl3DPie, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, true, ((Range)ws.Cells[1, 1]).Text , Type.Missing, Type.Missing);
            }
            else
            {
                chart.ChartWizard(rng, ChartType, Type.Missing, XlRowCol.xlColumns, Type.Missing, 0, true, ((Range)ws.Cells[1, 1]).Text, Type.Missing, Type.Missing);
            }
        }

    }
}