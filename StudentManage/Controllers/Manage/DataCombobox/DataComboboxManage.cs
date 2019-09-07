using System;
using StudentManage.Models;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class DataComboboxManage : DataListManage
    {
        public virtual dynamic GetDataComboboxModels(string sNowNames = "", string sNowValues = "", string sNowSigns = "")
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetDataComboboxModels(obj, sNowNames, sNowValues, sNowSigns);            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sNowNames"></param>
        /// <param name="sNowValues"></param>
        /// <param name="sNowSigns"></param>
        /// <returns></returns>
        private object _GetDataComboboxModels<T>(T obj, string sNowNames = "", string sNowValues = "", string sNowSigns = "", bool bComboModel = true) where T : class
        {
            if (sNowNames != "ClearDataCombo")
            {
                Expression<Func<T, bool>> where = GetWhereForStore<T>(sNowNames, sNowValues, sNowSigns);
                Expression<Func<T, short>> order = null;
                try
                {
                    order = GetNowOrder<T>("TheOrder", CosDataBase.SMALLINT_TYPE);
                }
                catch
                {
                    order = null;
                }
                try
                {
                    List<T> models = GetRecord(where, order);
                    if (bComboModel)
                    {
                        List<tblSimpleInfo> returnmodels = new List<tblSimpleInfo>();
                        foreach (T n in models)
                        {
                            tblSimpleInfo mm = new tblSimpleInfo();
                            mm.ID = Convert.ToInt64(n.GetType().GetProperty("ID").GetValue(n, null));
                            mm.Name = Convert.ToString(n.GetType().GetProperty("Name").GetValue(n, null));
                            returnmodels.Add(mm);
                        }
                        return returnmodels;
                    }
                    else
                        return models;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return null;
        }

        /// <summary>
        /// 根据当前的名称获得当前的ID，找不到返回0。主要用在DataComboBox中
        /// </summary>
        /// <returns></returns>
        public virtual long GetNowIDFromName(string sName)
        {
            Type type = GetTypeObj().GetType();
            dynamic obj = type.Assembly.CreateInstance(type.ToString());
            return _GetNowIDFromName(obj, sName);            
        }
        private long _GetNowIDFromName<T>(T obj, string sName) where T : class
        {
            try
            {
                return Convert.ToInt64(sName);
            }
            catch
            {
                ArrayList arrNames = new ArrayList();
                ArrayList arrValues = new ArrayList();

                arrNames.Add("Name");
                arrValues.Add(sName);
                arrNames.Add("DelFlag");
                arrValues.Add(false);

                var where = GetAndLambdaExpression<T>(arrNames, arrValues);
                var list = GetRecord(where);
                if ((list == null) || (list.Count == 0))
                    return 0;
                return Convert.ToInt64(list[0].GetType().GetProperty("ID").GetValue(list[0], null));
            }
        }

    }




}