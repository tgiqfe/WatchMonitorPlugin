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
                    //  CreationTime
                    if ((_IsCreationTime ?? false) || (target_db.IsCreationTime ?? false))
                    {
                        target_monitor.CheckCreationTime();
                        bool ret = target_monitor.CreationTime != target_db.CreationTime;
                        if (target_db.CreationTime != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_CreationTime_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.CreationTime,
                                    target_monitor.CreationTime) :
                                target_monitor.CreationTime;
                        }
                        Success |= ret;
                    }

                    //  LastWriteTime
                    if ((_IsLastWriteTime ?? false) || (target_db.IsLastWriteTime ?? false))
                    {
                        target_monitor.CheckLastWriteTime();
                        bool ret = target_monitor.LastWriteTime != target_db.LastWriteTime;
                        if (target_db.LastWriteTime != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_LastWriteTime_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.LastWriteTime,
                                    target_monitor.LastWriteTime) :
                                target_monitor.LastWriteTime;
                        }
                        Success |= ret;
                    }

                    //  LastAccessTime
                    if ((_IsLastAccessTime ?? false) || (target_db.IsLastAccessTime ?? false))
                    {
                        target_monitor.CheckLastAccessTime();
                        bool ret = target_monitor.LastAccessTime != target_db.LastAccessTime;
                        if (target_db.LastAccessTime != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_LastAccessTime_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.LastAccessTime,
                                    target_monitor.LastAccessTime) :
                                target_monitor.LastAccessTime;
                        }
                        Success |= ret;
                    }

                    //  Access
                    if ((_IsAccess ?? false) || (target_db.IsAccess ?? false))
                    {
                        target_monitor.CheckAccess();
                        bool ret = target_monitor.Access != target_db.Access;
                        if (target_db.Access != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Access_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.Access,
                                    target_monitor.Access) :
                                target_monitor.Access;
                        }
                        Success |= ret;
                    }

                    //  Owner
                    if ((_IsOwner ?? false) || (target_db.IsOwner ?? false))
                    {
                        target_monitor.CheckOwner();
                        bool ret = target_monitor.Owner != target_db.Owner;
                        if (target_db.Owner != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Owner_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.Owner,
                                    target_monitor.Owner) :
                                target_monitor.Owner;
                        }
                        Success |= ret;
                    }

                    //  Inherited
                    if ((_IsInherited ?? false) || (target_db.IsInherited ?? false))
                    {
                        target_monitor.CheckInherited();
                        bool ret = target_monitor.Inherited != target_db.Inherited;
                        if (target_db.Inherited != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Inherited_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.Inherited,
                                    target_monitor.Inherited) :
                                target_monitor.Inherited.ToString();
                        }
                        Success |= ret;
                    }

                    //  Attributes
                    if ((_IsAttributes ?? false) || (target_db.IsAttributes ?? false))
                    {
                        target_monitor.CheckAttributes();
                        bool ret = !target_monitor.Attributes.SequenceEqual(target_db.Attributes);
                        if (target_db.Attributes != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Attributes_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    MonitorFunctions.ToReadable(target_db.Attributes),
                                    MonitorFunctions.ToReadable(target_monitor.Attributes)) :
                                MonitorFunctions.ToReadable(target_monitor.Attributes);
                        }
                        Success |= ret;
                    }

                    //  MD5Hash
                    if ((_IsMD5Hash ?? false) || (target_db.IsMD5Hash ?? false))
                    {
                        target_monitor.CheckMD5Hash();
                        bool ret = target_monitor.MD5Hash != target_db.MD5Hash;
                        if (target_db.MD5Hash != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_MD5Hash_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.MD5Hash,
                                    target_monitor.MD5Hash) :
                                target_monitor.MD5Hash;
                        }
                        Success |= ret;
                    }

                    //  SHA256Hash
                    if ((_IsSHA256Hash ?? false) || (target_db.IsSHA256Hash ?? false))
                    {
                        target_monitor.CheckSHA256Hash();
                        bool ret = target_monitor.SHA256Hash != target_db.SHA256Hash;
                        if (target_db.SHA256Hash != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_SHA256Hash_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.SHA256Hash,
                                    target_monitor.SHA256Hash) :
                                target_monitor.SHA256Hash;
                        }
                        Success |= ret;
                    }

                    //  SHA512Hash
                    if ((_IsSHA512Hash ?? false) || (target_db.IsSHA512Hash ?? false))
                    {
                        target_monitor.CheckSHA512Hash();
                        bool ret = target_monitor.SHA512Hash != target_db.SHA512Hash;
                        if (target_db.SHA512Hash != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_SHA512Hash_{_serial}"] = ret ?
                                string.Format("{0} -> {1}",
                                    target_db.SHA512Hash,
                                    target_monitor.SHA512Hash) :
                                target_monitor.SHA512Hash;
                        }
                        Success |= ret;
                    }

                    //  Size
                    if ((_IsSize ?? false) || (target_db.IsSize ?? false))
                    {
                        target_monitor.CheckSize();
                        bool ret = target_monitor.Size != target_db.Size;
                        if (target_db.Size != null)
                        {
                            dictionary[$"{target_monitor.PathTypeName}_Size_{_serial}"] = ret ?
                                string.Format("{0}({1}) -> {2}({3})",
                                    target_db.Size,
                                    MonitorFunctions.ToReadable(target_db.Size ?? 0),
                                    target_monitor.Size,
                                    MonitorFunctions.ToReadable(target_monitor.Size ?? 0)) :
                                string.Format("{0}({1})",
                                    target_monitor.Size,
                                    MonitorFunctions.ToReadable(target_monitor.Size ?? 0));
                        }
                        Success |= ret;
                    }
                }
                collection.SetMonitoredTarget(path, target_monitor);
            }
            collection.Save(dbDir, _ID);
        }
    }
}
