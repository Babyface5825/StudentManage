using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;


namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class zykTableDataListManage : DataListManage
    {

        public zykTableDataListManage()
        {
            Manage_KeyWord = "zykTable";
        }


        public override void SetMainListType()
        {
            bHasDelFlag = false;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("投档单位", "jhk.tddw.Name");
            ColumnModel model2 = new ColumnModel("专业代号", "jhk.zydh.Name");
            ColumnModel model3 = new ColumnModel("批次", "jhk.tddw.pc.Name", CosDataBase.STRING_TYPE, 120);
            ColumnModel model4 = new ColumnModel("科类", "jhk.tddw.kl.Name", CosDataBase.STRING_TYPE, 60);
            ColumnModel model5 = new ColumnModel("计划性质", "jhk.tddw.jhxz.Name", CosDataBase.STRING_TYPE, 60);
            ColumnModel model6 = new ColumnModel("计划类别", "jhk.tddw.jhlb.Name");
            ColumnModel model7 = new ColumnModel("专业序号", "Zyxh", CosDataBase.SMALLINT_TYPE, 60);
            
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
            arrColumn.Add(model7);
        }

        public override void SetarrTopButton()
        {
            //ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            //ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit);
            //ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            //ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            //ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            arrTopButton.Clear();
            //arrTopButton.Add(btnMAdd);
            //arrTopButton.Add(btnMEdit);
            //arrTopButton.Add(btnMDelete);
            //arrTopButton.Add(btnMSearch);
            //arrTopButton.Add(btnMReset);
        }
    }


}