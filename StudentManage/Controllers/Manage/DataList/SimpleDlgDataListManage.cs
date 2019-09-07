using System;
using StudentManage.Models;
using System.Collections.Generic;
using System.Collections;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleDlgDataListManage : DataListManage
    {        
        public override void SetMainListType()
        {
            bSort = true;
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("代码", "Code");
            ColumnModel model2 = new ColumnModel("名称", "Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model3 = new ColumnModel("排序", "TheOrder", CosDataBase.SMALLINT_TYPE);
            ColumnModel model4 = new ColumnModel("说明", "Remark");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit);
            ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            ButtonModel btnMore1 = new ButtonModel(DataListButton.btnMore1, "合并", Ext.Net.Icon.ArrowIn);
            //ButtonModel btnMOrderUp = new ButtonModel(DataListButton.btnOrderUp);
            //ButtonModel btnMOrderDown = new ButtonModel(DataListButton.btnOrderDown);
            ButtonModel btnInport = new ButtonModel(DataListButton.btnInport);
            ButtonModel btnExport = new ButtonModel(DataListButton.btnRegularExport);
            arrTopButton.Clear();
            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
            arrTopButton.Add(btnMore1);
            //arrTopButton.Add(btnMOrderUp);
            //arrTopButton.Add(btnMOrderDown);
            arrTopButton.Add(btnInport);
            arrTopButton.Add(btnExport);
        }


        /// <summary>
        /// 得到一个新的model，主要用于新建model时自动算theOrder
        /// </summary>
        /// <param name="model"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public tblSimpleInfo GetNewModel(tblSimpleInfo model, string keyword)
        {
            Type type = GetTypeObj(keyword).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetNewModel(obj, model);
        }

        private tblSimpleInfo _GetNewModel<T>(T obj, tblSimpleInfo model) where T : class
        {
            short nOrder = 1;
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            names.Add("DelFlag");
            values.Add(false);
            var express = GetAndLambdaExpression<T>(names, values);
            var order = BuildOrder<T, short>("TheOrder");
            List<T> list = GetRecord(express, order, false);
            if ((list != null) && (list.Count > 0))
            {
                nOrder = Convert.ToInt16(list[0].GetType().GetProperty("TheOrder").GetValue(list[0], null));
                nOrder++;
            }
            model.TheOrder = nOrder;
            return model;
        }

        





    }




}