using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportTypeSelDataListManage : DataListManage
    {

        public ReportTypeSelDataListManage()
        {
            Manage_KeyWord = "ReportType";
        }



        public override void SetMainListType()
        {
            bSort = true;
            bHasDelFlag = true;
            bSingleSelect = true;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("名称", "Name");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
        }

        public override void SetarrTopButton()
        {
            arrTopButton.Clear();
        }

    }




}