﻿using System;
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

        public static bool WatchFile(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitor, string path)
        {
            if ((!isMonitor ?? true) && watch.Attributes == null) { return false; }

            bool ret = false;
            if (watch.Attributes == null)
            {
                ret = true;
                watch.Attributes = GetAttributes(path);
            }
            else
            {
                string pathType = "file";
                string checkTarget = "Attributes";

                bool[] ret_bools = GetAttributes(path);
                ret = !ret_bools.SequenceEqual(watch.Attributes);

                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    string.Format(
                        "[{0}]Readonly [{1}]Hidden [{2}]System",
                        watch.Attributes[0] ? "x" : " ",
                        watch.Attributes[1] ? "x" : " ",
                        watch.Attributes[2] ? "x" : " ") + " -> " +
                    string.Format(
                        "[{0}]Readonly [{1}]Hidden [{2}]System",
                        ret_bools[0] ? "x" : " ",
                        ret_bools[1] ? "x" : " ",
                        ret_bools[2] ? "x" : " ") :
                    string.Format(
                        "[{0}]Readonly [{1}]Hidden [{2}]System",
                        ret_bools[0] ? "x" : " ",
                        ret_bools[1] ? "x" : " ",
                        ret_bools[2] ? "x" : " ");
                watch.Attributes = ret_bools;
            }

            return ret;
        }

        public static bool WatchDirectory(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitor, string path)
        {
            if ((!isMonitor ?? true) && watch.Attributes == null) { return false; }

            bool ret = false;
            if (watch.Attributes == null)
            {
                ret = true;
                watch.Attributes = GetAttributes(path);
            }
            else
            {
                string pathType = "directory";
                string checkTarget = "Attributes";

                bool[] ret_bools = GetAttributes(path);
                ret = !ret_bools.SequenceEqual(watch.Attributes);

                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    string.Format(
                        "[{0}]Readonly [{1}]Hidden [{2}]System -> [{3}]Readonly [{4}]Hidden [{5}]System",
                        watch.Attributes[0] ? "x" : " ",
                        watch.Attributes[1] ? "x" : " ",
                        watch.Attributes[2] ? "x" : " ",
                        ret_bools[0] ? "x" : " ",
                        ret_bools[1] ? "x" : " ",
                        ret_bools[2] ? "x" : " ") :
                    string.Format(
                        "[{0}]Readonly [{1}]Hidden [{2}]System",
                        ret_bools[0] ? "x" : " ",
                        ret_bools[1] ? "x" : " ",
                        ret_bools[2] ? "x" : " ");

                watch.Attributes = ret_bools;
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
