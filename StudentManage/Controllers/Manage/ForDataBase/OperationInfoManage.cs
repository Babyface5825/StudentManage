using System;
using StudentManage.Models;

namespace StudentManage.Manage
{
    /// <summary>
    /// 跟菜单管理相关的特有操作
    /// </summary>
    public class OperationInfoManage : DataTreeManage
    {

        public OperationInfoManage() 
        {
            Manage_KeyWord = "OperationInfo";
        }
        
        public override void InitSimpleAuthorizationArray(string sRelationIDs)
        {
            tblOperationInfo obj = new tblOperationInfo();
            _InitSimpleAuthorizationArray<tblOperationInfo, tblOperationAuthorRelation>(obj, sRelationIDs, "authorRelations", "AuthorID");
        }
        public override void InitAuthorizationArray(string sRelationIDs)
        {
            tblOperationInfo obj = new tblOperationInfo();
            _InitAuthorizationArray<tblOperationAuthorRelation>(sRelationIDs,  "AuthorID", "OperationID");

        }

        /// <summary>
        /// 根据节点的ID，获取该节点的名称和URL全称
        /// </summary>
        /// <param name="sID">节点ID</param>
        /// <returns>Name|URL</returns>
        public string GetNameandWholeURL(string sID)
        {
            string sReturn = "";
            string sWholeURL = "";            
            long nParentIndex = Convert.ToInt64(sID);

            while (nParentIndex != -1)
            {
                tblOperationInfo result = GetOneRecordbyID<tblOperationInfo, long>(nParentIndex);
                if (result != null)
                {
                    if (nParentIndex == Convert.ToInt64(sID))
                        sReturn = result.Name;
                    string sURL = result.Code;
                    if (sURL[0] == '/') //如果链接前面带了反斜杠，则认为是绝对链接
                    {
                        sWholeURL = sURL.Remove(0, 1) + "/";
                        nParentIndex = -1;
                    }
                    else //注意二级菜单和三级菜单的不同
                    {
                        if (sWholeURL != "" && result.ParentIndex != -1)
                            sWholeURL = sURL + "_" + sWholeURL;
                        else
                            sWholeURL = sURL + "/" + sWholeURL;
                        nParentIndex = result.ParentIndex;
                    }
                }
            }
            if (sWholeURL.Length > 0)
            {
                if (!(sWholeURL.Contains("?")))
                    sWholeURL = sWholeURL.Remove(sWholeURL.Length - 1, 1) + "/";
                else
                    sWholeURL = sWholeURL.Remove(sWholeURL.Length - 1, 1);
            }
            sReturn = sReturn + CosValue.SPLITOPERATOR1 + sWholeURL;
            return sReturn;
        }
    }
}