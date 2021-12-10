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
        const string CHECK_TARGET_dm5 = "MD5Hash";
        const string CHECK_TARGET_sha256 = "SHA256Hash";
        const string CHECK_TARGET_sha512 = "SHA512Hash";

        #region Compare method



        #endregion

        /*
        #region Watch method

        public static bool WatchFileMD5Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsMD5Hash ?? false)
            {
                if (File.Exists(path))
                {
                    string ret_string = GetFileMD5Hash(path);
                    ret = ret_string != watch.MD5Hash;
                    if (watch.MD5Hash != null)
                    {
                        string pathType = "file";
                        string checkTarget = "MD5Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.MD5Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.MD5Hash = ret_string;
                }
                else
                {
                    watch.MD5Hash = null;
                }
            }
            return ret;
        }

        public static bool WatchFileSHA256Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsSHA256Hash ?? false)
            {
                if (File.Exists(path))
                {
                    string ret_string = GetFileSHA256Hash(path);
                    ret = ret_string != watch.SHA256Hash;
                    if (watch.SHA256Hash != null)
                    {
                        string pathType = "file";
                        string checkTarget = "SHA256Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.SHA256Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.SHA256Hash = ret_string;
                }
                else
                {
                    watch.SHA256Hash = null;
                }
            }
            return ret;
        }

        public static bool WatchFileSHA512Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsSHA512Hash ?? false)
            {
                if (File.Exists(path))
                {
                    string ret_string = GetFileSHA512Hash(path);
                    ret = ret_string != watch.SHA512Hash;
                    if (watch.SHA512Hash != null)
                    {
                        string pathType = "file";
                        string checkTarget = "SHA512Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.SHA512Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.SHA512Hash = ret_string;
                }
                else
                {
                    watch.SHA512Hash = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryValueMD5Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey, string name)
        {
            bool ret = false;
            if (watch.IsMD5Hash ?? false)
            {
                if (regKey != null && regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    string ret_string = GetRegistryValueMD5Hash(regKey, name);
                    ret = ret_string != watch.MD5Hash;
                    if (watch.MD5Hash != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "MD5Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.MD5Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.MD5Hash = ret_string;
                }
                else
                {
                    watch.MD5Hash = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryValueSHA256Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey, string name)
        {
            bool ret = false;
            if (watch.IsSHA256Hash ?? false)
            {
                if (regKey != null && regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    string ret_string = GetRegistryValueSHA256Hash(regKey, name);
                    ret = ret_string != watch.SHA256Hash;
                    if (watch.SHA256Hash != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "SHA256Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.SHA256Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.SHA256Hash = ret_string;
                }
                else
                {
                    watch.SHA256Hash = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryValueSHA512Hash(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey, string name)
        {
            bool ret = false;
            if (watch.IsSHA512Hash ?? false)
            {
                if (regKey != null && regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    string ret_string = GetRegistryValueSHA512Hash(regKey, name);
                    ret = ret_string != watch.SHA512Hash;
                    if (watch.SHA512Hash != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "SHA512Hash";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.SHA512Hash} -> {ret_string}" :
                            ret_string;
                    }
                    watch.SHA512Hash = ret_string;
                }
                else
                {
                    watch.SHA512Hash = null;
                }
            }
            return ret;
        }

        #endregion
        */

        #region Get method

        public static string GetFileMD5Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = MD5.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetFileSHA256Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = SHA256.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetFileSHA512Hash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hashAlg = SHA512.Create();
                string text = BitConverter.ToString(hashAlg.ComputeHash(fs)).Replace("-", "");
                hashAlg.Clear();
                return text;
            }
        }

        public static string GetRegistryValueMD5Hash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = MD5.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        public static string GetRegistryValueSHA256Hash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = MD5.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        public static string GetRegistryValueSHA512Hash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = MD5.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        #endregion

        protected virtual string GetHash(string filePath) { return null; }

        protected virtual string GetHash(RegistryKey regKey, string name) { return null; }

    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorMD5Hash : MonitorHash
    {
        public override string CheckTarget { get { return "MD5Hash"; } }

        protected override string GetHash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hash = MD5.Create();
                string text = BitConverter.ToString(hash.ComputeHash(fs)).Replace("-", "");
                hash.Clear();
                return text;
            }
        }

        protected override string GetHash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = MD5.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsMD5Hash ?? false)
            {
                if (!monitoring.InfoA.Exists || !monitoring.InfoB.Exists) { return false; }

                string retA = GetHash(monitoring.PathA);
                string retB = GetHash(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool CompareRegistryValue(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsMD5Hash ?? false)
            {
                if (monitoring.KeyA == null || monitoring.KeyB == null) { return false; }

                string retA = GetHash(monitoring.KeyA, monitoring.NameA);
                string retB = GetHash(monitoring.KeyB, monitoring.NameB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsMD5Hash ?? false)
            {
                if (monitoring.Info.Exists)
                {
                    string result = GetHash(monitoring.Path);
                    ret = result != monitoring.MD5Hash;
                    if (monitoring.MD5Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.MD5Hash} -> {result}" :
                            result;
                    }
                    monitoring.MD5Hash = result;
                }
                else
                {
                    monitoring.MD5Hash = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsMD5Hash ?? false)
            {
                if (monitoring.Key != null && monitoring.Key.GetValueNames().Any(x => x.Equals(monitoring.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    string result = GetHash(monitoring.Key, monitoring.Name);
                    ret = result != monitoring.MD5Hash;
                    if (monitoring.MD5Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.MD5Hash} -> {result}" :
                            result;
                    }
                    monitoring.MD5Hash = result;
                }
                else
                {
                    monitoring.MD5Hash = null;
                }
            }
            return ret;
        }

        #endregion
    }


    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorSHA256Hash : MonitorHash
    {
        public override string CheckTarget { get { return "SHA256Hash"; } }

        protected override string GetHash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hash = SHA256.Create();
                string text = BitConverter.ToString(hash.ComputeHash(fs)).Replace("-", "");
                hash.Clear();
                return text;
            }
        }

        protected override string GetHash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = SHA256.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsSHA256Hash ?? false)
            {
                if (!monitoring.InfoA.Exists || !monitoring.InfoB.Exists) { return false; }

                string retA = GetHash(monitoring.PathA);
                string retB = GetHash(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool CompareRegistryValue(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsSHA256Hash ?? false)
            {
                if (monitoring.KeyA == null || monitoring.KeyB == null) { return false; }

                string retA = GetHash(monitoring.KeyA, monitoring.NameA);
                string retB = GetHash(monitoring.KeyB, monitoring.NameB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsSHA256Hash ?? false)
            {
                if (monitoring.Info.Exists)
                {
                    string result = GetHash(monitoring.Path);
                    ret = result != monitoring.SHA256Hash;
                    if (monitoring.SHA256Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.SHA256Hash} -> {result}" :
                            result;
                    }
                    monitoring.SHA256Hash = result;
                }
                else
                {
                    monitoring.SHA256Hash = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsSHA256Hash ?? false)
            {
                if (monitoring.Key != null && monitoring.Key.GetValueNames().Any(x => x.Equals(monitoring.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    string result = GetHash(monitoring.Key, monitoring.Name);
                    ret = result != monitoring.SHA256Hash;
                    if (monitoring.SHA256Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.SHA256Hash} -> {result}" :
                            result;
                    }
                    monitoring.SHA256Hash = result;
                }
                else
                {
                    monitoring.SHA256Hash = null;
                }
            }
            return ret;
        }

        #endregion
    }


    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorSHA512Hash : MonitorHash
    {
        public override string CheckTarget { get { return "SHA512Hash"; } }

        protected override string GetHash(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hash = SHA512.Create();
                string text = BitConverter.ToString(hash.ComputeHash(fs)).Replace("-", "");
                hash.Clear();
                return text;
            }
        }

        protected override string GetHash(RegistryKey regKey, string name)
        {
            byte[] bytes = RegistryControl.RegistryValueToBytes(regKey, name, null, true);

            var hashAlg = SHA512.Create();
            string text = BitConverter.ToString(hashAlg.ComputeHash(bytes)).Replace("-", "");
            hashAlg.Clear();
            return text;
        }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsSHA512Hash ?? false)
            {
                if (!monitoring.InfoA.Exists || !monitoring.InfoB.Exists) { return false; }

                string retA = GetHash(monitoring.PathA);
                string retB = GetHash(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool CompareRegistryValue(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsSHA512Hash ?? false)
            {
                if (monitoring.KeyA == null || monitoring.KeyB == null) { return false; }

                string retA = GetHash(monitoring.KeyA, monitoring.NameA);
                string retB = GetHash(monitoring.KeyB, monitoring.NameB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsSHA512Hash ?? false)
            {
                if (monitoring.Info.Exists)
                {
                    string result = GetHash(monitoring.Path);
                    ret = result != monitoring.SHA512Hash;
                    if (monitoring.SHA512Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.SHA512Hash} -> {result}" :
                            result;
                    }
                    monitoring.SHA512Hash = result;
                }
                else
                {
                    monitoring.SHA512Hash = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsSHA512Hash ?? false)
            {
                if (monitoring.Key != null && monitoring.Key.GetValueNames().Any(x => x.Equals(monitoring.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    string result = GetHash(monitoring.Key, monitoring.Name);
                    ret = result != monitoring.SHA512Hash;
                    if (monitoring.SHA512Hash != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.SHA512Hash} -> {result}" :
                            result;
                    }
                    monitoring.SHA512Hash = result;
                }
                else
                {
                    monitoring.SHA512Hash = null;
                }
            }
            return ret;
        }

        #endregion
    }
}
