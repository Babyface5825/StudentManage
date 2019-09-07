using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;


namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinSimpleMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "详细信息";
            win.Resizable = false;
            SetWindowSize(win, 340, 255, sWidth, sHeight);
            return this.Direct();
        }


        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
            string[] sss = new string[2];
            sss[0] = ss[0];
            if (ss.Length == 2)
                sss[1] = ss[1];
            else
                sss[1] = null;
            DataListManage man = GetDataListManage(sss[0], sss[1]);
            man.InitSelection(ParentVar.transValue);
            var model = man.GetSpecialModelFromCurrentID(man.sRowNowID);
            if (model.ID == 0)
            {
                SimpleDlgDataListManage simpleMan = new SimpleDlgDataListManage();
                model = simpleMan.GetNewModel(model, sss[1]);
            }
            var form = this.GetCmp<FormPanel>(PanelID + "_Form");
            form.SetValues(model);
            return this.Direct();
        }


        public ActionResult WindowSaveClick(tblSimpleInfo mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            string[] ss = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2);
            SimpleDlgDataListManage smpDlgMan = (SimpleDlgDataListManage)GetDataListManage(ss[0], ss[1]);
            tblSimpleInfo transModel = (tblSimpleInfo)smpDlgMan.GetSpecialModelFromCurrentModel(mo);
            long nNewID = smpDlgMan.SaveOneSimpleRecord(transModel);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}