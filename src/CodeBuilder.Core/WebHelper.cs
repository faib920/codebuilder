// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeBuilder.Core
{
    public static class WebHelper
    {
        public static readonly string HomeUrl = "http://www.fireasy.cn";

        public static string GetRedirectUrl(IDevHosting hosting, string url)
        {
            return hosting.IsAuthorized ? $"{HomeUrl}/user/accept?token={Uri.EscapeDataString(Encrypt(hosting.GetConfig("AccessToken")?.ToString()))}&redirect={Uri.EscapeDataString(url)}" : $"{HomeUrl}{url}";
        }

        public static string Encrypt(string str)
        {
            var key = new byte[] { 78, 5, 66, 34, 121, 34, 89, 41, 67, 90, 33, 45, 121, 15, 63, 93 };
            var iv = new byte[] { 171, 52, 46, 54, 71, 56, 45, 51, 6, 6, 76, 23, 76, 32, 87, 74 };
            using (var aes = Aes.Create())
            using (var trans = aes.CreateEncryptor(key, iv))
            using (var stream = new MemoryStream())
            {
                var cryptStream = new CryptoStream(stream, trans, CryptoStreamMode.Write);
                var buffer = Encoding.UTF8.GetBytes(str);
                cryptStream.Write(buffer, 0, buffer.Length);
                cryptStream.FlushFinalBlock();

                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
