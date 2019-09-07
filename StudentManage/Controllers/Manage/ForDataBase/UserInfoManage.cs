using System;
using System.Linq;
using StudentManage.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Collections;
using Ext.Net;
using System.Diagnostics;

namespace StudentManage.Manage
{
    /// <summary>
    /// 用户相关的各种Manage，主表为tblUserInfo
    /// </summary>
    public class UserInfoManage : ParentManage
    {
        public List<string> NowAuthorIDs = new List<string>(); //用于保存当前用户的权限ID集合

        public UserInfoManage()
        {
            Manage_KeyWord = "UserInfo";
        }
        #region 对密码的加密解密
        /// <summary>
        /// 获得解密后的密码
        /// </summary>
        /// <param name="sPass"></param>
        /// <returns></returns>
        public string GetRealPass(string sPass)
        {
            CryptManage crypt = new CryptManage();
            return crypt.Decrypto(sPass);
        }
        /// <summary>
        /// 对密码进行加密
        /// </summary>
        /// <param name="sPass"></param>
        /// <returns></returns>
        public string GetSecretPass(string sPass)
        {
            CryptManage crypt = new CryptManage();
            return crypt.Encrypto(sPass);
        }
        #endregion

        public tblUserInfo GetOneUserModel(string sUserID)
        {
            tblUserInfo model = GetSpecialModelFromCurrentID(sUserID);
            model.UserIPsd = GetRealPass(model.UserIPsd);
            return model;
        }


        public void Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<T_bmk> list = GetRecord<T_bmk>();
            foreach (T_bmk m in list)
            {
                m.Tag = "11";
                m.DelFlag = false;
            }            
            SaveManyRecords(list);
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;

        }


        /// <summary>
        /// 用户登录的判断函数正数
        /// </summary>
        /// <returns>登录不成功：错误提示字符串；登录成功：用户主ID</returns>
        public string UserLogin(tblUserInfo model, HttpResponseBase Response, string sValidateNum = "")
        {
            if (sValidateNum != "")
            {
                if (model.ValidatePic != sValidateNum)
                    return "验证码输入错误！";
            }

            if (!(string.IsNullOrEmpty(model.UserICode)))
            {
                Expression<Func<tblUserInfo, bool>> where = (c => c.UserICode == model.UserICode && c.DelFlag == false);                
                List<tblUserInfo> result = GetRecord(where);
                if ((result == null) || (result.Count == 0))
                    return "用户名不存在！";
                //if (!result[0].VerifyFlag)
                //    return "该用户尚未审核！";
                if (GetRealPass(result[0].UserIPsd) == model.UserIPsd)
                {
                    if (result[0].authorRelations.Count == 0)
                        return "该用户尚无权限登录后台！";
                    else //登录成功，同时写入cookie
                    {
                        LoginConfirm(result[0], Response);
                        return result[0].ID.ToString();
                    }
                }
                else
                    return "密码输入错误！";
            }
            else
                return "用户名不存在！";
        }

