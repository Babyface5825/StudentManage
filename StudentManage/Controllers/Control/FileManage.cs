using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Ext.Net;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    public class FileManager
    {
        public FileUploadField fuHand;

        /// <summary>
        /// 删除文件夹下的所有文件
        /// </summary>
        /// <param name="dir">文件夹名称</param>
        public void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    DeleteFolder(d1.FullName);////递归删除子文件夹                    
                }
            }
            Directory.Delete(dir);
        }

        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        public void DelOneFile(string sFileName)
        {
            FileInfo file = new FileInfo(sFileName);
            if (file.Exists)
                file.Delete();
        }
        /// <summary>
        /// 修改一个文件名称
        /// </summary>
        /// <param name="sOldFileName">源文件名称</param>
        /// <param name="sNewFileName">新文件名称</param>
        public void RenameFile(string sOldFileName, string sNewFileName)
        {
            FileInfo file = new FileInfo(sOldFileName);
            if (file.Exists)
            {
                file.CopyTo(sNewFileName, true);
                file.Delete();
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns></returns>
        public bool HasFile(string sFileName)
        {
            FileInfo file = new FileInfo(sFileName);
            return file.Exists;
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns></returns>
        public bool HasPath(string sFilePath)
        {
            return Directory.Exists(sFilePath);
        }

        public bool DownloadFile(string FileName)
        {
            return DownloadFile(FileName, "");
        }

        //public bool DownloadFile(string FileName, string sContentType, string DownloadName)
        //{
        //    if (HasFile(FileName))
        //    {
        //        FileInfo DownloadFile = new FileInfo(FileName);
        //        FileStream myFile = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //        //Reads file as binary values
        //        BinaryReader _BinaryReader = new BinaryReader(myFile);

        //        if (DownloadName == "")
        //            DownloadName = System.Web.HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.UTF8);
        //        else
        //            DownloadName = System.Web.HttpUtility.UrlEncode(DownloadName, System.Text.Encoding.UTF8);
        //        try
        //        {
        //            //指定块大小   
        //            long chunkSize = 4096;
        //            //建立一个4K的缓冲区   
        //            byte[] buffer = new byte[chunkSize];
        //            //剩余的字节数   
        //            long dataToRead = 0;
        //            dataToRead = myFile.Length;

        //            //添加Http头   
        //            HttpContext.Current.Response.ContentType = sContentType;
        //            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + DownloadName);
        //            HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

        //            while (dataToRead > 0)
        //            {
        //                if (HttpContext.Current.Response.IsClientConnected)
        //                {
        //                    int length = myFile.Read(buffer, 0, Convert.ToInt32(chunkSize));
        //                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
        //                    HttpContext.Current.Response.Flush();
        //                    HttpContext.Current.Response.Clear();
        //                    dataToRead -= length;
        //                }
        //                else
        //                {
        //                    //防止client失去连接   
        //                    dataToRead = -1;
        //                }
        //            }
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            HttpContext.Current.Response.Write("Error:" + ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            if (myFile != null)
        //                myFile.Close();                    
        //        }
        //    }
        //    else
        //        return false; 
        //}

        public bool DownloadFile(string FileName, string DownloadName)
        {
            if (HasFile(FileName))
            {
                FileInfo DownloadFile = new FileInfo(FileName);
                FileStream myFile = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //Reads file as binary values
                BinaryReader _BinaryReader = new BinaryReader(myFile);

                if (DownloadName == "")
                    DownloadName = HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.UTF8);
                else
                    DownloadName = HttpUtility.UrlEncode(DownloadName, System.Text.Encoding.UTF8);
                try
                {
                    long startBytes = 0;
                    string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(DownloadFile.Directory.ToString()).ToString("r");
                    string _EncodedData = HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;


                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    System.Web.HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                    System.Web.HttpContext.Current.Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    System.Web.HttpContext.Current.Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                    //System.Web.HttpContext.Current.Response.ContentType = sContentType;
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + DownloadName);
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", (DownloadFile.Length - startBytes).ToString());
                    System.Web.HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);

                    ////Send data
                    //_BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                    ////Dividing the data in 1024 bytes package
                    //int maxCount = (int)Math.Ceiling((DownloadFile.Length - startBytes + 0.0) / 128);

                    ////Download in block of 1024 bytes
                    //int i;
                    //for (i = 0; i < maxCount && System.Web.HttpContext.Current.Response.IsClientConnected; i++)
                    //{
                    //    System.Web.HttpContext.Current.Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    //    System.Web.HttpContext.Current.Response.Flush();
                    //}
                    ////if blocks transfered not equals total number of blocks
                    //if (i < maxCount)
                    //    return false;
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    //System.Web.HttpContext.Current.Response.End();
                    _BinaryReader.Close();
                    myFile.Close();
                }
            }
            else
                return false;
        }
        public bool UploadFile(FileUploadField fuTransHand, string sFileName)
        {
            fuHand = fuTransHand;
            return upFile(sFileName);
        }
        bool upFile(string sFileName)
        {
            if (fuHand.HasFile)
            {
                string name = fuHand.PostedFile.FileName;

                FileInfo file = new FileInfo(name);
                string fileName = file.Name;
                string webFilePath = sFileName;
                {
                    try
                    {
                        fuHand.PostedFile.SaveAs(webFilePath);
                    }
                    catch (Exception ex)
                    {
                        string errText = webFilePath + "提示：文件上传失败，失败原因：" + ex.Message;
                        return false;
                    }
                }
                return true;

            }
            return false;
        }

        public void CopyFile(string sSourceName, string sDestinationName, bool overwrite = true)
        {
            File.Copy(sSourceName, sDestinationName, overwrite);          
        }

        public void MoveFile(string sSourceName, string sDestinationName)
        {
            //如果目的地文件夹不存在，则创建一个
            string sFileFolder = sDestinationName.Remove(sDestinationName.LastIndexOf('\\'));
            if (!Directory.Exists(sFileFolder))
            {
                Directory.CreateDirectory(sFileFolder);
            }
            if(File.Exists(sDestinationName))
            {
                File.Delete(sDestinationName);
            }
            File.Move(sSourceName, sDestinationName);

        }


        ////递归获得子文件目录下的文件
        List<string> filePaths = new List<string>();
        public List<string> GetAllFiles(string rootPath)
        {

            string[] files = Directory.GetFiles(rootPath);
            foreach(string file in files)
                filePaths.Add(file);
            string[] subPaths = Directory.GetDirectories(rootPath);
            foreach (string path in subPaths)
            {
                GetAllFiles(path);
            }
            return filePaths;
        }

        
    }
}