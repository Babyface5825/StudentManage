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
    public class zylbdmTableDataListManage : SimpleDlgDataListManage
    {

        public zylbdmTableDataListManage()
        {
            Manage_KeyWord = "zylbdmTable";
        }


        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("上级名称", "ParentIndex", CosDataBase.PARENTNAME_TYPE, 160);
            ColumnModel model3 = new ColumnModel("名称", "Name", CosDataBase.STRING_TYPE);
            ColumnModel model4 = new ColumnModel("层级", "TheLevel", CosDataBase.SMALLINT_TYPE);
            ColumnModel model5 = new ColumnModel("本层排序", "TheOrder", CosDataBase.SMALLINT_TYPE);
            ColumnModel model6 = new ColumnModel("说明", "Remark");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
        }



    }




}