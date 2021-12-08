using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;
using Microsoft.Win32;
using IO.Lib;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class ProcessReg
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

        public bool _Begin { get; set; }
        protected bool Success { get; set; }

        private int _serial;
        public int? _MaxDepth { get; set; }
        private string _checkingPath;
        const string REGPATH_PREFIX = "[registry]";

        private WatchPath CreateForRegistryKey()
        {
            return new WatchPath(PathType.Registry)
            {
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsChildCount = _IsChildCount,
            };
        }
        private WatchPath CreateForRegistryValue()
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
            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var dictionary = new Dictionary<string, string>();
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            _MaxDepth ??= 5;

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
                        dictionary[$"registry_{_serial}"] = regPath.Replace(_checkingPath, "");
                        WatchPath watch = _Begin ?
                            CreateForRegistryValue() :
                            collection.GetWatchPath(regPath) ?? CreateForRegistryValue();
                        Success |= WatchRegistryValueCheck(watch, dictionary, regKey, name);
                        collection.SetWatchPath(regPath, watch);
                    }
                }
            }
            else
            {
                //  レジストリキーのWatch
                foreach (string path in _Path)
                {
                    _checkingPath = path;

                    using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
                    {
                        //  指定したレジストリキーが存在しない場合
                        if (regKey == null)
                        {
                            string keyPath = path;

                            _serial++;
                            dictionary[$"registry_{_serial}"] = (path == _checkingPath) ?
                                path :
                                keyPath.Replace(_checkingPath, "");
                            WatchPath watch = _Begin ?
                                CreateForRegistryKey() :
                                collection.GetWatchPath(keyPath) ?? CreateForRegistryKey();
                            Success |= WatchRegistryKeyCheck(watch, dictionary, regKey);
                            collection.SetWatchPath(keyPath, watch);
                            continue;
                        }
                        Success |= RecursiveTree(collection, dictionary, regKey, 0);
                    }
                }
                foreach (string uncheckedPath in collection.GetUncheckedKeys())
                {
                    _serial++;
                    dictionary[$"remove_{_serial}"] = uncheckedPath;
                    collection.Remove(uncheckedPath);
                }
            }


            collection.Save(dbDir, _Serial);



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

        private bool RecursiveTree(WatchPathCollection collection, Dictionary<string, string> dictionary, RegistryKey regKey, int depth)
        {
            bool ret = false;
            string keyPath = regKey.Name;

            _serial++;
            dictionary[$"registry_{_serial}"] = (regKey.Name == _checkingPath) ?
                regKey.Name :
                keyPath.Replace(_checkingPath, "");
            WatchPath watch = _Begin ?
                CreateForRegistryKey() :
                collection.GetWatchPath(keyPath) ?? CreateForRegistryKey();
            ret |= WatchRegistryKeyCheck(watch, dictionary, regKey);
            collection.SetWatchPath(keyPath, watch);

            if (depth < _MaxDepth)
            {
                foreach (string name in regKey.GetValueNames())
                {
                    _serial++;
                    string regPath = REGPATH_PREFIX + keyPath + "\\" + name;
                    dictionary[$"registry_{_serial}"] = regPath.Replace(_checkingPath, "");
                    WatchPath childWatch = _Begin ?
                        CreateForRegistryValue() :
                        collection.GetWatchPath(regPath) ?? CreateForRegistryValue();
                    ret |= WatchRegistryValueCheck(childWatch, dictionary, regKey, name);
                    collection.SetWatchPath(regPath, childWatch);
                }
                foreach (string key in regKey.GetSubKeyNames())
                {
                    ret |= RecursiveTree(collection, dictionary, regKey.OpenSubKey(key, false), depth + 1);
                }
            }

            return ret;
        }

        private bool WatchRegistryKeyCheck(WatchPath watch, Dictionary<string, string> dictionary, RegistryKey regKey)
        {
            bool ret = MonitorExists.WatchRegistryKey(watch, dictionary, _serial, regKey);
            ret |= MonitorSecurity.WatchRegistryKeyAccess(watch, dictionary, _serial, regKey);
            ret |= MonitorSecurity.WatchRegistryKeyOwner(watch, dictionary, _serial, regKey);
            ret |= MonitorSecurity.WatchRegistryKeyInherited(watch, dictionary, _serial, regKey);
            ret |= MonitorChildCount.WatchRegistryKey(watch, dictionary, _serial, regKey);

            return ret;
        }

        private bool WatchRegistryValueCheck(WatchPath watch, Dictionary<string, string> dictionary, RegistryKey regKey, string name)
        {
            bool ret = MonitorExists.WatchRegistryValue(watch, dictionary, _serial, regKey, name);
            ret |= MonitorHash.WatchRegistryValueMD5Hash(watch, dictionary, _serial, regKey, name);
            ret |= MonitorHash.WatchRegistryValueSHA256Hash(watch, dictionary, _serial, regKey, name);
            ret |= MonitorHash.WatchRegistryValueSHA512Hash(watch, dictionary, _serial, regKey, name);
            ret |= MonitorRegistryType.WatchRegistryValue(watch, dictionary, _serial, regKey, name);

            return ret;
        }
    }
}
