using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Configuration;
using StudentManage.Areas.adminSystemManage.Models;
using StudentManage.Models;
using Ext.Net.MVC;
using Ext.Net;
using StudentManage.Controllers;

namespace StudentManage.Areas.adminSystemManage.Controllers
{
    public class SetConstValueController : Controller
    {


        /// <summary>  
        /// 设置appSetting的值  
        /// </summary>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        private void SetAppSettingConfiguration(string key, string value)
        {
            string XPath = Server.MapPath("~/Web.config");
            Configuration cfa = WebConfigurationManager.OpenWebConfiguration("~");
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
               

        // GET: /OperationInfo/
        public ActionResult Index()
        {
            ConstValueStr model = new ConstValueStr();
            //model.sConnStr = AppSettingValues.DB_CONN_STRING;
            //model.sConnSysStr = AppSettingValues.SYDB_CONN_STRING;
            model.sDataListPageLimit = AppSettingValues.DATALISTPAGELIMIT;
            model.sFloatDecimal = AppSettingValues.FLOATDECIMAL;
            model.cmbMenuType = AppSettingValues.MENUTYPE;
            model.cmbTabType = AppSettingValues.TABTYPE;
            //model.cmbTheme = AppSettingValues.THEME;
            var form = this.GetCmp<FormPanel>("SetConstValue_Form");
            form.SetValues(model);
            return View();
        }

        public ActionResult SetConstValue_Save(string sData)
        {
            ConstValueStr model = JSON.Deserialize<ConstValueStr>(sData);
            //SetConnectConfiguration("ConnStr", model.sConnStr);
            //SetConnectConfiguration("ConnSyStr", model.sConnSysStr);
            SetAppSettingConfiguration("DataListPageLimit", model.sDataListPageLimit.ToString());
            SetAppSettingConfiguration("FloatDecimal", model.sFloatDecimal.ToString());
            SetAppSettingConfiguration("MenuType", model.cmbMenuType);
            SetAppSettingConfiguration("TabType", model.cmbTabType);
            X.Msg.Alert("成功", "保存完毕，请刷新整个系统。").Show();
            DirectResult result = new DirectResult();
            //result.IsUpload = true;
            return result;
        }


        public ActionResult SetConstValue_InportClick()
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;

            //var upFile = this.GetCmp<FileUploadField>("SetConstValue_File");
            //if (upFile.HasFile)
            //{
            //    FileManager clsFile = new FileManager();
            //    string sFilePath = ConstAddressPath.TEMP_WEBPATH;
            //    string sAddress = DateTime.Now.ToFileTimeUtc().ToString() + upFile.FileName.Remove(0, upFile.FileName.LastIndexOf('.'));
            //    if (clsFile.UploadFile(upFile, sFilePath + sAddress))
            //    {
            //        ExcelIn clsExcel = new ExcelIn();
            //        clsExcel.WholeStep(sFilePath + sAddress, "OriginData");
            //    }
            //}
            return result;
        }
        
    }
}
