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
    public class jhkTableDataListManage : DataListManage
    {

        public jhkTableDataListManage()
        {
            Manage_KeyWord = "jhkTable";
        }


        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            //ColumnModel model1 = new ColumnModel("所属院校", "ParentDepartment.Name");
            ColumnModel model1 = new ColumnModel("计划省份", "dq.Name");
            ColumnModel model2 = new ColumnModel("计划年份", "YearCode", CosDataBase.INT_TYPE, 60);
            ColumnModel model3 = new ColumnModel("批次", "tddw.pc.Name");
            ColumnModel model4 = new ColumnModel("科类", "tddw.kl.Name");
            ColumnModel model5 = new ColumnModel("计划性质", "tddw.jhxz.Name");
            ColumnModel model6 = new ColumnModel("计划类别", "tddw.jhlb.Name");            
            ColumnModel model7 = new ColumnModel("专业名称", "zydh.zylb.Name");
            ColumnModel model8 = new ColumnModel("计划人数", "Jhrs", CosDataBase.INT_TYPE);
            ColumnModel model9 = new ColumnModel("计划执行数", "Jhzxs", CosDataBase.INT_TYPE);
            ColumnModel model10 = new ColumnModel("专业代号", "zydh.Code");
            ColumnModel model11 = new ColumnModel("专业代码", "zydh.zylb.Code");            
            //ColumnModel model13 = new ColumnModel("体检受限标志", "zydh.Tjsxbz");
            ColumnModel model12 = new ColumnModel("学制年限", "zydh.xz.Name");
            ColumnModel model13 = new ColumnModel("是否师范", "Sfsf", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model14 = new ColumnModel("投档单位", "tddw.Name");
            //ColumnModel model17 = new ColumnModel("预投差额数", "Ytces", CosDataBase.INT_TYPE);
            //ColumnModel model18 = new ColumnModel("专业优先序号", "Yxxh", CosDataBase.INT_TYPE);
            arrColumn.Clear();
            arrColumn.Add(model1);
            arrColumn.Add(model2);
            arrColumn.Add(model3);
            arrColumn.Add(model4);
            arrColumn.Add(model5);
            arrColumn.Add(model6);
            arrColumn.Add(model7);
            arrColumn.Add(model8);
            arrColumn.Add(model9);
            arrColumn.Add(model10);
            arrColumn.Add(model11);
            arrColumn.Add(model12);
            arrColumn.Add(model13);
            arrColumn.Add(model14);
            //arrColumn.Add(model15);
            //arrColumn.Add(model16);
        }

        public override void SetarrTopButton()
        {
            //ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit,"查看");
            //ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);
            ButtonModel btnMExport = new ButtonModel(DataListButton.btnRegularExport);
            arrTopButton.Clear();
            //arrTopButton.Add(btnMAdd);
            arrTopButton.Add(btnMEdit);
            //arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
            arrTopButton.Add(btnMExport);
        }


        public override dynamic GetSpecialModelFromCurrentID<T>(T ID)
        {
            T_jhk obj = base.GetSpecialModelFromCurrentID(ID);
            if ((obj != null) && (obj.ID != 0))
            {
                T_jhk nowObj = GetOneRecordbyID<T_jhk, T>(ID);
                obj.GxName = nowObj.ParentDepartment.Name;
                if (nowObj.tddw.Jhxzdm != 0)
                    obj.JhxzName = nowObj.tddw.jhxz.Name;
                if (nowObj.tddw.Jhlbdm != 0)
                    obj.JhlbName = nowObj.tddw.jhlb.Name;
                if (nowObj.tddw.Pcdm != 0)
                    obj.PcName = nowObj.tddw.pc.Name;
                if (nowObj.tddw.Kldm != 0)
                    obj.KlName = nowObj.tddw.kl.Name;

                Td_dqdm dq = GetOneRecordbyID<Td_dqdm, long>(obj.ProvinceID);
                if (dq != null)
                    obj.ProvinceName = dq.Name;
            }

            return obj;
        }

    }


}