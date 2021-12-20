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
    internal class WatchDirectory : WatchBase
    {
        public string _Id { get; set; }
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


        public Dictionary<string, string> Propeties = null;


        private int _serial = 0;

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
            var collection = _Begin ?
                new MonitorTargetCollection() :
                MonitorTargetCollection.Load(GetWatchDBDirectory(), _Id);
            this._MaxDepth ??= 5;
            this.Success = _Begin || (collection.Count == 0);

            foreach (string path in _Path)
            {
                Success |= RecursiveTree(
                    collection,
                    CreateForDirectory(path, "directory"),
                    dictionary,
                    0);
            }
            foreach (string uncheckedPath in collection.GetUncheckedKeys())
            {
                _serial++;
                dictionary[$"{_serial}_remove"] = uncheckedPath;
                collection.Remove(uncheckedPath);
                Success = true;
            }
            collection.Save(GetWatchDBDirectory(), _Id);



            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitorTargetCollection collection, MonitorTarget target_monitor, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"{_serial}_directory"] = target_monitor.Path;
            MonitorTarget target_db =
                collection.GetMonitorTarget(target_monitor.Path) ?? CreateForDirectory(target_monitor.Path, "directory");

            target_monitor.Merge_is_Property(target_db);
            target_monitor.CheckExists();

            if (target_monitor.Exists ?? false)
            {
                ret |= WatchFunctions.CheckDirectory(target_monitor, target_db, dictionary, _serial, depth);
            }
            collection.SetMonitorTarget(target_monitor.Path, target_monitor);

            if (depth < _MaxDepth && (target_monitor.Exists ?? false))
            {
                foreach (string filePath in System.IO.Directory.GetFiles(target_monitor.Path))
                {
                    _serial++;
                    dictionary[$"{_serial}_file"] = filePath;
                    MonitorTarget target_db_leaf =
                        collection.GetMonitorTarget(filePath) ?? CreateForFile(filePath, "file");

                    MonitorTarget target_monitor_leaf = CreateForFile(filePath, "file");
                    target_monitor_leaf.Merge_is_Property(target_db_leaf);
                    target_monitor_leaf.CheckExists();

                    ret |= WatchFunctions.CheckFile(target_monitor_leaf, target_db_leaf, dictionary, _serial);
                    collection.SetMonitorTarget(filePath, target_monitor_leaf);
                }
                foreach (string dirPath in System.IO.Directory.GetDirectories(target_monitor.Path))
                {
                    ret |= RecursiveTree(
                        collection,
                        CreateForDirectory(dirPath, "directory"),
                        dictionary,
                        depth + 1);
                }
            }

            return ret;
        }
    }
}
