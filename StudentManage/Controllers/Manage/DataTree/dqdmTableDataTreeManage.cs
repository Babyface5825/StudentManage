using System;
using System.Linq;
using StudentManage.Models;
using Ext.Net;
using System.Collections;
using System.Collections.Generic;

namespace StudentManage.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public class dqdmTableDataTreeManage : DataTreeManage
    {
        public dqdmTableDataTreeManage() 
        {
            Manage_KeyWord = "dqdmTable";
        }


        public Td_dqdm GetParentNode(long lID)
        {
            Td_dqdm model = GetOneRecordbyID<Td_dqdm, long>(lID);
            if (model.TheLevel <= 2)
                return model;
            else
                return GetOneRecordbyID<Td_dqdm, long>(model.ParentIndex);
        }
    }
}