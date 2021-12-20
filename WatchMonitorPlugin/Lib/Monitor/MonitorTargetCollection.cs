using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using IO.Lib;
using System.Security.Principal;
using System.Security.Cryptography;

namespace Audit.Lib.Monitor
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorTargetCollection : Dictionary<string, MonitorTarget>
    {
        public bool? IsCreationTime { get; set; }
        public bool? IsLastWriteTime { get; set; }
        public bool? IsLastAccessTime { get; set; }
        public bool? IsAccess { get; set; }
        public bool? IsOwner { get; set; }
        public bool? IsInherited { get; set; }
        public bool? IsAttributes { get; set; }
        public bool? IsMD5Hash { get; set; }
        public bool? IsSHA256Hash { get; set; }
        public bool? IsSHA512Hash { get; set; }
        public bool? IsSize { get; set; }
        public bool? IsChildCount { get; set; }
        public bool? IsRegistryType { get; set; }
        public bool? IsDateOnly { get; set; }
        public bool? IsTimeOnly { get; set; }


        private List<string> _CheckedKeys = new List<string>();

        const string REGPATH_PREFIX = "[reg]";

        public MonitorTargetCollection() { }

        #region Get/Set MonitorTarget

        public MonitorTarget GetMonitorTarget(string path)
        {
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(path, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ? null : this[matchKey];
        }

        public MonitorTarget GetMonitorTarget(string path, string name)
        {
            string regPath = REGPATH_PREFIX + path + "\\" + name;
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(regPath, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ? null : this[matchKey];
        }

        public void SetMonitorTarget(string path, MonitorTarget target)
        {
            this[path] = target;
            this._CheckedKeys.Add(path);
        }

        public void SetMonitorTarget(string path, string name, MonitorTarget target)
        {
            string regPath = REGPATH_PREFIX + path + "\\" + name;
            this[regPath] = target;
            this._CheckedKeys.Add(regPath);
        }

        #endregion

        public IEnumerable<string> GetUncheckedKeys()
        {
            return this.Keys.Where(x => !_CheckedKeys.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase)));
        }

        #region Load/Save

        public static MonitorTargetCollection Load(string dbDir, string id)
        {
            try
            {
                using (var sr = new StreamReader(Path.Combine(dbDir, id), Encoding.UTF8))
                {
                    return JsonSerializer.Deserialize<MonitorTargetCollection>(sr.ReadToEnd());
                }
            }
            catch { }
            return new MonitorTargetCollection();
        }

        public void Save(string dbDir, string id)
        {
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            using (var sw = new StreamWriter(Path.Combine(dbDir, id), false, Encoding.UTF8))
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                sw.WriteLine(json);
            }
        }

        #endregion

        public void CheckFile(string path)
        {
            MonitorTarget target = this.ContainsKey(path) ? this[path] : new MonitorTarget();

            if (IsCreationTime ?? false)
            {
                target.CreationTime = MonitorFunctions.GetCreationTime(target.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false);
            }
            if (IsLastWriteTime ?? false)
            {
                target.LastWriteTime = MonitorFunctions.GetLastWriteTime(target.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false);
            }
            if (IsLastAccessTime ?? false)
            {
                target.LastAccessTime = MonitorFunctions.GetLastAccessTime(target.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false);
            }
            if (IsAccess ?? false)
            {
                target.Access = AccessRuleSummary.FileToAccessString(target.FileInfo);
            }
            if (IsOwner ?? false)
            {
                target.Owner = target.FileInfo.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
            }
            if (IsInherited ?? false)
            {
                target.Inherited = !target.FileInfo.GetAccessControl().AreAccessRulesProtected;
            }
            if (IsAttributes ?? false)
            {
                target.Attributes = MonitorFunctions.GetAttributes(target.Path);
            }
            if (IsMD5Hash ?? false)
            {
                target.MD5Hash = MonitorFunctions.GetHash(target.Path, MD5.Create());
            }
            if (IsSHA256Hash ?? false)
            {
                target.SHA256Hash = MonitorFunctions.GetHash(target.Path, SHA256.Create());
            }
            if (IsSHA512Hash ?? false)
            {
                target.SHA512Hash = MonitorFunctions.GetHash(target.Path, SHA512.Create());
            }
            if (IsSize ?? false)
            {
                target.Size = target.FileInfo.Length;
            }




        }
    }
}
