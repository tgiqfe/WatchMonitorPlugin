using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorChildCount
    {
        #region Check method

        public static bool WatchDirectory(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitor, string path)
        {
            if ((!isMonitor ?? true) && watch.ChildCount == null) { return false; }

            bool ret = false;
            if (watch.ChildCount == null)
            {
                ret = true;
                watch.ChildCount = GetDirectoryChildCount(path);
            }
            else
            {
                string pathType = "directory";
                string checkTarget = "ChildCount";

                int[] ret_integers = GetDirectoryChildCount(path);
                ret = !ret_integers.SequenceEqual(watch.ChildCount);
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    string.Format("File:{0} / Directory:{1} -> File:{2} / Directory:{3}",
                        watch.ChildCount[0],
                        watch.ChildCount[1],
                        ret_integers[0],
                        ret_integers[1]) :
                    string.Format("File:{0} / Directory:{1}",
                        ret_integers[0],
                        ret_integers[1]);

                watch.ChildCount = ret_integers;
            }
            return ret;
        }

        #endregion
        #region Get method

        public static int[] GetDirectoryChildCount(string path)
        {
            return new int[2]
            {
                Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length,
                Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length
            };
        }

        #endregion
    }
}
