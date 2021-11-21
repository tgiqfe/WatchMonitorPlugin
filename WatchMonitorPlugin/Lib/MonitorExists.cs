using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorExists
    {
        #region Check method

        public static bool Watch(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "Exists";

            bool ret_bool = File.Exists(path);
            ret = ret_bool != watch.Exists;
            if (!ret_bool && watch.Exists)
            {
                watch = new WatchPath(PathType.File);
            }
            dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                $"{watch.Exists} -> {ret_bool}" :
                ret_bool.ToString();
            watch.Exists = ret_bool;

            return ret;
        }

        #endregion
        #region Get method

        #endregion
    }
}
