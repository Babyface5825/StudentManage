using StudentManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class tddTableDataListManage : DataListManage
    {

        public tddTableDataListManage()
        {
            Manage_KeyWord = "tddTable";
        }


        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = true;
        }

        public override void SetarrColumn()
        {
            ColumnModel model2 = new ColumnModel("逻辑志愿号", "Ljzyh");
            ColumnModel model3 = new ColumnModel("投档单位", "tddw.Name");
            ColumnModel model4 = new ColumnModel("投档类型代码", "tdlx.Name");
            ColumnModel model5 = new ColumnModel("投档时间", "Tdsj", CosDataBase.DATETIME_TYPE);
            ColumnModel model6 = new ColumnModel("投档志愿", "Tdzy");
            ColumnModel model7 = new ColumnModel("成绩", "Cj", CosDataBase.FLOAT_TYPE);
            ColumnModel model8 = new ColumnModel("特征成绩", "Tzcj", CosDataBase.FLOAT_TYPE);
            ColumnModel model9 = new ColumnModel("投档成绩", "Tdcj", CosDataBase.FLOAT_TYPE);
            ColumnModel model10 = new ColumnModel("资格分", "Zgf", CosDataBase.FLOAT_TYPE);
            ColumnModel model11 = new ColumnModel("院校导入成绩", "Yxdrcj", CosDataBase.FLOAT_TYPE);
            ColumnModel model12 = new ColumnModel("专业预投基准成绩", "Zyytjzcj", CosDataBase.FLOAT_TYPE);
            ColumnModel model13 = new ColumnModel("专业预投附加成绩", "Zyytfjcj", CosDataBase.FLOAT_TYPE);
            ColumnModel model14 = new ColumnModel("当前投档单位", "Dqtddw");
            ColumnModel model15 = new ColumnModel("已投专业", "Ytzy");
            ColumnModel model16 = new ColumnModel("录取专业", "Lqzy");
            ColumnModel model17 = new ColumnModel("录取时间", "Lqsj", CosDataBase.DATETIME_TYPE);
            ColumnModel model18 = new ColumnModel("录取类型", "lqlx.Name");
            ColumnModel model19 = new ColumnModel("退档原因", "tdyy.Name");
            ColumnModel model20 = new ColumnModel("专业合格", "Zyhg", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model21 = new ColumnModel("体检合格", "Tjhg", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model22 = new ColumnModel("学科科目合格", "Xkkmhg", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model23 = new ColumnModel("术科科目合格", "Skkmhg", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model24 = new ColumnModel("录取序号", "Lqxh");
            ColumnModel model25 = new ColumnModel("投档单编号", "Tddbh");
            ColumnModel model26 = new ColumnModel("下载编号", "Xzbh");
            ColumnModel model27 = new ColumnModel("锁定标志", "Sdbz");
            ColumnModel model28 = new ColumnModel("备注", "Remark");
            ColumnModel model29 = new ColumnModel("志愿特征", "zytz.Name",CosDataBase.LONGINT_TYPE);
            ColumnModel model30 = new ColumnModel("志愿成绩", "Zycj",CosDataBase.FLOAT_TYPE);
            ColumnModel model31 = new ColumnModel("志愿成绩同分排位", "Zycjtfpw", CosDataBase.INT_TYPE);
            ColumnModel model32 = new ColumnModel("专业调剂类型", "zytjlx.Name", CosDataBase.LONGINT_TYPE);
            ColumnModel model33 = new ColumnModel("考生临时操作标志", "Kslsczbz");
            ColumnModel model34 = new ColumnModel("录取方式", "lqfs.Name", CosDataBase.LONGINT_TYPE);
            ColumnModel model35 = new ColumnModel("考生状态", "Kszt");
            ColumnModel model37 = new ColumnModel("专业志愿调剂", "Zyzytj", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model38 = new ColumnModel("定向调剂", "Dxtj", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model39 = new ColumnModel("专业调剂服从", "Zytjfc", CosDataBase.BOOLEAN_TYPE);
            ColumnModel model40 = new ColumnModel("体检结论", "tjjl.Name");
            ColumnModel model41 = new ColumnModel("编号", "Bh");


            arrColumn.Clear();
            arrColumn.Add(model2);
            //arrColumn.Add(model3);
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
            arrColumn.Add(model15);
            arrColumn.Add(model16);
            arrColumn.Add(model17);
            arrColumn.Add(model18);
            arrColumn.Add(model19);
            arrColumn.Add(model20);
            arrColumn.Add(model21);
            arrColumn.Add(model22);
            arrColumn.Add(model23);
            arrColumn.Add(model24);
            arrColumn.Add(model25);
            arrColumn.Add(model26);
            arrColumn.Add(model27);
            arrColumn.Add(model28);
            arrColumn.Add(model29);
            arrColumn.Add(model30);
            arrColumn.Add(model31);
            arrColumn.Add(model32);
            arrColumn.Add(model33);
            arrColumn.Add(model34);
            arrColumn.Add(model35);
            arrColumn.Add(model37);
            arrColumn.Add(model38);
            arrColumn.Add(model39);
            arrColumn.Add(model40);
            arrColumn.Add(model41);

        }

        public override void SetarrTopButton()
        {
            //ButtonModel btnMAdd = new ButtonModel(DataListButton.btnAdd);
            //ButtonModel btnMEdit = new ButtonModel(DataListButton.btnEdit);
            //ButtonModel btnMDelete = new ButtonModel(DataListButton.btnDelete);
            ButtonModel btnMSearch = new ButtonModel(DataListButton.btnSearch);
            ButtonModel btnMReset = new ButtonModel(DataListButton.btnReset);

            arrTopButton.Clear();
            //arrTopButton.Add(btnMAdd);
            //arrTopButton.Add(btnMEdit);
            //arrTopButton.Add(btnMDelete);
            arrTopButton.Add(btnMSearch);
            arrTopButton.Add(btnMReset);
        }
    }


}