using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WatchMonitorPlugin.Lib
{
    internal class MonitorTimeStamp
    {
        #region Check method

        public static bool WatchFileCreation(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isCreationTime, FileInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "CreationTime";

            if ((isCreationTime ?? false) || watch.CreationTime != null)
            {
                string ret_string = GetCreationTime(info,
                    isDateOnly ?? watch.IsDateOnly ?? false,
                    isTimeOnly ?? watch.IsTimeOnly ?? false);
                ret = ret_string != watch.CreationTime;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.CreationTime} -> {ret_string}" :
                    ret_string;
                watch.CreationTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchFileLastWrite(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastWriteTime, FileInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "LastWriteTime";

            if ((isLastWriteTime ?? false) || watch.LastWriteTime != null)
            {
                string ret_string = GetLastWriteTime(info,
                    isDateOnly ?? watch.IsDateOnly ?? false,
                    isTimeOnly ?? watch.IsTimeOnly ?? false);
                ret = ret_string != watch.LastWriteTime;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.LastWriteTime} -> {ret_string}" :
                    ret_string;
                watch.LastWriteTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchFileLastAccess(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastAccessTime, FileInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            string pathType = "file";
            string checkTarget = "LastAccessTime";

            if ((isLastAccessTime ?? false) || watch.LastAccessTime != null)
            {
                string ret_string = GetLastAccessTime(info,
                    isDateOnly ?? watch.IsDateOnly ?? false,
                    isTimeOnly ?? watch.IsTimeOnly ?? false);
                ret = ret_string != watch.LastAccessTime;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                    $"{watch.LastAccessTime} -> {ret_string}" :
                    ret_string;
                watch.LastAccessTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string GetCreationTime(string filePath, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(filePath).CreationTime, isDateOnly, isTimeOnly);
        }

        public static string GetCreationTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.CreationTime, isDateOnly, isTimeOnly);
        }

        public static string GetLastWriteTime(string filePath, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(filePath).LastWriteTime, isDateOnly, isTimeOnly);
        }

        public static string GetLastWriteTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.LastWriteTime, isDateOnly, isTimeOnly);
        }

        public static string GetLastAccessTime(string filePath, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(filePath).LastAccessTime, isDateOnly, isTimeOnly);
        }

        public static string GetLastAccessTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.LastAccessTime, isDateOnly, isTimeOnly);
        }

        private static string DateToString(DateTime date, bool isDateOnly, bool isTimeOnly)
        {
            if (isDateOnly)
                return date.ToString("yyyy/MM/dd");
            else if (isTimeOnly)
                return date.ToString("HH:mm:ss");
            else
                return date.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion
    }
}
