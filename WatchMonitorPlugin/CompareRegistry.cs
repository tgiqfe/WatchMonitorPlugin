using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib.Monitor;
using IO.Lib;
using Microsoft.Win32;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class CompareRegistry
    {
        public string _PathA { get; set; }
        public string _PathB { get; set; }
        public string _NameA { get; set; }
        public string _NameB { get; set; }

        //public bool? _IsCreationTime { get; set; }
        //public bool? _IsLastWriteTime { get; set; }
        //public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        //public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        //public bool? _IsSize { get; set; }
        public bool? _IsChildCount { get; set; }
        public bool? _IsRegistryType { get; set; }

        //public bool? _IsDateOnly { get; set; }
        //public bool? _IsTimeOnly { get; set; }

        public bool Success { get; set; }
        public int? _MaxDepth { get; set; }



        public Dictionary<string, string> Propeties = null;

        private int _serial = 0;

        private MonitorTargetPair CreateMonitorTargetPair(MonitorTarget targetA, MonitorTarget targetB)
        {
            return new MonitorTargetPair(targetA, targetB)
            {
                //IsCreationTime = _IsCreationTime,
                //IsLastWriteTime = _IsLastWriteTime,
                //IsLastAccessTime = _IsLastAccessTime,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                //IsAttributes = _IsAttributes,
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                //IsSize = _IsSize,
                IsChildCount = _IsChildCount,
                IsRegistryType = _IsRegistryType,
                //IsDateOnly = _IsDateOnly,
                //IsTimeOnly = _IsTimeOnly,
            };
        }

        public void MainProcess()
        {
            //  MaxDepth無指定の場合は[5]をセット
            _MaxDepth ??= 5;

            var dictionary = new Dictionary<string, string>();
            this.Success = true;

            if ((_NameA == null && _NameB != null) || (_NameA != null && _NameB == null))
            {
                //Manager.WriteLog(LogLevel.Error, "Failed parameter, Both name parameter required.");
                return;
            }

            if (_NameA != null && _NameB != null)
            {
                _serial++;
                using (RegistryKey keyA = RegistryControl.GetRegistryKey(_PathA, false, false))
                using (RegistryKey keyB = RegistryControl.GetRegistryKey(_PathB, false, false))
                {
                    MonitorTarget targetA = new MonitorTarget(PathType.Registry, _PathA, "registryA", keyA, _NameA);
                    MonitorTarget targetB = new MonitorTarget(PathType.Registry, _PathB, "registryB", keyB, _NameB);
                    targetA.CheckExists();
                    targetB.CheckExists();

                    if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
                    {
                        dictionary["registryA_Exists"] = _PathA + "\\" + _NameA;
                        dictionary["registryB_Exists"] = _PathB + "\\" + _NameB;

                        var targetPair = CreateMonitorTargetPair(targetA, targetB);
                        Success &= targetPair.CheckRegistryValue(dictionary, _serial);
                    }
                    else
                    {
                        if (!targetA.Exists ?? false)
                        {
                            dictionary["registryA_NotExists"] = _PathA;
                            Success = false;
                        }
                        if (!targetB.Exists ?? false)
                        {
                            dictionary["registryB_NotExists"] = _PathB;
                            Success = false;
                        }
                    }
                }
            }
            else
            {
                using (RegistryKey keyA = RegistryControl.GetRegistryKey(_PathA, false, false))
                using (RegistryKey keyB = RegistryControl.GetRegistryKey(_PathB, false, false))
                {
                    Success &= RecursiveTree(
                        new MonitorTarget(PathType.Registry, _PathA, "registryA", keyA),
                        new MonitorTarget(PathType.Registry, _PathB, "registryB", keyB),
                         dictionary,
                         0);
                }
            }





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
                dictionary[$"{_serial}_registryA_Exists"] = targetA.Path;
                dictionary[$"{_serial}_registryB_Exists"] = targetB.Path;
                ret &= CompareFunctions.CheckRegistryKey(targetA, targetB, dictionary, _serial, depth);

                if (depth < _MaxDepth)
                {
                    var leaves = targetA.Key.GetValueNames().ToList();
                    leaves.AddRange(targetB.Key.GetValueNames());
                    foreach (string childName in leaves.Distinct())
                    {
                        _serial++;
                        MonitorTarget targetA_leaf = new MonitorTarget(PathType.Registry, targetA.Path, "registryA", targetA.Key, childName);
                        MonitorTarget targetB_leaf = new MonitorTarget(PathType.Registry, targetB.Path, "registryB", targetB.Key, childName);
                        targetA_leaf.CheckExists();
                        targetB_leaf.CheckExists();

                        if ((targetA_leaf.Exists ?? false) && (targetB_leaf.Exists ?? false))
                        {
                            dictionary[$"{_serial}_registryA_Exists"] = targetA_leaf.Path + "\\" + targetA_leaf.Name;
                            dictionary[$"{_serial}_registryB_Exists"] = targetB_leaf.Path + "\\" + targetB_leaf.Name;
                            ret &= CompareFunctions.CheckRegistryValue(targetA_leaf, targetB_leaf, dictionary, _serial);
                        }
                        else
                        {
                            if(targetA_leaf.Exists ?? false)
                            {
                                dictionary[$"{_serial}_registryA_NotExists"] = targetA_leaf.Path + "\\" + targetA_leaf.Name;
                                ret = false;
                            }
                            if (targetB_leaf.Exists ?? false)
                            {
                                dictionary[$"{_serial}_registryB_NotExists"] = targetB_leaf.Path + "\\" + targetB_leaf.Name;
                                ret = false;
                            }
                        }
                    }

                    var containers = targetA.Key.GetSubKeyNames().ToList();
                    containers.AddRange(targetB.Key.GetSubKeyNames());
                    foreach (string keyPath in containers.Distinct())
                    {
                        using (RegistryKey subRegKeyA = targetA.Key.OpenSubKey(keyPath, false))
                        using (RegistryKey subRegKeyB = targetB.Key.OpenSubKey(keyPath, false))
                        {
                            ret &= RecursiveTree(
                                new MonitorTarget(PathType.Registry, Path.Combine(targetA.Path, keyPath), "registryA", subRegKeyA),
                                new MonitorTarget(PathType.Registry, Path.Combine(targetB.Path, keyPath), "registryB", subRegKeyB),
                                dictionary,
                                depth + 1);
                        }
                    }
                }
            }
            else
            {
                if (!targetA.Exists ?? false)
                {
                    dictionary[$"{_serial}_registryA_NotExists"] = targetA.Path;
                    ret = false;
                }
                if (!targetB.Exists ?? false)
                {
                    dictionary[$"{_serial}_registryB_NotExists"] = targetB.Path;
                    ret = false;
                }
            }

            return ret;
        }
    }
}
