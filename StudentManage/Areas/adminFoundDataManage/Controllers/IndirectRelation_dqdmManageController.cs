using Ext.Net;
using Ext.Net.MVC;
using StudentManage.Controllers;
using System.Web.Mvc;

namespace StudentManage.Areas.adminFoundDataManage.Controllers
{
    [DirectController(AreaName = "adminFoundDataManage")]
    public class IndirectRelation_dqdmManageController : TreeMasterController
    {
        public IndirectRelation_dqdmManageController()
        {
            sAreaName = "adminFoundDataManage";
            sPageKeyWord = "IndirectRelation_dqdmManage";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_Form";
            sDataTreeKey = "dqdmTable";
        }
        public override void InitPage()
        {
            var btn = this.GetCmp<FileUploadField>(sMainFormName + "_btnInport");            
            btn.Hidden = false;
            base.InitPage();
        }
        
    }
}