using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorAttributes
    {
        #region Check method

        public static bool Watch
            (WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isAttributes, string path)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "Attributes";

            if ((isAttributes ?? false) || watch.Attributes != null)
            {
                bool[] ret_bools = GetAttributes(path);
                ret = ret_bools.SequenceEqual(watch.Attributes);
                watch.Attributes = ret_bools;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = string.Format(
                    "[{0}]Readonly [{1}]Hidden [{2}]System",
                    ret_bools[0] ? "x" : " ",
                    ret_bools[1] ? "x" : " ",
                    ret_bools[2] ? "x" : " ");
            }
            return ret;
        }

        #endregion
        #region Get method

        public static bool[] GetAttributes(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return new bool[]
            {
                (attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly,
                (attr & FileAttributes.Hidden) == FileAttributes.Hidden,
                (attr & FileAttributes.System) == FileAttributes.System
            };
        }

        #endregion
    }
}
