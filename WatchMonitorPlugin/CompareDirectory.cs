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
        private int _serial = 0;
        private string _checkingPathA;
        private string _checkingPathB;


        public Dictionary<string, string> Propeties = null;



        private MonitorTarget CreateForFile(string path, string pathTypeName)
        {
            return new MonitorTarget(PathType.File, path)
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

        private MonitorTarget CreateForDirectory(string path, string pathTypeName)
        {
            return new MonitorTarget(PathType.File, path)
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
            dictionary["directoryA"] = _PathA;
            dictionary["directoryB"] = _PathB;
            this.Success = true;
            _MaxDepth ??= 5;

            _checkingPathA = _PathA;
            _checkingPathB = _PathB;
            Success &= RecursiveTree(_PathA, _PathB, dictionary, 0);


            this.Propeties = dictionary;
        }

        private bool RecursiveTree(string pathA, string pathB, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = true;

            _serial++;
            MonitorTarget targetA = CreateForDirectory(pathA, "directoryA");
            MonitorTarget targetB = CreateForDirectory(pathB, "directoryB");
            targetA.CheckExists();
            targetB.CheckExists();
            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                dictionary[$"directoryA_Exists_{_serial}"] = _PathA;
                dictionary[$"directoryB_Exists_{_serial}"] = _PathB;
                ret &= CompareFunctions.CheckDirectory(targetA, targetB, dictionary, _serial, depth);

                if (depth < _MaxDepth)
                {
                    foreach (string childPathA in System.IO.Directory.GetFiles(pathA))
                    {
                        _serial++;
                        string childPathB = Path.Combine(pathB, Path.GetFileName(childPathA));
                        MonitorTarget targetA_leaf = CreateForFile(childPathA, "file");
                        MonitorTarget targetB_leaf = CreateForFile(childPathB, "file");
                        targetA_leaf.CheckExists();
                        targetB_leaf.CheckExists();

                        if (targetB_leaf.Exists ?? false)
                        {
                            ret &= CompareFunctions.CheckFile(targetA_leaf, targetB_leaf, dictionary, _serial);
                        }
                        else
                        {
                            dictionary[$"fileB_NotExists_{_serial}"] = childPathB;
                            ret = false;
                        }
                    }
                    foreach (string childPath in System.IO.Directory.GetDirectories(pathA))
                    {
                        RecursiveTree(
                            childPath,
                            Path.Combine(pathB, Path.GetFileName(childPath)),
                            dictionary,
                            _serial);
                    }
                }
            }
            else
            {
                if (!targetA.Exists ?? false)
                {
                    dictionary[$"directoryA_NotExists_{_serial}"] = pathA;
                    ret = false;
                }
                if (!targetB.Exists ?? false)
                {
                    dictionary[$"directoryB_NotExists_{_serial}"] = pathB;
                    ret = false;
                }
            }

            return ret;
        }


    }
}
