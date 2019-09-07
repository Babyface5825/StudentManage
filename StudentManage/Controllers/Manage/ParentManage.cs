using System;
using StudentManage.Models;
using System.Data.Entity;
using System.Linq;
using LinqKit;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Collections;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Data.OleDb;
using StudentManage.Controllers;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Text;


namespace StudentManage.Manage
{


    #region DBContext
    public static class DBContext
    {
        

        public static void Detach<T>(this DbContext db, T obj) where T : class
        {
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            oc.Detach(obj);
        }

        public static IEnumerable<string> GetEntityKeys<T>(this DbContext db) where T : class
        {
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            var keys = oc.CreateObjectSet<T>().EntitySet.ElementType.KeyProperties.Select(x => x.Name);
            return keys;
        }

        private static Expression<Func<T, bool>> GetFindExp<T>(T obj, IEnumerable<string> keys) where T : class
        {
            var p = Expression.Parameter(typeof(T), "x");

            var keyexps = keys.Select(x =>
            {
                var member = Expression.PropertyOrField(p, x);
                var objV = typeof(T).GetProperty(x).GetValue(obj);
                var eq = Expression.Equal(member, Expression.Constant(objV));
                return eq;
            }).ToList();

            if (keys.Count() == 1)
            {
                return Expression.Lambda<Func<T, bool>>(keyexps[0], new[] { p });
            }

            var and = Expression.AndAlso(keyexps[0], keyexps[1]);
            for (var i = 2; i < keyexps.Count; i++)
            {
                and = Expression.AndAlso(and, keyexps[i]);
            }
            return Expression.Lambda<Func<T, bool>>(and, new[] { p });
        }

        public static T FindLocal<T>(this DbContext db, T obj) where T : class
        {
            var keys = db.GetEntityKeys<T>();
            var func = GetFindExp<T>(obj, keys).Compile();
            return db.Set<T>().Local.FirstOrDefault(func);
        }


        public static void DetachOther<T>(this DbContext db, T obj) where T : class
        {
            var local = db.FindLocal(obj);
            if (local != null)
                db.Detach(local);
        }

        public static BaseContext GetDataBaseContext()
        {
            // 首先先线程上下文中查看是否有已存在的DBContext  
            // 如果有那么直接返回这个，如果没有就新建   
            BaseContext DB = CallContext.GetData("BaseContext") as BaseContext;
            if (DB == null)
            {
                DB = new BaseContext();               
                CallContext.SetData("BaseContext", DB);
            }            
            return DB;
        }
               


    }
#endregion


    /// <summary>
    ///  所有manage的根。主要是拼接和处理各种lambda表达式，进而操作数据库
    /// </summary>
    public class ParentManage
    {
        public static object locker = new object();
        public string sApplicationPath = "";
        public string Manage_KeyWord;

        public ParentManage()
        {
            
        }

