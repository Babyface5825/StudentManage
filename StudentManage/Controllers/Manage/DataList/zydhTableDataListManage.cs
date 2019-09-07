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
    public class zydhTableDataListManage : SimpleDlgDataListManage
    {

        public zydhTableDataListManage()
        {
            Manage_KeyWord = "zydhTable";
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

        public override dynamic GetSpecialModelFromCurrentID<T>(T ID)
        {
            Td_zydh obj = base.GetSpecialModelFromCurrentID(ID);
            if ((obj != null) && (obj.ID != 0))
            {
                Td_zydh nowObj = GetOneRecordbyID<Td_zydh, T>(ID);
                obj.GxName = nowObj.Gx.Name;
            }
            return obj;
        }
    }
}