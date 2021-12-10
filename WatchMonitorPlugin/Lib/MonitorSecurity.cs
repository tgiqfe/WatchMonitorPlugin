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
        const string CHECK_TARGET = "RegistryType";

        #region Compare method



        #endregion
        #region Watch method

        public static bool WatchFileAccess(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, FileInfo info)
        {
            bool ret = false;
            if (watch.IsAccess ?? false)
            {
                if (info.Exists)
                {
                    string ret_string = AccessRuleSummary.FileToAccessString(info);
                    ret = ret_string != watch.Access;
                    if (watch.Access != null)
                    {
                        string pathType = "file";
                        string checkTarget = "Access";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Access} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Access = ret_string;
                }
                else
                {
                    watch.Access = null;
                }
            }
            return ret;
        }

        public static bool WatchFileOwner(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, FileInfo info)
        {
            bool ret = false;
            if (watch.IsOwner ?? false)
            {
                if (info.Exists)
                {
                    string ret_string = GetFileOwner(info);
                    ret = ret_string != watch.Owner;
                    if (watch.Owner != null)
                    {
                        string pathType = "file";
                        string checkTarget = "Owner";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Owner} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Owner = ret_string;
                }
                else
                {
                    watch.Owner = null;
                }
            }
            return ret;
        }

        public static bool WatchFileInherited(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, FileInfo info)
        {
            bool ret = false;
            if (watch.Inherited ?? false)
            {
                if (info.Exists)
                {
                    bool ret_bool = GetFileInherited(info);
                    ret = ret_bool != watch.Inherited;
                    if (watch.Inherited != null)
                    {
                        string pathType = "file";
                        string checkTarget = "Inherited";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Inherited} -> {ret_bool}" :
                            ret_bool.ToString();
                    }
                    watch.Inherited = ret_bool;
                }
                else
                {
                    watch.Inherited = null;
                }
            }
            return ret;
        }

        public static bool WatchDirectoryAccess(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, DirectoryInfo info)
        {
            bool ret = false;
            if (watch.IsAccess ?? false)
            {
                if (info.Exists)
                {
                    string ret_string = AccessRuleSummary.DirectoryToAccessString(info);
                    ret = ret_string != watch.Access;
                    if (watch.Access != null)
                    {
                        string pathType = "directory";
                        string checkTarget = "Access";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Access} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Access = ret_string;
                }
                else
                {
                    watch.Access = null;
                }
            }
            return ret;
        }

        public static bool WatchDirectoryOwner(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, DirectoryInfo info)
        {
            bool ret = false;
            if (watch.IsOwner ?? false)
            {
                if (info.Exists)
                {
                    string ret_string = GetDirectoryOwner(info);
                    ret = ret_string != watch.Owner;
                    if (watch.Owner != null)
                    {
                        string pathType = "directory";
                        string checkTarget = "Owner";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Owner} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Owner = ret_string;
                }
                else
                {
                    watch.Owner = null;
                }
            }
            return ret;
        }

        public static bool WatchDirectoryInherited(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, DirectoryInfo info)
        {
            bool ret = false;
            if (watch.Inherited ?? false)
            {
                if (info.Exists)
                {
                    bool ret_bool = GetDirectoryInherited(info);
                    ret = ret_bool != watch.Inherited;
                    if (watch.Inherited != null)
                    {
                        string pathType = "directory";
                        string checkTarget = "Inherited";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Inherited} -> {ret_bool}" :
                            ret_bool.ToString();
                    }
                    watch.Inherited = ret_bool;
                }
                else
                {
                    watch.Inherited = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryKeyAccess(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey)
        {
            bool ret = false;
            if (watch.IsAccess ?? false)
            {
                if (regKey != null)
                {
                    string ret_string = AccessRuleSummary.RegistryKeyToAccessString(regKey);
                    ret = ret_string != watch.Access;
                    if (watch.Access != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "Access";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Access} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Access = ret_string;
                }
                else
                {
                    watch.Access = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryKeyOwner(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey)
        {
            bool ret = false;
            if (watch.IsOwner ?? false)
            {
                if (regKey != null)
                {
                    string ret_string = GetRegistryKeyOwner(regKey);
                    ret = ret_string != watch.Owner;
                    if (watch.Owner != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "Owner";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Owner} -> {ret_string}" :
                            ret_string;
                    }
                    watch.Owner = ret_string;
                }
                else
                {
                    watch.Owner = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryKeyInherited(
            MonitoringWatch watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey)
        {
            bool ret = false;
            if (watch.IsInherited ?? false)
            {
                if (regKey != null)
                {
                    bool ret_bool = GetRegistryKeyInherited(regKey);
                    ret = ret_bool != watch.Inherited;
                    if (watch.Inherited != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "Inherited";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            $"{watch.Inherited} -> {ret_bool}" :
                            ret_bool.ToString();
                    }
                    watch.Inherited = ret_bool;
                }
                else
                {
                    watch.Inherited = null;
                }
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string GetFileOwner(FileInfo info)
        {
            return info.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }
        public static string GetDirectoryOwner(DirectoryInfo info)
        {
            return info.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }

        public static bool GetFileInherited(FileInfo info)
        {
            return !info.GetAccessControl().AreAccessRulesProtected;
        }
        public static bool GetDirectoryInherited(DirectoryInfo info)
        {
            return !info.GetAccessControl().AreAccessRulesProtected;
        }

        public static string GetRegistryKeyOwner(RegistryKey regKey)
        {
            return regKey.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }
        public static bool GetRegistryKeyInherited(RegistryKey regKey)
        {
            return !regKey.GetAccessControl().AreAccessRulesProtected;
        }

        #endregion


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
                if (!monitoring.TestExists()) { return false; }

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
                if (!monitoring.TestExists()) { return false; }

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
                if (!monitoring.TestExists()) { return false; }

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
                if (monitoring.TestExists())
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
                if (monitoring.TestExists())
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
                if (monitoring.TestExists())
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
                if (monitoring.TestExists()) { return false; }



            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists()) { return false; }



            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists()) { return false; }



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
                if (monitoring.TestExists())
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
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists())
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
            if (monitoring.IsOwner ?? false)
            {
                if (monitoring.TestExists())
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

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorInherited : MonitorSecurity
    {
        public override string CheckTarget { get { return "Inherited"; } }

        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists()) { return false; }



            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists()) { return false; }



            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsInherited ?? false)
            {
                if (monitoring.TestExists()) { return false; }



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
                if (monitoring.TestExists())
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
                if (monitoring.TestExists())
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
                if (monitoring.TestExists())
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
