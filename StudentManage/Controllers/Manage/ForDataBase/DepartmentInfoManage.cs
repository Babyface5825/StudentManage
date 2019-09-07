using System;
using StudentManage.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class DepartmentInfoManage : DataTreeManage
    {

        public DepartmentInfoManage() 
        {
            Manage_KeyWord = "DepartmentInfo";
        }

        /// <summary>
        /// 根据当前用户权限，返回该用户可以访问的所有ID
        /// </summary>
        /// <param name="sAuthorIDs"></param>
        /// <returns></returns>
        public string GetSpecialIDsFromAuthors(string sAuthorIDs, string sDepartmentID)
        {
            string sReturn = "-1";
            AuthorizationInfoDataListManage manAuthor = new AuthorizationInfoDataListManage();
            DepartmentInfoManage manDepart = new DepartmentInfoManage();
            List<tblAuthorizationInfo> authors = manAuthor.GetRecordsByID<tblAuthorizationInfo>(sAuthorIDs);
            if (!authors.Select(x => x.Name).ToArray().Contains(UserRoleName.SUPER)) //如果不是超级管理员，只显示其所在部门和下级部门
                sReturn = manDepart.GetAllLeaf(sDepartmentID);
            return sReturn;
        }

        ///// <summary>
        ///// 根据分拣条件，把所有符合的ID组成字符串返回
        ///// </summary>
        ///// <param name="sNames"></param>
        ///// <param name="sValues"></param>
        ///// <param name="sSigns"></param>
        ///// <returns></returns>
        //public string GetSpecialIDsFromWhere(string sNames, string sValues, string sSigns)
        //{
        //    ArrayList arrNames = new ArrayList(sNames.Split(CosValue.SPLITOPERATOR1));
        //    ArrayList arrValues = new ArrayList(sValues.Split(CosValue.SPLITOPERATOR1));
        //    ArrayList arrSigns = new ArrayList(sSigns.Split(CosValue.SPLITOPERATOR1));
        //    Expression<Func<tblDepartmentInfo, bool>> where = GetAndLambdaExpression<tblDepartmentInfo>(arrNames, arrValues, arrSigns);
        //    List<tblDepartmentInfo> list = GetRecord(where);
        //    string sReturn = "";
        //    if ((list != null) && (list.Count > 0))
        //        sReturn = string.Join(CosValue.SPLITOPERATOR2.ToString(), list.Select(c => c.ID).ToArray());
        //    return sReturn;
        //}

        /// <summary>
        /// 获得所有上级部门名称拼起来的完整名称
        /// </summary>
        /// <param name="lDepartmentID"></param>
        /// <returns></returns>
        public string GetWholeDepartmentName(long lDepartmentID)
        {
            string s = "";
            tblDepartmentInfo model = GetOneRecordbyID<tblDepartmentInfo, long>(lDepartmentID);
            while (model != null)
            {
                s = model.Name + s;
                model = GetOneRecordbyID<tblDepartmentInfo, long>(model.ParentIndex);
            }
            return s;
        }


        public override dynamic GetSpecialModelFromCurrentID<T>(T ID)
        {
            tblDepartmentInfo obj = base.GetSpecialModelFromCurrentID(ID);
            if ((obj.ZYIDs != null) && (obj.ZYIDs != ""))
            {
                ArrayList arrNames = new ArrayList();
                ArrayList arrSigns = new ArrayList();
                ArrayList arrValues = new ArrayList();
                arrNames.Add("ID");
                arrValues.Add(obj.ZYIDs);
                arrSigns.Add(SIGN.In);
                var where = GetAndLambdaExpression<Td_zydh>(arrNames, arrValues, arrSigns);
                List<Td_zydh> list = GetRecord(where);
                if ((list != null) && (list.Count > 0))
                    obj.ZYNames = string.Join(CosValue.SPLITOPERATOR2.ToString(), list.Select(x => x.Name).ToArray());
                //obj.GetType().GetProperty("ParentName").SetValue(obj, GetAllParentName(ID), null);
            }
            return obj;
        }

        public object[] GetNameStoreforSelection(string GxID, string SelectedStore, bool useFlag = true, string sTypeID = "Department")
        {
            string sUsedIDs = GetUsedIDs(SelectedStore, sTypeID);
            ReportManage man = new ReportManage();
            long lParent = man.GetParentDepartIDfromGxID(Convert.ToInt64(GxID));            
            ArrayList arrNames = new ArrayList(); ArrayList arrValues = new ArrayList(); ArrayList arrSigns = new ArrayList();
            arrNames.Add("DelFlag"); arrNames.Add("ParentIndex"); arrNames.Add("TheLevel"); arrNames.Add("ZYIDs"); arrNames.Add("ID");
            arrValues.Add(false); arrValues.Add(lParent); arrValues.Add(2); arrValues.Add(" ");  arrValues.Add(sUsedIDs);
            arrSigns.Add(SIGN.Equal); arrSigns.Add(SIGN.Equal); arrSigns.Add(SIGN.Equal); arrSigns.Add(SIGN.NotEqual);
            if (useFlag)
                arrSigns.Add(SIGN.In);
            else
                arrSigns.Add(SIGN.NotIn);
            var where = GetAndLambdaExpression<tblDepartmentInfo>(arrNames, arrValues, arrSigns);

            object[] data = GetReturnData(sTypeID, where, "院系-");
            if (data != null)
                return data;

            object[] data0 = new object[0];
            return data0;
        }

    }
}