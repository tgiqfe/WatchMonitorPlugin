using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class ProcessDir
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

        public bool _Begin { get; set; }
        protected bool Success { get; set; }

        private int _serial;
        public int? _MaxDepth { get; set; }
        private string _checkingPath;

        private WatchPath CreateForDirectory()
        {
            return new WatchPath(PathType.Directory)
            {
                IsCreationTime = _IsCreationTime,
                IsLastWriteTime = _IsLastWriteTime,
                IsLastAccessTime = _IsLastAccessTime,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsAttributes = _IsAttributes,
                IsChildCount = _IsChildCount,
            };
        }
        private WatchPath CreateForFile()
        {
            return new WatchPath(PathType.File)
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
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            _MaxDepth ??= 5;

            foreach (string path in _Path)
            {
                _checkingPath = path;
                Success |= RecursiveTree(collection, dictionary, path, 0);
            }
            foreach (string uncheckedPath in collection.GetUncheckedKeys())
            {
                _serial++;
                dictionary[$"remove_{_serial}"] = uncheckedPath;
                collection.Remove(uncheckedPath);
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

        private bool RecursiveTree(WatchPathCollection collection, Dictionary<string, string> dictionary, string path, int depth)
        {
            bool ret = false;

            _serial++;
            dictionary[$"directory_{_serial}"] = (path == _checkingPath) ?
                path :
                path.Replace(_checkingPath, "");
            WatchPath watch = _Begin ?
                CreateForDirectory() :
                collection.GetWatchPath(path) ?? CreateForDirectory();
            ret |= WatchDirectoryCheck(watch, dictionary, path);
            collection.SetWatchPath(path, watch);

            if (depth < _MaxDepth)
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    _serial++;
                    dictionary[$"file_{_serial}"] = filePath.Replace(_checkingPath, "");
                    WatchPath childWatch = _Begin ?
                        CreateForFile() :
                        collection.GetWatchPath(filePath) ?? CreateForFile();
                    ret |= WatchFileCheck(childWatch, dictionary, filePath);
                    collection.SetWatchPath(filePath, childWatch);
                }
                foreach (string dir in Directory.GetDirectories(path))
                {
                    ret |= RecursiveTree(collection, dictionary, dir, depth + 1);
                }
            }

            return ret;
        }

        private bool WatchDirectoryCheck(WatchPath watch, Dictionary<string, string> dictionary, string path)
        {
            var info = new DirectoryInfo(path);
            bool ret = MonitorExists.WatchDirectory(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchDirectoryCreationTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchDirectoryLastWriteTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchDirectoryLastAccessTime(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchDirectoryAccess(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchDirectoryOwner(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchDirectoryInherited(watch, dictionary, _serial, info);
            ret |= MonitorAttributes.WatchDirectory(watch, dictionary, _serial, path);
            ret |= MonitorChildCount.WatchDirectory(watch, dictionary, _serial, path);
            return ret;
        }

        private bool WatchFileCheck(WatchPath watch, Dictionary<string, string> dictionary, string path)
        {
            var info = new FileInfo(path);
            bool ret = MonitorExists.WatchFile(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileCreationTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileLastWriteTime(watch, dictionary, _serial, info);
            ret |= MonitorTimeStamp.WatchFileLastAccessTime(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileAccess(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileOwner(watch, dictionary, _serial, info);
            ret |= MonitorSecurity.WatchFileInherited(watch, dictionary, _serial, info);
            ret |= MonitorAttributes.WatchFile(watch, dictionary, _serial, path);
            ret |= MonitorHash.WatchFileMD5Hash(watch, dictionary, _serial, path);
            ret |= MonitorHash.WatchFileSHA256Hash(watch, dictionary, _serial, path);
            ret |= MonitorHash.WatchFileSHA512Hash(watch, dictionary, _serial, path);
            ret |= MonitorSize.WatchFile(watch, dictionary, _serial, info);
            return ret;
        }
    }
}
