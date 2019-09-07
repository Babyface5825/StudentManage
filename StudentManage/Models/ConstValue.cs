using System;
using System.Web;
using System.Web.Configuration;

namespace StudentManage.Models
{
    public class AppSettingValues
    {
        public static string DB_CONN_STRING = "name=ConnStr";
        public static string MENUTYPE = WebConfigurationManager.AppSettings["MenuType"];
        public static string TABTYPE = WebConfigurationManager.AppSettings["TabType"];
        public static int DATALISTPAGELIMIT = Convert.ToInt32(WebConfigurationManager.AppSettings["DataListPageLimit"]);
        public static int FLOATDECIMAL = Convert.ToInt16(WebConfigurationManager.AppSettings["FloatDecimal"]);

    }
    
    public class CosValue
    {
        public const char SPLITOPERATOR1 = '|';
        public const char SPLITOPERATOR2 = ',';
        public const char SPLITOPERATOR3 = '.';
    }

    public class SIGN
    {
        public const int Equal = 1;
        public const int NotEqual = 2;
        public const int GreaterThanOrEqual = 3;
        public const int LessThanOrEqual = 4;
        public const int GreaterThan = 5;
        public const int LessThan = 6;
        public const int Like = 7;
        public const int NotLike = 8;
        public const int In = 9;
        public const int NotIn = 10;
        public const int Contain1 = 11; //中间用SPLITOPERATOR1分割
        public const int Contain2 = 12;//中间用SPLITOPERATOR2分割
    }


    /// <summary>
    /// DataList顶端按钮
    /// </summary>
    public class DataListButton
    {
        public const int btnAdd = 1;
        public const int btnEdit = 2;
        public const int btnDelete = 3;
        public const int btnReset = 4;
        public const int btnSearch = 5;
        public const int btnAll = 6;
        public const int btnOrderUp = 7;
        public const int btnOrderDown = 8;
        public const int btnReverse = 9;
        public const int btnVerify = 10;
        public const int btnInport = 11;
        public const int btnExport = 12;
        public const int btnMore1 = 13;
        public const int btnVerifyAll = 14;
        public const int btnMoreSearch = 15;
        public const int btnPrintBrand = 16;
        public const int btnInportPic = 17;
        public const int btnRegularExport = 18;
        public const int btnMore2 = 19;

    }

    /// <summary>
    /// 当前操作类型
    /// </summary>
    public class OperationType
    {
        public const int DELETE = 0;
        public const int ADD = 1;
        public const int EDIT = 2;
        public const int SEARCH = 3;
        public const int ALL = 4;
    }


    public class CosDataBase
    {
        //以下为字段类型,注意如果这里修改，一定要跟着修改DBObject中相关的新增修改删除类型
        /// <summary>
        /// 字符串类型
        /// </summary>
        public const int STRING_TYPE = 1;
        /// <summary>
        /// 长字符串类型
        /// </summary>
        public const int STRINGL_TYPE = 2;
        /// <summary>
        /// 浮点型
        /// </summary>
        public const int FLOAT_TYPE = 3;
        /// <summary>
        /// 布尔类型
        /// </summary>
        public const int BOOLEAN_TYPE = 4;
        public const int BOOLEANBOX_TYPE = 11;        
        /// <summary>
        /// 时间类型
        /// </summary>
        public const int DATE_TYPE = 5;
        public const int TIME_TYPE = 8;
        public const int DATETIME_TYPE = 9;
        /// <summary>
        /// 多选一，Combox选择方式实现
        /// </summary>
        public const int SELSTR_TYPE = 6;        
        /// <summary>
        /// 图片类型
        /// </summary>
        public const int PIC_TYPE = 7;
        /// <summary>
        /// 整型
        /// </summary>
        public const int INT_TYPE = 10;
        /// <summary>
        /// 短整型
        /// </summary>
        public const int SMALLINT_TYPE = 12;
        /// <summary>
        /// 长整型
        /// </summary>
        public const int LONGINT_TYPE = 13;
        /// <summary>
        /// 拼接型，一张表里几个字段名称的拼接
        /// </summary>
        public const int JOIN_TYPE = 14;
        /// <summary>
        /// 父Name型
        /// </summary>
        public const int PARENTNAME_TYPE = 15;

        public static implicit operator CosDataBase(AppSettingValues v)
        {
            throw new NotImplementedException();
        }
    }

    public class ConstAddressPath
    {
        public static string VIMG_PATH = "/ValidateImg/";
        public static string VIMG_WEBPATH = HttpContext.Current.Server.MapPath("~/ValidateImg/");
        public static string DOC_PATH = "/doc/";
        public static string DOC_WEBPATH = HttpContext.Current.Server.MapPath("~/doc/");
        public static string PIC_PATH = "/images/";
        public static string PIC_WEBPATH = HttpContext.Current.Server.MapPath("~/images/");
        public static string ORIGINZIP_PATH = "/originzipfile/";
        public static string ORIGINZIP_WEBPATH = HttpContext.Current.Server.MapPath("~/originzipfile/");
        public static string ORIGINSTUPIC_PATH = "/originStuPic/";
        public static string ORIGINSTUPIC_WEBPATH = HttpContext.Current.Server.MapPath("~/originStuPic/");
        public static string TEMP_PATH = "/temp/";
        public static string TEMP_WEBPATH = HttpContext.Current.Server.MapPath("~/temp/");
        public static string ICON_PATH = "/icons/";
        public static string ICON_WEBPATH = HttpContext.Current.Server.MapPath("~/icons/");
        public static string REPORT_PATH = "/report/";
        public static string REPORT_WEBPATH = HttpContext.Current.Server.MapPath("~/report/");
    }

    public class CookieKeys
    {
        public const string ID = "ID";
        public const string USERNAME = "UserName";
        public const string REGISTERNAME = "RegisterName";
        public const string USERDUTY = "UserDuty";
        public const string DEPARTMENTID = "DepartmentID";
        public const string PARENTDEPARTMENTID = "ParentDepartmentID";
        public const string USERGXID = "UserGxID";
        public const string USERROLES = "UserRoles";
        public const string USERROLENAMES = "UserRoleNames";
    }


    public class UserRoleName
    {
        public const string SUPER = "超级管理员";
    }


}