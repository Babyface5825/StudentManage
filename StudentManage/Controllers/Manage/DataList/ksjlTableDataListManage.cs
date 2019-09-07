using StudentManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Manage
{
    public class ksjlTableDataListManage:SimpleDlgDataListManage
    {
        public ksjlTableDataListManage()
        {
            Manage_KeyWord = "ksjlTable";
        }

        public override void SetMainListType()
        {
            bHasDelFlag = false;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("起始日期", "Qsrq",CosDataBase.DATE_TYPE);
            ColumnModel model2 = new ColumnModel("终结日期", "Zjrq",CosDataBase.DATE_TYPE);
            ColumnModel model3 = new ColumnModel("简历", "Jl");
            ColumnModel model4 = new ColumnModel("任何职务", "Rhzw");
            ColumnModel model5 = new ColumnModel("证明人", "Zmr");

            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);

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
