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
    public class ExcelOut
    {
        private CExcelApp clsExcelApp = new CExcelApp();
        private FileManager clsFile = new FileManager();
        private CryptManage crypt = new CryptManage();
        private ParentManage man = new ParentManage();


        public ExcelOut()
        {

        }

        private void GetProperties(System.Data.DataTable dt, DataRow dr, int start)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            int type = Convert.ToInt16(dt.Rows[0]["Type"]);

            if (type == CosDataBase.FLOAT_TYPE)
                dr[start] = dt.Rows[0]["NumValue"];
            else if (type == CosDataBase.BOOLEAN_TYPE)
            {
                if ((bool)dt.Rows[0]["BoolValue"])
                    dr[start] = "是";
                else
                    dr[start] = "否";
            }
            else if (type == CosDataBase.DATETIME_TYPE)
            {
                if (dt.Rows[0]["DateValue"] != DBNull.Value)
                    dr[start] = Convert.ToDateTime(dt.Rows[0]["DateValue"]).ToString("yyyy-MM-dd");
            }
            else
                dr[start] = dt.Rows[0]["StrValue"];
        }

        /// <summary>
        /// Excel的生成和下载
        /// </summary>
        /// <param name="dt"></param>
        public bool ExcelCreateAndDownload(System.Data.DataTable dt, string sFileName = "")
        {
            //创建 excel 文件
            bool delFlag = false;
            string sAddress = ConstAddressPath.TEMP_WEBPATH;
            if (sFileName == "")
            {
                Random dm = new Random();
                sFileName = "导出数据(" + dm.Next(1000) + ").xls";                
                delFlag = true;
            }
            if (ExcelCreate(dt, sAddress + sFileName))
            {
                ExcelDownLoad(sAddress + sFileName);
                if (delFlag)
                    clsFile.DelOneFile(sAddress + sFileName);
            }
            else
                return false;
            return true;
        }

        private bool ExcelCreate(System.Data.DataTable dt, string sFileName)
        {
            try
            {
                return clsExcelApp.FastAddTable(sFileName, dt);
            }
            catch
            {
                clsExcelApp.Close();
                return false;
            }
        }

        private void ExcelDownLoad(string sFileName)
        {
            try
            {
                clsExcelApp.FileDownload(sFileName, "导出数据.xls");
            }
            catch
            {
                clsExcelApp.Close();
            }
        }

    }
}
