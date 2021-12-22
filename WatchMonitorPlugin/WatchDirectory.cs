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

        private MonitorTargetCollection CreateMonitorTargetCollection()
        {
            return new MonitorTargetCollection()
            {
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
                IsChildCount = _IsChildCount,
                //IsRegistryType = _IsRegistryType,
                IsDateOnly = _IsDateOnly,
                IsTimeOnly = _IsTimeOnly,
            };
        }

        private MonitorTargetCollection MergeMonitorTargetCollection(MonitorTargetCollection collection)
        {
            if (_IsCreationTime != null) { collection.IsCreationTime = _IsCreationTime; }
            if (_IsLastWriteTime != null) { collection.IsLastWriteTime = _IsLastWriteTime; }
            if (_IsLastAccessTime != null) { collection.IsLastAccessTime = _IsLastAccessTime; }
            if (_IsAccess != null) { collection.IsAccess = _IsAccess; }
            if (_IsOwner != null) { collection.IsOwner = _IsOwner; }
            if (_IsInherited != null) { collection.IsInherited = _IsInherited; }
            if (_IsAttributes != null) { collection.IsAttributes = _IsAttributes; }
            if (_IsMD5Hash != null) { collection.IsMD5Hash = _IsMD5Hash; }
            if (_IsSHA256Hash != null) { collection.IsSHA256Hash = _IsSHA256Hash; }
            if (_IsSHA512Hash != null) { collection.IsSHA512Hash = _IsSHA512Hash; }
            if (_IsSize != null) { collection.IsSize = _IsSize; }
            if(_IsChildCount != null){collection.IsChildCount = _IsChildCount;}
            //if(_IsRegistryType != null){collection.IsRegistryType = _IsRegistryType;}
            if (_IsDateOnly != null) { collection.IsDateOnly = _IsDateOnly; }
            if (_IsTimeOnly != null) { collection.IsTimeOnly = _IsTimeOnly; }

            if (collection.PrevTargetPaths?.Length > 0)
            {
                var tempPaths = collection.PrevTargetPaths.ToList();
                if (_Path?.Length > 0)
                {
                    tempPaths.AddRange(_Path);
                }
                this._Path = tempPaths.Distinct().ToArray();
            }

            return collection;
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            var collection = _Begin ?
                CreateMonitorTargetCollection() :
                MergeMonitorTargetCollection(MonitorTargetCollection.Load(GetWatchDBDirectory(), _Id));
            this._MaxDepth ??= 5;
            this.Success = _Begin || (collection.Targets.Count == 0);

            foreach (string path in _Path)
            {
                Success |= RecursiveTree(
                    collection,
                    new MonitorTarget(PathType.Directory, path, "directory"),
                    dictionary,
                    0);
            }
            foreach (string uncheckedPath in collection.GetUncheckedKeys())
            {
                _serial++;
                dictionary[$"{_serial}_remove"] = uncheckedPath;
                collection.Targets.Remove(uncheckedPath);
                Success = true;
            }
            collection.PrevTargetPaths = _Path;
            collection.Save(GetWatchDBDirectory(), _Id);



            Propeties = dictionary;
        }

        private bool RecursiveTree(MonitorTargetCollection collection, MonitorTarget target, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"{_serial}_directory"] = target.Path;
            target.CheckExists();
            if (target.Exists ?? false)
            {
                ret |= collection.CheckDirectory(target, dictionary, _serial, depth);
            }
            collection.SetMonitorTarget(target.Path, target);

            if (depth < _MaxDepth && (target.Exists ?? false))
            {
                foreach (string filePath in System.IO.Directory.GetFiles(target.Path))
                {
                    _serial++;
                    dictionary[$"{_serial}_file"] = filePath;

                    MonitorTarget target_leaf = new MonitorTarget(PathType.File, filePath, "file");
                    target_leaf.CheckExists();
                    ret |= collection.CheckFile(target_leaf, dictionary, _serial);
                    collection.SetMonitorTarget(filePath, target_leaf);
                }
                foreach (string dirPath in System.IO.Directory.GetDirectories(target.Path))
                {
                    ret |= RecursiveTree(
                        collection,
                        new MonitorTarget(PathType.Directory, dirPath, "directory"),
                        dictionary,
                        depth + 1);
                }
            }

            return ret;
        }
    }
}
