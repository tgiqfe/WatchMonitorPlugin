using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib.Monitor;
using IO.Lib;
using Microsoft.Win32;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WatchRegistry : WatchBase
    {
        public string _Id { get; set; }
        public string[] _Path { get; set; }
        public string[] _Name { get; set; }

        //public bool? _IsCreationTime { get; set; }
        //public bool? _IsLastWriteTime { get; set; }
        //public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        //public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        //public bool? _IsSize { get; set; }
        public bool? _IsChildCount { get; set; }
        public bool? _IsRegistryType { get; set; }

        //public bool? _IsDateOnly { get; set; }
        //public bool? _IsTimeOnly { get; set; }

        public bool _Begin { get; set; }
        public bool Success { get; set; }
        public int? _MaxDepth { get; set; }


        public Dictionary<string, string> Propeties = null;


        private int _serial = 0;

        private MonitorTarget CreateForRegistryKey(string path, RegistryKey key, string pathTypeName)
        {
            return new MonitorTarget(PathType.Registry, path, key)
            {
                PathTypeName = pathTypeName,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }

        private MonitorTarget CreateForRegistryValue(string path, RegistryKey key, string name, string pathTypeName)
        {
            return new MonitorTarget(PathType.Registry, path, key, name)
            {
                PathTypeName = pathTypeName,
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                IsRegistryType = _IsRegistryType,
            };
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            var collection = _Begin ?
                new MonitorTargetCollection() :
                MonitorTargetCollection.Load(GetWatchDBDirectory(), _Id);
            this._MaxDepth ??= 5;
            this.Success = _Begin || (collection.Count == 0);

            if (_Name?.Length > 0)
            {
                string keyPath = _Path[0];
                using (RegistryKey regKey = RegistryControl.GetRegistryKey(keyPath, false, false))
                {
                    foreach (string name in _Name)
                    {
                        _serial++;
                        dictionary[$"{_serial}_registry"] = regKey.Name + "\\" + name;
                        MonitorTarget target_dbfs =
                            collection.GetMonitorTarget(keyPath, name) ?? CreateForRegistryValue(keyPath, regKey, name, "registry");

                        MonitorTarget target_monitorfs = CreateForRegistryValue(keyPath, regKey, name, "registry");
                        target_monitorfs.Merge_is_Property(target_dbfs);
                        target_monitorfs.CheckExists();

                        if (target_monitorfs.Exists ?? false)
                        {
                            Success |= WatchFunctions.CheckRegistryValue(target_monitorfs, target_dbfs, dictionary, _serial);
                        }
                        collection.SetMonitorTarget(keyPath, name, target_monitorfs);
                    }
                }
            }
            else
            {
                foreach (string path in _Path)
                {
                    using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
                    {
                        Success |= RecursiveTree(
                            collection,
                            CreateForRegistryKey(path, regKey, "registry"),
                            dictionary,
                            0);
                    }
                }
                foreach (string uncheckedPath in collection.GetUncheckedKeys())
                {
                    _serial++;
                    dictionary[$"{_serial}_remove"] = uncheckedPath;
                    collection.Remove(uncheckedPath);
                    Success = true;
                }
            }
            collection.Save(GetWatchDBDirectory(), _Id);



            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitorTargetCollection collection, MonitorTarget target_monitor, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"{_serial}_registry"] = target_monitor.Path;
            MonitorTarget target_db =
                collection.GetMonitorTarget(target_monitor.Path) ?? CreateForRegistryKey(target_monitor.Path, target_monitor.Key, "registry");

            target_monitor.Merge_is_Property(target_db);
            target_monitor.CheckExists();

            if (target_monitor.Exists ?? false)
            {
                ret |= WatchFunctions.CheckRegistrykey(target_monitor, target_db, dictionary, _serial, depth);
            }
            collection.SetMonitorTarget(target_monitor.Path, target_monitor);

            if (depth < _MaxDepth && (target_monitor.Exists ?? false))
            {
                foreach (string name in target_monitor.Key.GetValueNames())
                {
                    _serial++;
                    dictionary[$"{_serial}_registry"] = target_monitor.Path + "\\" + name;
                    MonitorTarget target_db_leaf =
                        collection.GetMonitorTarget(target_monitor.Path, name) ?? CreateForRegistryValue(target_monitor.Path, target_monitor.Key, name, "registry");

                    MonitorTarget target_monitor_leaf = CreateForRegistryValue(target_monitor.Path, target_monitor.Key, name, "registry");
                    target_monitor_leaf.Merge_is_Property(target_db_leaf);
                    target_monitor_leaf.CheckExists();

                    ret |= WatchFunctions.CheckRegistryValue(target_monitor_leaf, target_db_leaf, dictionary, _serial);
                    collection.SetMonitorTarget(target_monitor_leaf.Path, name, target_monitor_leaf);
                }
                foreach (string keyPath in target_monitor.Key.GetSubKeyNames())
                {
                    using (RegistryKey subRegKey = target_monitor.Key.OpenSubKey(keyPath, false))
                    {
                        ret |= RecursiveTree(
                            collection,
                            CreateForRegistryKey(Path.Combine(target_monitor.Path, keyPath), subRegKey, "registry"),
                            dictionary,
                            depth + 1);
                    }
                }
            }

            return ret;
        }
    }
}
