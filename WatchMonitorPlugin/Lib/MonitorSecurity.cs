using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorSecurity
    {
        #region Check method

        public static bool WatchFileAccess(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isAccess, FileInfo info)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "Access";

            if ((isAccess ?? false) || watch.Access != null)
            {
                string ret_string = AccessRuleSummary.FileToAccessString(info);
                ret = ret_string != watch.Access;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Access} -> {ret_string}" :
                    ret_string;

                watch.Access = ret_string;
            }
            return ret;
        }

        public static bool WatchFileOwner(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isOwner, FileInfo info)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "Owner";

            if ((isOwner ?? false) || watch.Owner != null)
            {
                string ret_string = GetOwner(info);
                ret = ret_string != watch.Owner;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Owner} -> {ret_string}" :
                    ret_string;

                watch.Owner = ret_string;
            }
            return ret;
        }

        public static bool WatchFileInherited(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isInherited, FileInfo info)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "Inherited";

            if ((isInherited ?? false) || watch.Inherited != null)
            {
                bool ret_bool = GetInherited(info);
                ret = ret_bool != watch.Inherited;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Inherited} -> {ret_bool}" :
                    ret_bool.ToString();

                watch.Inherited = ret_bool;
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string GetOwner(string filePath)
        {
            return new FileInfo(filePath).GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }

        public static string GetOwner(FileInfo info)
        {
            return info.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }

        public static bool GetInherited(string filePath)
        {
            return !(new FileInfo(filePath).GetAccessControl().AreAccessRulesProtected);
        }

        public static bool GetInherited(FileInfo info)
        {
            return !info.GetAccessControl().AreAccessRulesProtected;
        }

        #endregion
    }
}
