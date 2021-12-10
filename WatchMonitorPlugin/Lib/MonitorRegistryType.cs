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
                if (!monitoring.TestExists_old()) { return false; }

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

        public override bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsRegistryType ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = RegistryControl.ValueKindToString(monitoring.Key.GetValueKind(monitoring.Name));
                    ret = result != monitoring.RegistryType;
                    if (monitoring.RegistryType != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.RegistryType} -> {result}" :
                            result;
                    }
                    monitoring.RegistryType = result;
                }
                else
                {
                    monitoring.RegistryType = null;
                }
            }
            return ret;
        }

        #endregion
        #region Get method

        #endregion
    }
}
