using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class zylbdmTableDataTreeManage : DataTreeManage
    {
        public zylbdmTableDataTreeManage() 
        {
            Manage_KeyWord = "zylbdmTable";
        }
        public Td_zylbdm GetParentNode(long lID)
        {
            Td_zylbdm model = GetOneRecordbyID<Td_zylbdm, long>(lID);
            if (model.TheLevel <= 2)
                return model;
            else
                return GetOneRecordbyID<Td_zylbdm, long>(model.ParentIndex);
        }
        


    }
}