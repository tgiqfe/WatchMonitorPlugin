﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class Process05
    {
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
        public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool _IsStart { get; set; }

        private int _serial;

        public bool Monitor(ref WatchPath watch, string path)
        {
            bool ret = false;
            var dictionary = new Dictionary<string, string>();

            _serial++;
            var info = new FileInfo(path);

            dictionary[$"file_{_serial}"] = path;
            if (watch == null || this._IsStart)
            {
                ret = true;
                watch = new WatchPath(PathType.File);

                if (File.Exists(path))
                {
                    watch.Exists = true;
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
                    if (_IsMD5Hash ?? false) { watch.MD5Hash = MonitorHash.GetMD5Hash(path); }
                    if (_IsSHA256Hash ?? false) { watch.SHA256Hash = MonitorHash.GetSHA256Hash(path); }
                    if (_IsSHA512Hash ?? false) { watch.SHA512Hash = MonitorHash.GetSHA512Hash(path); }
                    if (_IsSize ?? false) { watch.Size = info.Length; }
                }
            }
            else
            {
                ret |= MonitorExists.WatchFile(watch, dictionary, _serial, path);
                if (File.Exists(path))
                {
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
                }
            }


            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }

            return ret;
        }
    }
}
