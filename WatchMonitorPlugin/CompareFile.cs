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

        public bool Success { get; set; }

        public Dictionary<string, string> Propeties = null;


        private int _serial = 1;

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


        public void MainProcess2()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary["fileA"] = _PathA;
            dictionary["fileB"] = _PathB;
            this.Success = true;

            MonitorTarget targetA = new MonitorTarget(PathType.File, _PathA, "fileA");
            MonitorTarget targetB = new MonitorTarget(PathType.File, _PathB, "fileB");
            targetA.CheckExists();
            targetB.CheckExists();

            MonitorTargetPair targetPair = new MonitorTargetPair(targetA, targetB)
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

            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                dictionary["fileA_Exists"] = _PathA;
                dictionary["fileB_Exists"] = _PathB;
                Success &= targetPair.CheckFile(dictionary, _serial);
            }
            else
            {
                if (!targetA.Exists ?? false)
                {
                    dictionary["fileA_NotExists"] = _PathA;
                    Success = false;
                }
                if (!targetB.Exists ?? false)
                {
                    dictionary["fileB_NotExists"] = _PathB;
                    Success = false;
                }
            }
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary["fileA"] = _PathA;
            dictionary["fileB"] = _PathB;
            this.Success = true;

            MonitorTarget targetA = CreateForFile(_PathA, "fileA");
            MonitorTarget targetB = CreateForFile(_PathB, "fileB");
            targetA.CheckExists();
            targetB.CheckExists();

            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                dictionary["fileA_Exists"] = _PathA;
                dictionary["fileB_Exists"] = _PathB;
                Success &= CompareFunctions.CheckFile(targetA, targetB, dictionary, _serial);
            }
            else
            {
                if (!targetA.Exists ?? false)
                {
                    dictionary["fileA_NotExists"] = _PathA;
                    Success = false;
                }
                if (!targetB.Exists ?? false)
                {
                    dictionary["fileB_NotExists"] = _PathB;
                    Success = false;
                }
            }






            this.Propeties = dictionary;
        }
    }
}
