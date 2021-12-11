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
        public string _Serial { get; set; }
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

        private MonitoredTarget CreateForFile(string path)
        {
            return new MonitoredTarget(IO.Lib.PathType.File, path)
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
                IsDateOnly = _IsDateOnly,
                IsTimeOnly = _IsTimeOnly,
            };
        }

        public void MainProcess()
        {
            foreach (string path in _Path)
            {

            }
        }
    }
}
