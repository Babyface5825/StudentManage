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
    public class kldmTableDataListManage : SimpleDlgDataListManage
    {

        public kldmTableDataListManage()
        {
            Manage_KeyWord = "kldmTable";
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("名称", "Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model3 = new ColumnModel("兼报科类", "Jbkl");
            ColumnModel model4 = new ColumnModel("考生成绩比较序号", "Kscjbjxh", CosDataBase.INT_TYPE, 120);
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