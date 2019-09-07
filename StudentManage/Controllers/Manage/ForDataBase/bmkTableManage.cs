using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Data.OleDb;
using System.Reflection;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class bmkTableManage : OriginTableManage
    {
        private DataTable XBDM_dt;

        public List<BMKTable> BMK_list;

        public List<ListCompareTable> BYLBDM_list;
        public List<ListCompareTable> CJXDM_list;
        public List<ListCompareTable> CZLBDM_list;
        public List<ListCompareTable> DQDM_list;
        public List<ListCompareTable> HJLBDM_list;

        public List<ListCompareTable> KLDM_list;
        public List<ListCompareTable> KMDM_list;
        public List<ListCompareTable> KSLBDM_list;
        public List<ListCompareTable> KSLXDM_list;
        public List<ListCompareTable> KSTZDM_list;

        public List<ListCompareTable> KSZGDM_list;
        public List<ListCompareTable> MZDM_list;
        public List<ListCompareTable> WYYZDM_list;
        public List<ListCompareTable> ZJLXDM_list;
        public List<ListCompareTable> XTDWDM_list;
                
        public List<ListCompareTable> ZZMMDM_list;
        public List<ListCompareTable> BYXXDM_list;
        public List<ListCompareTable> XBDM_list;


        public bmkTableManage() 
        {
            Manage_KeyWord = "bmkTable";
        }
        public void InitAllFoundDataList(OleDbConnection con, string sNowPath, DataTable dt, int nYear, long lProvinceID)
        {
            //XBDM_dt = BuildXBDM(con, sNowPath); //性别代码
            BuildBYXXDM(dt, nYear, lProvinceID); //毕业学校代码
            //BuildZYDH(dt, nYear, lProvinceID); //专业代号
        }
        
        #region 毕业学校代码

        private void BuildBYXXDM(DataTable dt, int nYear, long lProvinceID)
        {
            string[] mainColumns = { "BYXXDM", "BYXXMC" };
            BYXXDM_list = InportSimpleTableOriginData<Td_byxxdm, Td_byxxdm_Origin>(dt, nYear, lProvinceID, "byxxdmTable", mainColumns);
        }
        #endregion

        #region 性别代码
        private DataTable BuildXBDM(OleDbConnection con, string sNowPath)
        {
            string sNowFileName = "TD_XBDM.dbf";
            return GetDbfDataByODBC(GetPropertyFileName(sNowFileName), sNowPath, con);
        }
        private string GetNowXB(string xbdm)
        {
            if (XBDM_dt != null)
            {
                foreach (DataRow listRow in XBDM_dt.Rows)
                {
                    if (listRow["XBDM"].ToString() == xbdm)
                        return listRow["XBMC"].ToString();
                }
            }
            return "未知";
        }
        #endregion

        #region 审核相关
        private string VerifyOneStrResult(string keyword, string Str, string oldStr, string ColumnName, List<string> EditColumns)
        {
            if (Str == null)
                Str = "";
            if (oldStr == null)
                oldStr = "";
            if (Str != oldStr)
            {
                EditColumns.Add(ColumnName);
                return keyword + "由 " + "“"+oldStr+ "”" + " 修改为 " + "“" + Str + "”" + "；\n";
            }
            else
                return "";
        }
        private string VerifyOneDateResult(string keyword, DateTime Str, DateTime oldStr, string ColumnName, List<string> EditColumns)
        {
            if (Str != oldStr)
            {
                EditColumns.Add(ColumnName);
                return keyword + "由 " + "“" + oldStr.ToString("yyyy-MM-dd") + "”" + " 修改为 " + "“" + Str.ToString("yyyy-MM-dd") + "”" + "；\n";
            }
            else
                return "";
        }

        private string VerifyOneSelResult<T>(string keyword, long sel, long oldSel, string ColumnName, List<string> EditColumns) where T : class
        {
            if (sel != oldSel)
            {
                T mo = GetOneRecordbyID<T,long>(sel);
                T oldmo = GetOneRecordbyID<T, long>(oldSel);
                string Str = Convert.ToString(mo.GetType().GetProperty("Name").GetValue(mo, null));
                string oldStr = Convert.ToString(oldmo.GetType().GetProperty("Name").GetValue(oldmo, null));
                EditColumns.Add(ColumnName);
                return keyword + "由 " + "“" + oldStr + "”" + " 修改为 " + "“" + Str + "”" + "；\n";
            }
            else
                return "";
        }
        public void ReVerifyOneRecord(T_bmk mo)
        {
            List<string> EditColumns = new List<string>();
            mo.VerifyStatus = "未审核"; EditColumns.Add("VerifyStatus");
            SaveOneRecord(mo, EditColumns.ToArray());
        }

        public void FirstVerifyRecords(string sSelIDs)
        {
            ArrayList names = new ArrayList();
            ArrayList values = new ArrayList();
            ArrayList signs = new ArrayList();
            names.Add("ID"); values.Add(sSelIDs); signs.Add(SIGN.In);
            Expression<Func<T_bmk, bool>> where = GetAndLambdaExpression<T_bmk>(names, values, signs);
            List<T_bmk> list = GetRecord(where);
            List<T_bmk> list1 = new List<T_bmk>();
            if ((list != null) && (list.Count > 0))
            {
                int num = ProduceFirstNumber(list[0]);

                foreach (T_bmk l in list)
                {
                    if (l.VerifyStatus == "未审核")
                    {
                        l.VerifyStatus = "一次审核";
                        l.FlowNumber = l.YearCode.ToString() + l.GxID.ToString() + num.ToString().PadLeft(5, '0');
                        list1.Add(l);
                        num++;
                    }
                }
            }

            string[] EditColumns = { "VerifyStatus", "FlowNumber" };
            SaveSomeRecords(list1, EditColumns);
        }
        public void SecondVerifyRecords(string sSelIDs)
        {
            ArrayList names = new ArrayList(); ArrayList values = new ArrayList(); ArrayList signs = new ArrayList();
            names.Add("ID"); values.Add(sSelIDs); signs.Add(SIGN.In);
            Expression<Func<T_bmk, bool>> where = GetAndLambdaExpression<T_bmk>(names, values, signs);
            List<T_bmk> list = GetRecord(where);
            List<T_bmk> list1 = new List<T_bmk>();
            if ((list != null) && (list.Count > 0))
            {                
                foreach (T_bmk l in list)
                {
                    if (l.VerifyStatus == "一次审核")
                    {
                        l.VerifyStatus = "二次审核";                        
                        list1.Add(l);
                    }
                }
            }
            string[] EditColumns = { "VerifyStatus" };
            SaveSomeRecords(list1, EditColumns);
        }


        private string ProduceFlowNumber(T_bmk mo)
        {
            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("YearCode"); values.Add(mo.YearCode);
            names.Add("GxID"); values.Add(mo.GxID);
            names.Add("DelFlag"); values.Add(false);
            Expression<Func<T_bmk, bool>> where = GetAndLambdaExpression<T_bmk>(names, values);
            Expression<Func<T_bmk, string>> order = BuildOrder<T_bmk, string>("FlowNumber");
            List<T_bmk> list = GetRecord(where, order, false);
            string sStr = mo.YearCode.ToString() + mo.GxID.ToString();
            if ((list != null) && (list.Count > 0) && list[0].FlowNumber != null)
            {
                int nMaxNum = Convert.ToInt32(list[0].FlowNumber.Remove(0, sStr.Length));
                sStr += (nMaxNum + 1).ToString().PadLeft(5, '0');
            }
            else
                sStr += "00001";
            return sStr;
        }

        private int ProduceFirstNumber(T_bmk mo)
        {
            ArrayList names = new ArrayList(); ArrayList values = new ArrayList();
            names.Add("YearCode"); values.Add(mo.YearCode);
            names.Add("GxID"); values.Add(mo.GxID);
            names.Add("DelFlag"); values.Add(false);
            Expression<Func<T_bmk, bool>> where = GetAndLambdaExpression<T_bmk>(names, values);
            Expression<Func<T_bmk, string>> order = BuildOrder<T_bmk, string>("FlowNumber");
            List<T_bmk> list = GetRecord(where, order, false);
            string sStr = mo.YearCode.ToString() + mo.GxID.ToString();
            if ((list != null) && (list.Count > 0) && list[0].FlowNumber != null)
                return Convert.ToInt32(list[0].FlowNumber.Remove(0, sStr.Length)) + 1;
            else
                return 1;            
        }

        public void FirstVerifyOneRecord(T_bmk mo)
        {   
            List<string> EditColumns;
            string sVerifyResult = VerifyCore(mo, out EditColumns);
            mo.VerifyStatus = "一次审核"; EditColumns.Add("VerifyStatus");
            mo.VerifyResult = sVerifyResult; EditColumns.Add("VerifyResult");
            if ((mo.FlowNumber == null) || (mo.FlowNumber == ""))
            {
                mo.FlowNumber = ProduceFlowNumber(mo);
                EditColumns.Add("FlowNumber");
            }
            SaveOneRecord(mo, EditColumns.ToArray());
        }

        public void SecondVerifyOneRecord(T_bmk mo)
        {
            List<string> EditColumns;
            string sVerifyResult = VerifyCore(mo, out EditColumns);
            mo.VerifyStatus = "二次审核"; EditColumns.Add("VerifyStatus");
            mo.VerifyResult = sVerifyResult; EditColumns.Add("VerifyResult");
            SaveOneRecord(mo, EditColumns.ToArray());
        }

        private string VerifyCore(T_bmk mo, out List<string> EditColumns)
        {
            string sVerifyResult = mo.VerifyResult;
            T_bmk_Origin oldMo = GetOneRecordbyID<T_bmk_Origin, long>(mo.RelationID);
            sVerifyResult = mo.VerifyResult;
            EditColumns = new List<string>();
            sVerifyResult += VerifyOneStrResult("考生号", mo.Ksh, oldMo.Ksh, "Ksh", EditColumns);
            sVerifyResult += VerifyOneStrResult("准考证号", mo.Zkzh, oldMo.Zkzh, "Zkzh", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_xbdm>("性别", mo.Xbdm, oldMo.Xbdm, "Xbdm", EditColumns);
            sVerifyResult += VerifyOneDateResult("出生日期", mo.Csrq, oldMo.Csrq, "Csrq", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_xbdm>("政治面貌", mo.Zzmmdm, oldMo.Zzmmdm, "Zzmmdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_mzdm>("民族", mo.Mzdm, oldMo.Mzdm, "Mzdm", EditColumns);
            sVerifyResult += VerifyOneStrResult("会考考号", mo.Hkkh, oldMo.Hkkh, "Hkkh", EditColumns);
            sVerifyResult += VerifyOneStrResult("政审意见", mo.Zsyj, oldMo.Zsyj, "Zsyj", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_kslbdm>("考生类别", mo.Kslbdm, oldMo.Kslbdm, "Kslbdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_kslxdm>("考试类型", mo.Kslxdm, oldMo.Kslxdm, "Kslxdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_bylbdm>("毕业类别", mo.Bylbdm, oldMo.Bylbdm, "Bylbdm", EditColumns);

            sVerifyResult += VerifyOneSelResult<Td_wyyzdm>("外语语种", mo.Wyyzdm, oldMo.Wyyzdm, "Wyyzdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_dqdm>("地区", mo.Dqdm, oldMo.Dqdm, "Dqdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_zjlxdm>("证件类型", mo.Zjlxdm, oldMo.Zjlxdm, "Zjlxdm", EditColumns);
            sVerifyResult += VerifyOneStrResult("证件号码", mo.Zjhm, oldMo.Zjhm, "Zjhm", EditColumns);
            sVerifyResult += VerifyOneStrResult("学籍号", mo.Xjh, oldMo.Xjh, "Xjh", EditColumns);

            sVerifyResult += VerifyOneStrResult("通讯地址", mo.Txdz, oldMo.Txdz, "Txdz", EditColumns);
            sVerifyResult += VerifyOneStrResult("邮政编码", mo.Yzbm, oldMo.Yzbm, "Yzbm", EditColumns);
            sVerifyResult += VerifyOneStrResult("联系手机", mo.Lxsj, oldMo.Lxsj, "Lxsj", EditColumns);
            sVerifyResult += VerifyOneStrResult("联系电话", mo.Lxdh, oldMo.Lxdh, "Lxdh", EditColumns);
            sVerifyResult += VerifyOneStrResult("收件人", mo.Sjr, oldMo.Sjr, "Sjr", EditColumns);
            sVerifyResult += VerifyOneStrResult("考生特长", mo.Kstc, oldMo.Kstc, "Kstc", EditColumns);
            sVerifyResult += VerifyOneStrResult("考生奖惩", mo.Ksjlhcf, oldMo.Ksjlhcf, "Ksjlhcf", EditColumns);
            sVerifyResult += VerifyOneStrResult("思品意见", mo.Sxpdkhyj, oldMo.Sxpdkhyj, "Sxpdkhyj", EditColumns);

            sVerifyResult += VerifyOneSelResult<Td_hjlbdm>("户籍类别", mo.Hjlbdm, oldMo.Hjlbdm, "Hjlbdm", EditColumns);
            sVerifyResult += VerifyOneSelResult<Td_czlbdm>("残障类别", mo.Czlbdm, oldMo.Czlbdm, "Czlbdm", EditColumns);
            return sVerifyResult;
        }

        #endregion




        /// <summary>
        /// 当前考生在报名库中是否存在
        /// </summary>
        /// <param name="ksh"></param>
        /// <returns></returns>
        public bool ExistStudent(string ksh)
        {
            Expression<Func<T_bmk, bool>> where = (c => c.Ksh == ksh && c.DelFlag == false);
            List<T_bmk> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 当前考生在报名库中是否存在
        /// </summary>
        /// <param name="ksh"></param>
        /// <returns></returns>
        public long GetStudentID(string ksh)
        {
            Expression<Func<T_bmk, bool>> where = (c => c.Ksh == ksh && c.DelFlag == false);
            return ExistOneRecord(where);
            //List<T_bmk> list = GetRecord(where);
            //if ((list != null) && (list.Count > 0))
            //    return list[0].ID;
            //else
            //    return 0;
        }
        /// <summary>
        /// 根据当前考生号返回在报名库中的主ID
        /// </summary>
        /// <param name="ksh"></param>
        /// <returns></returns>
        public long GetbmkID(string ksh)
        {
            Expression<Func<T_bmk, bool>> where = (c => c.Ksh == ksh && c.DelFlag == false);
            return ExistOneRecord(where);
            //List<T_bmk> list = GetRecord(where);
            //if ((list != null) && (list.Count > 0))
            //    return list[0].ID;
            //else
            //    return 0;
        }


        private string GetKstz(string sKstz, bool isOrigin)
        {
            string sReturn = "";
            if (sKstz.Length > 0)
            {
                for (int i = 0; i < sKstz.Length; i++)
                {
                    if (isOrigin)
                        sReturn = sReturn + GetOriginListID(KSTZDM_list, sKstz[i].ToString()) + CosValue.SPLITOPERATOR2;
                    else
                        sReturn = sReturn + GetMainListID(KSTZDM_list, sKstz[i].ToString()) + CosValue.SPLITOPERATOR2;
                }
            }
            if (sReturn.Length > 1)
                sReturn = sReturn.Remove(sReturn.Length - 1);
            return sReturn;
        }

        private string GetKszg(string sKszg, bool isOrigin)
        {
            string sReturn = "";
            if (sKszg.Length > 0)
            {
                for (int i = 0; i < sKszg.Length; i++)
                {
                    if (isOrigin)
                        sReturn = sReturn + GetOriginListID(KSZGDM_list, sKszg[i].ToString()) + CosValue.SPLITOPERATOR2;
                    else
                        sReturn = sReturn + GetMainListID(KSZGDM_list, sKszg[i].ToString()) + CosValue.SPLITOPERATOR2;
                }
            }
            if (sReturn.Length > 1)
                sReturn = sReturn.Remove(sReturn.Length - 1);
            return sReturn;
        }

        private string GetKmxx(string sKmxx, bool isOrigin)
        {
            string sReturn = "";
            if (sKmxx.Length > 0)
            {
                string[] code = sKmxx.Split('*');

                for (int i = 0; i < code.Length; i++)
                {
                    if (isOrigin)
                        sReturn = sReturn + GetOriginListID(KMDM_list, code[i].ToString()) + CosValue.SPLITOPERATOR2;
                    else
                        sReturn = sReturn + GetMainListID(KMDM_list, code[i].ToString()) + CosValue.SPLITOPERATOR2;
                }
            }
            if (sReturn.Length > 1)
                sReturn = sReturn.Remove(sReturn.Length - 1);
            return sReturn;
        }

        private T_bmk_Origin CreateNewModel(DataRow dr, int nYear, long GxID, long lProvinceID)
        {
            T_bmk_Origin bmk_model = new T_bmk_Origin();
            bmk_model.Ksh = GetOneStrValue(dr, "KSH");
            bmk_model.Zkzh = GetOneStrValue(dr, "ZKZH");
            bmk_model.Xm = GetOneStrValue(dr, "XM");
            //bmk_model.Xb = GetNowXB(GetOneStrValue(dr, "XBDM"));
            bmk_model.Csrq = Convert.ToDateTime(GetOneStrValue(dr, "CSNY", CosDataBase.DATE_TYPE));
            bmk_model.YearCode = nYear;
            bmk_model.GxID = GxID;
            bmk_model.ProvinceID = lProvinceID;


            ListCompareTable oneCompare = GetCompareList(ZZMMDM_list, GetOneStrValue(dr, "ZZMMDM"));
            if (oneCompare != null)
            {
                bmk_model.ZzmmdmOrigin = oneCompare.lOriginID;
                bmk_model.Zzmmdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(MZDM_list, GetOneStrValue(dr, "MZDM"));
            if (oneCompare != null)
            {
                bmk_model.MzdmOrigin = oneCompare.lOriginID;
                bmk_model.Mzdm = oneCompare.lNewID;
            }
            oneCompare = GetCompareList(XBDM_list, GetOneStrValue(dr, "XBDM"));
            if (oneCompare != null)
            {
                bmk_model.XbdmOrigin = oneCompare.lOriginID;
                bmk_model.Xbdm = oneCompare.lNewID;
            }

            bmk_model.Hkkh = GetOneStrValue(dr, "HKKH");
            bmk_model.Hkdj = GetOneStrValue(dr, "HKDJ");

            oneCompare = GetCompareList(XTDWDM_list, GetOneStrValue(dr, "XTDW"));
            if (oneCompare != null)
            {
                bmk_model.XtdwdmOrigin = oneCompare.lOriginID;
                bmk_model.Xtdwdm = oneCompare.lNewID;
            }

            //bmk_model.Sfzh = GetOneStrValue(dr, "SFZH");
            //bmk_model.Jtdz = GetOneStrValue(dr, "JTDZ");
            bmk_model.Zsyj = GetOneStrValue(dr, "ZSYJ");
            //bmk_model.Kldm = GetMainListID(KLDM_list, GetOneStrValue(dr, "KLDM"));
            bmk_model.KldmOrigin = GetOriginListID(KLDM_list, GetOneStrValue(dr, "KLDM"));

            oneCompare = GetCompareList(KSLBDM_list, GetOneStrValue(dr, "KSLBDM"));
            if (oneCompare != null)
            {
                bmk_model.KslbdmOrigin = oneCompare.lOriginID;
                bmk_model.Kslbdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(KSLXDM_list, GetOneStrValue(dr, "KSLXDM"));
            if (oneCompare != null)
            {
                bmk_model.KslxdmOrigin = oneCompare.lOriginID;
                bmk_model.Kslxdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(BYLBDM_list, GetOneStrValue(dr, "BYLBDM"));
            if (oneCompare != null)
            {
                bmk_model.BylbdmOrigin = oneCompare.lOriginID;
                bmk_model.Bylbdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(BYXXDM_list, GetOneStrValue(dr, "BYXXDM"));
            if (oneCompare != null)
            {
                bmk_model.ByxxdmOrigin = oneCompare.lOriginID;
                bmk_model.Byxxdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(WYYZDM_list, GetOneStrValue(dr, "WYYZDM"));
            if (oneCompare != null)
            {
                bmk_model.WyyzdmOrigin = oneCompare.lOriginID;
                bmk_model.Wyyzdm = oneCompare.lNewID;
            }

            bmk_model.Kstz = GetKstz(GetOneStrValue(dr, "KSTZ"), false);
            bmk_model.KstzOrigin = GetKstz(GetOneStrValue(dr, "KSTZ"), true);

            oneCompare = GetCompareList(DQDM_list, GetOneStrValue(dr, "DQDM"));
            if (oneCompare != null)
            {
                bmk_model.DqdmOrigin = oneCompare.lOriginID;
                bmk_model.Dqdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(ZJLXDM_list, GetOneStrValue(dr, "ZJLXDM"));
            if (oneCompare != null)
            {
                bmk_model.ZjlxdmOrigin = oneCompare.lOriginID;
                bmk_model.Zjlxdm = oneCompare.lNewID;
            }

            bmk_model.Zjhm = GetOneStrValue(dr, "ZJHM");
            bmk_model.Xjh = GetOneStrValue(dr, "XJH");
            bmk_model.Wyks = GetOneStrValue(dr, "WYKS");
            bmk_model.Wytl = GetOneStrValue(dr, "WYTl");
            bmk_model.Txdz = GetOneStrValue(dr, "TXDZ");
            bmk_model.Yzbm = GetOneStrValue(dr, "YZBM");
            bmk_model.Lxsj = GetOneStrValue(dr, "LXSJ");
            bmk_model.Lxdh = GetOneStrValue(dr, "LXDH");
            bmk_model.Sjr = GetOneStrValue(dr, "SJR");
            bmk_model.Kstc = GetOneStrValue(dr, "KSTC");
            bmk_model.Ksjlhcf = GetOneStrValue(dr, "KSJLHCF");
            bmk_model.Sxpdkhyj = GetOneStrValue(dr, "SXPDKHYJ");            
            bmk_model.Kszg = GetKszg(GetOneStrValue(dr, "KSZG"), false);
            bmk_model.KszgOrigin = GetKszg(GetOneStrValue(dr, "KSZG"), true);

            oneCompare = GetCompareList(HJLBDM_list, GetOneStrValue(dr, "HJLBDM"));
            if (oneCompare != null)
            {
                bmk_model.HjlbdmOrigin = oneCompare.lOriginID;
                bmk_model.Hjlbdm = oneCompare.lNewID;
            }

            oneCompare = GetCompareList(CZLBDM_list, GetOneStrValue(dr, "CZLBDM"));
            if (oneCompare != null)
            {
                bmk_model.CzlbdmOrigin = oneCompare.lOriginID;
                bmk_model.Czlbdm = oneCompare.lNewID;
            }

            //bmk_model.Tag = GetOneStrValue(dr, "TAG");
            bmk_model.Xkkm = GetKmxx(GetOneStrValue(dr, "XKKM"), false);
            bmk_model.XkkmOrigin = GetKmxx(GetOneStrValue(dr, "XKKM"), true);
            bmk_model.Skkm = GetKmxx(GetOneStrValue(dr, "SKKM"), false);
            bmk_model.SkkmOrigin = GetKmxx(GetOneStrValue(dr, "SKKM"), true);
            return bmk_model;
        }

        private T_bmk CopyOneBmk(T_bmk_Origin or)
        {
            T_bmk bmk = new T_bmk();

            Type type = typeof(T_bmk_Origin);
            MemberInfo[] mifos = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            foreach (MemberInfo m in mifos)
            {
                if ((m.MemberType == MemberTypes.Property) && !(m.ToString().Contains("List`1")) && !(m.ToString().Contains("Models")))
                {
                    var ob = or.GetType().GetProperty(m.Name);
                    if (ob != null)
                        bmk.GetType().GetProperty(m.Name).SetValue(bmk, ob.GetValue(or, null));
                    else
                        bmk.GetType().GetProperty(m.Name).SetValue(bmk, null);
                }
            }
            return bmk;
        }

        public List<BMKTable> AddSomeNewStudents(DataTable dt, int nYear, long GxID, long lProvinceID)
        {
            List<BMKTable> listReturn = new List<BMKTable>();
            List<T_bmk_Origin> listOrigin = new List<T_bmk_Origin>();
            List<T_bmk> list = new List<T_bmk>();
            foreach (DataRow dr in dt.Rows)
            {
                long lNewID = GetStudentID(dr["KSH"].ToString());
                if (lNewID == 0)
                {
                    BMKTable oneReturn = new BMKTable();
                    oneReturn.KSH = dr["KSH"].ToString();
                    if (oneReturn.KSH.Length > 4)
                        oneReturn.smallKSH = dr["KSH"].ToString().Substring(4);
                    listReturn.Add(oneReturn);
                    T_bmk_Origin or = CreateNewModel(dr, nYear, GxID, lProvinceID);
                    listOrigin.Add(or);
                    list.Add(CopyOneBmk(or));
                }
                else //如果存在的话就不要再加了。再加会影响其他的
                {
                    //BMKTable oneReturn = new BMKTable();
                    //oneReturn.KSH = dr["KSH"].ToString();
                    //if (oneReturn.KSH.Length > 4)
                    //    oneReturn.smallKSH = dr["KSH"].ToString().Substring(4);
                    //oneReturn.ID = lNewID;
                    //listReturn.Add(oneReturn);
                }
            }
            List<long> allOriginIDs = AddRecords(listOrigin);
            if ((allOriginIDs != null) && (allOriginIDs.Count > 0))
                for (int i = 0; i < list.Count; i++)
                    list[i].RelationID = allOriginIDs[i];

            List<long> allIDs = AddRecords(list);
            if ((allIDs != null) && (allIDs.Count > 0))
            {
                int j = 0;
                for (int i = 0; i < listReturn.Count; i++)
                {
                    if (listReturn[i].ID == 0)
                    {
                        listReturn[i].ID = allIDs[j];
                        j++;
                    }
                }
            }
            return listReturn;
        }

        /// <summary>
        /// 往报名库中增加一条记录
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public long AddNewStudent(DataRow dr, int nYear, long GxID, long lProvinceID)
        {
            long lNewID = 0, lOriginNewID = 0;
            if (!ExistStudent(dr["KSH"].ToString()))
            {
                T_bmk_Origin or = CreateNewModel(dr, nYear, GxID, lProvinceID);
                lOriginNewID = AddOneRecord(or);
                T_bmk bmk = CopyOneBmk(or);
                bmk.RelationID = lOriginNewID;
                lNewID = AddOneRecord(bmk);
            }
            return lNewID;
        }
        
        /// <summary>
        /// 报名库数据导入。报名库数据中含有唯一标志KSH，因此不需要创建ListTable，不需要保留过去信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="con"></param>
        /// <param name="sNowPath"></param>
        /// <param name="nYear"></param>
        /// <param name="lProvinceID"></param>
        public List<BMKTable> InportOriginData(DataTable dt, OleDbConnection con, string sNowPath, int nYear, long lProvinceID, long GxID)
        {
            if (dt != null)
            {
                InitAllFoundDataList(con, sNowPath, dt, nYear, lProvinceID);
                BMK_list = AddSomeNewStudents(dt, nYear, GxID, lProvinceID);
                InportGkcj(dt, BMK_list, CJXDM_list);
                return BMK_list;
            }
            return null;
        }
    }
}