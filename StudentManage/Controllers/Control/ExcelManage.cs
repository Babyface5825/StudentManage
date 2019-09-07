using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

using StudentManage.Models;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Text;
using StudentManage.Controllers;

namespace StudentManage.Controllers
{
    /// <SUMMARY> 
    /// ExcelEdit 的摘要说明 
    /// </SUMMARY> 
    public class CExcelApp
    {
        public string mFilename;
        private FileManager clsFile = new FileManager();
        public Application app;
        public Workbooks wbs;
        public Workbook wb;
        public Worksheets wss;
        public Worksheet ws;
        public ArrayList sheetnames;
        public ArrayList dts;

        public CExcelApp()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }

        public bool FileDownload(string FileName)
        {
            return FileDownload(FileName, "");

        }

        public bool FileDownload(string FileName, string DownloadName)
        {
            return clsFile.DownloadFile(FileName, DownloadName);
        }


        public bool OpenConnection(OleDbConnection aConnection)//打开数据库
        {
            try
            {
                aConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
        public void CloseConnection(OleDbConnection aConnection)//关闭数据库
        {
            if (aConnection.State == ConnectionState.Open)
                aConnection.Close();
        }

        internal void SetCellProperty(Worksheet ws, int v1, int v2, int v3, int v4, int v5, string v6, bool v7, object xlCenter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 导入Excel表格到Datebles中
        /// </summary>
        /// <param name="path">表格的集合</param>
        public ArrayList Import(string path)
        {
            //设置数据源
            try
            {
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
                //string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties='Excel 12.0;HDR=YES'";
                OleDbConnection aConnection = new OleDbConnection(conStr);

                if (!OpenConnection(aConnection))
                    return null;

                OleDbDataAdapter oda;
                sheetnames = new ArrayList();//Excel文件中的sheet的集合
                OleDbCommand aCommand;

                string sql_F = "Select * FROM [{0}]";
                System.Data.DataTable dt1 = aConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null });
                foreach (DataRow row in dt1.Rows)
                {
                    sheetnames.Add(row["TABLE_NAME"].ToString()); // 读取表名
                }
                dts = new ArrayList();
                for (int i = 0; i < sheetnames.Count; i++)
                {
                    aCommand = new OleDbCommand(string.Format(sql_F, sheetnames[i].ToString()), aConnection);
                    oda = new OleDbDataAdapter(aCommand);
                    System.Data.DataTable Table = new System.Data.DataTable();
                    Table.TableName = sheetnames[i].ToString();
                    oda.Fill(Table);
                    dts.Add(Table);
                }
                CloseConnection(aConnection);
                return dts;
            }
            catch
            {
                return null;
            }
        }

        public void Create()//创建一个Excel对象 
        {
            try
            {
                app = new Application();
                wbs = app.Workbooks;
                wb = wbs.Add(true);
            }
            catch
            {
                this.Close();
            }
        }

        public bool Open(string FileName)//打开一个Excel文件 
        {
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                wbs = app.Workbooks;
                wb = wbs.Add(FileName);
                mFilename = FileName;
                return true;
            }
            catch
            {
                this.Close();
                return false;

            }
        }

        //获取一个工作表 
        public Microsoft.Office.Interop.Excel.Worksheet GetSheet(string SheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }

