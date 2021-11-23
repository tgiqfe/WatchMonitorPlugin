using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class ProcessFile06
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

        private int _serial;

        protected bool Success { get; set; }

        public void MainProcess()
        {
            bool ret = false;

            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            var dictionary = new Dictionary<string, string>();

            foreach (string path in _Path)
            {
                _serial++;
                dictionary[$"file_{_serial}"] = path;
                WatchPath watch = _Begin ? new WatchPath(PathType.File) : collection.GetWatchPath(path);
                watch ??= new WatchPath(PathType.File);
                ret |= WatchFileCheck(watch, dictionary, path);
                collection.SetWatchPath(path, watch);
            }
            collection.Save(dbDir, _Serial);

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

        private bool WatchFileCheck(WatchPath watch, Dictionary<string, string> dictionary, string path)
        {
            var info = new FileInfo(path);

            bool ret = MonitorExists.WatchFile(watch, dictionary, _serial, path);
            ret |= MonitorTimeStamp.WatchFileCreation(watch, dictionary, _serial, _IsCreationTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorTimeStamp.WatchFileLastWrite(watch, dictionary, _serial, _IsLastWriteTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorTimeStamp.WatchFileLastAccess(watch, dictionary, _serial, _IsLastAccessTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorSecurity.WatchFileAccess(watch, dictionary, _serial, _IsAccess, info);
            ret |= MonitorSecurity.WatchFileOwner(watch, dictionary, _serial, _IsOwner, info);
            ret |= MonitorSecurity.WatchFileInherited(watch, dictionary, _serial, _IsInherited, info);
            ret |= MonitorAttributes.WatchFile(watch, dictionary, _serial, _IsAttributes, path);
            ret |= MonitorHash.WatchFileMD5Hash(watch, dictionary, _serial, _IsMD5Hash, path);
            ret |= MonitorHash.WatchFileSHA256Hash(watch, dictionary, _serial, _IsSHA256Hash, path);
            ret |= MonitorHash.WatchFileSHA512Hash(watch, dictionary, _serial, _IsSHA512Hash, path);
            ret |= MonitorSize.WatchFile(watch, dictionary, _serial, _IsSize, info);

            return ret;
        }
    }
}
