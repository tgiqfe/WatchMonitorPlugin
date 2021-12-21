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
    internal class CompareDirectory
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
        public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool Success { get; set; }
        public int? _MaxDepth { get; set; }

        public Dictionary<string, string> Propeties = null;




        private int _serial = 0;

        private MonitorTargetPair CreateMonitorTargetPair(MonitorTarget targetA, MonitorTarget targetB)
        {
            return new MonitorTargetPair(targetA, targetB)
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

        public void MainProcess()
        {
            //  MaxDepth無指定の場合は[5]をセット
            _MaxDepth ??= 5;

            var dictionary = new Dictionary<string, string>();
            dictionary["directoryA"] = _PathA;
            dictionary["directoryB"] = _PathB;
            this.Success = true;

            Success &= RecursiveTree(
                new MonitorTarget(PathType.Directory, _PathA, "directoryA"),
                new MonitorTarget(PathType.Directory, _PathB, "directoryB"),
                dictionary,
                0);



            this.Propeties = dictionary;
        }

        private bool RecursiveTree(MonitorTarget targetA, MonitorTarget targetB, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = true;

            _serial++;
            targetA.CheckExists();
            targetB.CheckExists();

            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                dictionary[$"{_serial}_directoryA_Exists"] = targetA.Path;
                dictionary[$"{_serial}_directoryB_Exists"] = targetB.Path;

                var targetPair = CreateMonitorTargetPair(targetA, targetB);
                ret &= targetPair.CheckDirectory(dictionary, _serial, depth);

                if (depth < _MaxDepth)
                {
                    foreach (string childPathA in System.IO.Directory.GetFiles(targetA.Path))
                    {
                        _serial++;
                        string childPathB = Path.Combine(targetB.Path, Path.GetFileName(childPathA));
                        MonitorTarget targetA_leaf = new MonitorTarget(PathType.File, childPathA, "fileA");
                        MonitorTarget targetB_leaf = new MonitorTarget(PathType.File, childPathB, "fileB");
                        targetA_leaf.CheckExists();
                        targetB_leaf.CheckExists();

                        if (targetB_leaf.Exists ?? false)
                        {
                            dictionary[$"{_serial}_fileA_Exists"] = childPathA;
                            dictionary[$"{_serial}_fileB_Exists"] = childPathB;

                            var targetPair_leaf = CreateMonitorTargetPair(targetA_leaf, targetB_leaf);
                            ret &= targetPair_leaf.CheckFile(dictionary, _serial);
                        }
                        else
                        {
                            dictionary[$"{_serial}_fileB_NotExists"] = childPathB;
                            ret = false;
                        }
                    }
                    foreach (string childPath in System.IO.Directory.GetDirectories(targetA.Path))
                    {
                        ret &= RecursiveTree(
                            new MonitorTarget(PathType.Directory, childPath, "directoryA"),
                            new MonitorTarget(PathType.Directory, Path.Combine(targetB.Path, Path.GetFileName(childPath), "directoryB")),
                            dictionary,
                            depth + 1);
                    }
                }
            }
            else
            {
                if (!targetA.Exists ?? false)
                {
                    dictionary[$"{_serial}_directoryA_NotExists"] = targetA.Path;
                    ret = false;
                }
                if (!targetB.Exists ?? false)
                {
                    dictionary[$"{_serial}_directoryB_NotExists"] = targetB.Path;
                    ret = false;
                }
            }
            return ret;
        }
    }
}
