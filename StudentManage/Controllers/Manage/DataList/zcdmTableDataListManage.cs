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
    public class zcdmTableDataListManage : SimpleDlgDataListManage
    {

        public zcdmTableDataListManage()
        {
            Manage_KeyWord = "zcdmTable";
        }


        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("名称", "Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model3 = new ColumnModel("照顾分", "Zgf",CosDataBase.INT_TYPE);
            ColumnModel model4 = new ColumnModel("相对线", "Xdx");
            ColumnModel model5 = new ColumnModel("说明", "Remark");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
        }
    }




}