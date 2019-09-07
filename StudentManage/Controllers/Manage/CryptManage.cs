using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace StudentManage.Manage
{
    /// <summary>
    /// 加解密
    /// </summary>
    public class CryptManage
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string Key;

        public CryptManage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            mobjCryptoService = new RijndaelManaged();
            Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
        }

        /// <summary>    
        /// 获得密钥    
        /// </summary>    
        /// <returns>密钥</returns>    
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>    
        /// 加密方法    
        /// </summary>    
        /// <param name="Source">待加密的串</param>    
        /// <returns>经过加密的串</returns>    
        public string Encrypto(string Source)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>    
        /// 解密方法    
        /// </summary>    
        /// <param name="Source">待解密的串</param>    
        /// <returns>经过解密的串</returns>    
        public string Decrypto(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch(Exception e)
            {
                return "";
            }
        }

    }
}