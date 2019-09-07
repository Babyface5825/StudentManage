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
    public class bmkTableDataListManage : DataListManage
    {

        public bmkTableDataListManage()
        {
            Manage_KeyWord = "bmkTable";
        }


        public override void SetMainListType()
        {
            bHasDelFlag = true;
            bSingleSelect = false;
            bFirstVisit = false;
        }

        public override void SetarrColumn()
        {
            ColumnModel model0 = new ColumnModel("当前状态", "VerifyStatus", CosDataBase.STRING_TYPE, 60);
            ColumnModel model1 = new ColumnModel("录取年份", "YearCode", CosDataBase.INT_TYPE, 60);
            ColumnModel model2 = new ColumnModel("考生号", "Ksh", CosDataBase.STRING_TYPE, 120);
            ColumnModel model3 = new ColumnModel("准考证号", "Zkzh");
            ColumnModel model4 = new ColumnModel("姓名", "Xm", CosDataBase.STRING_TYPE, 60);
            ColumnModel model5 = new ColumnModel("性别", "xb.Name", CosDataBase.STRING_TYPE, 40);
            ColumnModel model6 = new ColumnModel("出生日期", "Csrq", CosDataBase.DATE_TYPE);
            ColumnModel model7 = new ColumnModel("政治面貌", "zzmm.Name", CosDataBase.STRING_TYPE, 60);
            ColumnModel model8 = new ColumnModel("民族", "mz.Name", CosDataBase.STRING_TYPE, 40);
            ColumnModel model9 = new ColumnModel("考生类别", "kslb.Name");
            ColumnModel model10 = new ColumnModel("考试类型", "kslx.Name");
            ColumnModel model11 = new ColumnModel("毕业类别", "bylb.Name");            
            ColumnModel model12 = new ColumnModel("毕业学校名称", "byxx.Name");
            ColumnModel model13 = new ColumnModel("外语语种", "wyyz.Name");
            ColumnModel model14 = new ColumnModel("所在地区", "dq.Name");
            ColumnModel model15 = new ColumnModel("证件号码", "Zjhm");


            //ColumnModel model8 = new ColumnModel("会考考号", "Hkkh");
            ColumnModel model16 = new ColumnModel("会考等级", "Hkdj");
            ColumnModel model17 = new ColumnModel("系统单位", "xtdw.Name");
            ColumnModel model18 = new ColumnModel("政审意见", "Zsyj");
            //ColumnModel model19 = new ColumnModel("证件类型", "zjlx.Name");
            ColumnModel model19 = new ColumnModel("学籍号", "Xjh");
            ColumnModel model20 = new ColumnModel("外语口试", "Wyks");
            ColumnModel model21 = new ColumnModel("外语听力", "Wytl");
            ColumnModel model22 = new ColumnModel("通讯地址", "Txdz");
            ColumnModel model23 = new ColumnModel("邮政编码", "Yzbm");
            ColumnModel model24 = new ColumnModel("联系手机", "Lxsj");
            ColumnModel model25 = new ColumnModel("联系电话", "Lxdh");
            ColumnModel model26 = new ColumnModel("收件人", "Sjr");
            ColumnModel model27 = new ColumnModel("考生特长", "Kstc");
            ColumnModel model28 = new ColumnModel("考生奖励和处罚", "Ksjlhcf");
            ColumnModel model29 = new ColumnModel("思想品德考核意见", "Sxpdkhyj");
            ColumnModel model30 = new ColumnModel("户籍类别", "hjlb.Name");
            ColumnModel model31 = new ColumnModel("残障类别", "czlb.Name");
            //ColumnModel model40 = new ColumnModel("考生临时操作标志", "Tag");
            arrColumn.Clear();
            arrColumn.Add(model0);
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
            //arrColumn.Add(model32);
            //arrColumn.Add(model33);            
            //arrColumn.Add(model36);
            //arrColumn.Add(model37);
            //arrColumn.Add(model40);            
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
            T_bmk obj = base.GetSpecialModelFromCurrentID(ID);
            if ((obj != null) && (obj.ID != 0))
            {
                T_bmk nowObj = GetOneRecordbyID<T_bmk, T>(ID);
                obj.ByxxName = nowObj.byxx.Name;
            }
            return obj;
        }

    }


}