using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using StudentManage.Controllers;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StudentManage.Areas.UserControl.Controllers
{

    public class WinStudentMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            long ID = InitFoundData(PanelID, ParentVariable);
            InitTddData(PanelID, ID);
            //InitKsjlData(PanelID, ID);
            InitPropertyGridData(PanelID, ID);
            InitTjxxData(PanelID, ID);
            
            return this.Direct();
        }

        /// <summary>
        /// 初始化基础信息
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="ParentVariable"></param>
        /// <returns></returns>
        private long InitFoundData(string PanelID, string ParentVariable)
        {
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectZzmmdmChange", null, "zzmmdmTable");
            SetHiddenValue(PanelID + "_cmbZzmmdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectMzdmChange", null, "mzdmTable");
            SetHiddenValue(PanelID + "_cmbMzdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectXbdmChange", null, "xbdmTable");
            SetHiddenValue(PanelID + "_cmbXbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectZjlxdmChange", null, "zjlxdmTable");
            SetHiddenValue(PanelID + "_cmbZjlxdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectKslbdmChange", null, "kslbdmTable");
            SetHiddenValue(PanelID + "_cmbKslbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectKslxdmChange", null, "kslxdmTable");
            SetHiddenValue(PanelID + "_cmbKslxdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectBylbdmChange", null, "bylbdmTable");
            SetHiddenValue(PanelID + "_cmbBylbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectDqdmChange", null, "dqdmTable");
            SetHiddenValue(PanelID + "_cmbDqdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectWyyzdmChange", null, "wyyzdmTable");
            SetHiddenValue(PanelID + "_cmbWyyzdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectHjlbdmChange", null, "hjlbdmTable");
            SetHiddenValue(PanelID + "_cmbHjlbdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectCzlbdmChange", null, "czlbdmTable");
            SetHiddenValue(PanelID + "_cmbCzlbdm_ParentVariable", ParentVar.DefaultVal);
            GetParentVariable(ParentVariable);
            bmkTableDataListManage studentMan = new bmkTableDataListManage();

            //如果userId中包含Row，说明是从DataList传来的selection
            string sStudentID = "";
            if (ParentVar.transValue.Contains("Row"))
            {
                studentMan.InitSelection(ParentVar.transValue);
                sStudentID = studentMan.sRowNowID;
            }            
            T_bmk model = studentMan.GetSpecialModelFromCurrentID(sStudentID);
            
            var form1 = this.GetCmp<FormPanel>(PanelID + "_Form1");
            var form5 = this.GetCmp<FormPanel>(PanelID + "_Form5");
            form1.SetValues(model);
            form5.SetValues(model);

            string sStudentImg = ConstAddressPath.ORIGINSTUPIC_WEBPATH + model.GxID + "/" + model.YearCode + "/" + model.Ksh + ".jpg";
            FileManager file = new FileManager();
            if (!file.HasFile(sStudentImg))
            {
                sStudentImg = ConstAddressPath.ORIGINSTUPIC_WEBPATH + model.GxID + "/" + model.YearCode + "/" + model.Ksh.Remove(0, 4) + ".jpg";
                if (!file.HasFile(sStudentImg))
                    sStudentImg = ConstAddressPath.ORIGINSTUPIC_PATH + "0.jpg";
                else
                    sStudentImg = ConstAddressPath.ORIGINSTUPIC_PATH + model.GxID + "/" + model.YearCode + "/" + model.Ksh.Remove(0, 4) + ".jpg";
            }
            else
                sStudentImg = ConstAddressPath.ORIGINSTUPIC_PATH + model.GxID + "/" + model.YearCode + "/" + model.Ksh + ".jpg";
            Image img = this.GetCmp<Image>(PanelID + "_StudentImg");
            img.ImageUrl = sStudentImg;
            if (model.Zzmmdm == 0)
                SetComboValue(PanelID + "_cmbZzmmdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbZzmmdm", model.Zzmmdm.ToString(), true);

            if (model.Mzdm == 0)
                SetComboValue(PanelID + "_cmbMzdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbMzdm", model.Mzdm.ToString(), true);

            if (model.Xbdm == 0)
                SetComboValue(PanelID + "_cmbXbdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbXbdm", model.Xbdm.ToString(), true);

            if (model.Zjlxdm == 0)
                SetComboValue(PanelID + "_cmbZjlxdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbZjlxdm", model.Zjlxdm.ToString(), true);

            if (model.Kslbdm == 0)
                SetComboValue(PanelID + "_cmbKslbdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbKslbdm", model.Kslbdm.ToString(), true);

            if (model.Kslxdm == 0)
                SetComboValue(PanelID + "_cmbKslxdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbKslxdm", model.Kslxdm.ToString(), true);

            if (model.Bylbdm == 0)
                SetComboValue(PanelID + "_cmbBylbdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbBylbdm", model.Bylbdm.ToString(), true);

            if (model.Dqdm == 0)
                SetThreeListComboValue(PanelID + "_cmbDqdm", "0");
            else
                SetThreeListComboValue(PanelID + "_cmbDqdm", model.Dqdm.ToString());

            if (model.Wyyzdm == 0)
                SetComboValue(PanelID + "_cmbWyyzdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbWyyzdm", model.Wyyzdm.ToString(), true);

            if (model.Hjlbdm == 0)
                SetComboValue(PanelID + "_cmbHjlbdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbHjlbdm", model.Hjlbdm.ToString(), true);

            if (model.Czlbdm == 0)
                SetComboValue(PanelID + "_cmbCzlbdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbCzlbdm", model.Czlbdm.ToString(), true);

            return model.ID;
        }
        /// <summary>
        /// 初始化投档单基础信息
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="ID"></param>
        private void InitTddData(string PanelID, long ID)
        {
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectTdlxdmChange", null, "tdlxdmTable");
            SetHiddenValue(PanelID + "_cmbTdlxdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectZytzdmChange", null, "zytzdmTable");
            SetHiddenValue(PanelID + "_cmbZytzdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectLqfsdmChange", null, "lqfsdmTable");
            SetHiddenValue(PanelID + "_cmbLqfsdm_ParentVariable", ParentVar.DefaultVal);
            setParentVariable("UserControl", "WinStudentMore", "WinStudentMore_SelectTdyydmChange", null, "tdyydmTable");
            SetHiddenValue(PanelID + "_cmbTdyydm_ParentVariable", ParentVar.DefaultVal);
            tddTableManage tddMan = new tddTableManage();
            T_tdd tddModel = tddMan.GetOneRecordbyRelationID(ID);
            var form2 = this.GetCmp<FormPanel>(PanelID + "_Form2");
            form2.SetValues(tddModel);

            if (tddModel.Tdlxdm == 0)
                SetComboValue(PanelID + "_cmbTdlxdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbTdlxdm", tddModel.Tdlxdm.ToString(), true);

            if (tddModel.Zytz == 0)
                SetComboValue(PanelID + "_cmbZytzdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbZytzdm", tddModel.Zytz.ToString(), true);

            if (tddModel.Lqfs == 0)
                SetComboValue(PanelID + "_cmbLqfsdm", "0", true);
            else
                SetComboValue(PanelID + "_cmbLqfsdm", tddModel.Lqfs.ToString(), true);

            if (tddModel.Tdyydm == 0)
                SetComboValue(PanelID + "_cmbTdyydm", "0", true);
            else
                SetComboValue(PanelID + "_cmbTdyydm", tddModel.Tdyydm.ToString(), true);

        }
        ///// <summary>
        ///// 初始化考生简历
        ///// </summary>
        ///// <param name="PanelID"></param>
        ///// <param name="ID"></param>
        //private void InitKsjlData(string PanelID, long ID)
        //{
        //    ksjlTableManage ksjlMan = new ksjlTableManage();
        //    //这个地方是用RelationID，因为只用ID的话得到的是一段简历，并不全
        //    List<T_ksjl> ksjlModel = ksjlMan.GetOneRecordbyRelationID(ID);
        //    //坑！考生简历这一段在cshtml中根本没有写FormPanel,如果写了下面两行代码，下面的Form4根本读不出来
        //    //var form3 = this.GetCmp<FormPanel>(PanelID + "_Form3");
        //    //form3.SetValues(ksjlModel);
        //}

        private void InitPropertyGridData(string PanelID, long ID)
        {
            ksxkcjTableManage ksxkcjMan = new ksxkcjTableManage();

            PropertyGrid proGrid1 = this.GetCmp<PropertyGrid>(PanelID + "_PropertyGrid1");
            PropertyGrid proGrid2 = this.GetCmp<PropertyGrid>(PanelID + "_PropertyGrid2");
            List<T_ksxkcj> ksxkcjModel = ksxkcjMan.GetOneRecordbyRelationID(ID);
             if((ksxkcjModel!=null)&&(ksxkcjModel.Count>0))
            {
                foreach(var ksxkcj in ksxkcjModel)
                {
                    PropertyGridParameter proPara = new PropertyGridParameter();
                    proPara.Name = ksxkcj.cjx.Name.ToString();
                    proPara.Value = ksxkcj.Ddcj.ToString();
                    if (ksxkcj.Cjlx == 2) //高考成绩
                        proGrid1.AddProperty(proPara);
                    else if (ksxkcj.Cjlx == 2) //会考成绩
                        proGrid2.AddProperty(proPara);
                }
            }
        }

        //private void InitKsxkcjData(string PanelID, long ID)
        //{
        //    ksxkcjTableManage ksxkcjMan = new ksxkcjTableManage();
        //    List<T_ksxkcj> ksxkcjModel = ksxkcjMan.GetOneRecordbyRelationID(ID);
        //}

        private void InitTjxxData(string PanelID, long ID)
        {
            tjxxTableManage tjxxMan = new tjxxTableManage();
            T_tjxx tjxxModel = tjxxMan.GetOneRecordbyRelationID(ID);
            var form4 = this.GetCmp<FormPanel>(PanelID + "_Form4");
            form4.SetValues(tjxxModel);
        }
        
        /// <summary>
        /// 初始化志愿信息
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sID"></param>
        /// <returns></returns>
        public ActionResult WinStudentMore_TddFocus(string PanelID, string sID)
        {
            SetHiddenValue(PanelID + "_zykDataList" + "_InitNames", "RelationID1");
            SetHiddenValue(PanelID + "_zykDataList" + "_InitValues", sID);
            SetHiddenValue(PanelID + "_zykDataList" + "_InitSigns", SIGN.Equal.ToString());
            SetHiddenValue(PanelID + "_zykDataList" + "_VisitStatus", "1");
            RefreashDataList(PanelID + "_zykDataList");
            return this.Direct();
        }

        /// <summary>
        /// 初始化考生简历
        /// </summary>
        /// <param name="PanelID"></param>
        /// <param name="sID"></param>
        /// <returns></returns>
        public ActionResult WinStudentMore_KsjlFocus(string PanelID, string sID)
        {
            SetHiddenValue(PanelID + "_ksjlDataList" + "_InitNames", "RelationID");
            SetHiddenValue(PanelID + "_ksjlDataList" + "_InitValues", sID);
            SetHiddenValue(PanelID + "_ksjlDataList" + "_InitSigns", SIGN.Equal.ToString());
            SetHiddenValue(PanelID + "_ksjlDataList" + "_VisitStatus", "1");
            RefreashDataList(PanelID + "_ksjlDataList");
            return this.Direct();
        }


        #region ItemChange
        public ActionResult WinStudentMore_SelectZzmmdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelZzmmdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectMzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelMzdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectXbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelXbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectZjlxdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelZjlxdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectKslbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelKslbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectKslxdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelKslxdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectBylbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelBylbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectDqdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelDqdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectWyyzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelWyyzdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectHjlbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelHjlbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectCzlbdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelCzlbdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectTdlxdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelTdlxdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectZytzdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelZytzdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectLqfsdmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelLqfsdm", sSelID);
            return this.Direct();
        }
        public ActionResult WinStudentMore_SelectTdyydmChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_SelTdyydm", sSelID);
            return this.Direct();
        }
        #endregion

        public ActionResult WindowSaveClick(T_bmk mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            if ((mo.VerifyStatus != "未审核") && (ParentVar.KeyWords == "1"))
                X.Msg.Alert("注意", "已经审核，不能修改！").Show();
            else if((mo.VerifyStatus=="未审核")&&(ParentVar.KeyWords=="1"))
            {                
                string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
                bmkTableManage man = new bmkTableManage();
                man.FirstVerifyOneRecord(mo);
                SetHiddenValue(PanelID + "_ExitCode", "1");
            }
            else if(ParentVar.KeyWords=="2")
            {
                bmkTableManage man = new bmkTableManage();
                man.SecondVerifyOneRecord(mo);
                SetHiddenValue(PanelID + "_ExitCode", "1");
            }            
            return this.Direct();
        }

    }
}