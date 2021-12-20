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

        private MonitorTarget CreateForFile(string path, string pathTypeName)
        {
            return new MonitorTarget(IO.Lib.PathType.File, path)
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

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            var collection = _Begin ?
                new MonitorTargetCollection() :
                MonitorTargetCollection.Load(GetWatchDBDirectory(), _Id);

            //  Begin=trueあるいは、collectionが空っぽ(今回初回watch)の場合、Successをtrue
            this.Success = _Begin || (collection.Count == 0);

            foreach (string path in _Path)
            {
                _serial++;
                dictionary[$"{_serial}_file"] = path;
                MonitorTarget target_db = collection.GetMonitorTarget(path) ?? CreateForFile(path, "file");

                MonitorTarget target_monitor = CreateForFile(path, "file");
                target_monitor.Merge_is_Property(target_db);
                target_monitor.CheckExists();

                if (target_monitor.Exists ?? false)
                {
                    Success |= WatchFunctions.CheckFile(target_monitor, target_db, dictionary, _serial);
                }
                collection.SetMonitorTarget(path, target_monitor);
            }
            collection.Save(GetWatchDBDirectory(), _Id);


            Propeties = dictionary;
        }
    }
}
