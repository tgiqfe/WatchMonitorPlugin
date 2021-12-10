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
    internal class MonitorExists : MonitorBase
    {
        public override string CheckTarget { get { return "Exists"; } }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool retA = ((FileInfo)monitoring.InfoA).Exists;
            bool retB = ((FileInfo)monitoring.InfoB).Exists;

            dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA.ToString();
            dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB.ToString();
            return retA && retB;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool retA = ((DirectoryInfo)monitoring.InfoA).Exists;
            bool retB = ((DirectoryInfo)monitoring.InfoB).Exists;

            dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA.ToString();
            dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB.ToString();
            return retA && retB;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool retA = monitoring.KeyA != null;
            bool retB = monitoring.KeyB != null;

            dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA.ToString();
            dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB.ToString();
            return retA && retB;
        }

        public override bool CompareRegistryValue(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool retA = monitoring.KeyA?.GetValueNames().Any(x => x.Equals(monitoring.NameA, StringComparison.OrdinalIgnoreCase)) ?? false;
            bool retB = monitoring.KeyB?.GetValueNames().Any(x => x.Equals(monitoring.NameB, StringComparison.OrdinalIgnoreCase)) ?? false;

            dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA.ToString();
            dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB.ToString();
            return retA && retB;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            bool result = ((FileInfo)monitoring.Info).Exists;
            ret = result != monitoring.Exists;
            if (monitoring.Exists != null)
            {
                dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                    $"{monitoring.Exists} -> {result}" :
                    result.ToString();
            }
            monitoring.Exists = result;

            return ret;
        }

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            bool result = ((DirectoryInfo)monitoring.Info).Exists;
            ret = result != monitoring.Exists;
            if (monitoring.Exists != null)
            {
                dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                    $"{monitoring.Exists} -> {result}" :
                    result.ToString();
            }
            monitoring.Exists = result;

            return ret;
        }

        public override bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            bool ret_bool = monitoring.Key != null;
            ret = ret_bool != monitoring.Exists;
            if (monitoring.Exists != null)
            {
                dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                    $"{monitoring.Exists} -> {ret_bool}" :
                    ret_bool.ToString();
            }
            monitoring.Exists = ret_bool;

            return ret;
        }

        public override bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            bool result = monitoring.Key?.GetValueNames().Any(x => x.Equals(monitoring.Name, StringComparison.OrdinalIgnoreCase)) ?? false;
            ret = result != monitoring.Exists;
            if (monitoring.Exists != null)
            {
                dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                    $"{monitoring.Exists} -> {result}" :
                    result.ToString();
            }
            monitoring.Exists = result;

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
                return regKey?.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? false;


                if (regKey == null) { return false; }
                return regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        #endregion
    }
}
