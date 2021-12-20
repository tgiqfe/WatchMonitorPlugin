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

        private MonitorTarget CreateForRegistryKey(string path, RegistryKey key, string pathTypeName)
        {
            return new MonitorTarget(PathType.Registry, path, key)
            {
                PathTypeName = pathTypeName,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }

        private MonitorTarget CreateForRegistryValue(string path, RegistryKey key, string name, string pathTypeName)
        {
            return new MonitorTarget(PathType.Registry, path, key, name)
            {
                PathTypeName = pathTypeName,
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                IsRegistryType = _IsRegistryType,
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
                    MonitorTarget targetA = CreateForRegistryValue(_PathA, keyA, _NameA, "registryA");
                    MonitorTarget targetB = CreateForRegistryValue(_PathB, keyB, _NameB, "registryB");
                    targetA.CheckExists();
                    targetB.CheckExists();

                    if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
                    {
                        dictionary["registryA_Exists"] = _PathA + "\\" + _NameA;
                        dictionary["registryB_Exists"] = _PathB + "\\" + _NameB;
                        Success &= CompareFunctions.CheckRegistryValue(targetA, targetB, dictionary, _serial);
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
                        CreateForRegistryKey(_PathA, keyA, "registryA"),
                        CreateForRegistryKey(_PathB, keyB, "registryB"),
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
                    foreach (string childName in targetA.Key.GetValueNames())
                    {
                        _serial++;
                        MonitorTarget targetA_leaf = CreateForRegistryValue(targetA.Path, targetA.Key, childName, "registryA");
                        MonitorTarget targetB_leaf = CreateForRegistryValue(targetB.Path, targetB.Key, childName, "registryB");
                        targetA_leaf.CheckExists();
                        targetB_leaf.CheckExists();

                        if (targetB_leaf.Exists ?? false)
                        {
                            dictionary[$"{_serial}_registryA_Exists"] = targetA_leaf.Path + "\\" + targetA_leaf.Name;
                            dictionary[$"{_serial}_registryB_Exists"] = targetB_leaf.Path + "\\" + targetB_leaf.Name;
                            ret &= CompareFunctions.CheckRegistryValue(targetA_leaf, targetB_leaf, dictionary, _serial);
                        }
                        else
                        {
                            dictionary[$"{_serial}_registryB_NotExists"] = targetB_leaf.Path + "\\" + targetB_leaf.Name;
                            ret = false;
                        }
                    }
                    foreach (string keyPath in targetA.Key.GetSubKeyNames())
                    {
                        using (RegistryKey subRegKeyA = targetA.Key.OpenSubKey(keyPath, false))
                        using (RegistryKey subRegKeyB = targetB.Key.OpenSubKey(keyPath, false))
                        {
                            ret &= RecursiveTree(
                                CreateForRegistryKey(Path.Combine(targetA.Path, keyPath), subRegKeyA, "registryA"),
                                CreateForRegistryKey(Path.Combine(targetB.Path, keyPath), subRegKeyB, "registryB"),
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
