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

        public static bool WatchDirectoryAccess(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isAccess, DirectoryInfo info)
        {
            bool ret = false;
            string pathType = "directory";
            string checkTarget = "Access";

            if ((isAccess ?? false) || watch.Access != null)
            {
                string ret_string = AccessRuleSummary.DirectoryToAccessString(info);
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
                string ret_string = GetFileOwner(info);
                ret = ret_string != watch.Owner;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Owner} -> {ret_string}" :
                    ret_string;

                watch.Owner = ret_string;
            }
            return ret;
        }

        public static bool WatchDirectoryOwner(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isOwner, DirectoryInfo info)
        {
            bool ret = false;
            string pathType = "directory";
            string checkTarget = "Owner";

            if ((isOwner ?? false) || watch.Owner != null)
            {
                string ret_string = GetDirectoryOwner(info);
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
                bool ret_bool = GetFileInherited(info);
                ret = ret_bool != watch.Inherited;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.Inherited} -> {ret_bool}" :
                    ret_bool.ToString();

                watch.Inherited = ret_bool;
            }
            return ret;
        }

        public static bool WatchDirectoryInherited(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isInherited, DirectoryInfo info)
        {
            bool ret = false;
            string pathType = "directory";
            string checkTarget = "Inherited";

            if ((isInherited ?? false) || watch.Inherited != null)
            {
                bool ret_bool = GetDirectoryInherited(info);
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

        public static string GetFileOwner(string path)
        {
            return new FileInfo(path).GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }
        public static string GetFileOwner(FileInfo info)
        {
            return info.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }
        public static string GetDirectoryOwner(string path)
        {
            return new DirectoryInfo(path).GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }
        public static string GetDirectoryOwner(DirectoryInfo info)
        {
            return info.GetAccessControl().GetOwner(typeof(NTAccount)).Value;
        }

        public static bool GetFileInherited(string path)
        {
            return !(new FileInfo(path).GetAccessControl().AreAccessRulesProtected);
        }
        public static bool GetFileInherited(FileInfo info)
        {
            return !info.GetAccessControl().AreAccessRulesProtected;
        }
        public static bool GetDirectoryInherited(string path)
        {
            return !(new DirectoryInfo(path).GetAccessControl().AreAccessRulesProtected);
        }
        public static bool GetDirectoryInherited(DirectoryInfo info)
        {
            return !info.GetAccessControl().AreAccessRulesProtected;
        }

        #endregion
    }
}
