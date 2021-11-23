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
            
            if (isCreationTime ?? false)
            {
                if (watch.CreationTime == null)
                {
                    ret = true;
                    watch.CreationTime = GetFileCreationTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "file";
                    string checkTarget = "CreationTime";

                    string ret_string = GetFileCreationTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.CreationTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.CreationTime} -> {ret_string}" :
                        ret_string;
                    watch.CreationTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchDirectoryCreation(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isCreationTime, DirectoryInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            
            if (isCreationTime ?? false)
            {
                if (watch.CreationTime == null)
                {
                    ret = true;
                    watch.CreationTime = GetDirectoryCreationTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "directory";
                    string checkTarget = "CreationTime";

                    string ret_string = GetDirectoryCreationTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.CreationTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.CreationTime} -> {ret_string}" :
                        ret_string;
                    watch.CreationTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchFileLastWrite(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastWriteTime, FileInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            
            if (isLastWriteTime ?? false) 
            {
                if (watch.LastWriteTime == null)
                {
                    ret = true;
                    watch.LastWriteTime = GetFileLastWriteTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "file";
                    string checkTarget = "LastWriteTime";

                    string ret_string = GetFileLastWriteTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.LastWriteTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.LastWriteTime} -> {ret_string}" :
                        ret_string;
                    watch.LastWriteTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchDirectoryLastWrite(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastWriteTime, DirectoryInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            
            if (isLastWriteTime ?? false)
            {
                if (watch.LastWriteTime == null)
                {
                    ret = true;
                    watch.LastWriteTime = GetDirectoryLastWriteTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "directory";
                    string checkTarget = "LastWriteTime";

                    string ret_string = GetDirectoryLastWriteTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.LastWriteTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.LastWriteTime} -> {ret_string}" :
                        ret_string;
                    watch.LastWriteTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchFileLastAccess(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastAccessTime, FileInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            
            if (isLastAccessTime ?? false)
            {
                if (watch.LastAccessTime == null)
                {
                    ret = true;
                    watch.LastAccessTime = GetFileLastAccessTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "file";
                    string checkTarget = "LastAccessTime";

                    string ret_string = GetFileLastAccessTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.LastAccessTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.LastAccessTime} -> {ret_string}" :
                        ret_string;
                    watch.LastAccessTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        public static bool WatchDirectoryLastAccess(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, bool? isLastAccessTime, DirectoryInfo info, bool? isDateOnly, bool? isTimeOnly)
        {
            bool ret = false;
            
            if (isLastAccessTime ?? false) 
            {
                if (watch.LastAccessTime == null)
                {
                    ret = true;
                    watch.LastAccessTime = GetDirectoryLastAccessTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                }
                else
                {
                    string pathType = "directory";
                    string checkTarget = "LastAccessTime";

                    string ret_string = GetDirectoryLastAccessTime(info,
                        isDateOnly ?? watch.IsDateOnly ?? false,
                        isTimeOnly ?? watch.IsTimeOnly ?? false);
                    ret = ret_string != watch.LastAccessTime;
                    dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                        $"{watch.LastAccessTime} -> {ret_string}" :
                        ret_string;
                    watch.LastAccessTime = ret_string;
                }
                watch.IsDateOnly ??= isDateOnly;
                watch.IsTimeOnly ??= isTimeOnly;
            }
            return ret;
        }

        #endregion
        #region Get method

        public static string GetFileCreationTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(path).CreationTime, isDateOnly, isTimeOnly);
        }
        public static string GetFileCreationTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.CreationTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryCreationTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new DirectoryInfo(path).CreationTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryCreationTime(DirectoryInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.CreationTime, isDateOnly, isTimeOnly);
        }

        public static string GetFileLastWriteTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(path).LastWriteTime, isDateOnly, isTimeOnly);
        }
        public static string GetFileLastWriteTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.LastWriteTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryLastWriteTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new DirectoryInfo(path).LastWriteTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryLastWriteTime(DirectoryInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.LastWriteTime, isDateOnly, isTimeOnly);
        }

        public static string GetFileLastAccessTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new FileInfo(path).LastAccessTime, isDateOnly, isTimeOnly);
        }
        public static string GetFileLastAccessTime(FileInfo info, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(info.LastAccessTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryLastAccessTime(string path, bool isDateOnly, bool isTimeOnly)
        {
            return DateToString(new DirectoryInfo(path).LastAccessTime, isDateOnly, isTimeOnly);
        }
        public static string GetDirectoryLastAccessTime(DirectoryInfo info, bool isDateOnly, bool isTimeOnly)
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
