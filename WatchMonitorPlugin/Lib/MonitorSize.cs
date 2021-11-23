using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorSize
    {
        #region Check method

        public static bool WatchFile(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isSize, FileInfo info)
        {
            bool ret = false;
            
            if (isSize ?? false)
            {
                if (watch.Size == null)
                {
                    ret = true;
                    watch.Size = info.Length;
                }
                else
                {
                    string pathType = "file";
                    string checkTarget = "Size";

                    long ret_long = info.Length;
                    ret = ret_long != watch.Size;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.Size} -> {ret_long}" :
                        ret_long.ToString();

                    watch.Size = ret_long;
                }
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string ToReadable(long size)
        {
            if (size < 1024 * 0.9)
            {
                //  byte
                return $"{size} byte";
            }
            else if (size < 1024 * 1024 * 0.9)
            {
                //  KB
                return $"{Math.Round(size / 1024.0, 2, MidpointRounding.AwayFromZero)} KB";
            }
            else if (size < 1024 * 1024 * 1024 * 0.9)
            {
                //  MB
                return $"{Math.Round(size / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} MB";
            }
            else if (size < (1024L * 1024L * 1024L * 1024L * 0.9))
            {
                //  GB
                return $"{Math.Round(size / 1024.0 / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} GB";
            }
            else
            {
                //  TB
                return $"{Math.Round(size / 1024.0 / 1024.0 / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} TB";
            }
        }

        #endregion
    }
}
