using Ext.Net;
using StudentManage.Models;

namespace StudentManage.Manage
{
    /// <summary>
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class RoleAuthorizationDataListManage : DataListManage
    {

        public RoleAuthorizationDataListManage()
        {
            Manage_KeyWord = "AuthorizationInfo";
            bNeedAuthor = false;
        }

        public override void SetMainListType()
        {
            bSort = false;
            bHasDelFlag = true;
            bSingleSelect = true;
            bFirstVisit = true;
            sOrder = "TheOrder";
            nOrderType = CosDataBase.SMALLINT_TYPE;
        }

        public override void SetarrColumn()
        {
            ColumnModel model1 = new ColumnModel("名称", "Name");
            arrColumn.Clear();
            arrColumn.Add(model1);
        }

        public override void SetarrTopButton()
        {
            ButtonModel btnMMore1 = new ButtonModel(DataListButton.btnMore1, "保存权限", Icon.Disk);
            arrTopButton.Clear();
            arrTopButton.Add(btnMMore1);
        }
     
    }   

}