using System.Data;
using StudentManage.Models;
using StudentManage.Controllers.Task;
using StudentManage.Controllers;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data.OleDb;
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Web;

namespace StudentManage.Manage
{
    /// <summary>
    /// 新旧代码对照类
    /// </summary>
    public class ListCompareTable
    {
        /// <summary>
        /// 原始数据库中的DM
        /// </summary>
        public string sOldID;
        /// <summary>
        /// 主表中的ID
        /// </summary>
        public long lNewID;
        /// <summary>
        /// 原始表中的ID
        /// </summary>
        public long lOriginID;
    }


    public class oneThreadModel
    {
        public string sNowPath;
        public long lProvinceID;
        public int nNowYear;
        public tblOriginDataPackageInfo model = new tblOriginDataPackageInfo();
    }

    public class oneAlertThreadModel
    {
        public Thread th;
        public string sID;
        public HttpRequestBase response;
    }


    /// <summary>
    /// 
    /// </summary>
    public class OriginDataPackageManage : ParentManage
    {
        public Stopwatch sw = new Stopwatch();
        public string[] sNowFileNameSet = new string[41];


        public OriginDataPackageManage() 
        {
            Manage_KeyWord = "dqdmTable";
        }



        #region 数据导入处理
        /// <summary>
        /// 获得一个所有导入文件名的列表
        /// </summary>
        /// <returns></returns>
        private string[] GetNowFileSet()
        {
            string[] _NowFileNameSet =
                    {"td_bylbdm.dbf", "td_ccdm.dbf","td_cjxdm.dbf", "td_czlbdm.dbf","td_dqdm.dbf",
                        "td_hjlbdm.dbf","td_jhlbdm.dbf","td_jhxzdm.dbf","td_kldm.dbf","td_kmdm.dbf",
                        "td_kslbdm.dbf","td_kslxdm.dbf","td_kstzdm.dbf","td_kszgdm.dbf","td_lqfsdm.dbf","td_lqlxdm.dbf",
                        "td_mzdm.dbf","td_pcdm.dbf","td_tdlxdm.dbf","td_tdyydm.dbf","td_tjjldm.dbf",
                        "td_wyyzdm.dbf","td_xtdwdm.dbf","td_xzdm.dbf", "td_zjlxdm.dbf","td_zklxdm.dbf",
                        "td_zylbdm.dbf", "td_zytjlxdm.dbf","td_zytzdm.dbf","td_zzmmdm.dbf","td_byxxdm.dbf" ,"td_xbdm.dbf" };
            List<string> sSet = _NowFileNameSet.OrderBy(c => Guid.NewGuid()).ToArray().ToList(); //打乱顺序
            sSet.Add("t_tddw.dbf"); sSet.Add("Td_zydh.dbf"); sSet.Add("T_jhk.dbf");
            sSet.Add("t_bmk.dbf"); sSet.Add("t_tdd.dbf"); sSet.Add("t_zykmx.dbf"); sSet.Add("t_ksjl.dbf"); sSet.Add("t_kshkcj.dbf");
            sSet.Add("t_ksxkcj.dbf"); sSet.Add("t_tjxx.dbf");
            return sSet.ToArray();
        }
        /// <summary>
        /// 将一个dbf文件内容以文本方式读入
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        private List<string> ReadWholeFile(string sFileName)
        {
            List<string> ss = new List<string>();            
            StreamReader sr = new StreamReader(sFileName, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
                ss.Add(line);
            return ss;
        }

        /// <summary>
        /// 对解压出来后的所有dbf文件重命名，原则是文件名长度不能超过8
        /// </summary>
        /// <param name="sRootPath"></param>
        private void RenameAllDBFFiles(string sRootPath)
        {
            FileManager file = new FileManager();
            string[] AllFileNames = Directory.GetFileSystemEntries(sRootPath);
            foreach (string sNowPath in AllFileNames)
            {
                if (Directory.Exists(sNowPath))
                    RenameAllDBFFiles(sNowPath);
                else
                {
                    string sNowFileName = sNowPath.Substring(sNowPath.LastIndexOf('\\') + 1);
                    sNowFileName = sNowFileName.Substring(0, sNowFileName.IndexOf('.'));
                    if (sNowFileName.Length > 8)
                    {
                        sNowFileName = sNowFileName.Remove(0, sNowFileName.Length - 8);
                        string sNewFileName = sNowPath.Substring(0, sNowPath.LastIndexOf('\\') + 1) + sNowFileName + ".dbf";
                        file.RenameFile(sNowPath, sNewFileName);
                    }
                }
            }
        }

        private void InportOneWholePath(string sNowPath, int nNowYear, long lProvinceID, tblOriginDataPackageInfo model)
        {
            string[] sAllNextPath = Directory.GetFileSystemEntries(sNowPath);
            int nn = 1;
            foreach (string sNextPath in sAllNextPath)
            {                
                if (Directory.Exists(sNextPath))
                {
                    string ss = nn.ToString();
                    string sNewPath = sNextPath.Substring(0, sNextPath.LastIndexOf('\\') + 1) + ss;
                    Directory.Move(sNextPath, sNewPath);                    
                    nn++;
                    InportOneWholePath(sNewPath, nNowYear, lProvinceID, model);
                }
            }
            string[] sNowFileNameSet = GetNowFileSet();
            //打开连接
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sNowPath + ";Extended Properties=dBASE IV;";
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();
            //初始化相关的list
            List<ListCompareTable> BYLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> CCDM_list = new List<ListCompareTable>();
            List<ListCompareTable> CJXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> CZLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> DQDM_list = new List<ListCompareTable>();

            List<ListCompareTable> HJLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> JHLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> JHXZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> KLDM_list = new List<ListCompareTable>();
            List<ListCompareTable> KMDM_list = new List<ListCompareTable>();

            List<ListCompareTable> KSLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> KSLXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> KSTZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> KSZGDM_list = new List<ListCompareTable>();
            List<ListCompareTable> LQFSDM_list = new List<ListCompareTable>();

            List<ListCompareTable> MZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> PCDM_list = new List<ListCompareTable>();
            List<ListCompareTable> TDLXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> TDYYDM_list = new List<ListCompareTable>();
            List<ListCompareTable> TJJLDM_list = new List<ListCompareTable>();

            List<ListCompareTable> WYYZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> XTDWDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZCDM_list = new List<ListCompareTable>();
            List<ListCompareTable> XZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZJLXDM_list = new List<ListCompareTable>();

            List<ListCompareTable> ZKLXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZYLBDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZYTJLXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZYTZDM_list = new List<ListCompareTable>();
            List<ListCompareTable> ZZMMDM_list = new List<ListCompareTable>();

            List<ListCompareTable> BYXXDM_list = new List<ListCompareTable>();
            List<ListCompareTable> XBDM_list = new List<ListCompareTable>();

            List<ListCompareTable> TDDW_list = new List<ListCompareTable>();
            List<ListCompareTable> ZYDH_list = new List<ListCompareTable>();
                        
            List<BMKTable> BMK_list = new List<BMKTable>();
            List<T_jhk> JHK_list = new List<T_jhk>();
            OriginTableManage man = new OriginTableManage();
            dqdmTableManage dqdmMan = new dqdmTableManage();

            //下面就是一个个表的导入了
            string[] mainColumns = new string[2];
            for (int i = 0; i < sNowFileNameSet.Length; i++)
            {
                string sNowFileName = sNowFileNameSet[i];
                DataTable dt = GetDbfDataByODBC(GetPropertyFileName(sNowFileName), sNowPath, con);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    //sw.Start();
                    switch (sNowFileName.ToLower())
                    {
                        case "td_bylbdm.dbf"://毕业类别代码
                            mainColumns[0] = "BYLBDM"; mainColumns[1] = "BYLBMC";
                            BYLBDM_list = man.InportSimpleTableOriginData<Td_bylbdm, Td_bylbdm_Origin>(dt, nNowYear, lProvinceID, "bylbdmTable", mainColumns);
                            break;
                        case "td_ccdm.dbf"://层次代码
                            mainColumns[0] = "CCDM"; mainColumns[1] = "CCMC";
                            CCDM_list = man.InportSimpleTableOriginData<Td_ccdm, Td_ccdm_Origin>(dt, nNowYear, lProvinceID, "ccdmTable", mainColumns);
                            break;
                        case "td_cjxdm.dbf"://成绩项代码
                            mainColumns[0] = "CJXDM"; mainColumns[1] = "CJXMC";
                            CJXDM_list = man.InportSimpleTableOriginData<Td_cjxdm, Td_cjxdm_Origin>(dt, nNowYear, lProvinceID, "cjxdmTable", mainColumns);
                            break;
                        case "td_czlbdm.dbf"://残障类别代码
                            mainColumns[0] = "CZLBDM"; mainColumns[1] = "CZLBMC";
                            CZLBDM_list = man.InportSimpleTableOriginData<Td_czlbdm, Td_czlbdm_Origin>(dt, nNowYear, lProvinceID, "czlbdmTable", mainColumns);
                            break;
                        case "td_dqdm.dbf": //地区代码
                            mainColumns[0] = "DQDM"; mainColumns[1] = "DQMC";
                            DQDM_list = dqdmMan.InportOriginData(dt, nNowYear, lProvinceID, mainColumns);
                            break;

                        case "td_hjlbdm.dbf": //户籍类别代码
                            mainColumns[0] = "HJLBDM"; mainColumns[1] = "HJLBMC";
                            HJLBDM_list = man.InportSimpleTableOriginData<Td_hjlbdm, Td_hjlbdm_Origin>(dt, nNowYear, lProvinceID, "hjlbdmTable", mainColumns);
                            break;
                        case "td_jhlbdm.dbf": //计划类别代码
                            mainColumns[0] = "JHLBDM"; mainColumns[1] = "JHLBMC";
                            JHLBDM_list = man.InportSimpleTableOriginData<Td_jhlbdm, Td_jhlbdm_Origin>(dt, nNowYear, lProvinceID, "jhlbdmTable", mainColumns);
                            break;
                        case "td_jhxzdm.dbf": //计划性质代码
                            mainColumns[0] = "JHXZDM"; mainColumns[1] = "JHXZMC";
                            JHXZDM_list = man.InportSimpleTableOriginData<Td_jhxzdm, Td_jhxzdm_Origin>(dt, nNowYear, lProvinceID, "jhxzdmTable", mainColumns);
                            break;
                        case "td_kldm.dbf": //科类代码
                            mainColumns[0] = "KLDM"; mainColumns[1] = "KLMC";
                            KLDM_list = man.InportSimpleTableOriginData<Td_kldm, Td_kldm_Origin>(dt, nNowYear, lProvinceID, "kldmTable", mainColumns);
                            break;
                        case "td_kmdm.dbf": //科目代码
                            mainColumns[0] = "KMDM"; mainColumns[1] = "KMMC";
                            KMDM_list = man.InportSimpleTableOriginData<Td_kmdm, Td_kmdm_Origin>(dt, nNowYear, lProvinceID, "kmdmTable", mainColumns);
                            break;

                        case "td_kslbdm.dbf": //考生类别代码
                            mainColumns[0] = "KSLBDM"; mainColumns[1] = "KSLBMC";
                            KSLBDM_list = man.InportSimpleTableOriginData<Td_kslbdm, Td_kslbdm_Origin>(dt, nNowYear, lProvinceID, "kslbdmTable", mainColumns);
                            break;
                        case "td_kslxdm.dbf": //考试类型代码
                            mainColumns[0] = "KSLXDM"; mainColumns[1] = "KSLXMC";
                            KSLXDM_list = man.InportSimpleTableOriginData<Td_kslxdm, Td_kslxdm_Origin>(dt, nNowYear, lProvinceID, "kslxdmTable", mainColumns);
                            break;
                        case "td_zcdm.dbf"://政策代码（12有17无）
                            mainColumns[0] = "ZCDM"; mainColumns[1] = "ZCMC";
                            string[] moreColumns1 = { "ZGF" };
                            int[] moreColumnTypes1 = { CosDataBase.FLOAT_TYPE };
                            KSTZDM_list = man.InportSimpleTableOriginData<Td_kstzdm, Td_kstzdm_Origin>(dt, nNowYear, lProvinceID, "kstzdmTable", mainColumns, moreColumns1, moreColumnTypes1);
                            break;
                        case "td_kstzdm.dbf"://考生特征代码（17有12无）
                            mainColumns[0] = "KSTZDM"; mainColumns[1] = "KSTZMC";
                            string[] moreColumns2 = { "ZGFS" };
                            int[] moreColumnTypes2 = { CosDataBase.FLOAT_TYPE };
                            KSTZDM_list = man.InportSimpleTableOriginData<Td_kstzdm, Td_kstzdm_Origin>(dt, nNowYear, lProvinceID, "kstzdmTable", mainColumns, moreColumns2, moreColumnTypes2);
                            break;
                        case "td_kszgdm.dbf": //考生资格代码
                            mainColumns[0] = "KSZGDM"; mainColumns[1] = "KSZGMC";
                            KSZGDM_list = man.InportSimpleTableOriginData<Td_kszgdm, Td_kszgdm_Origin>(dt, nNowYear, lProvinceID, "kszgdmTable", mainColumns);
                            break;
                        case "td_lqlxdm.dbf"://录取类型代码（12）
                            mainColumns[0] = "LQLXDM"; mainColumns[1] = "LQLXMC";
                            LQFSDM_list = man.InportSimpleTableOriginData<Td_lqfsdm, Td_lqfsdm_Origin>(dt, nNowYear, lProvinceID, "lqfsdmTable", mainColumns);
                            break;
                        case "td_lqfsdm.dbf": //录取方式代码（17）
                            mainColumns[0] = "LQFSDM"; mainColumns[1] = "LQFSMC";
                            LQFSDM_list = man.InportSimpleTableOriginData<Td_lqfsdm, Td_lqfsdm_Origin>(dt, nNowYear, lProvinceID, "lqfsdmTable", mainColumns);
                            break;

                        case "td_mzdm.dbf": //民族代码
                            mainColumns[0] = "MZDM"; mainColumns[1] = "MZMC";
                            MZDM_list = man.InportSimpleTableOriginData<Td_mzdm, Td_mzdm_Origin>(dt, nNowYear, lProvinceID, "mzdmTable", mainColumns);
                            break;
                        case "td_pcdm.dbf": //批次代码
                            mainColumns[0] = "PCDM"; mainColumns[1] = "PCMC";
                            PCDM_list = man.InportSimpleTableOriginData<Td_pcdm, Td_pcdm_Origin>(dt, nNowYear, lProvinceID, "pcdmTable", mainColumns);
                            break;
                        case "td_tdlxdm.dbf": //投档类型代码
                            mainColumns[0] = "TDLXDM"; mainColumns[1] = "TDLXMC";
                            TDLXDM_list = man.InportSimpleTableOriginData<Td_tdlxdm, Td_tdlxdm_Origin>(dt, nNowYear, lProvinceID, "tdlxdmTable", mainColumns);
                            break;
                        case "td_tdyydm.dbf": //退档原因代码
                            mainColumns[0] = "TDYYDM"; mainColumns[1] = "TDYYMC";
                            TDYYDM_list = man.InportSimpleTableOriginData<Td_tdyydm, Td_tdyydm_Origin>(dt, nNowYear, lProvinceID, "tdyydmTable", mainColumns);
                            break;
                        case "td_tjjldm.dbf": //体检结论代码
                            mainColumns[0] = "TJJLDM"; mainColumns[1] = "TJJLMC";
                            TJJLDM_list = man.InportSimpleTableOriginData<Td_tjjldm, Td_tjjldm_Origin>(dt, nNowYear, lProvinceID, "tjjldmTable", mainColumns);
                            break;

                        case "td_wyyzdm.dbf": //外语语种代码
                            mainColumns[0] = "WYYZDM"; mainColumns[1] = "WYYZMC";
                            WYYZDM_list = man.InportSimpleTableOriginData<Td_wyyzdm, Td_wyyzdm_Origin>(dt, nNowYear, lProvinceID, "wyyzdmTable", mainColumns);
                            break;
                        case "td_xtdwdm.dbf": //系统单位代码
                            mainColumns[0] = "XTDWDM"; mainColumns[1] = "XTDWMC";
                            XTDWDM_list = man.InportSimpleTableOriginData<Td_xtdwdm, Td_xtdwdm_Origin>(dt, nNowYear, lProvinceID, "xtdwdmTable", mainColumns);
                            break;

                        case "td_xzdm.dbf": //学制代码
                            mainColumns[0] = "XZDM"; mainColumns[1] = "XZMC";
                            XZDM_list = man.InportSimpleTableOriginData<Td_xzdm, Td_xzdm_Origin>(dt, nNowYear, lProvinceID, "xzdmTable", mainColumns);
                            break;
                        case "td_zjlxdm.dbf": //证件类型代码
                            mainColumns[0] = "ZJLXDM"; mainColumns[1] = "ZJLXMC";
                            ZJLXDM_list = man.InportSimpleTableOriginData<Td_zjlxdm, Td_zjlxdm_Origin>(dt, nNowYear, lProvinceID, "zjlxdmTable", mainColumns);
                            break;

                        case "td_zklxdm.dbf": //招考类型代码
                            mainColumns[0] = "ZKLXDM"; mainColumns[1] = "ZKLXMC";
                            ZKLXDM_list = man.InportSimpleTableOriginData<Td_zklxdm, Td_zklxdm_Origin>(dt, nNowYear, lProvinceID, "zklxdmTable", mainColumns);
                            break;
                        case "td_zylbdm.dbf": //专业类别代码 这个表很重要，但是12没有，要从计划库中提取                            
                            ZYLBDM_list = (new zylbdmTableManage()).InportOriginData(dt, nNowYear, lProvinceID);
                            break;
                        case "td_zytjlxdm.dbf": //专业调剂类型代码
                            mainColumns[0] = "ZYTJLXDM"; mainColumns[1] = "ZYTJLXMC";
                            ZYTJLXDM_list = man.InportSimpleTableOriginData<Td_zytjlxdm, Td_zytjlxdm_Origin>(dt, nNowYear, lProvinceID, "zytjlxdmTable", mainColumns);
                            break;
                        case "td_zytzdm.dbf": //志愿特征代码
                            mainColumns[0] = "ZYTZDM"; mainColumns[1] = "ZYTZMC";
                            ZYTZDM_list = man.InportSimpleTableOriginData<Td_zytzdm, Td_zytzdm_Origin>(dt, nNowYear, lProvinceID, "zytzdmTable", mainColumns);
                            break;
                        case "td_zzmmdm.dbf"://政治面貌代码
                            mainColumns[0] = "ZZMMDM"; mainColumns[1] = "ZZMMMC";
                            ZZMMDM_list = man.InportSimpleTableOriginData<Td_zzmmdm, Td_zzmmdm_Origin>(dt, nNowYear, lProvinceID, "zzmmdmTable", mainColumns);
                            break;
                        case "td_xbdm.dbf"://性别代码
                            mainColumns[0] = "XBDM"; mainColumns[1] = "XBMC";
                            XBDM_list = man.InportSimpleTableOriginData<Td_xbdm, Td_xbdm_Origin>(dt, nNowYear, lProvinceID, "xbdmTable", mainColumns);
                            break;
                        case "td_byxxdm.dbf": BYXXDM_list = man.InportSimpleTableOriginData<Td_byxxdm, Td_byxxdm_Origin>(dt, nNowYear, lProvinceID, "byxxdmTable"); break;     //毕业学校代码，这个表原始库中可能没有

                        case "t_tddw.dbf":
                            tddwTableManage tddwMan = new tddwTableManage();
                            tddwMan.PCDM_list = PCDM_list;
                            tddwMan.KLDM_list = KLDM_list;
                            tddwMan.JHXZDM_list = JHXZDM_list;
                            tddwMan.JHLBDM_list = JHLBDM_list;
                            TDDW_list = tddwMan.InportOriginData(dt, nNowYear, lProvinceID, model.GxID);
                            break;
                        case "td_zydh.dbf": //2017有，2012没有。12的数据来自计划库
                            zydhTableManage zydhMan = new zydhTableManage();
                            zydhMan.CCDM_list = CCDM_list;
                            zydhMan.KLDM_list = KLDM_list;
                            zydhMan.XZDM_list = XZDM_list;
                            zydhMan.ZYLBDM_list = ZYLBDM_list;
                            zydhMan.TDDW_list = TDDW_list;
                            ZYDH_list = zydhMan.InportOriginData(dt, nNowYear, lProvinceID, model.ParentDepartmentID, model.GxID);
                            break;
                        case "t_jhk.dbf":
                            jhkTableManage jhkMan = new jhkTableManage();
                            jhkMan.PCDM_list = PCDM_list;
                            jhkMan.KLDM_list = KLDM_list;
                            jhkMan.JHXZDM_list = JHXZDM_list;
                            jhkMan.JHLBDM_list = JHLBDM_list;
                            jhkMan.TDDW_list = TDDW_list;
                            jhkMan.ZYDH_list = ZYDH_list;
                            jhkMan.ZKLXDM_list = ZKLXDM_list;
                            jhkMan.ZYLBDM_list = ZYLBDM_list;
                            jhkMan.XZDM_list = XZDM_list;
                            List<ListCompareTable> old12ZYDH_list = new List<ListCompareTable>();
                            JHK_list = jhkMan.InportOriginData(dt, nNowYear, lProvinceID, model.ParentDepartmentID, model.GxID, out old12ZYDH_list);
                            if ((old12ZYDH_list != null) && (old12ZYDH_list.Count > 0))
                                ZYDH_list = old12ZYDH_list;
                            break;
                        case "t_bmk.dbf":
                            bmkTableManage bmkMan = new bmkTableManage();

                            bmkMan.BYLBDM_list = BYLBDM_list;
                            bmkMan.CJXDM_list = CJXDM_list;
                            bmkMan.CZLBDM_list = CZLBDM_list;
                            bmkMan.DQDM_list = DQDM_list;
                            bmkMan.HJLBDM_list = HJLBDM_list;

                            bmkMan.KLDM_list = KLDM_list;
                            bmkMan.KMDM_list = KMDM_list;
                            bmkMan.KSLBDM_list = KSLBDM_list;
                            bmkMan.KSLXDM_list = KSLXDM_list;
                            bmkMan.KSTZDM_list = KSTZDM_list;

                            bmkMan.KSZGDM_list = KSZGDM_list;
                            bmkMan.MZDM_list = MZDM_list;
                            bmkMan.WYYZDM_list = WYYZDM_list;
                            bmkMan.ZJLXDM_list = ZJLXDM_list;
                            bmkMan.XTDWDM_list = XTDWDM_list;

                            bmkMan.ZZMMDM_list = ZZMMDM_list;
                            bmkMan.BYXXDM_list = BYXXDM_list;
                            bmkMan.XBDM_list = XBDM_list;

                            BMK_list = bmkMan.InportOriginData(dt, con, sNowPath, nNowYear, lProvinceID, model.GxID);
                            break;
                        case "t_tdd.dbf":
                            tddTableManage tddMan = new tddTableManage();

                            tddMan.BYLBDM_list = BYLBDM_list;
                            tddMan.CCDM_list = CCDM_list;
                            tddMan.CJXDM_list = CJXDM_list;
                            tddMan.CZLBDM_list = CZLBDM_list;
                            tddMan.DQDM_list = DQDM_list;

                            tddMan.HJLBDM_list = HJLBDM_list;
                            tddMan.JHLBDM_list = JHLBDM_list;
                            tddMan.JHXZDM_list = JHXZDM_list;
                            tddMan.KLDM_list = KLDM_list;
                            tddMan.KMDM_list = KMDM_list;

                            tddMan.KSLBDM_list = KSLBDM_list;
                            tddMan.KSLXDM_list = KSLXDM_list;
                            tddMan.KSTZDM_list = KSTZDM_list;
                            tddMan.KSZGDM_list = KSZGDM_list;
                            tddMan.LQFSDM_list = LQFSDM_list;

                            tddMan.MZDM_list = MZDM_list;
                            tddMan.PCDM_list = PCDM_list;
                            tddMan.TDLXDM_list = TDLXDM_list;
                            tddMan.TDYYDM_list = TDYYDM_list;
                            tddMan.TJJLDM_list = TJJLDM_list;

                            tddMan.WYYZDM_list = WYYZDM_list;
                            tddMan.XTDWDM_list = XTDWDM_list;
                            //tddMan.ZCDM_list = ZCDM_list;
                            tddMan.XZDM_list = XZDM_list;
                            tddMan.ZJLXDM_list = ZJLXDM_list;

                            tddMan.ZKLXDM_list = ZKLXDM_list;
                            tddMan.ZYLBDM_list = ZYLBDM_list;
                            tddMan.ZYTJLXDM_list = ZYTJLXDM_list;
                            tddMan.ZYTZDM_list = ZYTZDM_list;
                            tddMan.ZZMMDM_list = ZZMMDM_list;

                            tddMan.BYXXDM_list = BYXXDM_list;
                            tddMan.XBDM_list = XBDM_list;
                            tddMan.ZYDH_list = ZYDH_list;
                            tddMan.TDDW_list = TDDW_list;

                            tddMan.BMK_list = BMK_list;
                            tddMan.JHK_list = JHK_list;

                            BMK_list = tddMan.InportOriginData(dt, con, sNowPath, nNowYear, lProvinceID, model.GxID);
                            break;
                        case "t_zykmx.dbf":
                            zykTableManage zykMan = new zykTableManage();
                            zykMan.PCDM_list = PCDM_list;
                            zykMan.KLDM_list = KLDM_list;
                            zykMan.JHXZDM_list = JHXZDM_list;
                            zykMan.JHLBDM_list = JHLBDM_list;
                            zykMan.TDDW_list = TDDW_list;
                            zykMan.ZYDH_list = ZYDH_list;
                            zykMan.BMK_list = BMK_list;
                            zykMan.JHK_list = JHK_list;
                            zykMan.InportOriginData(dt, nNowYear, lProvinceID);
                            break;
                        case "t_ksjl.dbf":
                            ksjlTableManage ksjlMan = new ksjlTableManage();
                            ksjlMan.BMK_list = BMK_list;
                            ksjlMan.InportOriginData(dt, nNowYear, lProvinceID);
                            break;
                        case "t_kshkcj.dbf":
                        case "t_ksxkcj.dbf":
                            ksxkcjTableManage ksxkcjMan = new ksxkcjTableManage();
                            ksxkcjMan.KMDM_list = KMDM_list;
                            ksxkcjMan.CJXDM_list = CJXDM_list;
                            ksxkcjMan.BMK_list = BMK_list;
                            ksxkcjMan.InportOriginData(dt, nNowYear, lProvinceID);
                            break;
                        case "t_tjxx.dbf":
                            tjxxTableManage tjxxMan = new tjxxTableManage();
                            tjxxMan.BMK_list = BMK_list;
                            tjxxMan.InportOriginData(dt, nNowYear, lProvinceID);
                            break;
                    }
                    //sw.Stop();
                    //TimeSpan ts2 = sw.Elapsed;
                }
            }
            con.Close();
        }

