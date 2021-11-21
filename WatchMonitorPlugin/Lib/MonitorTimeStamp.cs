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

        public static bool WatchCreation(
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
                watch.CreationTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
            }
            return ret;
        }

        public static bool WatchLastWrite(
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
                watch.LastWriteTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
            }
            return ret;
        }

        public static bool WatchLastAccess(
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
                watch.LastAccessTime = ret_string;
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
                dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret_string;
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
