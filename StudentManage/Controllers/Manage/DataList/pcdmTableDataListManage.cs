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
    public class pcdmTableDataListManage : SimpleDlgDataListManage
    {

        public pcdmTableDataListManage()
        {
            Manage_KeyWord = "pcdmTable";
        }


        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("名称", "Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model3 = new ColumnModel("国标批次代码", "Gbpcdm");
            ColumnModel model4 = new ColumnModel("国标层次代码", "Gbccdm");
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