using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class AuthorizationInfoDataListManage : SimpleDlgDataListManage
    {

        public AuthorizationInfoDataListManage()
        {
            Manage_KeyWord = "AuthorizationInfo";
        }
       
    }




}