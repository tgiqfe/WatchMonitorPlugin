using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using IO.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorExists
    {
        const string CHECK_TARGET = "Exists";

        #region Compare method

        public static bool CompareFile(MonitoringCompare compare, Dictionary<string, string> dictionary, int serial)
        {
            string pathType = "file";
            string checkTarget = "Exists";

            bool retA = File.Exists(compare.PathA);
            bool retB = File.Exists(compare.PathB);

            dictionary[$"{pathType}A_{checkTarget}_{serial}"] = retA.ToString();
            dictionary[$"{pathType}B_{checkTarget}_{serial}"] = retB.ToString();

            return retA && retB;
        }





        #endregion
        #region Watch method

        public static bool WatchFile(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, FileInfo info)
        {
            bool ret = false;

            bool ret_bool = info.Exists;
            ret = ret_bool != watch.Exists;
            if (watch.Exists != null)
            {
                string pathType = "file";
                string checkTarget = "Exists";
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
            }
            watch.Exists = ret_bool;

            return ret;
        }

        public static bool WatchDirectory(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, DirectoryInfo info)
        {
            bool ret = false;

            bool ret_bool = info.Exists;
            ret = ret_bool != watch.Exists;
            if (watch.Exists != null)
            {
                string pathType = "directory";
                string checkTarget = "Exists";
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
            }
            watch.Exists = ret_bool;

            return ret;
        }

        public static bool WatchRegistryKey(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey)
        {
            bool ret = false;

            bool ret_bool = regKey != null;
            ret = ret_bool != watch.Exists;
            if (watch.Exists != null)
            {
                string pathType = "registry";
                string checkTarget = "Exists";
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
            }
            watch.Exists = ret_bool;

            return ret;
        }

        public static bool WatchRegistryValue(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey, string name)
        {
            bool ret = false;

            bool ret_bool = (regKey != null && regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)));
            ret = ret_bool != watch.Exists;
            if (watch.Exists != null)
            {
                string pathType = "registry";
                string checkTarget = "Exists";
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
            }
            watch.Exists = ret_bool;

            return ret;
        }

        #endregion
        #region Get method

        public static bool GetRegistryKeyExists(string path)
        {
            using (RegistryKey regKey = RegistryControl.GetRegistryKey(path, false, false))
            {
                return regKey != null;
            }
        }

        public static bool GetRegistryValueExists(string path, string name)
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
