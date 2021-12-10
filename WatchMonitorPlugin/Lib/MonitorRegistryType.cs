using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using IO.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorRegistryType : MonitorBase
    {
        public override string CheckTarget { get { return "RegistryType"; } }

        #region Compare method

        public override bool CompareRegistryValue(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsRegistryType ?? false)
            {
                if (!(monitoring.KeyA?.GetValueNames().Any(x => x.Equals(monitoring.NameA, StringComparison.OrdinalIgnoreCase)) ?? false) ||
                    !(monitoring.KeyB?.GetValueNames().Any(x => x.Equals(monitoring.NameB, StringComparison.OrdinalIgnoreCase)) ?? false))
                {
                    return false;
                }

                string retA = RegistryControl.ValueKindToString(monitoring.KeyA.GetValueKind(monitoring.NameA));
                string retB = RegistryControl.ValueKindToString(monitoring.KeyB.GetValueKind(monitoring.NameB));

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method



        #endregion


        const string CHECK_TARGET = "RegistryType";

        #region Compare method



        #endregion
        #region Watch method

        public static bool WatchRegistryValue(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey, string name)
        {
            bool ret = false;
            if (watch.IsRegistryType ?? false)
            {
                if (regKey != null && regKey.GetValueNames().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    string ret_string = RegistryControl.ValueKindToString(regKey.GetValueKind(name));
                    ret = ret_string != watch.RegistryType;
                    if (watch.RegistryType != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "RegistryType";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.RegistryType} -> {ret_string}" :
                            ret_string;
                    }
                    watch.RegistryType = ret_string;
                }
                else
                {
                    watch.RegistryType = null;
                }
            }
            return ret;
        }


        #endregion
        #region Get method

        #endregion
    }
}
