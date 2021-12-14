﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitoredTargetCollection : Dictionary<string, MonitoredTarget>
    {
        private List<string> _CheckedKeys = new List<string>();

        const string REGPATH_PREFIX = "[registry]";

        public MonitoredTargetCollection() { }

        #region Get/Set MonitoredTarget

        public MonitoredTarget GetMonitoredTarget(string path)
        {
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(path, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ? null : this[matchKey];
        }

        public MonitoredTarget GetMonitoredTarget(RegistryKey regKey)
        {
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(regKey.Name, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ? null : this[matchKey];
        }

        public MonitoredTarget GetMonitoredTarget(RegistryKey regKey, string name)
        {
            string regPath = REGPATH_PREFIX + regKey.Name + "\\" + name;
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(regPath, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ? null : this[matchKey];
        }

        public void SetMonitoredTarget(string path, MonitoredTarget target)
        {
            target.FullPath = path;
            this[path] = target;
            this._CheckedKeys.Add(path);
        }

        public void SetMonitoredTarget(RegistryKey regKey, MonitoredTarget target)
        {
            target.FullPath = regKey.Name;
            this[regKey.Name] = target;
            this._CheckedKeys.Add(regKey.Name);
        }

        public void SetMonitoredTarget(RegistryKey regKey, string name, MonitoredTarget target)
        {
            string regPath = REGPATH_PREFIX + regKey.Name + "\\" + name;
            target.FullPath = regPath;
            this[regPath] = target;
            this._CheckedKeys.Add(regPath);
        }

        #endregion

        public IEnumerable<string> GetUncheckedKeys()
        {
            return this.Keys.Where(x => !_CheckedKeys.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase)));
        }

        #region Load/Save

        public static MonitoredTargetCollection Load(string dbDir, string id)
        {
            try
            {
                using (var sr = new StreamReader(Path.Combine(dbDir, id), Encoding.UTF8))
                {
                    return JsonSerializer.Deserialize<MonitoredTargetCollection>(sr.ReadToEnd());
                }
            }
            catch { }
            return new MonitoredTargetCollection();
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
    }
}
