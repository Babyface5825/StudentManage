using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections;
using System.Collections.Generic;

namespace StudentManage.Manage
{
    /// <summary>
    /// 跟菜单管理相关的各种操作
    /// </summary>
    public class AuthorizationChannelTypeDataTreeManage : DataTreeManage
    {
        public AuthorizationChannelTypeDataTreeManage() 
        {
            Manage_KeyWord = "AuthorizationChannelType";
            bIsCheck = true;
        }



    }
}