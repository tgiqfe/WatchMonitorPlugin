using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorRegistryType
    {
        #region Check method

        public static bool WatchRegistry(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isMonitor, RegistryKey regKey, string name)
        {
            if ((!isMonitor ?? true) && watch.RegistryType == null) { return false; }

            bool ret = false;
            if (watch.RegistryType == null)
            {
                ret = true;
                watch.RegistryType = RegistryControl.ValueKindToString(regKey.GetValueKind(name));
            }
            else
            {
                string pathType = "registry";
                string checkTarget = "RegistryType";

                string ret_string = RegistryControl.ValueKindToString(regKey.GetValueKind(name));
                ret = ret_string != watch.RegistryType;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.RegistryType} -> {ret_string}" :
                    ret_string;

                watch.RegistryType = ret_string;
            }
            return ret;
        }

        #endregion
        #region Get method

        #endregion
    }
}
