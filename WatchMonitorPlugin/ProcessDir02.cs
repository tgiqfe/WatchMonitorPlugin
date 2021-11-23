using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class ProcessDir02
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
        public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public int? _MaxDepth { get; set; }

        public bool _Begin { get; set; }

        private int _serial;
        private string _checkingPath;

        protected bool Success { get; set; }

        public void MainProcess()
        {
            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";



            this._MaxDepth ??= 5;
            var dictionary = new Dictionary<string, string>();



            var collection = WatchPathCollection.Load(dbDir, _Serial);
            foreach (string path in _Path)
            {
                _checkingPath = path;
                Success |= RecursiveTree(collection, dictionary, path, 0);
            }

            //  ここに、前回Watch時に存在してい今回存在しないパスのチェックを

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

        private bool RecursiveTree(WatchPathCollection collection, Dictionary<string, string> dictionary, string path, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"directory_{_serial}"] = (path + "\\").Replace(_checkingPath, "");

            WatchPath watch = _Begin ? new WatchPath(PathType.Directory) : collection.GetWatchPath(path);
            watch ??= new WatchPath(PathType.Directory);

            if (Directory.Exists(path))
            {
                ret |= WatchDirectoryCheck(watch, dictionary, path);
                collection.SetWatchPath(path, watch);

                if (depth < _MaxDepth)
                {
                    //  配下ファイルチェック
                    foreach (string childPath in Directory.GetFiles(path))
                    {
                        _serial++;
                        dictionary[$"file_{_serial}"] = childPath.Replace(_checkingPath, "");
                        WatchPath childWatch = _Begin ? new WatchPath(PathType.File) : collection.GetWatchPath(childPath);
                        childWatch ??= new WatchPath(PathType.File);
                        ret |= WatchFileCheck(childWatch, dictionary, childPath);
                        collection.SetWatchPath(childPath, childWatch);
                    }
                    //  配下ディレクトリチェック
                    foreach (string childPath in Directory.GetDirectories(path))
                    {
                        RecursiveTree(collection, dictionary, childPath, depth + 1);
                    }
                }
            }
            else
            {
                ret |= MonitorExists.WatchDirectory(watch, dictionary, _serial, path);
                collection.SetWatchPath(path, watch);
            }

            return ret;
        }

        private bool WatchDirectoryCheck(WatchPath watch, Dictionary<string, string> dictionary, string path)
        {
            var info = new DirectoryInfo(path);

            bool ret = MonitorExists.WatchDirectory(watch, dictionary, _serial, path);
            ret |= MonitorTimeStamp.WatchDirectoryCreation(watch, dictionary, _serial, _IsCreationTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorTimeStamp.WatchDirectoryLastWrite(watch, dictionary, _serial, _IsLastWriteTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorTimeStamp.WatchDirectoryLastAccess(watch, dictionary, _serial, _IsLastAccessTime, info, _IsDateOnly, _IsTimeOnly);
            ret |= MonitorSecurity.WatchDirectoryAccess(watch, dictionary, _serial, _IsAccess, info);
            ret |= MonitorSecurity.WatchDirectoryOwner(watch, dictionary, _serial, _IsOwner, info);
            ret |= MonitorSecurity.WatchDirectoryInherited(watch, dictionary, _serial, _IsInherited, info);
            ret |= MonitorAttributes.WatchDirectory(watch, dictionary, _serial, _IsAttributes, path);

            return ret;
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