        #region 跟cookie相关的
        /// <summary>
        /// 用户登录成功，将ID等相关重要信息写入cookie中
        /// </summary>        
        private void LoginConfirm(tblUserInfo model, HttpResponseBase Response)
        {
            CryptManage crypt = new CryptManage();
            HttpCookie cookie = new HttpCookie("LoginUser");
            cookie.Expires = DateTime.Now.AddMinutes(30);

            DepartmentInfoDataTreeManage tree = new DepartmentInfoDataTreeManage();
            tblDepartmentInfo parent = tree.GetFirstParent(Convert.ToInt64(model.UserIDepartmentIID));

            cookie.Values.Add("ID", crypt.Encrypto(model.ID.ToString()));
            cookie.Values.Add("UserName", HttpUtility.UrlEncode(model.Name));
            cookie.Values.Add("RegisterName", HttpUtility.UrlEncode(model.UserICode));
            cookie.Values.Add("UserDuty", HttpUtility.UrlEncode(model.DutyInfo.Name));
            cookie.Values.Add("DepartmentID", crypt.Encrypto(model.UserIDepartmentIID.ToString()));
            if (parent != null)
            {
                cookie.Values.Add("ParentDepartmentID", crypt.Encrypto(parent.ID.ToString()));
                cookie.Values.Add("UserGxID", crypt.Encrypto(parent.GxID.ToString()));
            }
            else
            {
                cookie.Values.Add("ParentDepartmentID", crypt.Encrypto("0"));
                cookie.Values.Add("UserGxID", crypt.Encrypto("0"));
            }
            string sUserRoles = string.Join(CosValue.SPLITOPERATOR2.ToString(), model.authorRelations.Select(x => x.author.ID).ToArray());
            string sUserRoleNames = HttpUtility.UrlEncode(string.Join(CosValue.SPLITOPERATOR1.ToString(), model.authorRelations.Select(x => x.author.Name).ToArray()));
            cookie.Values.Add("UserRoles", crypt.Encrypto(sUserRoles));
            cookie.Values.Add("UserRoleNames", sUserRoleNames);

            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 根据传过来的键值获取当前登录用户中的cookie信息
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<string> GetLoginUserInfo(string[] keys)
        {
            var loginUser = HttpContext.Current.Request.Cookies.Get("LoginUser");
            CryptManage crypt = new CryptManage();
            List<string> s = new List<string>();
            foreach (string k in keys)
            {
                switch (k)
                {
                    case CookieKeys.ID: s.Add(crypt.Decrypto(loginUser["ID"])); break;
                    case CookieKeys.USERNAME: s.Add(HttpUtility.UrlDecode(loginUser["UserName"])); break;
                    case CookieKeys.REGISTERNAME: s.Add(HttpUtility.UrlDecode(loginUser["RegisterName"])); break;
                    case CookieKeys.USERDUTY: s.Add(HttpUtility.UrlDecode(loginUser["UserDuty"])); break;
                    case CookieKeys.DEPARTMENTID: s.Add(crypt.Decrypto(loginUser["DepartmentID"])); break;
                    case CookieKeys.PARENTDEPARTMENTID: s.Add(crypt.Decrypto(loginUser["ParentDepartmentID"])); break;
                    case CookieKeys.USERGXID: s.Add(crypt.Decrypto(loginUser["UserGxID"])); break;
                    case CookieKeys.USERROLES: s.Add(crypt.Decrypto(loginUser["UserRoles"])); break;
                    case CookieKeys.USERROLENAMES: s.Add(HttpUtility.UrlDecode(loginUser["UserRoleNames"])); break;
                }
            }
            return s;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="Response"></param>
        public void LoginOut(HttpResponseBase Response)
        {
            HttpCookie userCookies = Response.Cookies["LoginUser"];
            if (userCookies != null)
            {
                Response.Cookies.Remove("LoginUser");
                //userCookies.Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies.Add(userCookies);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoles"></param>
        /// <param name="sOperationID"></param>
        /// <returns></returns>
        public tblOperationAuthorRelation GetAccountOperationModel(string sRoles, string sOperationID)
        {
            ArrayList arrNames = new ArrayList();
            ArrayList arrValues = new ArrayList();
            ArrayList arrSigns = new ArrayList();
            arrNames.Add("AuthorID");arrNames.Add("OperationID");
            arrValues.Add(sRoles);arrValues.Add(sOperationID);
            arrSigns.Add(SIGN.In);arrSigns.Add(SIGN.Equal);
            Expression<Func<tblOperationAuthorRelation, bool>> where = GetAndLambdaExpression<tblOperationAuthorRelation>(arrNames, arrValues, arrSigns);
            List<tblOperationAuthorRelation> model = GetRecord(where);
            if (model != null)
                return model[0];
            else
                return null;
        }

        
        #region 用户权限显示保存相关
        /// <summary>
        /// 获取当前的author store
        /// </summary>
        /// <param name="sNowSelUserID">当前用户ID</param>
        /// <param name="flag">true：获取未选author；false：获取已选author。</param>
        /// <returns></returns>
        private Paging<object> GetAuthorStore(string sNowSelUserID, bool flag)
        {
            List<tblUserInfo> user = GetRecordsByID<tblUserInfo>(sNowSelUserID);
            int countNum = 0;
            if (!flag)
            {
                if (user != null)
                {
                    countNum = user[0].authorRelations.Count;
                    object[] data = new object[countNum];
                    for (int i = 0; i < countNum; i++)
                    {
                        object[] dataline = new object[2];
                        dataline[0] = user[0].authorRelations[i].AuthorID;
                        dataline[1] = user[0].authorRelations[i].author.Name;
                        data[i] = dataline;
                        NowAuthorIDs.Add(user[0].authorRelations[i].AuthorID.ToString());
                    }
                    return new Paging<object>(data, user[0].authorRelations.Count);
                }
                else
                {
                    object[] data = new object[0];
                    return new Paging<object>(data, 0);
                }
            }
            else
            {
                ArrayList arrNames = new ArrayList();
                ArrayList arrValues = new ArrayList();
                ArrayList arrSigns = new ArrayList();
                arrNames.Add("DelFlag"); arrValues.Add(false); arrSigns.Add(SIGN.Equal);
                Expression<Func<tblAuthorizationInfo, bool>> where = GetAndLambdaExpression<tblAuthorizationInfo>(arrNames, arrValues, arrSigns);
                var authors = GetRecord<tblAuthorizationInfo, bool>(where);
                if (user != null)
                    countNum = user[0].authorRelations.Count;
                object[] data = new object[authors.Count - countNum];
                int k = 0;
                foreach (tblAuthorizationInfo author in authors)
                {
                    bool flag1 = true;
                    int i = 0;
                    while ((flag1) && (i < countNum))
                    {
                        if (author.ID == user[0].authorRelations[i].AuthorID)
                            flag1 = false;
                        else
                            i++;
                    }
                    if (flag1)
                    {
                        object[] dataline = new object[2];
                        dataline[0] = author.ID;
                        dataline[1] = author.Name;
                        data[k] = dataline;
                        k++;
                    }
                }
                return new Paging<object>(data, authors.Count - countNum);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sNowSelUserID"></param>        
        /// <returns></returns>
        public Paging<object> GetAllAuthorStore(string sNowSelUserID)
        {
            return GetAuthorStore(sNowSelUserID, true);
        }
        public Paging<object> GetNowAuthorStore(string sNowSelUserID)
        {
            return GetAuthorStore(sNowSelUserID, false);
        }

        public void SaveNowAuthor(long lUserID, string sSelAuthors, string sOriginAuthors)
        {
            //首先判断权限是否有变化
            try
            {
                List<long> lSelAuthors = new List<long>();
                if (sSelAuthors != "")
                {
                    List<string> arrSelAuthors = sSelAuthors.Split(CosValue.SPLITOPERATOR2).ToList();
                    lSelAuthors = arrSelAuthors.Select(x => Convert.ToInt64(x)).ToList();
                    lSelAuthors.Sort();
                }

                List<long> lOriginAuthors = new List<long>();
                if (sOriginAuthors != "")
                {
                    List<string> arrOriginAuthors = sOriginAuthors.Split(CosValue.SPLITOPERATOR2).ToList();
                    lOriginAuthors = arrOriginAuthors.Select(x => Convert.ToInt64(x)).ToList();
                    lOriginAuthors.Sort();
                }
                
                if (!(lSelAuthors.All(lOriginAuthors.Contains) && lSelAuthors.Count == lOriginAuthors.Count))
                {
                    BaseContext db = new BaseContext();
                    Expression<Func<tblUserAuthorRelation, bool>> where = (c => c.UserID == lUserID);
                    List<tblUserAuthorRelation> models = GetRecord(where);
                    DeleteRecord(models);
                    var user = db.UserInfo.Find(lUserID);
                    var authors = GetRecordsByID<tblAuthorizationInfo>(sSelAuthors);
                    if (authors != null)
                    {
                        List<tblUserAuthorRelation> list = new List<tblUserAuthorRelation>();
                        foreach (tblAuthorizationInfo author in authors)
                        {
                            tblUserAuthorRelation relation = new tblUserAuthorRelation();
                            relation.author = author;
                            relation.AuthorID = author.ID;
                            relation.user = user;
                            relation.UserID = user.ID;
                            list.Add(relation);
                        }
                        AddRecords(list);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        #endregion
    }


}