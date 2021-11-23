﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;
using Microsoft.Win32;

namespace WatchMonitorPlugin
{
    internal class ProcessReg02
    {
        public string _Serial { get; set; }
        public string[] _Path { get; set; }
        public string[] _Name { get; set; }

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

        public int? _MaxDepth { get; set; }

        public bool _Begin { get; set; }

        private int _serial;
        protected bool Success { get; set; }

        const string REGPATH_PREFIX = "[registry]";

        private WatchPath CreateWatchRegistryKey()
        {
            return new WatchPath(PathType.Registry)
            {
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }
        private WatchPath CreateWatchRegistryValue()
        {
            return new WatchPath(PathType.Registry)
            {
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                IsRegistryType = _IsRegistryType,
            };
        }

        public void MainProcess()
        {
            bool ret = false;

            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var dictionary = new Dictionary<string, string>();
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            if (_Name?.Length > 0)
            {
                //  レジストリ値のWatch
                string keyPath = _Path[0];
                using (RegistryKey regKey = RegistryControl.GetRegistryKey(keyPath, false, false))
                {
                    foreach (string name in _Name)
                    {
                        _serial++;
                        string regPath = REGPATH_PREFIX + keyPath + "\\" + name;
                        dictionary[$"registry_{_serial}"] = regPath;
                        WatchPath watch = _Begin ?
                            CreateWatchRegistryValue() :
                            collection.GetWatchPath(regPath) ?? CreateWatchRegistryValue();
                        ret |= WatchRegistryValueCheck(watch, dictionary, regKey, name);
                        collection.SetWatchPath(regPath, watch);
                    }
                }
            }
            else
            {
                //  レジストリキーのWatch
                foreach (string path in _Path)
                {
                    _serial++;
                    dictionary[$"registry_{_serial}"] = path;
                    WatchPath watch = _Begin ?
                        CreateWatchRegistryKey() :
                        collection.GetWatchPath(path) ?? CreateWatchRegistryKey();
                    using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
                    {
                        ret |= WatchRegistryKeyCheck(watch, dictionary, regKey);
                        collection.SetWatchPath(path, watch);
                    }
                }
            }

            Success = ret;





            //  確認用
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }
            if (Success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed");
                Console.ResetColor();
            }
        }

        private bool WatchRegistryKeyCheck(WatchPath watch, Dictionary<string, string> dictionary, RegistryKey regKey)
        {
            bool ret = MonitorExists.WatchRegistryKey(watch, dictionary, _serial, regKey.Name);
            ret |= MonitorSecurity.WatchRegistryKeyAccess(watch, dictionary, _serial, _IsAccess, regKey);
            ret |= MonitorSecurity.WatchRegistryKeyOwner(watch, dictionary, _serial, _IsOwner, regKey);
            ret |= MonitorSecurity.WatchRegistryKeyInherited(watch, dictionary, _serial, _IsInherited, regKey);
            ret |= MonitorChildCount.WatchRegistryKey(watch, dictionary, _serial, _IsChildCount, regKey);

            return ret;
        }

        private bool WatchRegistryValueCheck(WatchPath watch, Dictionary<string, string> dictionary, RegistryKey regKey, string name)
        {
            bool ret = MonitorExists.WatchRegistryValue(watch, dictionary, _serial, regKey.Name, name);
            if (regKey != null || regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                ret |= MonitorHash.WatchRegistryValueMD5Hash(watch, dictionary, _serial, _IsMD5Hash, regKey, name);
                ret |= MonitorHash.WatchRegistryValueSHA256Hash(watch, dictionary, _serial, _IsSHA256Hash, regKey, name);
                ret |= MonitorHash.WatchRegistryValueSHA512Hash(watch, dictionary, _serial, _IsSHA512Hash, regKey, name);
                ret |= MonitorRegistryType.WatchRegistryValue(watch, dictionary, _serial, _IsRegistryType, regKey, name);
            }

            return ret;
        }
    }
}
