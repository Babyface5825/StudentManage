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


using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using StudentManage.Models;
using StudentManage.Manage;
using System.Linq.Expressions;

namespace StudentManage.Controllers
{

    public class ForDataRelation
    {
        public long lOldID;
        public long lNewID;
    }

    public class Excel : ParentManage
    {
        private CExcelApp clsExcelApp = new CExcelApp();
        private FileManager clsFile = new FileManager();
        private CryptManage crypt = new CryptManage();
        private ParentManage man = new ParentManage();

        private List<ForDataRelation> ChannelTypeRelation = new List<ForDataRelation>();
        private List<ForDataRelation> ChannelContentRelation = new List<ForDataRelation>();

        public Excel()
        {

        }

        public string WholeStep(string sFileName, string keywords, string transvalue = null)
        {
            if (clsExcelApp.Import(sFileName) == null)
            {
                clsFile.DelOneFile(sFileName);
                return "导入失败！请检查上传文件格式。";
            }

            string msg = ExcelImport(keywords, transvalue);
            clsFile.DelOneFile(sFileName);
            clsExcelApp.KillProcess("EXCEL");
            if (msg == null)
                return "导入文件失败，请检查文件内容是否正确。";
            else
                return msg;
        }



        private void ExcelImportSimpleTable<T>(T obj, System.Data.DataTable dt, ArrayList arrFlag) where T : class , new()
        {
            try
            {
                short nowOrder = 1;
                ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
                names.Add("DelFlag"); values.Add(false);

                var order = BuildOrder<T, short>("TheOrder");
                Expression<Func<T, bool>> where1 = GetAndLambdaExpression<T>(names, values);
                List<T> list1 = GetRecord(where1, order, false); //取出主表中所有字段
                if ((list1 != null) && (list1.Count > 0))
                {
                    nowOrder = Convert.ToInt16(list1[0].GetType().GetProperty("TheOrder").GetValue(list1[0], null));
                    nowOrder++;
                }

                names.Add("Code"); values.Add("");
                names.Add("Name"); values.Add("");
                List<T> records = new List<T>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    values[1] = dt.Rows[i][0].ToString(); //Code
                    values[2] = dt.Rows[i][1].ToString(); //Name
                    Expression<Func<T, bool>> where = GetAndLambdaExpression<T>(names, values);
                    List<T> list = GetRecord(where);
                    if ((list != null) && (list.Count > 0))
                        arrFlag[i] = false;
                    else
                    {
                        T model = new T();
                        model.GetType().GetProperty("Code").SetValue(model, dt.Rows[i][0].ToString());
                        model.GetType().GetProperty("Name").SetValue(model, dt.Rows[i][1].ToString());
                        model.GetType().GetProperty("Remark").SetValue(model, dt.Rows[i][2].ToString());
                        model.GetType().GetProperty("TheOrder").SetValue(model, nowOrder);
                        AddOneRecord(model);
                        nowOrder++;
                    }
                }
            }
            catch (Exception e)
            {

            }            
        }
        private void ExcelImportDqdmTable(System.Data.DataTable dt, ArrayList arrFlag)
        {
            try
            {
                dqdmTableManage dqdmMan = new dqdmTableManage();
                dqdmMan.InportInitData(dt);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 将一张表的数据导入数据库
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        private string ExcelImportOneTable(System.Data.DataTable dt, string keywords, string transvalue)
        {
            try
            {
                string returnstring = "";
                ArrayList arrFlag = new ArrayList(); //记录每一行导入情况
                for (int i = 0; i < dt.Rows.Count; i++)
                    arrFlag.Add(true);

                if (keywords == "SimpleDlg") //三列，Code、Name、Remark
                {
                    if (transvalue == "dqdmTable")
                        ExcelImportDqdmTable(dt, arrFlag);
                    else
                    {
                        Type type = GetTypeObj(transvalue).GetType();
                        dynamic obj = type.Assembly.CreateInstance(type.ToString());
                        ExcelImportSimpleTable(obj, dt, arrFlag);
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!Convert.ToBoolean(arrFlag[i]))
                        returnstring += "<li><h5> - 第" + (i + 1).ToString() + "条记录在添加时发生错误！</h5></li>";
                }
                return returnstring;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        


        /// <summary>
        /// 根据clsExcelApp/Excel传回的表格将数据导入数据库
        /// </summary>
        /// <param name="clsExcelApp">Excel传回的表格</param>
        /// <returns></returns>
        public string ExcelImport(string keywords, string transvalue)
        {
            int nCount = clsExcelApp.dts.Count;
            string tips = "";//提示信息

            for (int i = 0; i < nCount; i++)
            {
                string tiptemp;
                string sTableName = clsExcelApp.sheetnames[i].ToString();
                System.Data.DataTable dt = (System.Data.DataTable)clsExcelApp.dts[i];//每个sheet的名字，表
                if (dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "")//即使是空表还是有一行
                    tiptemp = "<h4>* 表单（" + sTableName.Remove(sTableName.Length - 1, 1) + "）内容为空！</h4><br><br>";
                else
                {
                    tiptemp = ExcelImportOneTable(dt, keywords, transvalue);//将一张表的数据导入数据库，dt是表
                    if (tiptemp == null)
                        return null;
                    else if (tiptemp != "")
                        tiptemp = "<h4>* 表单（" + sTableName.Remove(sTableName.Length - 1, 1) + "）导入时发生异常！</h4><ol>" + tiptemp + "</ol><br>";
                    else
                        tiptemp = "<h4>* 表单（" + sTableName.Remove(sTableName.Length - 1, 1) + "）导入成功！</h4><ol>";
                }
                tips += tiptemp;
            }

            string sFilePath = ConstAddressPath.TEMP_WEBPATH;
            FileStream fs = File.Open(sFilePath + "temp.html", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("<HTML><head><Title></title></head><body>");
            sw.Write(tips);
            sw.WriteLine("</body></html>");
            sw.Close();
            fs.Close();

            return tips;
        }

        /// <summary>
        /// 根据字段名查找数据表中的值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="att">名称</param>
        /// <returns></returns>
        private string GetTableValue(DataRow row, string att)
        {
            //日期型数据
            if (att.Contains("日期"))
            {
                DateTime date;
                if (row[att].ToString() == "" || !DateTime.TryParse(row[att].ToString(), out date))
                    return DateTime.Now.ToString("yyyy-MM-dd");
                else
                    return date.ToString("yyyy-MM-dd");
            }
            else
            {
                //除去字符串中的所有空格符
                string str = row[att].ToString();
                //string[] arr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                //str = string.Join("", arr);
                return str;
            }
        }
    }
}
