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
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class StudentInfoDataSelManage : DataListManage
    {

        public StudentInfoDataSelManage()
        {
            
        }

        public override void SetMainListType()
        {
            bSort = true;
            bHasDelFlag = true;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("所在班级", "DepartmentInfo.Name", CosDataBase.STRING_TYPE, 160);
            ColumnModel model2 = new ColumnModel("学生学号", "UserIStudentNumber");
            ColumnModel model3 = new ColumnModel("学生姓名", "Name");
            ColumnModel model4 = new ColumnModel("性别", "UserISex");
            ColumnModel model5 = new ColumnModel("联系电话", "UserIMobilePhone");
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
        }

        //public override Paging<object> InitStore(string sNowNames = "", string sNowValues = "", string sNowSigns = "", string sAndOrs = "")
        //{
        //    return _InitStore<tblUserInfo>(sNowNames, sNowValues, sNowSigns);
        //}

        public override object GetModelsForComboSearch()
        {
            List<SearchComboBoxModel> list = new List<SearchComboBoxModel>();
            list.Add(new SearchComboBoxModel(1, "所在班级", "DepartmentInfo.Name"));
            list.Add(new SearchComboBoxModel(2, "学生学号", "UserIStudentNumber"));
            list.Add(new SearchComboBoxModel(3, "学生姓名", "Name"));
            list.Add(new SearchComboBoxModel(4, "性别", "UserISex"));
            list.Add(new SearchComboBoxModel(5, "联系电话", "UserIMobilePhone"));
            return list;
        }

    }

}