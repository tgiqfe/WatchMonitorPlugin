using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    /// <summary>
    /// 監視開始。過去分の調査は無し
    /// </summary>
    internal class Process01
    {
        bool? _IsCreationTime = null;
        bool? _IsLastWriteTime = true;
        bool? _IsLastAccessTime = null;
        bool _IsDateOnly = true;
        bool _IsTimeOnly = false;

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
