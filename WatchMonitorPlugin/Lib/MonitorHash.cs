using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorHash
    {
        #region Check method

        public static bool WatchFileMD5Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitorTarget, string path)
        {
            if (!isMonitorTarget ?? true) { return false; }

            bool ret = false;
            if (watch.MD5Hash == null)
            {
                ret = true;
                watch.MD5Hash = GetMD5Hash(path);
            }
            else
            {
                string pathType = "file";
                string checkTarget = "MD5Hash";

                string ret_string = GetMD5Hash(path);
                ret = ret_string != watch.MD5Hash;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.MD5Hash} -> {ret_string}" :
                    ret_string;

                watch.MD5Hash = ret_string;
            }
            return ret;
        }

        public static bool WatchFileSHA256Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitorTarget, string path)
        {
            if (!isMonitorTarget ?? true) { return false; }

            bool ret = false;
            if (watch.SHA256Hash == null)
            {
                ret = true;
                watch.SHA256Hash = GetSHA256Hash(path);
            }
            else
            {
                string pathType = "file";
                string checkTarget = "SHA256Hash";

                string ret_string = GetSHA256Hash(path);
                ret = ret_string != watch.SHA256Hash;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.SHA256Hash} -> {ret_string}" :
                    ret_string;

                watch.SHA256Hash = ret_string;
            }
            return ret;
        }

        public static bool WatchFileSHA512Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitorTarget, string path)
        {
            if (!isMonitorTarget ?? true) { return false; }

            bool ret = false;
            if (watch.SHA512Hash == null)
            {
                ret |= true;
                watch.SHA512Hash = GetSHA512Hash(path);
            }
            else
            {
                string pathType = "file";
                string checkTarget = "SHA512Hash";

                string ret_string = GetSHA512Hash(path);
                ret = ret_string != watch.SHA512Hash;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.SHA512Hash} -> {ret_string}" :
                    ret_string;

                watch.SHA512Hash = ret_string;
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string GetMD5Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = MD5.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetMD5Hash(FileStream fs)
        {
            var hashAlg = MD5.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        public static string GetSHA256Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = SHA256.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetSHA256Hash(FileStream fs)
        {
            var hashAlg = SHA256.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        public static string GetSHA512Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = SHA512.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetSHA512Hash(FileStream fs)
        {
            var hashAlg = SHA512.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        #endregion
    }
}
