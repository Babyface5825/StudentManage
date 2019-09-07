using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class tjxxTableManage : OriginTableManage
    {
        public string[] NormalStr1 = { "有", "无" };
        public string[] NormalStr2 = { "未知", "正常", "其他" };
        public string[] NormalStr3 = { "未知", "喻自萍", "其他" };
        public string[] NormalStr4 = { "不能识别", "能识别" };
        public string[] NormalStr5 = { "未知", "合格", "专业受限", "不合格" };
        public string[] NormalStr6 = { "未知", "良", "中", "差" };
        public string[] NormalStr7 = { "迟钝", "正常"};
        public string[] NormalStr8 = { "是", "否" };


        public List<BMKTable> BMK_list;
        public tjxxTableManage()
        {
            Manage_KeyWord = "tjxxTable";
        }


        private short VerifyStr(string STR)
        {
            try
            {
                if ((STR != "") && (STR != "."))
                    return Convert.ToInt16(STR);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        private string GetNormalStr(string[] NormalStr, string ColumnName, DataRow dr)
        {
            try
            {
                return NormalStr[VerifyStr(GetOneStrValue(dr, ColumnName, CosDataBase.INT_TYPE))];
            }
            catch
            {
                return "数据错误"; //NormalStr[0];
            }
        }

        public T_tjxx GetOneRecordbyRelationID(long lID)
        {
            Expression<Func<T_tjxx, bool>> where = (c => c.RelationID == lID && c.DelFlag == false);
            List<T_tjxx> list = GetRecord(where);
            if ((list != null) && (list.Count > 0))
                return list[0];
            else
                return null;
        }

        public void InportOriginData(DataTable dt, int nYear, long lProvinceID)
        {
            bmkTableManage bmkMan = new bmkTableManage();
            List<T_tjxx> listAdd = new List<T_tjxx>();
            foreach (DataRow dr in dt.Rows)
            {
                //long lbmkID = bmkMan.GetbmkID(dr["KSH"].ToString());
                long lbmkID = GetBMKIDFromList(BMK_list, dr["KSH"].ToString());
                if (lbmkID != 0)
                {
                    //Expression<Func<T_tjxx, bool>> where = (c => c.RelationID == lbmkID && c.DelFlag == false);
                    //List<T_tjxx> list = GetRecord(where);
                    //if ((list == null) || (list.Count == 0)) //一条报名库记录对应一条投档单记录
                    //{
                        T_tjxx tjxx_model = new T_tjxx();
                        tjxx_model.RelationID = lbmkID;
                        tjxx_model.Tjxh = GetOneStrValue(dr, "TJXH");
                        tjxx_model.Jwbsbz = GetNormalStr(NormalStr1, "JWBSBZ", dr); 
                        tjxx_model.Jwbs = GetOneStrValue(dr, "JWBS");
                        tjxx_model.Yk_lysly = Convert.ToDouble(GetOneStrValue(dr, "YK_LYSLY", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_lyslz = Convert.ToDouble(GetOneStrValue(dr, "YK_LYSLZ", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_jzsly = Convert.ToDouble(GetOneStrValue(dr, "YK_JZSLY", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_jzslz = Convert.ToDouble(GetOneStrValue(dr, "YK_JZSLZ", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_jzdsy = Convert.ToDouble(GetOneStrValue(dr, "YK_JZDSY", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_jzdsz = Convert.ToDouble(GetOneStrValue(dr, "YK_JZDSZ", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Yk_sjjc = GetNormalStr(NormalStr2, "YK_SJJC", dr);
                        tjxx_model.Yk_sjjct = GetNormalStr(NormalStr3, "YK_SJJCT", dr); 
                        tjxx_model.Yk_sjds1 = GetNormalStr(NormalStr4, "YK_SJDS1", dr);
                        tjxx_model.Yk_sjds2 = GetNormalStr(NormalStr4, "YK_SJDS2", dr);
                        tjxx_model.Yk_sjds3 = GetNormalStr(NormalStr4, "YK_SJDS3", dr);
                        tjxx_model.Yk_sjds4 = GetNormalStr(NormalStr4, "YK_SJDS4", dr);
                        tjxx_model.Yk_sjds5 = GetNormalStr(NormalStr4, "YK_SJDS5", dr);
                        tjxx_model.Yk_ysyj = GetNormalStr(NormalStr5, "YK_YSYJ", dr); 
                        tjxx_model.Yk_yb = GetOneStrValue(dr, "YK_YB");

                        tjxx_model.Nk_xyssy = Convert.ToDouble(GetOneStrValue(dr, "NK_XYSSY", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Nk_xyszy = Convert.ToDouble(GetOneStrValue(dr, "NK_XYSZY", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Nk_fyqk = GetNormalStr(NormalStr6, "NK_FYQK", dr);
                        tjxx_model.Nk_xzjxg = GetNormalStr(NormalStr2, "NK_XZJXG", dr);
                        tjxx_model.Nk_hxxt = GetNormalStr(NormalStr2, "NK_HXXT", dr);
                        tjxx_model.Nk_sjxt = GetNormalStr(NormalStr2, "NK_SJXT", dr);
                        tjxx_model.Nk_g = Convert.ToDouble(GetOneStrValue(dr, "NK_G", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Nk_gxz = GetNormalStr(NormalStr2, "NK_GXZ", dr); 
                        tjxx_model.Nk_p = Convert.ToDouble(GetOneStrValue(dr, "NK_P", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Nk_pxz = GetNormalStr(NormalStr2, "NK_PXZ", dr); 
                        tjxx_model.Nk_ysyj = GetNormalStr(NormalStr5, "NK_YSYJ", dr);
                        tjxx_model.Nk_qt = GetOneStrValue(dr, "NK_QT");

                        tjxx_model.Wk_sg = Convert.ToDouble(GetOneStrValue(dr, "WK_SG", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Wk_tz = Convert.ToDouble(GetOneStrValue(dr, "WK_TZ", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Wk_pf = GetNormalStr(NormalStr2, "WK_PF", dr); 
                        tjxx_model.Wk_mb = GetNormalStr(NormalStr2, "WK_MB", dr); 
                        tjxx_model.Wk_jb = GetNormalStr(NormalStr2, "WK_JB", dr); 
                        tjxx_model.Wk_jz = GetNormalStr(NormalStr2, "WK_JZ", dr); 
                        tjxx_model.Wk_sz = GetNormalStr(NormalStr2, "WK_SZ", dr); 
                        tjxx_model.Wk_gj = GetNormalStr(NormalStr2, "WK_GJ", dr); 
                        tjxx_model.Wk_ysyj = GetNormalStr(NormalStr5, "WK_YSYJ", dr);
                        tjxx_model.Wk_qt = GetOneStrValue(dr, "WK_QT");

                        tjxx_model.Eb_zetl = Convert.ToDouble(GetOneStrValue(dr, "EB_ZETL", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Eb_yetl = Convert.ToDouble(GetOneStrValue(dr, "EB_YETL", CosDataBase.FLOAT_TYPE));
                        tjxx_model.Eb_xj = GetNormalStr(NormalStr7, "EB_XJ", dr); 
                        tjxx_model.Eb_ysyj = GetNormalStr(NormalStr5, "EB_YSYJ", dr);
                        tjxx_model.Eb_ebyh = GetOneStrValue(dr, "EB_EBYH");

                        tjxx_model.Kq_ce = GetNormalStr(NormalStr2, "KQ_CE", dr); 
                        tjxx_model.Kq_sfkc = GetNormalStr(NormalStr8, "KQ_SFKC", dr);
                        tjxx_model.Kq_yc = GetNormalStr(NormalStr2, "KQ_YC", dr); 
                        tjxx_model.Kq_ysyj = GetNormalStr(NormalStr5, "KQ_YSYJ", dr);

                        tjxx_model.Xt = GetNormalStr(NormalStr2, "XT", dr); 
                        tjxx_model.Xt_ysyj = GetNormalStr(NormalStr5, "XT_YSYJ", dr);
                        tjxx_model.Xt_qt = GetOneStrValue(dr, "XT_QT");

                        tjxx_model.Gg_zam = GetNormalStr(NormalStr2, "GG_ZAM", dr); 
                        tjxx_model.Gg_ygky = GetNormalStr(NormalStr2, "GG_YGKY", dr);
                        tjxx_model.Gg_ysyj = GetNormalStr(NormalStr5, "GG_YSYJ", dr);
                        tjxx_model.Gg_qt = GetOneStrValue(dr, "GG_QT");

                        tjxx_model.Zysxdm1 = GetOneStrValue(dr, "ZYSXDM1");
                        tjxx_model.Zysxdm2 = GetOneStrValue(dr, "ZYSXDM2");
                        tjxx_model.Zysxdm3 = GetOneStrValue(dr, "ZYSXDM3");
                        tjxx_model.Zysxdm4 = GetOneStrValue(dr, "ZYSXDM4");
                        tjxx_model.Zysxdm5 = GetOneStrValue(dr, "ZYSXDM5");
                        tjxx_model.Zysxdm6 = GetOneStrValue(dr, "ZYSXDM6");
                        //tjxx_model.Tjjldm = NormalStr5[Convert.ToInt16(GetOneStrValue(dr, "TJJLDM", CosDataBase.INT_TYPE))];
                        tjxx_model.Remark = GetOneStrValue(dr, "BZ");
                        listAdd.Add(tjxx_model);
                        //AddOneRecord(tjxx_model);
                    //}
                }
            }
            AddRecords(listAdd);
        }
    }
}