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
    public class OriginDataPackageDataListManage : DataListManage
    {

        public OriginDataPackageDataListManage()
        {
            Manage_KeyWord = "OriginDataInport";
        }
        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("所在单位", "user.WholeDepartmentName", CosDataBase.STRING_TYPE, 120);            
            ColumnModel model2 = new ColumnModel("操作用户", "user.Name");
            ColumnModel model3 = new ColumnModel("原始文件名", "OriginName");
            ColumnModel model4 = new ColumnModel("上传时间", "UploadDate", CosDataBase.DATETIME_TYPE, 120);            
            ColumnModel model5 = new ColumnModel("是否处理", "HandleFlag");
            ColumnModel model6 = new ColumnModel("结束时间", "FinishDate", CosDataBase.DATETIME_TYPE, 120);
            //ColumnModel model6 = new ColumnModel("成功记录数", "SuccessNumber", CosDataBase.INT_TYPE);
            //ColumnModel model7 = new ColumnModel("异常记录数", "FailureNumber", CosDataBase.INT_TYPE);
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
            //arrColumn.Add(model7);
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit, "编辑名称");
            ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            arrTopButton.Clear();
            arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            arrTopButton.Add(btnMDelete);
        }

    }




}