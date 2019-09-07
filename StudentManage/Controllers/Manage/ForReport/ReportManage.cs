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
using System.Threading;
using Ext.Net;
using System.Linq;

namespace StudentManage.Manage
{
    /// <summary>
    /// 报表生成
    /// </summary>
    public class ReportManage : ParentManage
    {
        /// <summary>
        /// 报表存放的路径，在ReportManage类中定义并写死
        /// </summary>
        protected string sFileAddress = ConstAddressPath.REPORT_WEBPATH;
        /// <summary>
        /// 生成excel中的sheet名称，需要在继承类中定义
        /// </summary>
        protected string sTableName;
        /// <summary>
        /// 所有动态列关键字，继承类中定义
        /// </summary>
        protected string[] DynamicKeys;
        /// <summary>
        /// 生成excel中的实际列数
        /// </summary>
        //protected int nWholeColumnNum;
        /// <summary>
        /// 高校名称
        /// </summary>
        protected string GxName;

        protected int nUnitRow1 = 0;
        protected int nUnitRow2 = 0;
        protected int nUnitRow3 = 0;
        protected int nUnitRow4 = 0;
        protected int nUnitColumn1 = 0;
        protected int nUnitColumn2 = 0;
        protected int nUnitColumn3 = 0;
        protected int nUnitColumn4 = 0;

        //在AddColumnHeader表示第num列
        //protected int num = 1;
        //protected List<object> obList;
        protected object tempMan;

        protected CExcelApp clsExcelApp = new CExcelApp();
        protected FileManager clsFile = new FileManager();
        
        public string sReportTypeID = "";
        public string sReportTypeName = "";//不同类型的报表名称
               
        public ReportManage()
        {

        }

        protected void GetGxName(long lGxID)
        {
            gxdmTableDataListManage man = new gxdmTableDataListManage();
            Td_gxdm model = man.GetOneRecordbyID<Td_gxdm, long>(lGxID);
            if (model != null)
                GxName = model.Name;
        }

        /// <summary>
        /// 根据高校ID获取ParentDepartmentID
        /// </summary>
        /// <param name="sGxID"></param>
        /// <returns></returns>
        public long GetParentDepartIDfromGxID(long lGxID)
        {
            DepartmentInfoManage man = new DepartmentInfoManage();
            Expression<Func<tblDepartmentInfo, bool>> where = (c => c.GxID == lGxID && c.DelFlag == false);
            List<tblDepartmentInfo> list = man.GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0].ID;
            else
                return 0;
        }

        /// <summary>
        /// 计算报表总共多少列
        /// </summary>
        /// <param name="sSelTypeIDs"></param>
        /// <param name="sSelItems"></param>
        /// <returns></returns>
        protected virtual int  SetWholeNum(string sSelTypeIDs, string sSelItems)
        {
            return 0;
        }

        protected virtual int _SetWholeNum(string sSelTypeIDs, string sSelItems, int nWholeColumnNum)
        {
            List<string> ss = new List<string>(sSelTypeIDs.Split(CosValue.SPLITOPERATOR2));
            string sss =  string.Join(CosValue.SPLITOPERATOR1.ToString(), ss.Distinct().ToList());
            int nfind = 0;
            int nNowColumnNum = 1;
            foreach (string s in DynamicKeys) //string s in sss)
            {
                if (sss.Contains(s))
                {
                    switch (s)
                    {
                        case "Department": //院系始终放最前面//院系，专业代码, 专业名称
                            nUnitColumn1 = 3;
                            //nUnitColumn2 = 4;
                            nNowColumnNum = 3;
                            nfind++; 
                            break;
                        case "pcdmTable":
                            for (int i = 0; i < ss.Count; i++)
                                if (ss[i] == "pcdmTable")
                                    nNowColumnNum++;
                            break;
                        default:
                            nfind++;
                            nNowColumnNum += 1;
                            if (nfind == 1)
                                nUnitColumn1 = nNowColumnNum;
                            else if (nfind == 2) 
                                nUnitColumn2 = nNowColumnNum;
                            else if (nfind == 3)
                                nUnitColumn3 = nNowColumnNum;
                            break;
                    }
                }
            }
            nWholeColumnNum = nWholeColumnNum + nNowColumnNum - 1;
            return nWholeColumnNum;
        }

