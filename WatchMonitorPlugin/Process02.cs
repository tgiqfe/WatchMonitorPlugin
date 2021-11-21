using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    /// <summary>
    /// 監視。すでに監視対象として登録されている箇所のみ監視
    /// </summary>
    internal class Process02
    {
        bool? _IsCreationTime = null;
        bool? _IsLastWriteTime = null;
        bool? _IsLastAccessTime = null;
        bool _IsDateOnly = false;
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

            return ret;
        }
    }
}
