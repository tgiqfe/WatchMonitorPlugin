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

        public static bool WatchMD5Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMD5Hash, string path)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "MD5Hash";

            if ((isMD5Hash ?? false) || watch.MD5Hash != null)
            {
                string ret_string = GetMD5Hash(path);
                ret = ret_string != watch.MD5Hash;
                watch.MD5Hash = ret_string;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
            }
            return ret;
        }

        public static bool WatchSHA256Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isSHA256Hash, string path)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "SHA256Hash";

            if ((isSHA256Hash ?? false) || watch.SHA256Hash != null)
            {
                string ret_string = GetSHA256Hash(path);
                ret = ret_string != watch.SHA256Hash;
                watch.SHA256Hash = ret_string;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
            }
            return ret;
        }

        public static bool WatchSHA512Hash(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isSHA512Hash, string path)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "SHA512Hash";

            if ((isSHA512Hash ?? false) || watch.SHA512Hash != null)
            {
                string ret_string = GetSHA512Hash(path);
                ret = ret_string != watch.SHA512Hash;
                watch.SHA512Hash = ret_string;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
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
