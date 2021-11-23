﻿using System;
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

        public static bool WatchFile(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.Exists == null)
            {
                ret = true;
                watch.Exists = File.Exists(path);
            }
            else
            {
                string pathType = "file";
                string checkTarget = "Exists";

                bool ret_bool = File.Exists(path);
                ret = ret_bool != watch.Exists;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
                watch.Exists = ret_bool;
            }

            return ret;
        }

        public static bool WatchDirectory(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.Exists == null)
            {
                ret = true;
                watch.Exists = Directory.Exists(path);
            }
            else
            {
                string pathType = "directory";
                string checkTarget = "Exists";

                bool ret_bool = Directory.Exists(path);
                ret = ret_bool != watch.Exists;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
                watch.Exists = ret_bool;
            }

            return ret;
        }

        public static bool WatchRegistryKey(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.Exists == null)
            {
                ret = true;
                watch.Exists = GetRegistryKeyExists(path);
            }
            else
            {
                string pathType = "registry";
                string checkTarget = "Exists";

                bool ret_bool = GetRegistryKeyExists(path);
                ret = ret_bool != watch.Exists;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
                watch.Exists = ret_bool;
            }
            return ret;
        }

        #endregion
        #region Get method

        private static bool GetRegistryKeyExists(string path)
        {
            using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
            {
                return regKey != null;
            }
        }

        private static bool GetRegistryValueExists(string path, string name)
        {
            using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
            {
                if (regKey == null) { return false; }
                return regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        #endregion
    }
}