        private void InportWhole(tblOriginDataPackageInfo model)
        {
            //首先要解压
            ZipFileTask ziptask = new ZipFileTask();
            if (model.FileType == "数据")
            {
                ziptask.ExtractZipFile(ConstAddressPath.ORIGINZIP_WEBPATH + model.NowAddress, ConstAddressPath.TEMP_WEBPATH + model.NowAddress);
                //解压完毕后，对每个dbf文件名进行处理（文件名长度不能超过8）
                string sRootPath = ConstAddressPath.TEMP_WEBPATH + model.NowAddress;
                RenameAllDBFFiles(sRootPath);

                OriginTableManage manOrigin = new OriginTableManage();
                string[] sAllYearPath = Directory.GetFileSystemEntries(sRootPath);
                ArrayList arrAllThreads = new ArrayList();
                foreach (string sYearPath in sAllYearPath)
                {
                    //第一层目录应该是年份
                    //int nNowYear = Convert.ToInt32(sYearPath.Substring(sYearPath.LastIndexOf('\\') + 1));
                    string sNowYear = sYearPath.Substring(sYearPath.LastIndexOf('\\') + 1);
                    int nNowYear = Convert.ToInt32(sNowYear.Substring(sNowYear.IndexOf("20"), 4));
                    //int nNowYear = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(sNowYear, @"[^0-9]+",""));


                    //第二层目录应该是省份名称
                    string[] sAllNowPath = Directory.GetFileSystemEntries(sYearPath);
                    foreach (string sNowPath in sAllNowPath)
                    {
                        string sProvinceName = sNowPath.Substring(sNowPath.LastIndexOf('\\') + 1);
                        dqdmTableManage dqdmMan = new dqdmTableManage();
                        long lProvinceID = dqdmMan.GetCurrentRegionbyName(sProvinceName);
                        //InportOneWholePath(sNowPath, nNowYear, lProvinceID, model);
                        oneThreadModel one = new oneThreadModel();
                        one.nNowYear = nNowYear;
                        one.sNowPath = sNowPath;
                        one.lProvinceID = lProvinceID;
                        one.model = model;                        
                        AllTableStatus.InitAllTableStatus();

                        //InportOneWholeThread(one); //非线程处理
                        Thread th = new Thread(InportOneWholeThread);  //新线程
                        th.Start(one);
                        arrAllThreads.Add(th);
                    }
                }
                bool flag = false;
                while (!flag)
                {
                    flag = true;
                    int i = 0;
                    while ((i < arrAllThreads.Count) && (flag))
                    {
                        Thread s = (Thread)arrAllThreads[i];
                        if (s.ThreadState == System.Threading.ThreadState.Running)
                            flag = false;
                        i++;
                    }
                    if (!flag)
                        Thread.Sleep(60000);
                }
                FileManager file = new FileManager();
                file.DeleteFolder(sRootPath);
            }
            else if (model.FileType == "照片")
            {
                ziptask.ExtractZipFile(ConstAddressPath.ORIGINZIP_WEBPATH + model.NowAddress, ConstAddressPath.ORIGINSTUPIC_WEBPATH + model.GxID + "\\temp");
                string sRootPath = ConstAddressPath.ORIGINSTUPIC_WEBPATH + model.GxID + "\\temp";

                OriginTableManage manOrigin = new OriginTableManage();
                string[] sAllYearPath = Directory.GetFileSystemEntries(sRootPath);
                ArrayList arrAllThreads = new ArrayList();
                foreach (string sYearPath in sAllYearPath)
                {
                    //第一层目录应该是年份,并获得其年份值
                    //int nNowYear = Convert.ToInt32(sYearPath.Substring(sYearPath.LastIndexOf('\\') + 1));
                    string sNowYear = sYearPath.Substring(sYearPath.LastIndexOf('\\') + 1);
                    int nNowYear = -1;
                    try
                    {
                        nNowYear = Convert.ToInt32(sNowYear.Substring(sNowYear.IndexOf("20"), 4));
                    }
                    catch
                    {
                        nNowYear = DateTime.Now.Year;
                    }
                    //接下来需要递归获得各层目录下的照片文件目录
                    //将子目录下的文件移动到根目录下
                    FileManager fm = new FileManager();
                    List<string> filespath = fm.GetAllFiles(sYearPath);
                    foreach (string filepath in filespath)
                    {
                        string sFileName = filepath.Substring(filepath.LastIndexOf('\\') + 1);
                        string sDestinationName = Path.Combine(ConstAddressPath.ORIGINSTUPIC_WEBPATH + model.GxID, nNowYear.ToString(), sFileName);
                        fm.MoveFile(filepath, sDestinationName);
                    }
                }
                FileManager file = new FileManager();
                file.DeleteFolder(sRootPath);
            }
            model.HandleFlag = "处理完成";
            model.FinishDate = DateTime.Now;
            string[] EditColumn = { "HandleFlag" , "FinishDate" };
            SaveOneRecord(model, EditColumn);
        }

        private void InportOneWholeThread(object mo)
        {
            InportOneWholePath(((oneThreadModel)mo).sNowPath, ((oneThreadModel)mo).nNowYear, ((oneThreadModel)mo).lProvinceID, ((oneThreadModel)mo).model);
            FileManager file = new FileManager();
            file.DeleteFolder(((oneThreadModel)mo).sNowPath);
        }


        private void InportThread(object model)
        {
            InportWhole((tblOriginDataPackageInfo)model);
        }


        public string InportData(string sID)
        {
            string returnMsg = "正在处理...";
            tblOriginDataPackageInfo model = GetOneRecordbyID<tblOriginDataPackageInfo, string>(sID);
            if (model.HandleFlag == "处理完成")
                returnMsg = "选择的文件已经处理！";
            else
            {
                string[] key = { CookieKeys.USERGXID };
                long lGxID = Convert.ToInt64((new UserInfoManage()).GetLoginUserInfo(key)[0]);                
                Thread th = new Thread(InportThread);  //新线程
                th.Start(model);
            }
            return returnMsg;
        }

        #endregion
    }
}