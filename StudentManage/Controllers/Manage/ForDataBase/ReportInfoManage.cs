using System.Data;
using StudentManage.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Data.OleDb;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportInfoManage : OriginTableManage
    {    
        public ReportInfoManage() 
        {
            Manage_KeyWord = "ReportInfo";
        }
    }
}