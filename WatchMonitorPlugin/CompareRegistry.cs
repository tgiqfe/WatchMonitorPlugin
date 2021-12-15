using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;
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
        private int _serial;
        private string _checkingPathA;
        private string _checkingPathB;


        public Dictionary<string, string> Propeties = null;



        private MonitoredTarget CreateForRegistryKey(RegistryKey key, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, key)
            {
                PathTypeName = pathTypeName,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }

        private MonitoredTarget CreateForRegistryValue(RegistryKey key, string name, string pathTypeName)
        {
            return new MonitoredTarget(PathType.Registry, key, name)
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
            var dictionary = new Dictionary<string, string>();
            this.Success = true;
            _MaxDepth = 5;



            if (_NameA != null && _NameB != null)
            {
                _serial++;
                using (RegistryKey keyA = RegistryControl.GetRegistryKey(_PathA, false, false))
                using (RegistryKey keyB = RegistryControl.GetRegistryKey(_PathB, false, false))
                {
                    MonitoredTarget targetA = CreateForRegistryValue(keyA, _NameA, "registryA");
                    MonitoredTarget targetB = CreateForRegistryValue(keyB, _NameB, "registryB");
                    targetA.CheckExists();
                    targetB.CheckExists();
                    if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
                    {



                        // ****


                    }


                }
            }
            else
            {
                _checkingPathA = _PathA;
                _checkingPathB = _PathB;
                using (RegistryKey keyA = RegistryControl.GetRegistryKey(_PathA, false, false))
                using (RegistryKey keyB = RegistryControl.GetRegistryKey(_PathB, false, false))
                {
                    Success &= RecursiveTree(keyA, keyB, dictionary, 0);
                }
            }

        }

        private bool RecursiveTree(RegistryKey keyA, RegistryKey keyB, Dictionary<string, string> dictionary, int depth)
        {
            bool ret = false;

            _serial++;


            // ****


            return ret;
        }
    }
}
