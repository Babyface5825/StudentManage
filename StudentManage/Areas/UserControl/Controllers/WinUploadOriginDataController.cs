using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Controllers;
using StudentManage.Models;
using Ext.Net;
using StudentManage.Areas.adminMain.Controllers;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinUploadOriginDataController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "招生数据文件上传";
            win.Resizable = false;
            SetWindowSize(win, 300, 200, sWidth, sHeight);
            return this.Direct();
        }

        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            return this.Direct();
        }
        public ActionResult WindowSaveClick(tblOriginDataPackageInfo mo, string PanelID, string ParentVariable)
        {
            DirectResult result = new DirectResult();
            result.IsUpload = true;
            result.Success = false;
            if ((mo.FileType == null) || (mo.FileType == ""))
                result.ErrorMessage = "请选择上传的文件类型";
            else if ((mo.OriginName == null) || (mo.OriginName == ""))
                result.ErrorMessage = "名称不能为空！";
            else
            {
                var upFile = this.GetCmp<FileUploadField>(PanelID + "_fileField");
                if (!upFile.HasFile)
                    result.ErrorMessage = "尚未选择要上传的文件！"; 
                else
                {
                    string latername = upFile.FileName.Remove(0, upFile.FileName.LastIndexOf('.'));
                    if (latername != ".zip")
                        result.ErrorMessage = "只能上传zip格式的文件！";
                    else
                    {
                        FileManager clsFile = new FileManager();
                        string sFilePath = ConstAddressPath.ORIGINZIP_WEBPATH;
                        mo.NowAddress = DateTime.Now.ToFileTimeUtc().ToString() + latername;
                        if (!clsFile.UploadFile(upFile, sFilePath + mo.NowAddress))
                            result.ErrorMessage = "文件上传出错！";
                        else
                        {
                            mo.UploadDate = DateTime.Now;
                            string[] key1 = { CookieKeys.ID };
                            string[] key2 = { CookieKeys.PARENTDEPARTMENTID };
                            string[] key3 = { CookieKeys.USERGXID };
                            mo.UserID = Convert.ToInt64((new UserInfoManage()).GetLoginUserInfo(key1)[0]);
                            mo.ParentDepartmentID = Convert.ToInt64(string.Join("", (new UserInfoManage()).GetLoginUserInfo(key2)));
                            mo.GxID = Convert.ToInt64((new UserInfoManage()).GetLoginUserInfo(key3)[0]);
                            if (!mo.HandleWay)
                                mo.HandleFlag = "尚未处理";
                            OriginDataPackageManage man = new OriginDataPackageManage();
                            long lNewID = man.AddOneRecord(mo);
                            if (mo.HandleWay)
                                man.InportData(lNewID.ToString());
                            SetHiddenValue(PanelID + "_ExitCode", "1");

                            //if (mo.FileType == "数据")
                            {
                                GetParentVariable(ParentVariable);
                                setParentVariable(ParentVar.areaName, ParentVar.controlName, ParentVar.actionName, lNewID.ToString(), ParentVar.KeyWords);
                                SetHiddenValue(PanelID + "_ParentVariable", ParentVar.DefaultVal);
                            }
                            result.Success = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}