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
    public class bmkOneVerifyDataListManage : bmkTableDataListManage
    {

        public bmkOneVerifyDataListManage()
        {
            Manage_KeyWord = "bmkTable";
        }

        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = false;
        }
        
        public override void SetarrTopButton()
        {
            
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit,"查看详情");            
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd, "审核多条");

            arrTopButton.Clear();

            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);            
        }
        
    }


}