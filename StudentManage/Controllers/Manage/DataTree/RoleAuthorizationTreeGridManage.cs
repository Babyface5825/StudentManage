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
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class RoleAuthorizationTreeGridManage : DataTreeManage
    {

        public RoleAuthorizationTreeGridManage()
        {
            Manage_KeyWord = "OperationInfo";
        }

        public override void SaveAllTreeGridAuthor(string sAuthorizaionNowID, SubmittedNode subNode)
        {
            _SaveAllTreeGridAuthor<tblOperationAuthorRelation>(sAuthorizaionNowID, "OperationID", subNode);
        }

        /// <summary>
        /// 权限数组初始化，只是针对菜单的 
        /// </summary>
        /// <param name="sRelationIDs">UserRole中的所有ID</param>
        public override void InitAuthorizationArray(string sRelationIDs)
        {
            arrNowAuthorization.Clear();

            if (sRelationIDs != "")
            {
                ArrayList names = new ArrayList(); ArrayList values = new ArrayList(); ArrayList signs = new ArrayList();
                names.Add("AuthorID"); values.Add(sRelationIDs); signs.Add(SIGN.In);
                Expression<Func<tblOperationAuthorRelation, bool>> where = GetAndLambdaExpression<tblOperationAuthorRelation>(names, values, signs);
                List<tblOperationAuthorRelation> list = GetRecord<tblOperationAuthorRelation, short>(where);
                for (int i = 0; i < list.Count; i++)
                {
                    TreeGridModel tgm = new TreeGridModel();
                    tgm.sID = list[i].OperationID;
                    tgm.bAdd = list[i].AddFlag;
                    tgm.bSearch = list[i].SearchFlag;
                    tgm.bEdit = list[i].EditFlag;
                    tgm.bDelete = list[i].DeleteFlag;
                    if ((tgm.bAdd) && (tgm.bSearch) && (tgm.bDelete) && (tgm.bEdit))
                        tgm.bAll = true;
                    arrNowAuthorization.Add(tgm);
                }
            }
        }



    }




}