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
    internal class WatchFile : WatchBase
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
        //public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool _Begin { get; set; }
        public bool Success { get; set; }


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
                //IsChildCount = _IsChildCount,
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
            //if(_IsChildCount != null){collection.IsChildCount = _IsChildCount;}
            //if(_IsRegistryType != null){collection.IsRegistryType = _IsRegistryType;}
            if (_IsDateOnly != null) { collection.IsDateOnly = _IsDateOnly; }
            if (_IsTimeOnly != null) { collection.IsTimeOnly = _IsTimeOnly; }

            return collection;
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            var collection = _Begin ?
                CreateMonitorTargetCollection() :
                MergeMonitorTargetCollection(MonitorTargetCollection.Load(GetWatchDBDirectory(), _Id));

            //  Begin=trueあるいは、collectionが空っぽ(今回初回watch)の場合、Successをtrue
            this.Success = _Begin || (collection.Count == 0);

            foreach (string path in _Path)
            {
                _serial++;
                dictionary[$"{_serial}_file"] = path;

                MonitorTarget target = new MonitorTarget(PathType.File, path, "file");
                target.CheckExists();
                if(target.Exists ?? false)
                {
                    Success |= collection.CheckFile(target, dictionary, _serial);
                }
                collection.SetMonitorTarget(path, target);
            }
            collection.Save(GetWatchDBDirectory(), _Id);





            Propeties = dictionary;
        }
    }
}
