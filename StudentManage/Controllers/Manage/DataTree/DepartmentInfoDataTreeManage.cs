using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class DepartmentInfoDataTreeManage : DataTreeManage
    {

        public DepartmentInfoDataTreeManage()
        {
            Manage_KeyWord = "DepartmentInfo";
        }        

        public override bool IsOneParent(long Id, long ParentId)
        {
            return _IsOneParent<tblDepartmentInfo>(Id, ParentId);
        }
        public override bool IsOneLeaf(long Id, long LeafId)
        {
            return _IsOneLeaf<tblDepartmentInfo>(Id, LeafId);
        }

        /// <summary>
        /// 根据当前的编号，获取其所在学校的编号（就是返回二级节点）
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public tblDepartmentInfo GetSchoolModel(string sID)
        {
            tblDepartmentInfo model = GetOneRecordbyID<tblDepartmentInfo, long>(Convert.ToInt64(sID));
            if (model.TheLevel == 3)
                return GetOneRecordbyID<tblDepartmentInfo, long>(model.ParentIndex);
            else
                return model;
        }

        /// <summary>
        /// 根据当前的编号，获取该学校和所有部门的编号（就是返回二级节点和所有三级节点）
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public List<tblDepartmentInfo> GetAllSchoolModels(string sID)
        {            
            tblDepartmentInfo model = GetOneRecordbyID<tblDepartmentInfo, long>(Convert.ToInt64(sID));
            long lSecondLevelID = 0;
            if (model.TheLevel == 3)
                lSecondLevelID = model.ParentIndex;
            else
                lSecondLevelID = model.ID;
            string sAllLeaf = GetAllLeaf(lSecondLevelID);
            return GetRecordsByID<tblDepartmentInfo>(sAllLeaf);
        }

        /// <summary>
        /// 根据传过来的学校编号和部门名称，得到该部门model
        /// </summary>
        /// <param name="lSchoolID"></param>
        /// <param name="sDepartmentName"></param>
        /// <returns></returns>
        public tblDepartmentInfo GetDepartmentFromName(long lSchoolID, string sDepartmentName)
        {
            Expression<Func<tblDepartmentInfo, bool>> where = (c => (c.Name == sDepartmentName && c.ParentIndex==lSchoolID && c.DelFlag == false));            
            List<tblDepartmentInfo> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }



    }




}