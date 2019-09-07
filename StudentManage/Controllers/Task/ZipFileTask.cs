 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Runtime.CompilerServices;
using Ionic.Zip;
using Ionic.Zlib;
using StudentManage.Models;

namespace StudentManage.Controllers.Task
{
    //定时压缩实验报告文件
    public class ZipFileTask
    {
                
        public ZipFileTask()
        {

        }

        public bool ExtractZipFile(string sWholeFileName, string sDesAddress)
        {
            FileManager file = new FileManager();
            if (file.HasFile(sWholeFileName))
            {
                ZipFile zip = new ZipFile(sWholeFileName, System.Text.Encoding.Default);
                zip.ExtractAll(sDesAddress,ExtractExistingFileAction.OverwriteSilently);//相同则重写
                return true;
            }
            else
                return false;            
        }
        

        //public void CreateZipFile()
        //{
        //    CoursePeriodRelationDataListManage man1 = new CoursePeriodRelationDataListManage();
        //    StudentCourseTestFileDataListManage man2 = new StudentCourseTestFileDataListManage();
        //    List<tblCoursePeriodRelation> list1 = man1.GetPeriodCourses(); //找到本学期所有开课班级
        //    foreach (tblCoursePeriodRelation c in list1)
        //    {
        //        //首先检查文件是否存在，如果存在，找到文件创建时间
        //        FileManager file = new FileManager();
        //        DateTime fileDate = DateTime.MinValue;
        //        ZipFile zip;                
        //        if (file.HasFile(sAddress + c.ID + ".zip"))
        //        {
        //            fileDate = file.FileDate(sAddress + c.ID + ".zip");
        //            zip = new ZipFile(sAddress + c.ID + ".zip", System.Text.Encoding.Default);
        //        }
        //        else
        //            zip = new ZipFile(System.Text.Encoding.Default);
        //        zip.CompressionLevel = CompressionLevel.BestSpeed;
        //        List<tblStudentCourseTestFile> list2 = man2.GetAllTestFileRecords(c.ID, fileDate);
        //        FileManager clsFile = new FileManager();
        //        if (list2.Count > 0)
        //        {
        //            foreach (tblStudentCourseTestFile m in list2)
        //            {
        //                string sWholeFileName = sAddress + m.Address;
        //                string sDesFileName = man2.GetDownloadName(m);
        //                string sDesName = sDesAddress + sDesFileName;
        //                if (clsFile.HasFile(sWholeFileName))
        //                {
        //                    clsFile.CopyFile(sWholeFileName, sDesName);
        //                    if (zip[sDesFileName] != null)
        //                         zip.RemoveEntry(sDesFileName);
        //                }    
        //            }
        //            zip.AddDirectory(sDesAddress);
        //            if (file.HasFile(sAddress + c.ID + ".zip"))
        //                zip.Save();
        //            else
        //                zip.Save(sAddress + c.ID + ".zip");
        //            zip.Dispose();
        //            clsFile.DeleteFolder(sDesAddress);
        //        }
        //    }
        //}
    }
}