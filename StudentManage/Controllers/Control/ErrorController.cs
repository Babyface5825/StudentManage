using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Manage;
using Ext.Net;
using Ext.Net.MVC;
using StudentManage.Areas.UserControl.Controllers;
using StudentManage.Models;
using System.Text;

namespace StudentManage.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index(string error)
        {
            //ViewData["Title"] = "WebSite 网站内部错误";  
            //ViewData["Description"] = error;  
            //return View("Index"); 
            return Redirect("/Main/");
        }
        public ActionResult HttpError404(string error)
        {
            //ViewData["Title"] = "HTTP 404- 无法找到文件";  
            //ViewData["Description"] = error;  
            //return View("Index");  
            return Redirect("/Main/");
        }
        public ActionResult HttpError500(string error)
        {
            //ViewData["Title"] = "HTTP 500 - 内部服务器错误";  
            //ViewData["Description"] = error;  
            //return View("Index");  
            return Redirect("/Main/");
        }
        public ActionResult General(string error)
        {
            //ViewData["Title"] = "HTTP 发生错误";  
            //ViewData["Description"] = error;  
            //return View("Index");  
            return Redirect("/Main/");
        }

    }
}