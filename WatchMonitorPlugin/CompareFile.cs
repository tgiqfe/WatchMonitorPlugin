using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;


namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class CompareFile
    {
        public string _PathA { get; set; }
        public string _PathB { get; set; }

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

        protected bool Success { get; set; }
        private int _serial = 1;

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

            MonitoredTarget targetA = CreateForFile(_PathA, "fileA");
            MonitoredTarget targetB = CreateForFile(_PathB, "fileB");
            targetA.CheckExists();
            targetB.CheckExists();

            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                targetA.CheckFile();
                targetB.CheckFile();

                if (_IsAttributes ?? false)
                {
                    dictionary[$"{targetA.PathTypeName}_Attributes_{_serial}"] = MonitorFunctions.ToReadable(targetA.Attributes);
                    dictionary[$"{targetA.PathTypeName}_Attributes_{_serial}"] = MonitorFunctions.ToReadable(targetB.Attributes);
                    Success &= targetA.Attributes.SequenceEqual(targetB.Attributes);
                }



            }

        }
    }
}
