﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class ProcessDir01
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

        public bool _IsStart { get; set; }

        private int _serial;
        private string _checkingPath;

        protected bool Success { get; set; }

        public void MainProcess()
        {
            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            var dictionary = new Dictionary<string, string>();

            this._MaxDepth ??= 5;

            foreach (string path in _Path)
            {
                _checkingPath = path;
                Success = RecursiveTree(collection, dictionary, path, 0);
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
            dictionary[$"directory_{_serial}"] = path.Replace(_checkingPath, "\\");

            WatchPath watch = collection.GetWatchPath(path);
            if (watch == null || this._IsStart)
            {
                ret = true;
                watch = new WatchPath(PathType.Directory);

                if (Directory.Exists(path))
                {
                    WatchDirectoryCheck_Start(watch, path);
                    if (depth == 0 && (_IsChildCount ?? false))
                    {
                        watch.ChildCount = MonitorChildCount.GetDirectoryChildCount(path);
                    }
                    collection.SetWatchPath(path, watch);

                    if (depth < _MaxDepth)
                    {
                        //  配下ファイルチェック
                        foreach (string childPath in Directory.GetFiles(path))
                        {
                            _serial++;
                            dictionary[$"file_{_serial}"] = childPath.Replace(_checkingPath, "\\");
                            WatchPath childWatch = new WatchPath(PathType.File);
                            WatchFileCheck_Start(childWatch, childPath);
                            collection.SetWatchPath(childPath, childWatch);
                        }
                        //  配下ディレクトリチェック
                        foreach (string childPath in Directory.GetDirectories(path))
                        {
                            RecursiveTree(collection, dictionary, childPath, depth + 1);
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(path))
                {
                    ret |= WatchDirectoryCheck(watch, dictionary, path);
                    if (depth == 0 && (_IsChildCount ?? false))
                    {
                        ret |= MonitorChildCount.WatchDirectory(watch, dictionary, _serial, _IsChildCount, path);
                    }
                    collection.SetWatchPath(path, watch);

                    if (depth < _MaxDepth)
                    {
                        //  配下ファイルチェック
                        foreach (string childPath in Directory.GetFiles(path))
                        {
                            _serial++;
                            dictionary[$"file_{_serial}"] = childPath.Replace(_checkingPath, "\\");
                            WatchPath childWatch = collection.GetWatchPath(childPath);
                            if (childWatch == null)
                            {
                                childWatch = new WatchPath(PathType.File);
                                WatchFileCheck_Start(childWatch, childPath);
                            }
                            else
                            {
                                WatchFileCheck(childWatch, dictionary, childPath);
                            }
                            collection.SetWatchPath(childPath, childWatch);
                        }
                        //  配下ディレクトリチェック
                        foreach (string childPath in Directory.GetDirectories(path))
                        {
                            RecursiveTree(collection, dictionary, childPath, depth + 1);
                        }
                    }
                }
            }

            return ret;
        }

        private void WatchDirectoryCheck_Start(WatchPath watch, string path)
        {
            watch.Exists = true;
            var info = new DirectoryInfo(path);

            if (_IsCreationTime ?? false)
            {
                watch.CreationTime = MonitorTimeStamp.GetDirectoryCreationTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsLastWriteTime ?? false)
            {
                watch.LastWriteTime = MonitorTimeStamp.GetDirectoryLastWriteTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsLastAccessTime ?? false)
            {
                watch.LastAccessTime = MonitorTimeStamp.GetDirectoryLastAccessTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsAccess ?? false) { watch.Access = AccessRuleSummary.DirectoryToAccessString(info); }
            if (_IsOwner ?? false) { watch.Owner = MonitorSecurity.GetDirectoryOwner(info); }
            if (_IsInherited ?? false) { watch.Inherited = MonitorSecurity.GetDirectoryInherited(info); }
            if (_IsAttributes ?? false) { watch.Attributes = MonitorAttributes.GetAttributes(path); }

            return;
        }

        private void WatchFileCheck_Start(WatchPath watch, string path)
        {
            watch.Exists = true;
            var info = new FileInfo(path);

            if (_IsCreationTime ?? false)
            {
                watch.CreationTime = MonitorTimeStamp.GetFileCreationTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsLastWriteTime ?? false)
            {
                watch.LastWriteTime = MonitorTimeStamp.GetFileLastWriteTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsLastAccessTime ?? false)
            {
                watch.LastAccessTime = MonitorTimeStamp.GetFileLastAccessTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                watch.IsDateOnly = _IsDateOnly;
                watch.IsTimeOnly = _IsTimeOnly;
            }
            if (_IsAccess ?? false) { watch.Access = AccessRuleSummary.FileToAccessString(info); }
            if (_IsOwner ?? false) { watch.Owner = MonitorSecurity.GetFileOwner(info); }
            if (_IsInherited ?? false) { watch.Inherited = MonitorSecurity.GetFileInherited(info); }
            if (_IsAttributes ?? false) { watch.Attributes = MonitorAttributes.GetAttributes(path); }
            if (_IsMD5Hash ?? false) { watch.MD5Hash = MonitorHash.GetFileMD5Hash(path); }
            if (_IsSHA256Hash ?? false) { watch.SHA256Hash = MonitorHash.GetFileSHA256Hash(path); }
            if (_IsSHA512Hash ?? false) { watch.SHA512Hash = MonitorHash.GetFileSHA512Hash(path); }
            if (_IsSize ?? false) { watch.Size = info.Length; }
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