using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32;
using IO.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorSecurity : MonitorBase
    {
        


        #region Get method

        protected string GetAccessString(MonitoringWatch monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => AccessRuleSummary.FileToAccessString((FileInfo)monitoring.Info),
                PathType.Directory => AccessRuleSummary.DirectoryToAccessString((DirectoryInfo)monitoring.Info),
                PathType.Registry => AccessRuleSummary.RegistryKeyToAccessString(monitoring.Key),
                _ => null,
            };
        }

        protected string GetAccessStringA(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => AccessRuleSummary.FileToAccessString((FileInfo)monitoring.InfoA),
                PathType.Directory => AccessRuleSummary.DirectoryToAccessString((DirectoryInfo)monitoring.InfoA),
                PathType.Registry => AccessRuleSummary.RegistryKeyToAccessString(monitoring.KeyA),
                _ => null,
            };
        }

        protected string GetAccessStringB(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => AccessRuleSummary.FileToAccessString((FileInfo)monitoring.InfoB),
                PathType.Directory => AccessRuleSummary.DirectoryToAccessString((DirectoryInfo)monitoring.InfoB),
                PathType.Registry => AccessRuleSummary.RegistryKeyToAccessString(monitoring.KeyB),
                _ => null,
            };
        }

        protected string GetownerString(MonitoringWatch monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => ((FileInfo)monitoring.Info).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Directory => ((DirectoryInfo)monitoring.Info).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Registry => monitoring.Key.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                _ => null,
            };
        }

        protected string GetownerStringA(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => ((FileInfo)monitoring.InfoA).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Directory => ((DirectoryInfo)monitoring.InfoA).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Registry => monitoring.KeyA.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                _ => null,
            };
        }

        protected string GetownerStringB(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => ((FileInfo)monitoring.InfoB).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Directory => ((DirectoryInfo)monitoring.InfoB).GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Registry => monitoring.KeyB.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                _ => null,
            };
        }

        protected bool GetInherited(MonitoringWatch monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => !((FileInfo)monitoring.Info).GetAccessControl().AreAccessRulesProtected,
                PathType.Directory => !((DirectoryInfo)monitoring.Info).GetAccessControl().AreAccessRulesProtected,
                PathType.Registry => !monitoring.Key.GetAccessControl().AreAccessRulesProtected,
                _ => false,
            };
        }

        protected bool GetInheritedA(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => !((FileInfo)monitoring.InfoA).GetAccessControl().AreAccessRulesProtected,
                PathType.Directory => !((DirectoryInfo)monitoring.InfoA).GetAccessControl().AreAccessRulesProtected,
                PathType.Registry => !monitoring.KeyA.GetAccessControl().AreAccessRulesProtected,
                _ => false,
            };
        }

        protected bool GetInheritedB(MonitoringCompare monitoring)
        {
            return monitoring.PathType switch
            {
                PathType.File => !((FileInfo)monitoring.InfoB).GetAccessControl().AreAccessRulesProtected,
                PathType.Directory => !((DirectoryInfo)monitoring.InfoB).GetAccessControl().AreAccessRulesProtected,
                PathType.Registry => !monitoring.KeyB.GetAccessControl().AreAccessRulesProtected,
                _ => false,
            };
        }

        #endregion
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorAccess : MonitorSecurity
    {
        public override string CheckTarget { get { return "Access"; } }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAccess ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetAccessStringA(monitoring);
                string retB = GetAccessStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAccess ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetAccessStringA(monitoring);
                string retB = GetAccessStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAccess ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetAccessStringA(monitoring);
                string retB = GetAccessStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAccess ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetAccessString(monitoring);
                    ret = result != monitoring.Access;
                    if (monitoring.Access != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Access} -> {result}" :
                            result;
                    }
                    monitoring.Access = result;
                }
                else
                {
                    monitoring.Access = null;
                }
            }
            return ret;
        }

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAccess ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetAccessString(monitoring);
                    ret = result != monitoring.Access;
                    if (monitoring.Access != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Access} -> {result}" :
                            result;
                    }
                    monitoring.Access = result;
                }
                else
                {
                    monitoring.Access = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAccess ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetAccessString(monitoring);
                    ret = result != monitoring.Access;
                    if (monitoring.Access != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Access} -> {result}" :
                            result;
                    }
                    monitoring.Access = result;
                }
                else
                {
                    monitoring.Access = null;
                }
            }
            return ret;
        }

        #endregion
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorOwner : MonitorSecurity
    {
        public override string CheckTarget { get { return "Owner"; } }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsOwner ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetownerStringA(monitoring);
                string retB = GetownerStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsOwner ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetownerStringA(monitoring);
                string retB = GetownerStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsOwner ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                string retA = GetownerStringA(monitoring);
                string retB = GetownerStringB(monitoring);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = retA;
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = retB;
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetownerString(monitoring);
                    ret = result != monitoring.Owner;
                    if(monitoring.Owner != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Owner} -> {result}" :
                            result;
                    }
                    monitoring.Owner = result;
                }
                else
                {
                    monitoring.Owner = null;
                }
            }
            return ret;
        }

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetownerString(monitoring);
                    ret = result != monitoring.Owner;
                    if (monitoring.Owner != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Owner} -> {result}" :
                            result;
                    }
                    monitoring.Owner = result;
                }
                else
                {
                    monitoring.Owner = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    string result = GetownerString(monitoring);
                    ret = result != monitoring.Owner;
                    if (monitoring.Owner != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            $"{monitoring.Owner} -> {result}" :
                            result;
                    }
                    monitoring.Owner = result;
                }
                else
                {
                    monitoring.Owner = null;
                }
            }
            return ret;
        }

        #endregion
    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorInherited : MonitorSecurity
    {
        public override string CheckTarget { get { return "Inherited"; } }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }



            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }



            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }



            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists_old())
                {

                }
                else
                {

                }
            }
            return ret;
        }

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists_old())
                {

                }
                else
                {

                }
            }
            return ret;
        }

        public override bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists_old())
                {

                }
                else
                {

                }
            }
            return ret;
        }

        #endregion
    }
}
