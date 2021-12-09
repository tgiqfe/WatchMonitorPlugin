using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorChildCount : MonitorBase
    {
        public override string CheckTarget { get { return "ChildCount"; } }

        #region Compare method

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsChildCount ?? false)
            {
                if (!monitoring.InfoA.Exists || !monitoring.InfoB.Exists) { return false; }

                int[] retA = GetDirectoryChildCount(monitoring.PathA);
                int[] retB = GetDirectoryChildCount(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA, isDirectory: true);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB, isDirectory: true);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool CompareRegistryKey(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsChildCount ?? false)
            {
                if (monitoring.KeyA == null || monitoring.KeyB == null) { return false; }

                int[] retA = GetRegistryKeyChildCount(monitoring.KeyA);
                int[] retB = GetRegistryKeyChildCount(monitoring.KeyB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA, isDirectory: false);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB, isDirectory: false);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsChildCount ?? false)
            {
                if (monitoring.Info.Exists)
                {
                    int[] result = GetDirectoryChildCount(monitoring.Path);
                    ret = !result.SequenceEqual(monitoring.ChildCount ?? new int[0] { });
                    if (monitoring.ChildCount != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            ToReadable(monitoring.ChildCount, isDirectory: true) + " -> " + ToReadable(result, isDirectory: true) :
                            ToReadable(result, isDirectory: true);
                    }
                    monitoring.ChildCount = result;
                }
                else
                {
                    monitoring.ChildCount = null;
                }
            }
            return ret;
        }

        public override bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsChildCount ?? false)
            {
                if (monitoring.Key != null)
                {
                    int[] result = GetRegistryKeyChildCount(monitoring.Key);
                    ret = !result.SequenceEqual(monitoring.ChildCount ?? new int[0] { });
                    if (monitoring.ChildCount != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            ToReadable(monitoring.ChildCount, isDirectory: false) + " -> " + ToReadable(result, isDirectory: false) :
                            ToReadable(result, isDirectory: false);
                    }
                    monitoring.ChildCount = result;
                }
                else
                {
                    monitoring.ChildCount = null;
                }
            }
            return ret;
        }

        #endregion

        /*
        const string CHECK_TARGET = "ChildCount";

        #region Compare method

        public static bool CompareDirectory(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsChildCount ?? false)
            {
                if (!compare.InfoA.Exists || !compare.InfoB.Exists) { return false; }

                string pathType = "directory";
                string checkTarget = "childCount";

                int[] retA = GetDirectoryChildCount(compare.PathA);
                int[] retB = GetDirectoryChildCount(compare.PathB);

                dictionary[$"{pathType}A_{checkTarget}_{serial}"] = ToReadableForDirectory(retA);
                dictionary[$"{pathType}B_{checkTarget}_{serial}"] = ToReadableForDirectory(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public static bool CompareRegistryKey(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if(compare.IsChildCount ?? false)
            {
                if(compare.KeyA == null || compare.KeyB == null) { return false; }

                string pathType = "registry";
                string checkTarget = "childCount";

                int[] retA = GetRegistryKeyChildCount(compare.KeyA);
                int[] retB = GetRegistryKeyChildCount(compare.KeyB);

                dictionary[$"{pathType}A_{checkTarget}_{serial}"] = ToReadableForRegistryKey(retA);
                dictionary[$"{pathType}B_{checkTarget}_{serial}"] = ToReadableForRegistryKey(retB);
                return retA.SequenceEqual(retB);
            }
            return false;
        }

        #endregion
        #region Watch method

        public static bool WatchDirectory(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsChildCount ?? false)
            {
                if (Directory.Exists(path))
                {
                    int[] ret_integers = GetDirectoryChildCount(path);
                    ret = !ret_integers.SequenceEqual(watch.ChildCount ?? new int[0] { });
                    if (watch.ChildCount != null)
                    {
                        string pathType = "directory";
                        string checkTarget = "ChildCount";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            string.Format("Directory:{0} / File:{1} -> Directory:{2} / File:{3}",
                                watch.ChildCount[0],
                                watch.ChildCount[1],
                                ret_integers[0],
                                ret_integers[1]) :
                            string.Format("Directory:{0} / File:{1}",
                                ret_integers[0],
                                ret_integers[1]);
                    }
                    watch.ChildCount = ret_integers;
                }
                else
                {
                    watch.ChildCount = null;
                }
            }
            return ret;
        }

        public static bool WatchRegistryKey(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, RegistryKey regKey)
        {
            bool ret = false;
            if (watch.IsChildCount ?? false)
            {
                if (regKey != null)
                {
                    int[] ret_integers = GetRegistryKeyChildCount(regKey);
                    ret = !ret_integers.SequenceEqual(watch.ChildCount ?? new int[0] { });
                    if (watch.ChildCount != null)
                    {
                        string pathType = "registry";
                        string checkTarget = "ChildCount";
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            string.Format("RegistryKey:{0} / Value:{1} -> RegistryKey:{2} / Value:{3}",
                                watch.ChildCount[0],
                                watch.ChildCount[1],
                                ret_integers[0],
                                ret_integers[1]) :
                            string.Format("RegistryKey:{0} / Value:{1}",
                                ret_integers[0],
                                ret_integers[1]);
                    }
                    watch.ChildCount = ret_integers;
                }
                else
                {
                    watch.ChildCount = null;
                }
            }
            return ret;
        }

        #endregion

        */

        #region Get method

        /// <summary>
        /// 配下のファイルとディレクトリの総数を返す。
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// index 0 ⇒ ディレクトリ総数
        /// index 1 ⇒ ファイル総数
        /// </returns>
        public static int[] GetDirectoryChildCount(string path)
        {
            return new int[2]
            {
                Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Length,
                Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length
            };
        }

        /// <summary>
        /// 配下のレジストリ値とキーの総数を返す
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// index 0 ⇒ キー総数
        /// index 1 ⇒ レジストリ値総数
        /// </returns>
        public static int[] GetRegistryKeyChildCount(RegistryKey regKey)
        {
            string[] childKeys = regKey.GetSubKeyNames();
            int[] ret_childCounts = new int[]
            {
                childKeys.Length,
                regKey.GetValueNames().Length
            };

            foreach (string childKey in childKeys)
            {
                using (RegistryKey regChildKey = regKey.OpenSubKey(childKey, false))
                {
                    int[] childCounts = GetRegistryKeyChildCount(regChildKey);
                    ret_childCounts[0] += childCounts[0];
                    ret_childCounts[1] += childCounts[1];
                }
            }

            return ret_childCounts;
        }

        /// <summary>
        /// 配下のフォルダー/ファイル/キー/値の数を、Directory/RegistryKey別に返す。
        /// </summary>
        /// <param name="ret_bools"></param>
        /// <param name="isDirectory"></param>
        /// <returns></returns>
        public static string ToReadable(int[] ret_bools, bool isDirectory)
        {
            return isDirectory ?
                string.Format("Directory:{0} / File:{1}", ret_bools[0], ret_bools[1]) :
                string.Format("RegistryKey:{0} / Value:{1}", ret_bools[0], ret_bools[1]);

        }

        #endregion
    }
}
