// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Microsoft.Win32;
using System.Linq;

namespace CodeBuilder.Core
{
    public class RegistryHelper
    {
        public static T GetValue<T>(string key)
        {
            var regKey = OpenSubKey();
            if (!regKey.GetValueNames().Contains(key))
            {
                return default(T);
            }

            return (T)regKey.GetValue(key);
        }

        public static void SetValue<T>(string key, T value, RegistryValueKind kind = RegistryValueKind.String)
        {
            var regKey = OpenSubKey();
            regKey.SetValue(key, value, kind);
        }

        public static bool ContainsKey(string key)
        {
            var regKey = OpenSubKey();
            return regKey.GetValueNames().Contains(key);
        }

        private static RegistryKey OpenSubKey()
        {
            var regPath = "Software\\Fireasy\\CodeBuilder";
            var regKey = Registry.CurrentUser.OpenSubKey(regPath, true);
            if (regKey == null)
            {
                regKey = Registry.CurrentUser.CreateSubKey(regPath, true);
            }

            return regKey;
        }
    }
}
