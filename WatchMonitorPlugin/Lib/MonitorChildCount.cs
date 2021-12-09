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
    internal class MonitorChildCount
    {
        #region Compare method

        public static bool CompareDirectory(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsChildCount ?? false)
            {
                if (!Directory.Exists(compare.PathA) || !Directory.Exists(compare.PathB)) { return false; }

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

        public static string ToReadableForDirectory(int[] ret_bools)
        {
            return string.Format("Directory:{0} / File:{1}",
                ret_bools[0],
                ret_bools[1]);
        }

        public static string ToReadableForRegistryKey(int[] ret_bools)
        {
            return string.Format("RegistryKey:{0} / Value:{1}",
                ret_bools[0],
                ret_bools[1]);
        }

        #endregion
    }
}
