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
    public class UserInfoDataListManage : DataListManage
    {

        public UserInfoDataListManage()
        {
            Manage_KeyWord = "UserInfo";
        }


        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            //ColumnModel model1 = new ColumnModel("审核", "VerifyFlag", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model2 = new ColumnModel("所在部门", "DepartmentInfo.Name", CosDataBase.STRING_TYPE, 120);
            ColumnModel model3 = new ColumnModel("编号", "UserINumber");
            ColumnModel model4 = new ColumnModel("姓名", "Name");
            ColumnModel model5 = new ColumnModel("性别", "UserISex");
            ColumnModel model6 = new ColumnModel("职务", "DutyInfo.Name");
            ColumnModel model7 = new ColumnModel("手机号", "UserIMobilePhone");
            ColumnModel model8 = new ColumnModel("电子邮箱", "UserIeMail");
            arrColumn.Clear();
            //arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
            arrColumn.Add(model7);
            arrColumn.Add(model8);
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit);
            ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);

            arrTopButton.Clear();
            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
        }
    }


}