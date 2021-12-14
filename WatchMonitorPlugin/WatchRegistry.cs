using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;
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


        private MonitoredTarget CreateForRegistryKey(string path, RegistryKey key, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, path, key)
            {
                PathTypeName = pathTypeName,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }

        private MonitoredTarget CreateForRegistryValue(string path, RegistryKey key, string name, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, path, key, name)
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
                        dictionary[$"registry_{_serial}"] = keyPath + "\\" + name;
                        MonitoredTarget target_db = _Begin ?
                            CreateForRegistryValue(keyPath, regKey, name, "registry") :
                            collection.GetMonitoredTarget(keyPath, name) ?? CreateForRegistryValue(keyPath, regKey, name, "registry");

                        MonitoredTarget target_monitor = CreateForRegistryValue(keyPath, regKey, name, "registry");
                        target_monitor.Merge_is_Property(target_db);
                        target_monitor.CheckExists();

                        if (target_monitor.Exists ?? false)
                        {
                            Success |= WatchFunctions.CheckRegistryValue(target_monitor, target_db, dictionary, _serial);
                        }
                        collection.SetMonitoredTarget(keyPath, name, target_monitor);
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
                        _serial++;
                        dictionary[$"registry_{_serial}"] = (path == _checkingPath) ?
                            path :
                            path.Replace(_checkingPath, "");
                        MonitoredTarget target_db = _Begin ?
                            CreateForRegistryKey(path, regKey,  "registry") :
                            collection.GetMonitoredTarget(path) ?? CreateForRegistryKey(path, regKey, "registry");

                        MonitoredTarget target_monitor = CreateForRegistryKey(path, regKey, "registry");
                        target_monitor.Merge_is_Property(target_db);
                        target_monitor.CheckExists();

                        if(target_monitor.Exists ?? false)
                        {
                            Success |= WatchFunctions.CheckRegistrykey(target_monitor, target_db, dictionary, _serial, 0);


                            //  再起処理が必要



                        }
                    }
                }
            }
            collection.Save(dbDir, _ID);
        }
    }
}
