using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class Process04
    {
        public bool? _IsCreationTime { get; set; }
        public bool? _IsLastWriteTime { get; set; }
        public bool? _IsLastAccessTime { get; set; }
        public bool _IsDateOnly { get; set; }
        public bool _IsTimeOnly { get; set; }

        public bool Monitor(WatchPath watch, string path)
        {
            string dateToString(DateTime date)
            {
                if (_IsDateOnly)
                    return date.ToString("yyyy/MM/dd");
                else if (_IsTimeOnly)
                    return date.ToString("HH:mm:ss");
                else
                    return date.ToString("yyyy/MM/dd HH:mm:ss");
            }

            bool ret = false;
            if (watch == null)
            {
                ret = true;
                watch = new WatchPath(PathType.File);
                var info = new FileInfo(path);

                if (_IsCreationTime ?? false) { watch.CreationTime = dateToString(info.CreationTime); }
                if (_IsLastWriteTime ?? false) { watch.LastWriteTime = dateToString(info.LastWriteTime); }
                if (_IsLastAccessTime ?? false) { watch.LastAccessTime = dateToString(info.LastAccessTime); }
            }
            else
            {
                var info = new FileInfo(path);
                if ((_IsCreationTime ?? false) || watch.CreationTime != null)
                {
                    string ret_file = dateToString(info.CreationTime);
                    if (ret_file != watch.CreationTime)
                    {
                        ret = true;
                        watch.CreationTime = ret_file;
                    }
                }
                if ((_IsLastWriteTime ?? false) || watch.LastWriteTime != null)
                {
                    string ret_file = dateToString(info.LastWriteTime);
                    if (ret_file != watch.LastWriteTime)
                    {
                        ret = true;
                        watch.LastWriteTime = ret_file;
                    }
                }
                if ((_IsLastAccessTime ?? false) || watch.LastAccessTime != null)
                {
                    string ret_file = dateToString(info.LastAccessTime);
                    if (ret_file != watch.LastAccessTime)
                    {
                        ret = true;
                        watch.LastAccessTime = ret_file;
                    }
                }
            }

            return ret;
        }
    }
}