        protected virtual void WriteDataToExcel(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear, long ReportTypeID)
        {                        
            AddTitleHeader(ws, sSelItems, sSelTypeIDs, nStartYear, nEndYear);
            AddColumnHeader(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear);
            SetLines(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear, ReportTypeID);
        }

        /// <summary>
        /// 创建excel的头
        /// </summary>
        /// <param name="ws"></param>
        protected virtual void AddTitleHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, int nStartYear, int nEndYear)
        {
            int nWholeColumnNum = SetWholeNum(sSelTypeIDs, sSelItems);
            clsExcelApp.UniteCells(ws, 1, 1, 1, nWholeColumnNum);
            clsExcelApp.SetCellProperty(ws, 1, 1, 1, nWholeColumnNum, 16, "楷体_GB2312", true, Constants.xlCenter);
            string datestr;
            if (nEndYear == nStartYear)
                datestr = nStartYear + "年";
            else
                datestr = nStartYear + "-" + nEndYear + "年";
            clsExcelApp.SetCellValue(ws, 1, 1, GxName + datestr + sTableName);

            clsExcelApp.UniteCells(ws, 2, 1, 2, nWholeColumnNum);
            clsExcelApp.SetCellProperty(ws, 2, 1, 2, 1, 12, "宋体", false, Constants.xlRight);            
            clsExcelApp.SetCellValue(ws, 2, 1, datestr);
        }



        /// <summary>
        /// 设定excel的表格头，需要在继承类中重写
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="chartTable"></param>
        protected virtual void AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {

        }

        protected int _AddColumnHeader(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear)
        {
            List<string> ss = new List<string>(sSelTypeIDs.Split(CosValue.SPLITOPERATOR2));
            List<string> sss = ss.Distinct().ToList();
            int  num = 1;
            clsExcelApp.SetCellValue(ws, 3, num, "序号"); num++;
            foreach (string s in DynamicKeys)
            {
                if (sss.Contains(s))
                {
                    switch (s)
                    {
                        case "Department":
                            clsExcelApp.SetCellValue(ws, 3, num, "院系"); num++;
                            clsExcelApp.SetCellValue(ws, 3, num, "专业名称"); num++;
                            //clsExcelApp.SetCellValue(ws, 3, num, "专业代码"); num++;
                            break;
                        case "pcdmTable":
                            List<string> ItemIDs = new List<string>(sSelItems.Split(CosValue.SPLITOPERATOR2));
                            for (int i = 0; i < ss.Count; i++)
                            {
                                if (ss[i] == "pcdmTable")
                                {
                                    pcdmTableManage man = new pcdmTableManage();
                                    Td_pcdm model = man.GetOneRecordbyID<Td_pcdm, string>(ItemIDs[i]);
                                    if (model != null)
                                    {
                                        clsExcelApp.SetCellValue(ws, 3, num, model.Name);
                                        num++;
                                    }
                                }
                            }
                            break;
                        default:
                            OriginTableManage tableMan = new OriginTableManage();
                            string sKeyWords = tableMan.GetKeyWords(s);
                            clsExcelApp.SetCellValue(ws, 3, num, sKeyWords);
                            num++;
                            break;
                    }
                }
            }
            return num;
        }

        /// <summary>
        /// 设定excel的具体内容，需要在继承类中重写
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="sSelItems"></param>
        /// <param name="sSelTypeIDs"></param>
        protected virtual void SetLines(Worksheet ws, string sSelItems, string sSelTypeIDs, long lGxID, int nStartYear, int nEndYear, long ReportTypeID)
        {
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            switch (ReportTypeID)
            {
                case 1: //计划库
                    arrNames.Add("DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
                    arrNames.Add("YearCode"); arrValues.Add(nStartYear); arrSigns.Add(SIGN.GreaterThanOrEqual);
                    arrNames.Add("YearCode"); arrValues.Add(nEndYear); arrSigns.Add(SIGN.LessThanOrEqual);
                    arrNames.Add("GxID"); arrValues.Add(lGxID); arrSigns.Add(SIGN.Equal);
                    break;
                default:
                    arrNames.Add("bmk.DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
                    arrNames.Add("bmk.YearCode"); arrValues.Add(nStartYear); arrSigns.Add(SIGN.GreaterThanOrEqual);
                    arrNames.Add("bmk.YearCode"); arrValues.Add(nEndYear); arrSigns.Add(SIGN.LessThanOrEqual);
                    arrNames.Add("bmk.GxID"); arrValues.Add(lGxID); arrSigns.Add(SIGN.Equal);
                    break;
            }

            int nItemPos;
            int nRowNumX;
            int nRowNumY;
            ArrayList ItemKeys = new ArrayList(); 
            ArrayList arrItems = new ArrayList(); 

            SetLinesPre(ws, sSelItems, sSelTypeIDs, out nItemPos, out nRowNumX, out nRowNumY, ItemKeys, arrItems);            
            DFSSetLine(nItemPos, nRowNumX, nRowNumY, ws, ItemKeys, arrItems, arrNames, arrValues, arrSigns,  ReportTypeID);
        }

        protected void SetLinesPre(Worksheet ws, string sSelItems, string sSelTypeIDs, out int nItemPos, out int nRowNumX, out int nRowNumY, ArrayList ItemKeys, ArrayList arrItems)
        {
            long[] lIDs = Array.ConvertAll(sSelItems.Split(CosValue.SPLITOPERATOR2), long.Parse);
            string[] sTypes = sSelTypeIDs.Split(CosValue.SPLITOPERATOR2);
            foreach (string s in DynamicKeys)
            {
                bool flag = false;
                ArrayList arrItemTemp = new ArrayList();
                for (int i = 0; i < sTypes.Length; i++)
                {
                    if (sTypes[i] == s)
                    {
                        if (!flag)
                        {
                            flag = true;
                            ItemKeys.Add(s);
                        }
                        arrItemTemp.Add(lIDs[i]);
                    }
                }
                if (flag)
                    arrItems.Add(arrItemTemp);
            }
            nRowNumX = 4;
            nRowNumY = 2;
            nItemPos = 0;
            nUnitRow1 = 4;
            if (ItemKeys.Count > 1)
                nUnitRow2 = 4;
            if (ItemKeys.Count > 2)
                nUnitRow3 = 4;
        }

        //protected virtual int _DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, object tempMan)
        //{
        //    return nRowNumX;
        //}

        protected virtual int DFSSetLine(int nItemPos, int nRowNumX, int nRowNumY, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNames, ArrayList arrValues, ArrayList arrSigns, long ReportTypeID)
        {
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
                                
                                switch (ReportTypeID)
                                {
                                    case 1: //计划库
                                        arrNowNames.Add("Zydh"); arrNowValues.Add(0); arrNowSigns.Add(SIGN.Equal);break;
                                    case 3: //批次，第一志愿
                                    case 4:
                                        arrNowNames.Add("jhk.Zydh"); arrNowValues.Add(0); arrNowSigns.Add(SIGN.Equal); break;
                                    default:
                                        arrNowNames.Add("Lqzy"); arrNowValues.Add(0); arrNowSigns.Add(SIGN.Equal);break;
                                }
                                for (int j = 0; j < ZyIDs.Length; j++)
                                {
                                    arrNowValues[arrNowValues.Count - 1] = ZyIDs[j];
                                    Td_zydh zymodel = GetOneRecordbyID<Td_zydh, long>(ZyIDs[j]);
                                    int nRowNowNumYY = nRowNowNumY;
                                    if (zymodel != null)
                                    {
                                        clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumYY, zymodel.Name); nRowNowNumYY++;
                                        //clsExcelApp.SetCellValue(ws, nRowNumX, nRowNowNumYY, zymodel.Code); nRowNowNumYY++;
                                        nRowNumX = _SetLineCore(nItemPos, nRowNumX, ws, ItemKeys, arrItems, arrNowNames, arrNowValues, arrNowSigns, nRowNowNumYY, ReportTypeID);
                                    }
                                }
                                clsExcelApp.UniteCells(ws, nRowNumXX, nRowNowNumY - 1, nRowNumX - 1, nRowNowNumY - 1);

                            }
                        }
                        break;
                    case "Province":
                        if (ReportTypeID == 1) //因为jhk里直接有ProvinceID字段,否则前面要加bmk来找
                            arrNowNames.Add("ProvinceID");
                        else
                            arrNowNames.Add("bmk.ProvinceID");
                        arrNowValues.Add(lNowID); arrNowSigns.Add(SIGN.Equal);

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


        protected virtual int _SetLineCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys, ArrayList arrItems, ArrayList arrNowNames, ArrayList arrNowValues, ArrayList arrNowSigns, int nRowNowNumY, long ReportTypeID)
        {
            return nRowNumX;
        }

        protected void UnitCellCore(int nItemPos, int nRowNumX, Worksheet ws, ArrayList ItemKeys)
        {
            if (nItemPos < ItemKeys.Count - 1)
            {
                switch (nItemPos)
                {
                    case 0:
                        if ((nUnitRow1 != nUnitRow2) && (nUnitRow2 != 0))
                        {
                            for (int j = nUnitColumn1; j < nUnitColumn2; j++)
                                clsExcelApp.UniteCells(ws, nUnitRow1, j, nUnitRow2 - 1, j);
                            nUnitRow1 = nUnitRow2;
                        }
                        break;
                    case 1:
                        if ((nUnitRow2 != nUnitRow3) && (nUnitRow3 != 0))
                        {
                            for (int j = nUnitColumn2; j < nUnitColumn3; j++)
                                clsExcelApp.UniteCells(ws, nUnitRow2, j, nUnitRow3 - 1, j);
                            nUnitRow2 = nUnitRow3;
                        }
                        break;
                }
                if (nItemPos == 0)
                    nUnitRow1 = nRowNumX;
                else if (nItemPos == 1)
                    nUnitRow2 = nRowNumX;
                else if (nItemPos == 2)
                    nUnitRow3 = nRowNumX;
            }
        }

        //生产报表这一部分开启一个新的线程
        public virtual void CreateReport(string sSelItems, string sSelTypeIDs, string sID, string sGxID, string sStartYear, string sEndYear,string sChartType, long ReportTypeID)
        {
            //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CreateReportThread);
            //Thread myThread = new Thread(ParStart);
            string returnMsg = "正在创建...";
            tblReportInfo model = GetOneRecordbyID<tblReportInfo, string>(sID);
            if (model.HandleFlag == "创建完成")
                returnMsg = "该报表已经创建完成!";
            else
            {
                object[] o = new object[] { sSelItems, sSelTypeIDs, sID, sGxID, sStartYear, sEndYear, sChartType, ReportTypeID };//装箱
                CreateReportThread(o);
            }

            //myThread.Start(o);

        }

        private void CreateReportThread(object ParaObject)
        {
            //开箱
            object[] o = (object[])ParaObject;
            string sSelItems = (string)o[0];
            string sSelTypeIDs = (string)o[1];
            long lID = Convert.ToInt64(o[2]);
            long lGxID = Convert.ToInt64(o[3]);
            int nStartYear = Convert.ToInt32(o[4]);
            int nEndYear = Convert.ToInt32(o[5]);
            int nChartType = 0;            
            if ((o[6].ToString() != ""))
                nChartType = Convert.ToInt32(o[6]);
            long ReportTypeID = Convert.ToInt16(o[7]);
            GetGxName(lGxID);
            try
            {
                clsExcelApp.Create();
                for (int i = nStartYear; i <= nEndYear; i++)
                {
                    Worksheet ws = new Worksheet();
                    ws = clsExcelApp.AddSheet(i.ToString());//sTableName
                    WriteDataToExcel(ws, sSelItems, sSelTypeIDs, lGxID, i, i, ReportTypeID);
                    if (nChartType != 0)
                        CreateChart(ws, sSelTypeIDs, nChartType, ReportTypeID);
                }
                if (nStartYear < nEndYear)
                {
                    Worksheet ws = new Worksheet();
                    ws = clsExcelApp.AddSheet(nStartYear.ToString() + "-" + nEndYear.ToString());
                    WriteDataToExcel(ws, sSelItems, sSelTypeIDs, lGxID, nStartYear, nEndYear, ReportTypeID);
                    if (nChartType != 0)
                        CreateChart(ws, sSelTypeIDs, nChartType, ReportTypeID);
                }
                clsExcelApp.DelSheet("Sheet1");
            }
            catch (Exception e)//这一部分可能会报错，原因通常是excel的权限问题
            {
                clsExcelApp.Close();
                return;
            }
            clsExcelApp.SaveAs(sFileAddress + lID + ".xls");
            clsExcelApp.Close();
            tblReportInfo model = GetOneRecordbyID<tblReportInfo, string>(lID.ToString());
            model.HandleFlag = "处理完成";
            string[] EditColumn = { "HandleFlag" };
            SaveOneRecord(model, EditColumn);

        }
        protected virtual void CreateChart(Worksheet ws, string sSelTypeIDs, int ChartType, long ReportTypeID)
        {

        }


    }
}
