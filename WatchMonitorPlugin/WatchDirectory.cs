using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib.Monitor;
using IO.Lib;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WatchDirectory
    {
        public string _ID { get; set; }
        public string[] _Path { get; set; }

        public bool? _IsCreationTime { get; set; }
        public bool? _IsLastWriteTime { get; set; }
        public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        public bool? _IsSize { get; set; }
        public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool _Begin { get; set; }
        public bool Success { get; set; }
        public int? _MaxDepth { get; set; }
        private int _serial;
        private string _checkingPath;



        private string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
        public Dictionary<string, string> Propeties = null;

        private MonitorTarget CreateForFile(string path, string pathTypeName)
        {
            return new MonitorTarget(PathType.File, path)
            {
                PathTypeName = pathTypeName,
                IsCreationTime = _IsCreationTime,
                IsLastWriteTime = _IsLastWriteTime,
                IsLastAccessTime = _IsLastAccessTime,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsAttributes = _IsAttributes,
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                IsSize = _IsSize,
                IsDateOnly = _IsDateOnly,
                IsTimeOnly = _IsTimeOnly,
            };
        }

        private MonitorTarget CreateForDirectory(string path, string pathTypeName)
        {
            return new MonitorTarget(PathType.Directory, path)
            {
                PathTypeName = pathTypeName,
                IsCreationTime = _IsCreationTime,
                IsLastWriteTime = _IsLastWriteTime,
                IsLastAccessTime = _IsLastAccessTime,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsAttributes = _IsAttributes,
                IsChildCount = _IsChildCount,
                IsDateOnly = _IsDateOnly,
                IsTimeOnly = _IsTimeOnly,
            };
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            var collection = MonitorTargetCollection.Load(dbDir, _ID);
            _MaxDepth ??= 5;

            foreach (string path in _Path)
            {
                _checkingPath = path;
                Success |= RecursiveTree(collection, dictionary, path, 0);
            }
            foreach (string uncheckedPath in collection.GetUncheckedKeys())
            {
                _serial++;
                dictionary[$"{_serial}_remove"] = uncheckedPath;
                collection.Remove(uncheckedPath);
                Success = true;
            }
            collection.Save(dbDir, _ID);




            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitorTargetCollection collection, Dictionary<string, string> dictionary, string path, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"{_serial}_directory"] = (path == _checkingPath) ?
                path :
                path.Replace(_checkingPath, "");
            MonitorTarget target_db = _Begin ?
                CreateForDirectory(path, "directory") :
                collection.GetMonitoredTarget(path) ?? CreateForDirectory(path, "directory");

            MonitorTarget target_monitor = CreateForDirectory(path, "directory");
            target_monitor.Merge_is_Property(target_db);
            target_monitor.CheckExists();

            if (target_monitor.Exists ?? false)
            {
                ret |= WatchFunctions.CheckDirectory(target_monitor, target_db, dictionary, _serial, depth);
            }
            collection.SetMonitoredTarget(path, target_monitor);

            if (depth < _MaxDepth && (target_monitor.Exists ?? false))
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    _serial++;
                    dictionary[$"{_serial}_file"] = filePath.Replace(_checkingPath, "");
                    MonitorTarget target_db_leaf = _Begin ?
                        CreateForFile(filePath, "file") :
                        collection.GetMonitoredTarget(filePath) ?? CreateForFile(path, "file");

                    MonitorTarget target_monitor_leaf = CreateForDirectory(filePath, "file");
                    target_monitor_leaf.Merge_is_Property(target_db_leaf);
                    target_monitor_leaf.CheckExists();

                    if (target_monitor_leaf.Exists ?? false)
                    {
                        ret |= WatchFunctions.CheckFile(target_monitor_leaf, target_db_leaf, dictionary, _serial);
                    }
                    collection.SetMonitoredTarget(filePath, target_monitor_leaf);
                }
                foreach (string dirPath in Directory.GetDirectories(path))
                {
                    ret |= RecursiveTree(collection, dictionary, dirPath, depth + 1);
                }
            }

            return ret;
        }
    }
}
