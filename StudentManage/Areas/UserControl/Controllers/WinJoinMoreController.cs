using Ext.Net.MVC;
using System.Web.Mvc;
using StudentManage.Manage;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;
using System.Linq;
using System;

namespace StudentManage.Areas.UserControl.Controllers
{
    public class WinJoinMoreController : ModelWindowsController
    {
        public override ActionResult WindowCommonBeforeShow(string PanelID, string sWidth, string sHeight)
        {
            Window win = this.GetCmp<Window>(PanelID);
            win.Title = "合并项目";
            win.Resizable = false;
            SetWindowSize(win, 310, 160, sWidth, sHeight);
            return this.Direct();
        }

        public override ActionResult WindowCommonShow(string PanelID, string ParentVariable, string ParentKey)
        {
            GetParentVariable(ParentVariable);
            List<SelectionModel> sel = JSON.Deserialize<List<SelectionModel>>(ParentVar.transValue);
            if (sel != null)
            {
                string keyWord = ParentVar.KeyWords;
                if (ParentVar.KeyWords.Contains(CosValue.SPLITOPERATOR2))
                    keyWord = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2)[1];

                OriginTableManage man = new OriginTableManage();
                JoinModel model = new JoinModel();
                model.ID = Convert.ToInt64(sel[0].RecordID);
                model.AllJoinIDs = string.Join(CosValue.SPLITOPERATOR2.ToString(), sel.Select(x => x.RecordID).ToArray());
                model.AllJoinNames = man.GetSomeNames(sel, keyWord);

                setParentVariable("UserControl", "WinJoinMore", "WinJoinMore_SelectChange", null, keyWord);
                SetHiddenValue(PanelID + "_cmbJoin_ParentVariable", ParentVar.DefaultVal);
                SetHiddenValue(PanelID + "_cmbJoin_NowNames", "ID");
                SetHiddenValue(PanelID + "_cmbJoin_NowValues", model.AllJoinIDs);
                SetHiddenValue(PanelID + "_cmbJoin_NowSigns", SIGN.In.ToString());
                SetComboValue(PanelID + "_cmbJoin", model.ID.ToString(), true);

                var form = this.GetCmp<FormPanel>(PanelID + "_Form");
                form.SetValues(model);
            }
            return this.Direct();
        }
        public ActionResult WinJoinMore_SelectChange(string PanelID, string sSelID)
        {
            SetHiddenValue(PanelID + "_NowSelID", sSelID);
            return this.Direct();
        }

        public ActionResult WindowSaveClick(JoinModel mo, string PanelID, string ParentVariable)
        {
            GetParentVariable(ParentVariable);
            string keyWord = ParentVar.KeyWords;
            if (ParentVar.KeyWords.Contains(CosValue.SPLITOPERATOR2))
                keyWord = ParentVar.KeyWords.Split(CosValue.SPLITOPERATOR2)[1];
            OriginTableManage man = new OriginTableManage();
            man.JoinRecords(mo, keyWord);
            SetHiddenValue(PanelID + "_ExitCode", "1");
            return this.Direct();
        }

    }
}