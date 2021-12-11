using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using IO.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorChildCount : MonitorBase
    {
        public override string CheckTarget { get { return "ChildCount"; } }

        public override bool Compare(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsChildCount ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                int[] ret = GetChildCount(monitoring);
                int[] retA = { ret[0], ret[1] };
                int[] retB = { ret[2], ret[3] };

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA, monitoring.PathType ==PathType.Directory);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB, monitoring.PathType == PathType.Directory);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool Watch(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            bool isDirectory = monitoring.PathType == PathType.Directory;
            if (monitoring.IsChildCount ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    int[] result = GetChildCount(monitoring);
                    ret = !result.SequenceEqual(monitoring.ChildCount ?? new int[0] { });
                    if (monitoring.ChildCount != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            ToReadable(monitoring.ChildCount, isDirectory) + " -> " + ToReadable(result, isDirectory) :
                            ToReadable(result, isDirectory);
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

        #region Get method

        public static int[] GetChildCount(Monitoring monitoring)
        {
            if (monitoring.PathType == PathType.Directory)
            {
                if (monitoring is MonitoringCompare monCompare)
                {
                    return new int[4]
                    {
                        Directory.GetDirectories(monCompare.PathA, "*", SearchOption.AllDirectories).Length,
                        Directory.GetFiles(monCompare.PathA, "*", SearchOption.AllDirectories).Length,
                        Directory.GetDirectories(monCompare.PathB, "*", SearchOption.AllDirectories).Length,
                        Directory.GetFiles(monCompare.PathB, "*", SearchOption.AllDirectories).Length,
                    };
                }
                else if (monitoring is MonitoringWatch monWatch)
                {
                    return new int[2]
                    {
                        Directory.GetDirectories(monWatch.Path, "*", SearchOption.AllDirectories).Length,
                        Directory.GetFiles(monWatch.Path, "*", SearchOption.AllDirectories).Length,
                    };
                }
            }
            else if (monitoring.PathType == PathType.Registry)
            {
                if (monitoring is MonitoringCompare monCompare)
                {
                    int[] retA = GetRegistryKeyChildCount(monCompare.KeyA);
                    int[] retB = GetRegistryKeyChildCount(monCompare.KeyB);
                    return new int[4]
                    {
                        retA[0], retA[1], retB[2], retB[3]
                    };
                }
                else if (monitoring is MonitoringWatch monWatch)
                {
                    return GetRegistryKeyChildCount(monWatch.Key);
                }
            }
            return null;
        }


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
