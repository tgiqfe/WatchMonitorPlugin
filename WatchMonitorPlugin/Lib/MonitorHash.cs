using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
using IO.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorHash : MonitorBase
    {

        public static string GetHash(string filePath, HashAlgorithm hash)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string text = BitConverter.ToString(hash.ComputeHash(fs)).Replace("-", "");
                hash.Clear();
                return text;
            }
        }

        public static string GetHash(RegistryKey regKey, string name, HashAlgorithm hash)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);
            string text = BitConverter.ToString(hash.ComputeHash(bytes)).Replace("-", "");
            hash.Clear();
            return text;
        }
    }
}
