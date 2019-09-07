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
    public class ReportInfoDataListManage : DataListManage
    {

        public ReportInfoDataListManage()
        {
            Manage_KeyWord = "ReportInfo";
        }



        public override void SetMainListType()
        {
            bSort = true;
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("当前院校", "gx.Name");
            ColumnModel model2 = new ColumnModel("创建日期", "CreateDate", CosDataBase.DATETIME_TYPE);
            ColumnModel model3 = new ColumnModel("报表类型", "reporttype.Name");
            ColumnModel model4 = new ColumnModel("报表名称", "Name", CosDataBase.STRING_TYPE, 200);
            ColumnModel model5 = new ColumnModel("是否生成", "HandleFlag");
            ColumnModel model6 = new ColumnModel("其他说明", "Remark");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit, "编辑名称");
            ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            ButtonModel btnMExport = new ButtonModel(DataListButton.btnExport,"导出报表");
            arrTopButton.Clear();
            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
            arrTopButton.Add(btnMExport);
        }

    }




}