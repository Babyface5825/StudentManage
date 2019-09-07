using Ext.Net.MVC;
using StudentManage.Controllers;

namespace StudentManage.Areas.adminSystemManage.Controllers
{
    [DirectController(AreaName = "adminSystemManage")]
    public class OperationInfoController : TreeMasterController
    {
        // GET: adminSystemManage/OperationInfo

        public OperationInfoController()
        {
            sAreaName = "adminSystemManage";
            sPageKeyWord = "OperationInfo";
            sDataListName = sPageKeyWord + "_DataList";
            sDataTreeName = sPageKeyWord + "_DataTree";
            sMainFormName = sPageKeyWord + "_Form";
            sDataTreeKey = sPageKeyWord;            
        }
        //public override void InitPage()
        //{
        //    var btn = this.GetCmp<Button>(sMainFormName + "_btnMore");
        //    btn.Text = "图标上传";            
        //    btn.Hidden = false;
        //    base.InitPage();
        //}

        //public override ActionResult TreeMaster_MoreInfoClick(tblTreeInfo model)
        //{
        //    if (model.ID == 0)
        //    {
        //        X.Msg.Alert("注意", "请先选中要操作的节点！").Show();
        //        return this.Direct();
        //    }
        //    OperationInfoManage tree = new OperationInfoManage();
        //    tblOperationInfo modelhere = tree.GetSpecialModelFromCurrentID(model.ID);
        //    if (!modelhere.IsLeaf)
        //    {
        //        X.Msg.Alert("注意", "只有叶子节点才能添加图标！").Show();
        //        return this.Direct();
        //    }
        //    setParentVariable(null, null, null, model.ID.ToString(), null);            
        //    Ext.Net.MVC.PartialViewResult pvr = new Ext.Net.MVC.PartialViewResult();
        //    pvr.ViewName = "~\\Areas\\UserControl\\Views\\ExtUserControl\\WinUploadOperationIcon.cshtml";
        //    pvr.ViewData["WinUploadOperationIcon_WindowID"] = "OperationInfo_WinIcon";
        //    pvr.ViewData["WinUploadOperationIcon_HiddenVariable"] = ParentVar.DefaultVal;
        //    return pvr;
        //}


    }
}