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
    internal class WatchRegistry
    {
        public string _ID { get; set; }
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
        private int _serial;
        private string _checkingPath;



        private string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
        public Dictionary<string, string> Propeties = null;


        private MonitoredTarget CreateForRegistryKey(RegistryKey key, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, key)
            {
                PathTypeName = pathTypeName,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }

        private MonitoredTarget CreateForRegistryValue(RegistryKey key, string name, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, key, name)
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
            var collection = MonitoredTargetCollection.Load(dbDir, _ID);
            _MaxDepth ??= 5;

            if (_Name?.Length > 0)
            {
                string keyPath = _Path[0];
                using (RegistryKey regKey = RegistryControl.GetRegistryKey(keyPath, false, false))
                {
                    foreach (string name in _Name)
                    {
                        _serial++;
                        dictionary[$"{_serial}_registry"] = regKey.Name + "\\" + name;
                        MonitoredTarget target_dbfs = _Begin ?
                            CreateForRegistryValue(regKey, name, "registry") :
                            collection.GetMonitoredTarget(regKey, name) ?? CreateForRegistryValue(regKey, name, "registry");

                        MonitoredTarget target_monitorfs = CreateForRegistryValue(regKey, name, "registry");
                        target_monitorfs.Merge_is_Property(target_dbfs);
                        target_monitorfs.CheckExists();

                        if (target_monitorfs.Exists ?? false)
                        {
                            Success |= WatchFunctions.CheckRegistryValue(target_monitorfs, target_dbfs, dictionary, _serial);
                        }
                        collection.SetMonitoredTarget(regKey, name, target_monitorfs);
                    }
                }
            }
            else
            {
                foreach (string path in _Path)
                {
                    _checkingPath = path;
                    using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
                    {
                        Success |= RecursiveTree(collection, dictionary, regKey, 0);
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
            collection.Save(dbDir, _ID);



            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitoredTargetCollection collection, Dictionary<string, string> dictionary, RegistryKey regKey, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"{_serial}_registry"] = (regKey.Name == _checkingPath) ?
                regKey.Name :
                regKey.Name.Replace(_checkingPath, "");
            MonitoredTarget target_db = _Begin ?
                CreateForRegistryKey(regKey, "registry") :
                collection.GetMonitoredTarget(regKey) ?? CreateForRegistryKey(regKey, "registry");

            MonitoredTarget target_monitor = CreateForRegistryKey(regKey, "registry");
            target_monitor.Merge_is_Property(target_db);
            target_monitor.CheckExists();

            if (target_monitor.Exists ?? false)
            {
                ret |= WatchFunctions.CheckRegistrykey(target_monitor, target_db, dictionary, _serial, depth);
            }
            collection.SetMonitoredTarget(regKey, target_monitor);

            if (depth < _MaxDepth && (target_monitor.Exists ?? false))
            {
                foreach (string name in regKey.GetValueNames())
                {
                    _serial++;
                    dictionary[$"{_serial}_registry"] = (regKey.Name + "\\" + name).Replace(_checkingPath, "");
                    MonitoredTarget target_db_leaf = _Begin ?
                        CreateForRegistryValue(regKey, name, "registry") :
                        collection.GetMonitoredTarget(regKey, name) ?? CreateForRegistryValue(regKey, name, "registry");

                    MonitoredTarget target_monitor_leaf = CreateForRegistryValue(regKey, name, "registry");
                    target_monitor_leaf.Merge_is_Property(target_db_leaf);
                    target_monitor_leaf.CheckExists();

                    if (target_monitor_leaf.Exists ?? false)
                    {
                        ret |= WatchFunctions.CheckRegistryValue(target_monitor_leaf, target_db_leaf, dictionary, _serial);
                    }
                    collection.SetMonitoredTarget(regKey, name, target_monitor_leaf);
                }
                foreach (string keyPath in regKey.GetSubKeyNames())
                {
                    using (RegistryKey subRegKey = regKey.OpenSubKey(keyPath, false))
                    {
                        ret |= RecursiveTree(collection, dictionary, subRegKey, depth + 1);
                    }
                    
                }
            }

            return ret;
        }
    }
}
