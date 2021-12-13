using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WatchDir
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

        private MonitoredTarget CreateForFile(string path, string pathTypeName)
        {
            return new MonitoredTarget(IO.Lib.PathType.File, path)
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

        private MonitoredTarget CreateForDirectory(string path, string pathTypeName)
        {
            return new MonitoredTarget(IO.Lib.PathType.File, path)
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
            var collection = MonitoredTargetCollection.Load(dbDir, _ID);
            _MaxDepth ??= 5;

            foreach (string path in _Path)
            {
                Success |= RecursiveTree(collection, dictionary, path, 0);
            }
            collection.Save(dbDir, _ID);




            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitoredTargetCollection collection, Dictionary<string, string> dictionary, string path, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"directory_{_serial}"] = (path == _checkingPath) ?
                path :
                path.Replace(_checkingPath, "");
            MonitoredTarget target_db = _Begin ?
                CreateForFile(path, "directory") :
                collection.GetMonitoredTarget(path) ?? CreateForFile(path, "directory");

            MonitoredTarget target_monitor = CreateForFile(path, "directory");
            target_monitor.Merge_is_Property(target_db);
            target_monitor.CheckExists();

            if (target_monitor.Exists ?? false)
            {
                ret |= WatchFunctions.CheckDirectory(target_monitor, target_db, dictionary, _serial, depth);
            }
            collection.SetMonitoredTarget(path, target_monitor);

            if (depth < _MaxDepth)
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    _serial++;
                    dictionary[$"file_{_serial}"] = filePath.Replace(_checkingPath, "");
                    target_db = _Begin ?
                        CreateForFile(filePath, "file") :
                        collection.GetMonitoredTarget(filePath) ?? CreateForFile(path, "file");

                    target_monitor = CreateForDirectory(filePath, "file");
                    target_monitor.Merge_is_Property(target_db);
                    target_monitor.CheckExists();

                    if (target_monitor.Exists ?? false)
                    {
                        ret |= WatchFunctions.CheckFile(target_monitor, target_db, dictionary, _serial);
                    }
                    collection.SetMonitoredTarget(filePath, target_monitor);
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
