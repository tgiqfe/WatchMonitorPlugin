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
    internal class WatchFile
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

        private MonitoringWatch CreateForFile()
        {
            return new MonitoringWatch(PathType.File)
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
            };
        }

        public void MainProcess()
        {
            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var dictionary = new Dictionary<string, string>();
            var collection = MonitoringWatchCollection.Load(dbDir, _Serial);

            foreach (string path in _Path)
            {
                _serial++;
                dictionary[$"file_{_serial}"] = path;
                MonitoringWatch watch = _Begin ?
                    CreateForFile() :
                    collection.GetWatchPath(path) ?? CreateForFile();
                Success |= WatchFileCheck(watch, dictionary, path);
                collection.SetWatchPath(path, watch);
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

        private bool WatchFileCheck(MonitoringWatch watch, Dictionary<string, string> dictionary, string path)
        {
            var info = new FileInfo(path);
            bool ret = MonitorExists.WatchFile(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileCreationTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileLastWriteTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileLastAccessTime(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileAccess(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileOwner(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileInherited(watch, dictionary, _serial, info);
            //ret |= MonitorAttributes.WatchFile(watch, dictionary, _serial, path);
            //ret |= MonitorHash.WatchFileMD5Hash(watch, dictionary, _serial, path);
            //ret |= MonitorHash.WatchFileSHA256Hash(watch, dictionary, _serial, path);
            //ret |= MonitorHash.WatchFileSHA512Hash(watch, dictionary, _serial, path);
            //ret |= MonitorSize.WatchFile(watch, dictionary, _serial, info);
            return ret;
        }
    }
}
