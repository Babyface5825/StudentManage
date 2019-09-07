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
    public class tddwTableDataListManage : SimpleDlgDataListManage
    {

        public tddwTableDataListManage()
        {
            Manage_KeyWord = "tddwTable";
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit);
            ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            ButtonModel btnInport = new ButtonModel(DataListButton.btnInport);
            ButtonModel btnExport = new ButtonModel(DataListButton.btnRegularExport);
            arrTopButton.Clear();
            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
            arrTopButton.Add(btnInport);
            arrTopButton.Add(btnExport);
        }

        public override void SetarrColumn()
        {            
            ColumnModel model1 = new ColumnModel("院校名称", "gx.Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model2 = new ColumnModel("投档单位代码", "Code");
            ColumnModel model3 = new ColumnModel("投档单位名称", "Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model4 = new ColumnModel("批次名称", "pc.Name");
            ColumnModel model5 = new ColumnModel("科类名称", "kl.Name");
            ColumnModel model6 = new ColumnModel("计划性质", "jhxz.Name");
            ColumnModel model7 = new ColumnModel("计划类别", "jhlb.Name");
            ColumnModel model8 = new ColumnModel("说明", "Remark");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
            arrColumn.Add(model7);
            arrColumn.Add(model8);
        }
    }




}