        #region GetNowNameFromID
        private string _GetNowNameFromID<T>(T obj, long ID) where T : class
        {
            try
            {
                T model = GetOneRecordbyID<T, long>(ID);
                if (model != null)
                    return model.GetType().GetProperty("Name").GetValue(model, null).ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 根据当前的ID获取Name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetNowNamefromID(long ID, string sKey = null)
        {
            Type type = GetTypeObj(sKey).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetNowNameFromID(obj, ID);
        }


        #endregion


        #region 一些反复调用的小函数

        /// <summary>
        /// 创建一个指定长度的ArrayList，里面元素都是同样的
        /// </summary>
        /// <param name="obj">相同的元素</param>
        /// <param name="count">数组长度</param>
        /// <returns>创建好的ArrayList</returns>
        public ArrayList NewDuplicateArray(object obj, int count)
        {
            ArrayList arr = new ArrayList();
            for (int i = 0; i < count; i++)
                arr.Add(obj);
            return arr;
        }
        private dynamic _GetSpecialModelFromCurrentID<T>(T obj, long id) where T : class, new()
        {
            var model = GetOneRecordbyID<T, long>(id);
            return _GetSpecialModel(obj, model);
        }
        private static dynamic _GetSpecialModel<T>(T obj, dynamic model) where T : class, new()
        {
            T returnModel = new T();
            if (model != null)
            {
                Type type = typeof(T);
                MemberInfo[] mifos = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
                foreach (MemberInfo m in mifos)
                {
                    if ((m.MemberType == MemberTypes.Property) && !(m.ToString().Contains("List`1")) && !(m.ToString().Contains("Models")))
                    {
                        var ob = model.GetType().GetProperty(m.Name);
                        if (ob != null)
                            returnModel.GetType().GetProperty(m.Name).SetValue(returnModel, ob.GetValue(model, null));
                        else
                            returnModel.GetType().GetProperty(m.Name).SetValue(returnModel, null);
                    }
                }
            }
            return returnModel;
        }
        /// <summary>
        /// 从dbf文件中获得一个DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultDir"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        protected DataTable GetDbfDataByODBC(string tableName, string defaultDir, OleDbConnection con)
        {
            try
            {
                string fileName = defaultDir + "\\" + tableName;
                FileManager fileMan = new FileManager();
                if (fileMan.HasFile(fileName))
                {
                    var sql = "select * from " + fileName;
                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dtData = new DataTable();
                    da.Fill(dtData);
                    return dtData;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据传过来的文件名获取长度不超过8的实际文件名，用于dbf
        /// </summary>
        /// <returns></returns>
        protected string GetPropertyFileName(string sOldFileName)
        {
            string sNewFileName = sOldFileName.Substring(0, sOldFileName.IndexOf('.'));
            if (sNewFileName.Length > 8)
                sNewFileName = sNewFileName.Remove(0, sNewFileName.Length - 8) + ".dbf";
            else
                sNewFileName = sOldFileName;
            return sNewFileName;
        }

        protected string GetSubString(string s, int nStart, int nLength)
        {
            try
            {
                if (s.Length >= nStart + nLength)
                {
                    if (nLength != 0)
                        return s.Substring(nStart, nLength);
                    else
                        return s.Substring(nStart);
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region 用与报表生成选取窗口两个Store相关的提取函数
        /// <summary>
        /// 获取当前已选框中已经存在的所有相关选项的ID
        /// </summary>
        /// <param name="SelectedStore"></param>
        /// <param name="sTypeID"></param>
        /// <returns></returns>
        protected string GetUsedIDs(string SelectedStore, string sTypeID)
        {
            string[] arrAllStore = SelectedStore.Split(CosValue.SPLITOPERATOR2);
            string sUsedIDs = "0";
            if (arrAllStore.Length > 0)
            {
                foreach (string store in arrAllStore)
                {
                    string[] ss = store.Split(CosValue.SPLITOPERATOR1);
                    if (ss[0] == sTypeID)
                        sUsedIDs += CosValue.SPLITOPERATOR2 + ss[1];
                }
            }
            return sUsedIDs;
        }
        /// <summary>
        /// 获取返回给可选框/已选框中的store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sTypeID"></param>
        /// <param name="where"></param>
        /// <param name="PreName"></param>
        /// <returns></returns>
        protected object[] GetReturnData<T>(string sTypeID, Expression<Func<T, bool>> where, string PreName) where T : class
        {
            List<T> list = GetRecord(where);
            if (list != null)
            {
                int countNum = list.Count;
                if (countNum > 0)
                {
                    object[] data = new object[countNum];
                    int j = 0;
                    foreach (T m in list)
                    {
                        object[] dataline = new object[3];
                        dataline[0] = sTypeID;
                        dataline[1] = m.GetType().GetProperty("ID").GetValue(m, null);
                        dataline[2] = PreName + m.GetType().GetProperty("Name").GetValue(m, null);
                        data[j] = dataline;
                        j++;
                    }
                    return data;
                }
            }
            return null;
        }

        #endregion
        
        #region 供子类调用的虚类
        protected object GetTypeObj(string keyword = null)
        {
            if (keyword != null)
                Manage_KeyWord = keyword;

            switch (Manage_KeyWord)
            {
                case "AuthorizationInfo": return new tblAuthorizationInfo();
                case "OperationInfo": return new tblOperationInfo();
                case "DepartmentInfo": return new tblDepartmentInfo();
                case "DutyInfo": return new tblDutyInfo();
                case "UserInfo": return new tblUserInfo();
                case "ReportTypeSel":
                case "ReportInfo":return new tblReportInfo();
                case "ReportType": return new tblReportType();
                //case "AuthorizationChannelType": return new tblChannelType();
                case "OriginDataInport": return new tblOriginDataPackageInfo();

                case "bylbdmTable": return new Td_bylbdm();
                case "ccdmTable": return new Td_ccdm();
                case "cjxdmTable": return new Td_cjxdm();
                case "czlbdmTable": return new Td_czlbdm();
                case "dqdmTable": return new Td_dqdm();

                case "hjlbdmTable": return new Td_hjlbdm();
                case "jhlbdmTable": return new Td_jhlbdm();
                case "jhxzdmTable": return new Td_jhxzdm();
                case "kldmTable": return new Td_kldm();
                case "kmdmTable": return new Td_kmdm();

                case "kslbdmTable": return new Td_kslbdm();
                case "kslxdmTable": return new Td_kslxdm();
                case "kstzdmTable": return new Td_kstzdm();
                case "kszgdmTable": return new Td_kszgdm();
                case "lqfsdmTable": return new Td_lqfsdm();

                case "mzdmTable": return new Td_mzdm();
                case "pcdmTable": return new Td_pcdm();
                case "tdlxdmTable": return new Td_tdlxdm();
                case "tdyydmTable": return new Td_tdyydm();
                case "tjjldmTable": return new Td_tjjldm();

                case "wyyzdmTable": return new Td_wyyzdm();
                case "xtdwdmTable": return new Td_xtdwdm();
                //case "zcdmTable": return new Td_zcdm();
                case "xzdmTable": return new Td_xzdm();
                case "zjlxdmTable": return new Td_zjlxdm();

                case "zklxdmTable": return new Td_zklxdm();
                case "zylbdmTable": return new Td_zylbdm();
                case "zytjlxdmTable": return new Td_zytjlxdm();
                case "zytzdmTable": return new Td_zytzdm();
                case "zzmmdmTable": return new Td_zzmmdm();

                case "byxxdmTable": return new Td_byxxdm();
                case "xbdmTable": return new Td_xbdm();

                case "gxdmTable": return new Td_gxdm();
                case "zydhTable": return new Td_zydh();
                case "tddwTable": return new T_tddw();                
                case "jhkTable": return new T_jhk();
                    
                case "bmkTable": return new T_bmk();
                case "tddTable": return new T_tdd();
                case "zykTable": return new T_zyk();
                case "ksjlTable": return new T_ksjl();
                case "ksxkcjTable": return new T_ksxkcj();

                case "tjxxTable": return new T_tjxx();
                

                default: return null;
            }
        }


        /// <summary>
        /// 根据当前ID，从数据库中获取并返回当前的具体model，用于form中（即去除ForeignKey关联项）
        /// </summary>        
        /// <returns></returns>
        public virtual dynamic GetSpecialModelFromCurrentID<T>(T ID)
        {
            try
            {
                long id = Convert.ToInt64(ID);
                Type type = GetTypeObj().GetType();
                dynamic obj = type.Assembly.CreateInstance(type.ToString());
                return _GetSpecialModelFromCurrentID(obj, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }


        /// <summary>
        /// 据当前model，返回当前的具体model，必须在具体manage里面重写。这个model是用于form的model
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public virtual dynamic GetSpecialModelFromCurrentModel(object mo)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetSpecialModel(obj, mo);
        }




        #endregion

        #region BuildExpression

        public Expression<Func<T, bool>> BuildOneEqualExpression<T>(string name, object value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            var left = Expression.Property(param, name);
            var right = Expression.Constant(value);
            var equal = Expression.Equal(left, right);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equal, param);
            return lambda;
        }

        /// <summary>
        /// 构造排序字段，用于Lambda表达式，通常是TheOrder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public Expression<Func<T, T1>> BuildOrder<T, T1>(string name) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var orderByExp = Expression.Lambda(GetMemberExpression(parameter, name), parameter);
            return (Expression<Func<T, T1>>)orderByExp;
        }
        
        /// <summary>
        /// 获得当前键值的类型，由于可能是userinfo.name这样的形式，需要判断最后一个
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">键值</param>
        /// <returns>类型</returns>
        protected PropertyInfo GetPropertyInfo<T>(string name)
        {
            if (name.Length > 0)
            {
                string[] na = name.Split('.');
                var left = typeof(T).GetProperty(na[0]);
                int i = 1;
                while (i < na.Length)
                {
                    left = Type.GetType(left.PropertyType.FullName).GetProperty(na[i]);
                    i++;
                }
                return left;
            }
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression_param"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private MemberExpression GetMemberExpression(ParameterExpression expression_param, string name)
        {
            if (name.Length > 0)
            {
                string[] na = name.Split('.');
                var propertyAccess = Expression.MakeMemberAccess(expression_param, expression_param.Type.GetMember(na[0]).First());
                for (int i = 1; i < na.Length; i++)
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, propertyAccess.Type.GetMember(na[i]).First());
                return propertyAccess;
            }
            else
                return null;
        }

        /// <summary>
        /// 根据多重条件获得一个lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="names">筛选名称</param>
        /// <param name="values">筛选值</param>
        /// <param name="signs">筛选符号</param>
        /// <param name="andor">与或关系</param>
        /// <returns></returns>
        private Expression<Func<T, bool>> GetAndOrLambdaExpression<T>(ArrayList names, ArrayList values, ArrayList signs, ArrayList andor)//, bool bIsIn = false)
        {
            Expression expression_return = Expression.Constant(Convert.ToBoolean(andor[0])); //除非全或，否则第一个肯定是与关系
            ParameterExpression expression_param = Expression.Parameter(typeof(T), "c");

            for (int i = 0; i < names.Count; i++)
            {
                Expression equal = null;
                Expression left;
                List<Expression> right = new List<Expression>();
                Type tp = GetPropertyInfo<T>(names[i].ToString()).PropertyType;
                //判断是否为nullable泛型类
                if (tp.IsGenericType && tp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    var left1 = GetMemberExpression(expression_param, names[i].ToString());
                    left = Expression.Convert(left1, tp.GetGenericArguments()[0]);
                    tp = tp.GetGenericArguments()[0];
                }
                else
                   left = GetMemberExpression(expression_param, names[i].ToString());

                if ((Convert.ToInt16(signs[i]) == SIGN.In) || (Convert.ToInt16(signs[i]) == SIGN.NotIn))
                {
                    string[] ss = values[i].ToString().Split(CosValue.SPLITOPERATOR2);
                    foreach (string s in ss)
                        right.Add(Expression.Constant(Convert.ChangeType(s, tp)));
                }
                else if ((Convert.ToInt16(signs[i]) == SIGN.Contain1) || (Convert.ToInt16(signs[i]) == SIGN.Contain2))
                    right.Add(Expression.Constant(Convert.ChangeType(values[i], TypeCode.String)));
                else
                    right.Add(Expression.Constant(Convert.ChangeType(values[i], tp)));

                switch (Convert.ToInt16(signs[i]))
                {
                    case SIGN.Equal: equal = Expression.Equal(left, right[0]); break;
                    case SIGN.NotEqual: equal = Expression.NotEqual(left, right[0]); break;
                    case SIGN.GreaterThanOrEqual: equal = Expression.GreaterThanOrEqual(left, right[0]); break;
                    case SIGN.LessThanOrEqual: equal = Expression.LessThanOrEqual(left, right[0]); break;
                    case SIGN.GreaterThan: equal = Expression.GreaterThan(left, right[0]); break;
                    case SIGN.LessThan: equal = Expression.LessThan(left, right[0]); break;
                    case SIGN.Like: equal = Expression.Call(left, typeof(string).GetMethod("Contains"), right); break;
                    case SIGN.NotLike: equal = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains"), right)); break;
                    case SIGN.In:
                        equal = Expression.Equal(left, right[0]);
                        for (int k = 1; k < right.Count; k++)
                        {
                            var equal1 = Expression.Equal(left, right[k]);
                            equal = Expression.Or(equal, equal1);
                        }
                        break;
                    case SIGN.NotIn:
                        equal = Expression.NotEqual(left, right[0]);
                        for (int k = 1; k < right.Count; k++)
                        {
                            var equal1 = Expression.NotEqual(left, right[k]);
                            equal = Expression.And(equal, equal1);
                        }
                        break;
                    case SIGN.Contain1:
                        equal = Expression.Equal(left, right[0]);
                        List<Expression> right2 = new List<Expression>();
                        List<Expression> right3 = new List<Expression>();
                        List<Expression> right4 = new List<Expression>();
                        right2.Add(Expression.Constant(Convert.ChangeType(right[0].ToString().Trim('\"') + CosValue.SPLITOPERATOR1, TypeCode.String)));
                        right3.Add(Expression.Constant(Convert.ChangeType(CosValue.SPLITOPERATOR1 + right[0].ToString().Trim('\"'), TypeCode.String)));
                        right4.Add(Expression.Constant(Convert.ChangeType(CosValue.SPLITOPERATOR1 + right[0].ToString().Trim('\"') + CosValue.SPLITOPERATOR1, TypeCode.String)));
                        MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                        var equal2 = Expression.Call(left, startsWithMethod, right2);
                        var equal3 = Expression.Call(left, endsWithMethod, right3);
                        var equal4 = Expression.Call(left, typeof(string).GetMethod("Contains"), right4);
                        equal = Expression.Or(equal, equal2);
                        equal = Expression.Or(equal, equal3);
                        equal = Expression.Or(equal, equal4);
                        break;
                    case SIGN.Contain2:
                        equal = Expression.Equal(left, right[0]);
                        List<Expression> right5 = new List<Expression>();
                        List<Expression> right6 = new List<Expression>();
                        List<Expression> right7 = new List<Expression>();
                        right5.Add(Expression.Constant(Convert.ChangeType(right[0].ToString().Trim('\"') + CosValue.SPLITOPERATOR2, TypeCode.String)));
                        right6.Add(Expression.Constant(Convert.ChangeType(CosValue.SPLITOPERATOR2 + right[0].ToString().Trim('\"'), TypeCode.String)));
                        right7.Add(Expression.Constant(Convert.ChangeType(CosValue.SPLITOPERATOR2 + right[0].ToString().Trim('\"') + CosValue.SPLITOPERATOR2, TypeCode.String)));
                        MethodInfo startsWithMethod2 = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        MethodInfo endsWithMethod2 = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                        var equal5 = Expression.Call(left, startsWithMethod2, right5);
                        var equal6 = Expression.Call(left, endsWithMethod2, right6);
                        var equal7 = Expression.Call(left, typeof(string).GetMethod("Contains"), right7);
                        equal = Expression.Or(equal, equal5);
                        equal = Expression.Or(equal, equal6);
                        equal = Expression.Or(equal, equal7);
                        break;
                    default: equal = Expression.Equal(left, right[0]); break;
                }
                if (Convert.ToBoolean(andor[i]))
                    expression_return = Expression.AndAlso(expression_return, equal);
                else
                    expression_return = Expression.Or(expression_return, equal);
            }
            return Expression.Lambda<Func<T, bool>>(expression_return, new ParameterExpression[] { expression_param });
        }       

        /// <summary>
        /// 获得全是与操作的where
        /// </summary>
        /// <typeparam name="T">具体数据类型</typeparam>
        /// <param name="names">筛选名称</param>
        /// <param name="values">筛选值</param>
        /// <param name="signs">筛选符号，默认都是等于</param>
        /// <returns>返回where</returns>
        public Expression<Func<T, bool>> GetAndLambdaExpression<T>(ArrayList names, ArrayList values, ArrayList signs = null)
        {
            if (signs == null)
                signs = NewDuplicateArray(SIGN.Equal, names.Count);
            ArrayList andors = NewDuplicateArray(true, names.Count);
            return GetAndOrLambdaExpression<T>(names, values, signs, andors);
        }

        /// <summary>
        /// 获得全是或操作的where
        /// </summary>
        /// <typeparam name="T">具体数据类型</typeparam>
        /// <param name="names">筛选名称</param>
        /// <param name="values">筛选值</param>
        /// <param name="signs">筛选符号，默认都是等于</param>
        /// <returns>返回where</returns>
        public Expression<Func<T, bool>> GetOrLambdaExpression<T>(ArrayList names, ArrayList values, ArrayList signs = null)
        {
            if (signs == null)
                signs = NewDuplicateArray(SIGN.Equal, names.Count);
            ArrayList andors = NewDuplicateArray(false, names.Count);
            return GetAndOrLambdaExpression<T>(names, values, signs, andors);
        }

        #endregion

        #region GetRecord

        public int GetRecordCount<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                var query = (db.Set<T>().AsExpandable());
                if (query != null)
                {
                    if (where != null)
                        query = query.Where(where);
                    return query.Count();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }



        /// <summary>
        /// 根据当前的ID返回一条记录
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public dynamic GetOneRecordbyID<T, T1>(T1 sID) where T : class 
        {
            //这个地方如果用db.userinfo.find(sID)会更快捷，但没搞明白程序该怎么写
            try
            {
                long lID = Convert.ToInt64(sID);
                var express = BuildOneEqualExpression<T>("ID", lID);
                List<T> record = GetRecord(express);
                if (record.Count > 0)
                    return record[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;

        }

        public dynamic GetRecordsByID<T>(string sIDs) where T : class
        {
            try
            {
                ArrayList names = new ArrayList();
                ArrayList values = new ArrayList();
                ArrayList signs = new ArrayList();
                names.Add("ID");
                values.Add(sIDs);
                signs.Add(SIGN.In);
                var express = GetOrLambdaExpression<T>(names, values, signs);
                List<T> records = GetRecord(express);
                if (records.Count > 0)
                    return records;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// 得到当前最大的排序号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="transDB"></param>
        /// <returns></returns>
        public short GetMaxOrder<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                var express = BuildOrder<T, short>("TheOrder");
                var query = (db.Set<T>().AsExpandable());
                if (query != null)
                {
                    if (where != null)
                        query = query.Where(where);
                    query.Select(express);                    
                    query = query.OrderByDescending(express);
                    query.Take(1);
                    List<T> list = query.AsNoTracking().ToList();
                    if ((list != null) && (list.Count > 0))
                        return Convert.ToInt16(list[0].GetType().GetProperty("TheOrder").GetValue(list[0], null));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;

        }

        /// <summary>
        /// 根据条件判断一条记录是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="transDB"></param>
        /// <returns></returns>
        public long ExistOneRecord<T>(Expression<Func<T, bool>> where = null) where T : class
        {            
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                //var express = BuildOrder<T, long>("ID");                
                var query = (db.Set<T>().AsExpandable());
                if (query != null)
                {                    
                    if (where != null)
                        query = query.Where(where);
                    //query.AsQueryable().Select(express);
                    List<T> list = query.AsNoTracking().ToList();
                    if ((list != null) && (list.Count > 0))
                        return Convert.ToInt64(list[0].GetType().GetProperty("ID").GetValue(list[0], null));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }



        public List<T> GetRecord<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            int nTotalRecord = 0;
            return GetRecord<T, bool>(out nTotalRecord, where, null, true, 0, 0);
        }

        public List<T> GetRecord<T, T1>(Expression<Func<T, bool>> where = null, Expression<Func<T, T1>> order = null, bool asc = true) where T : class
        {
            int nTotalRecord = 0;
            return GetRecord(out nTotalRecord, where, order, asc, 0, 0);
        }

        public List<T> GetRecord<T,T1>(out int nTotalRecord, Expression<Func<T, bool>> where = null, Expression<Func<T, T1>> order = null, bool asc = true, int pageIndex = 0, int pageSize = 0) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                var query = (db.Set<T>().AsExpandable());
                if (query != null)
                {
                    if (where != null)
                        query = query.Where(where);
                    if (order != null)
                    {
                        if (asc)
                            query = query.OrderBy(order);
                        else
                            query = query.OrderByDescending(order);
                    }
                    nTotalRecord = query.Count();
                    if (pageIndex != 0)
                        query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return query.AsNoTracking().ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            nTotalRecord = 0;
            return null;
        }


        #endregion

        #region SaveRecords

        private bool RemoveHoldingEntityInContext<T>(BaseContext db, T entity) where T : class 
        {
            try
            {
                db.DetachOther(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveOneRecord<T>(T model, string[] EditColNames = null) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();                
                RemoveHoldingEntityInContext(db, model);
                if (EditColNames != null)
                {                    
                    DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(model));
                    for (int i = 0; i < EditColNames.Length; i++)
                        entry.Property(EditColNames[i]).IsModified = true;
                }
                else
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool SaveManyRecords<T>(List<T> models) where T : class
        {
            try
            {                 
                lock (locker)
                {
                    BaseContext db = DBContext.GetDataBaseContext();
                    db.BulkUpdate(models);
                    db.BulkSaveChanges();
                    //SaveSomeRecords(models);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveSomeRecords<T>(List<T> models, string[] EditColNames = null) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();                
                foreach (T m in models)
                {
                    RemoveHoldingEntityInContext(db, m);
                    if (EditColNames != null)
                    {                        
                        DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(m));
                        for (int i = 0; i < EditColNames.Length; i++)
                            entry.Property(EditColNames[i]).IsModified = true;
                    }
                    else
                        db.Entry(m).State = EntityState.Modified;
                }
                db.SaveChanges();                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region AddRecords
        public long AddOneRecord<T>(T model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();
                DbEntityEntry<T> entry = db.Entry(model);                
                return Convert.ToInt64(entry.Property("ID").CurrentValue);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<long> AddRecords<T>(List<T> models) where T : class
        {
            try
            {
                List<long> ids = new List<long>();
                lock (locker)
                {
                    BaseContext db = DBContext.GetDataBaseContext();
                    db.BulkInsert(models);
                    db.BulkSaveChanges();
                    //foreach(T m in models)
                    //    db.Entry(m).State = EntityState.Added;
                    //db.SaveChanges();
                }
                foreach (T m in models)
                    ids.Add(Convert.ToInt64(m.GetType().GetProperty("ID").GetValue(m, null)));
                return ids;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region DeleteRecord

        public bool DeleteRecordByDelflag<T>(T model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                RemoveHoldingEntityInContext(db, model);
                DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(model));
                entry.Property("DelFlag").CurrentValue = true;
                entry.Property("DelFlag").IsModified = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        public bool DeleteRecordByDelflag<T>(List<T> model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();                
                foreach (T m in model)
                {
                    RemoveHoldingEntityInContext(db, m);
                    DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(m));
                    entry.Property("DelFlag").CurrentValue = true;
                    entry.Property("DelFlag").IsModified = true;
                }
                db.SaveChanges();                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteRecord<T>(List<T> models) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                db.BulkDelete(models);
                db.BulkSaveChanges();

                //foreach (T m in model)
                //    db.Entry(m).State = EntityState.Deleted;
                //db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteRecord<T>(T model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                db.Entry(model).State = EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        #endregion

        #region VerifyRecord

        public bool VerifyRecords<T>(T model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                RemoveHoldingEntityInContext(db, model);
                DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(model));
                if (Convert.ToBoolean(model.GetType().GetProperty("VerifyFlag").GetValue(model, null)))
                    entry.Property("VerifyFlag").CurrentValue = false;
                else
                    entry.Property("VerifyFlag").CurrentValue = true;
                entry.Property("VerifyFlag").IsModified = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool VerifyRecords<T>(List<T> model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                RemoveHoldingEntityInContext(db, model);
                foreach (T m in model)
                {
                    DbEntityEntry<T> entry = db.Entry<T>(db.Set<T>().Attach(m));
                    if (Convert.ToBoolean(m.GetType().GetProperty("VerifyFlag").GetValue(m, null)))
                        entry.Property("VerifyFlag").CurrentValue = false;
                    else
                        entry.Property("VerifyFlag").CurrentValue = true;
                    entry.Property("VerifyFlag").IsModified = true;
                }
                db.SaveChanges();                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool VerifyAllRecords<T>(List<T> model) where T : class
        {
            try
            {
                BaseContext db = DBContext.GetDataBaseContext();
                RemoveHoldingEntityInContext(db, model);
                foreach (T m in model)
                {
                    DbEntityEntry<T> entry = db.Entry(db.Set<T>().Attach(m));
                    entry.Property("VerifyFlag").CurrentValue = true;
                    entry.Property("VerifyFlag").IsModified = true;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        #endregion

        #region SimpleRecord
        public virtual long SaveOneSimpleRecord(dynamic model, string keyword = null, string[] EditNames = null)
        {
            Type type = GetTypeObj(keyword).GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _SaveOneSimpleRecord(obj, model, EditNames);
        }
        /// <summary>
        /// 新增或者修改一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private long _SaveOneSimpleRecord<T>(T obj, T mo, string[] EditNames = null) where T : class, new()
        {
            T model = GetSpecialModelFromCurrentModel(mo);
            if (Convert.ToInt64(mo.GetType().GetProperty("ID").GetValue(mo, null)) == 0) //新增
            {
                ArrayList names = new ArrayList();
                ArrayList values = new ArrayList();
                names.Add("DelFlag");
                values.Add(false);
                var express = GetAndLambdaExpression<T>(names, values);
                short order = GetMaxOrder(express);
                order++;
                model.GetType().GetProperty("TheOrder").SetValue(model, order);
                return AddOneRecord(model);
            }
            else
            {
                if (EditNames == null)
                {
                    EditNames = new string[4];
                    EditNames[0] = "Name";
                    EditNames[1] = "Code";
                    EditNames[2] = "Remark";
                    EditNames[3] = "TheOrder";
                }
                SaveOneRecord(model, EditNames);
                return Convert.ToInt64(mo.GetType().GetProperty("ID").GetValue(mo, null));
            }
        }

        #endregion

    }
}