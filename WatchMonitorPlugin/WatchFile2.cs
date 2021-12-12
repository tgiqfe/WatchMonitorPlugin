using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;
using IO.Lib;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WatchFile2
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
        //public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool _Begin { get; set; }
        protected bool Success { get; set; }
        private int _serial;

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

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            this.Success = true;

            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var collection = MonitoredTargetCollection.Load(dbDir, _ID);


            foreach (string path in _Path)
            {
                _serial++;
                dictionary[$"file_{_serial}"] = path;
                MonitoredTarget target_db = _Begin ?
                    CreateForFile(path, "file") :
                    collection.GetMonitoredTarget(path) ?? CreateForFile(path, "file");

                MonitoredTarget target_monitor = CreateForFile(path, "file");
                target_monitor.CheckExists();
                if (target_monitor.Exists ?? false)
                {
                    target_monitor.CheckFile();

                    if ((_IsAttributes ?? false) || (target_db.IsAttributes ?? false))
                    {
                        bool ret = !target_monitor.Attributes.SequenceEqual(target_db.Attributes);
                        if (target_db.Attributes != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Attributes_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    MonitorAttributes.ToReadable(target_db.Attributes),
                                    MonitorAttributes.ToReadable(target_monitor.Attributes)) :
                                MonitorAttributes.ToReadable(target_monitor.Attributes);
                        }
                        target_db.Attributes = target_monitor.Attributes;
                    }
                }
                else
                {
                    target_db.CreationTime = null;
                    target_db.LastWriteTime = null;
                    target_db.LastAccessTime = null;
                    target_db.Access = null;
                    target_db.Owner = null;
                    target_db.Inherited = null;
                    target_db.Attributes = null;
                    target_db.MD5Hash = null;
                    target_db.SHA256Hash = null;
                    target_db.SHA512Hash = null;
                    target_db.Size = null;
                    target_db.ChildCount = null;
                    target_db.RegistryType = null;
                }








            }
        }
    }
}
