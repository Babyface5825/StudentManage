using Ext.Net.MVC;
using StudentManage.Controllers;

namespace StudentManage.Areas.adminFoundDataManage.Controllers
{
    [DirectController(AreaName = "adminFoundDataManage")]
    public class DirectRelation_zylbdmManageController : TreeMasterController
    {
        public DirectRelation_zylbdmManageController()
        {
            sAreaName = "adminFoundDataManage";
            sPageKeyWord = "DirectRelation_zylbdmManage";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_Form";
            sDataTreeKey = "zylbdmTable";
                        
        }

    }
}