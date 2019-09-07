using System;
using System.Web;
using System.IO;
using System.Text;

namespace StudentManage.Controllers
{
    public class Pdf2SwfController 
    {
        public bool ConvertToSwf(string exeFile, string pdfPath, string swfPath, int page)
        {
            try
            {
                string exe1 = exeFile + "/swftools/pdf2swf.exe";
                string exe2 = exeFile + "/swftools/swfcombine.exe";
                if (!File.Exists(exe1) || !File.Exists(exe2))
                {
                    throw new ApplicationException("Can not find");
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(" -o \"" + swfPath + "\"");//output
                sb.Append(" -z");
                sb.Append(" -t");                
                //sb.Append(" -S");
                sb.Append(" -G");
                //sb.Append(" -s poly2bitmap");
                //sb.Append(" -s bitmap");
                sb.Append(" -s flashversion=8");//flash version
                sb.Append(" -s disablelinks");//禁止PDF里面的链接  
                //sb.Append(" -s languagedir=d:\\newIalab\\xpdf\\xpdf-chinese-simplified"); 
                sb.Append(" -p " + "1" + "-" + page);//page range
                sb.Append(" -j 85");//Set quality of embedded jpeg pictures to quality. 0 is worst (small), 100 is best (big). (default:85)
                sb.Append(" \"" + pdfPath + "\"");//input
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = exe1;
                proc.StartInfo.Arguments = sb.ToString();
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.Start();
                proc.WaitForExit();
                proc.Close();

                sb = new StringBuilder();
                string view = exeFile + "/swfs/default_viewer.swf";
                sb.Append(" -o \"" + swfPath + "\"");
                sb.Append(" \"" + view + "\"");
                sb.Append(" viewport=\"" + swfPath + "\"");
                proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = exe2;
                proc.StartInfo.Arguments = sb.ToString();
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
                return true;
            }
            catch (Exception ex)
            {
                // throw ex;
                return false;
            }
        }

        public int GetPageCount(string pdfPath)
        {
            try
            {
                byte[] buffer = File.ReadAllBytes(pdfPath);
                int length = buffer.Length;
                if (buffer == null)
                    return -1;

                if (buffer.Length <= 0)
                    return -1;

                string pdfText = Encoding.Default.GetString(buffer);
                System.Text.RegularExpressions.Regex rx1 = new System.Text.RegularExpressions.Regex(@"/Type\s*/Page[^s]");
                System.Text.RegularExpressions.MatchCollection matches = rx1.Matches(pdfText);
                return matches.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}