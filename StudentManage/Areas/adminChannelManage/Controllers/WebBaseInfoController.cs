using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Controllers;
using Ext.Net.MVC;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;

namespace StudentManage.Areas.adminChannelManage.Controllers
{
    [DirectController(AreaName = "adminChannelManage")]
    public class WebBaseInfoController : ParentController
    {
        // GET: adminWebBaseInfo/WebBaseInfo

        public override void InitPage()
        {
            ParentManage man = new ParentManage();
            List<tblMainInfo> list = man.GetRecord<tblMainInfo>();
            if ((list != null) && (list.Count > 0))
            {
                list[0].MainIFootMark = HttpUtility.HtmlDecode(list[0].MainIFootMark);
                list[0].MainIAboutUS = HttpUtility.HtmlDecode(list[0].MainIAboutUS);
                list[0].MainIInformation = HttpUtility.HtmlDecode(list[0].MainIInformation);

                var form = this.GetCmp<FormPanel>("WebBaseInfo_Form");
                form.SetValues(list[0]);
            }
            else
            {
                tblMainInfo model = new tblMainInfo();
                var form = this.GetCmp<FormPanel>("WebBaseInfo_Form");
                form.SetValues(model);
            }
        }

        public ActionResult WebBaseInfo_SaveClick(string sData)
        {
            tblMainInfo model = JSON.Deserialize<tblMainInfo>(sData);
            ParentManage man = new ParentManage();
            if (model.ID != 0)
                man.SaveOneRecord(model);
            else
                man.AddOneRecord(model);

            X.Msg.Alert("成功", "保存完毕").Show();
            return this.Direct();

        }


    }
}