        //添加一个工作表 
        public Microsoft.Office.Interop.Excel.Worksheet AddSheet(string SheetName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                s.Name = SheetName;
                return s;
            }
            catch
            {
                this.Close();
                return null;

            }
        }

        public void DelSheet(string SheetName)//删除一个工作表 
        {
            ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)//重命名一个工作表一 
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(Microsoft.Office.Interop.Excel.Worksheet Sheet, string NewSheetName)//重命名一个工作表二 
        {

            Sheet.Name = NewSheetName;

            return Sheet;
        }

        //ws：要设值的工作表     X行Y列     value   值 
        public void SetCellValue(Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y, object value)
        {
            ws.Cells[x, y] = value;
        }

        //ws：要设值的工作表的名称 X行Y列 value 值 
        public void SetCellValue(string ws, int x, int y, object value)
        {
            try
            {
                GetSheet(ws).Cells[x, y] = value;
            }
            catch
            {
                this.Close();
            }
        }

        //设置一个单元格的属性   字体，   大小，颜色   ，对齐方式 
        public void SetCellProperty(Microsoft.Office.Interop.Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            name = "宋体";
            size = 12;
            color = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        //name = "宋体"; 
        //size = 12; 
        //color = Excel.Constants.xlAutomatic; 
        //HorizontalAlignment = Excel.Constants.xlRight; 
        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            Microsoft.Office.Interop.Excel.Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;

        }

        public void SetCellProperty(Microsoft.Office.Interop.Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int size, string name, bool isBold, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Bold = isBold;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        //合并单元格 
        public void UniteCells(Microsoft.Office.Interop.Excel.Worksheet ws, int x1, int y1, int x2, int y2)
        {
            ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
        }

        //合并单元格 
        public void UniteCells(string ws, int x1, int y1, int x2, int y2)
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing);
        }


        //将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用一 
        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {
            try
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        GetSheet(ws).Cells[startX + i, j + startY] = "'" + dt.Rows[i][j].ToString();
            }
            catch
            {
                this.Close();
            }
        }
        //将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用一 
        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY, ArrayList arr)
        {
            try
            {
                object[,] strs = new object[dt.Rows.Count, dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (Convert.ToInt16(arr[j]) == CosDataBase.INT_TYPE)
                            strs[i, j] = Convert.ToInt64(dt.Rows[i][j]);
                        else
                            strs[i, j] = "'" + dt.Rows[i][j].ToString();
                    }
                }
                string sCellID = "";
                switch (startY)
                {
                    case 1: sCellID = "A"; break;
                    case 2: sCellID = "B"; break;
                    case 3: sCellID = "C"; break;
                    case 4: sCellID = "D"; break;
                    case 5: sCellID = "E"; break;
                    case 6: sCellID = "F"; break;
                    case 7: sCellID = "G"; break;
                    case 8: sCellID = "H"; break;
                    case 9: sCellID = "I"; break;
                    case 10: sCellID = "J"; break;
                    case 11: sCellID = "K"; break;
                    case 12: sCellID = "L"; break;
                    case 13: sCellID = "M"; break;
                    case 14: sCellID = "N"; break;
                    case 15: sCellID = "O"; break;
                    case 16: sCellID = "P"; break;
                    case 17: sCellID = "Q"; break;
                    case 18: sCellID = "R"; break;
                    case 19: sCellID = "S"; break;
                    case 20: sCellID = "T"; break;
                    case 21: sCellID = "U"; break;
                    case 22: sCellID = "V"; break;
                    case 23: sCellID = "W"; break;
                    case 24: sCellID = "X"; break;
                    case 25: sCellID = "Y"; break;
                    case 26: sCellID = "Z"; break;
                }
                sCellID = sCellID + startX;
                Range range = (Range)GetSheet(ws).get_Range(sCellID, Type.Missing);
                range = range.get_Resize(dt.Rows.Count, dt.Columns.Count);
                range.Value2 = strs;
            }
            catch
            {
                this.Close();
            }
        }

        //将内存中数据表格插入到Excel指定工作表的指定位置二 
        public void InsertTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)
        {
            try
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        ws.Cells[startX + i, j + startY] = "'" + dt.Rows[i][j].ToString();
            }
            catch
            {
                this.Close();
            }
        }

        //将内存中数据表格添加到Excel指定工作表的指定位置一 
        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {
            try
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        GetSheet(ws).Cells[i + startX, j + startY] = "'" + dt.Rows[i][j].ToString();
            }
            catch
            {
                this.Close();
            }
        }

        //将内存中数据表格添加到Excel指定工作表的指定位置二 
        public void AddTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)
        {
            try
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        ws.Cells[i + startX, j + startY] = "'" + dt.Rows[i][j].ToString();
            }
            catch
            {
                this.Close();
            }
        }

        //插入图片操作一 //后面的数字表示位置 
        //public void InsertPictures(string Filename, string ws) 
        //{ 
        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse,MsoTriState.msoTrue, 10, 10, 150, 150); 
        //} 

        //public void InsertPictures(string Filename, string ws, int Height, int Width) 
        //插入图片操作二 
        //{ 
        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150); 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height; 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width; 
        //} 
        //public void InsertPictures(string Filename, string ws, int left, int top, int Height, int Width) 
        //插入图片操作三 
        //{ 

        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150); 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementLeft(left); 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementTop(top); 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height; 
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width; 
        //} 

        //插入图表操作 
        public void InsertActiveChart(Microsoft.Office.Interop.Excel.XlChartType ChartType, string ws, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Microsoft.Office.Interop.Excel.XlRowCol ChartDataType)
        {
            ChartDataType = Microsoft.Office.Interop.Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(ws).get_Range(GetSheet(ws).Cells[DataSourcesX1, DataSourcesY1], GetSheet(ws).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAsObject, ws);
            }
        }

        //保存文档 
        public bool Save()
        {
            if (mFilename == "")
                return false;
            else
            {
                try
                {
                    wb.Save();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }

        //文档另存为 
        public bool SaveAs(object FileName)
        {
            try
            {
                wb.Application.DisplayAlerts = false;
                wb.SaveAs(FileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;
            }
            catch
            {
                this.Close();
                return false;
            }
        }

        //关闭一个Excel对象，销毁对象 
        public void Close()
        {
            //wb.Save(); 
            try
            {
                wb.Close(Type.Missing, Type.Missing, Type.Missing);
                wbs.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wbs);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                wb = null;
                wbs = null;
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {

            }
            KillProcess("EXCEL");
        }


        public void KillProcess(string processName)
        {
            System.Diagnostics.Process[] myproc = System.Diagnostics.Process.GetProcesses();
            //得到所有打开的进程
            try
            {
                foreach (System.Diagnostics.Process thisproc in myproc)
                {
                    if (thisproc.ProcessName.ToLower().Contains(processName.ToLower()))
                        thisproc.Kill();
                }
            }
            catch
            {
                //System.Console.WriteLine("Kill process named as " + processName + " fail.");
            }
        }

        private StringBuilder AddFastHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">");
            sb.Append(Environment.NewLine);
            sb.Append("<head>");
            sb.Append(Environment.NewLine);
            sb.Append("<style>");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-displayed-decimal-separator:\"\\.\"; mso-displayed-thousand-separator:\"\\,\";}\"");
            sb.Append(Environment.NewLine);
            sb.Append("@page");
            sb.Append(Environment.NewLine);
            sb.Append("{margin:1.0in .75in 1.0in .75in;	mso-header-margin:.5in;	mso-footer-margin:.5in;}");
            sb.Append(Environment.NewLine);
            sb.Append("tr");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-height-source:auto;	mso-ruby-visibility:none;}");
            sb.Append(Environment.NewLine);
            sb.Append("col");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-width-source:auto;mso-ruby-visibility:none;}");
            sb.Append(Environment.NewLine);
            sb.Append("br");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-data-placement:same-cell;}");
            sb.Append(Environment.NewLine);
            sb.Append(".style0");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-number-format:General;text-align:general;vertical-align:bottom;white-space:nowrap;mso-rotate:0;mso-background-source:auto;");
            sb.Append("mso-pattern:auto;color:windowtext;font-size:12.0pt;font-weight:400;font-style:normal;text-decoration:none;font-family:宋体;");
            sb.Append("mso-generic-font-family:auto;mso-font-charset:134;border:none;mso-protection:locked visible;mso-style-name:常规;mso-style-id:0;}");
            sb.Append(Environment.NewLine);
            sb.Append("td");
            sb.Append(Environment.NewLine);
            sb.Append("{mso-style-parent:style0;padding-top:1px;padding-right:1px;padding-left:1px;mso-ignore:padding;color:windowtext;font-size:12.0pt;");
            sb.Append("font-weight:400;font-style:normal;text-decoration:none;font-family:宋体;mso-generic-font-family:auto;mso-font-charset:134;");
            sb.Append("mso-number-format:General;text-align:general;vertical-align:bottom;border:none;mso-background-source:auto;mso-pattern:auto;");
            sb.Append("mso-protection:locked visible;white-space:nowrap;mso-rotate:0;}");
            sb.Append(Environment.NewLine);
            sb.Append("ruby");
            sb.Append(Environment.NewLine);
            sb.Append("{ruby-align:left;}");
            sb.Append(Environment.NewLine);
            sb.Append("rt");
            sb.Append(Environment.NewLine);
            sb.Append("{color:windowtext;font-size:9.0pt;font-weight:400;font-style:normal;text-decoration:none;font-family:宋体;mso-generic-font-family:auto;mso-font-charset:134;mso-char-type:none;display:none;}");
            sb.Append(Environment.NewLine);
            sb.Append("</style>");
            sb.Append(Environment.NewLine);
            sb.Append("<!--[if gte mso 9]><xml>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ExcelWorkbook>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ExcelWorksheets>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ExcelWorksheet>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:Name>Sheet1</x:Name>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:WorksheetOptions>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:DefaultRowHeight>285</x:DefaultRowHeight>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:CodeName>Sheet1</x:CodeName>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:Selected/>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:Panes>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:Pane>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:Number>1</x:Number>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ActiveRow>1</x:ActiveRow>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:Pane>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:Panes>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ProtectContents>False</x:ProtectContents>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ProtectObjects>False</x:ProtectObjects>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ProtectScenarios>False</x:ProtectScenarios>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:WorksheetOptions>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:ExcelWorksheet>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:ExcelWorksheets>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:WindowHeight>4530</x:WindowHeight>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:WindowWidth>8505</x:WindowWidth>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:WindowTopX>480</x:WindowTopX>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:WindowTopY>120</x:WindowTopY>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:AcceptLabelsInFormulas/>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ProtectStructure>False</x:ProtectStructure>");
            sb.Append(Environment.NewLine);
            sb.Append("<x:ProtectWindows>False</x:ProtectWindows>");
            sb.Append(Environment.NewLine);
            sb.Append("</x:ExcelWorkbook>");
            sb.Append(Environment.NewLine);
            sb.Append("</xml>");
            sb.Append(Environment.NewLine);
            sb.Append("</head>");
            sb.Append(Environment.NewLine);
            return sb;
        }
        public bool FastAddTable(string sSaveFileName, System.Data.DataTable dt)
        {
            try
            {
                StreamWriter sw = new StreamWriter(sSaveFileName, false, Encoding.GetEncoding("gb2312"));
                StringBuilder sb = AddFastHeader();

                sb.Append("<body link=blue vlink=purple>");
                sb.Append(Environment.NewLine);
                sb.Append("<table x:str border=0 cellpadding=0 cellspacing=0 width=72 style='border-collapse:collapse;table-layout:fixed;width:54pt'>");
                sb.Append(Environment.NewLine);
                sb.Append("<col width=72 style='width:54pt'>");
                sb.Append(Environment.NewLine);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("<tr height=19 style='height:14.25pt'>");
                    sb.Append(Environment.NewLine);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sb.Append("<td height=19 style='height:14.25pt'>");
                        sb.Append(dt.Rows[i][j].ToString());
                        sb.Append("</td>");
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append("</tr>");
                    sb.Append(Environment.NewLine);
                }

                sb.Append("</table>");
                sb.Append(Environment.NewLine);
                sb.Append("</body>");
                sb.Append(Environment.NewLine);
                sb.Append("</html>");
                sb.Append(Environment.NewLine);

                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
