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
    public class ksxkcjTableDataListManage : SimpleDlgDataListManage
    {

        public ksxkcjTableDataListManage()
        {
            Manage_KeyWord = "ksxkcjTable";
        }

        public override void SetMainListType()
        {
            bHasDelFlag = false;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("科目名称", "cjx.Name",CosDataBase.STRING_TYPE);//之前没写类型，一直找不到bug，气死了
            ColumnModel model2 = new ColumnModel("等第成绩", "Ddcj",CosDataBase.FLOAT_TYPE);


            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
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




