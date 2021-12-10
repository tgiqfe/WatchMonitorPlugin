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
        protected virtual string GetHash(string filePath) { return null; }

        protected virtual string GetHash(RegistryKey regKey, string name) { return null; }
    }

    /// <summary>
    /// MD5
    /// </summary>
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

    /// <summary>
    /// SHA256
    /// </summary>
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

    /// <summary>
    /// SHA512
    /// </summary>
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
