using System;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using StudentManage.Models;
using StudentManage.Manage;
using System.IO;
using System.Web;
using System.Text;

namespace StudentManage.Areas.adminMain.Controllers
{
    public class LoginController : Controller
    {
        // GET: adminMain/Login
        public ActionResult Index()
        {
            ViewBag.Title = "登录界面";
            return View();
        }

        public ActionResult Login_SureClick(tblUserInfo model)
        {
            //if (DateTime.Now > Convert.ToDateTime("2016-12-1"))
            //{
            //    X.Msg.Alert("注意", "系统已过期！").Show();
            //    return this.Direct();
            //}
            //ScanFoundDelTask task = new ScanFoundDelTask();
            //task.ScanTask();
            UserInfoManage userManage = new UserInfoManage();
            //userManage.Test();
            //return this.Direct();
            string s = userManage.UserLogin(model, Response);
            try
            {
                long lUserID = Convert.ToInt64(s);
                return RedirectToAction("Index", "MainPage", new { area = "adminMain" });
            }
            catch
            {
                X.Msg.Alert("错误", s).Show();
                return this.Direct();
            }
        }

        public ActionResult Login_Show(string sWidth, string sHeight)
        {
            int nWidth = Convert.ToInt32(sWidth);
            int nHeight = Convert.ToInt32(sHeight);

            nWidth = (nWidth-1006)/ 2;
            nHeight = (nHeight - 600) / 2;

            var panel = this.GetCmp<Panel>("Login_MainPanel");
            panel.SetPosition(nWidth, nHeight);
            panel.Hidden = false;

            return this.Direct();
        }

    }
}