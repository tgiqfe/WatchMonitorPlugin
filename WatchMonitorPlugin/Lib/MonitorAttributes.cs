using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorAttributes
    {
        const string CHECK_TARGET = "Attribute";

        #region Compare method

        public static bool CompareFile(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsAttributes ?? false)
            {
                if(!compare.InfoA.Exists || !compare.InfoB.Exists) { return false; }
                
                bool[] retA = GetAttributes(compare.PathA);
                bool[] retB = GetAttributes(compare.PathB);

                dictionary[$"{compare.PathTypeName}A_{CHECK_TARGET}_{serial}"] = ToReadable(retA);
                dictionary[$"{compare.PathTypeName}B_{CHECK_TARGET}_{serial}"] = ToReadable(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public static bool CompareDirectory(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsAttributes ?? false)
            {
                if (!compare.InfoA.Exists || !compare.InfoB.Exists) { return false; }

                bool[] retA = GetAttributes(compare.PathA);
                bool[] retB = GetAttributes(compare.PathB);

                dictionary[$"{compare.PathTypeName}A_{CHECK_TARGET}_{serial}"] = ToReadable(retA);
                dictionary[$"{compare.PathTypeName}B_{CHECK_TARGET}_{serial}"] = ToReadable(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public static bool WatchFile(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsAttributes ?? false)
            {
                if (File.Exists(path))
                {
                    bool[] ret_bools = GetAttributes(path);
                    ret = !ret_bools.SequenceEqual(watch.Attributes ?? new bool[0] { });
                    if (watch.Attributes != null)
                    {
                        /*
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            string.Format(
                                "[{0}]Readonly [{1}]Hidden [{2}]System -> [{3}]Readonly [{4}]Hidden [{5}]System",
                                watch.Attributes[0] ? "x" : " ",
                                watch.Attributes[1] ? "x" : " ",
                                watch.Attributes[2] ? "x" : " ",
                                ret_bools[0] ? "x" : " ",
                                ret_bools[1] ? "x" : " ",
                                ret_bools[2] ? "x" : " ") :
                            string.Format(
                                "[{0}]Readonly [{1}]Hidden [{2}]System",
                                ret_bools[0] ? "x" : " ",
                                ret_bools[1] ? "x" : " ",
                                ret_bools[2] ? "x" : " ");
                        */
                        dictionary[$"{watch.PathTypeName}_{CHECK_TARGET}_{serial}"] = ret ?
                            ToReadable(watch.Attributes) + " -> " + ToReadable(ret_bools) :
                            ToReadable(ret_bools);
                    }
                    watch.Attributes = ret_bools;
                }
                else
                {
                    watch.Attributes = null;
                }
            }
            return ret;
        }

        public static bool WatchDirectory(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, string path)
        {
            bool ret = false;
            if (watch.IsAttributes ?? false)
            {
                if (Directory.Exists(path))
                {
                    bool[] ret_bools = GetAttributes(path);
                    ret = !ret_bools.SequenceEqual(watch.Attributes ?? new bool[0] { });
                    if (watch.Attributes != null)
                    {
                        /*
                        dictionary[$"{pathType}_{checkTarget}_{serial}"] = ret ?
                            string.Format(
                                "[{0}]Readonly [{1}]Hidden [{2}]System -> [{3}]Readonly [{4}]Hidden [{5}]System",
                                watch.Attributes[0] ? "x" : " ",
                                watch.Attributes[1] ? "x" : " ",
                                watch.Attributes[2] ? "x" : " ",
                                ret_bools[0] ? "x" : " ",
                                ret_bools[1] ? "x" : " ",
                                ret_bools[2] ? "x" : " ") :
                            string.Format(
                                "[{0}]Readonly [{1}]Hidden [{2}]System",
                                ret_bools[0] ? "x" : " ",
                                ret_bools[1] ? "x" : " ",
                                ret_bools[2] ? "x" : " ");
                        */
                        dictionary[$"{watch.PathTypeName}_{CHECK_TARGET}_{serial}"] = ret ?
                            ToReadable(watch.Attributes) + " -> " + ToReadable(ret_bools) :
                            ToReadable(ret_bools);
                    }
                    watch.Attributes = ret_bools;
                }
                else
                {
                    watch.Attributes = null;
                }
            }
            return ret;
        }

        #endregion
        #region Get method

        /// <summary>
        /// ファイルorディレクトリの属性を取得。
        /// ファイルの場合もディレクトリの場合も、共通して使用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool[] GetAttributes(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return new bool[]
            {
                (attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly,
                (attr & FileAttributes.Hidden) == FileAttributes.Hidden,
                (attr & FileAttributes.System) == FileAttributes.System
            };
        }

        public static string ToReadable(bool[] ret_bools)
        {
            return string.Format(
                "[{0}]Readonly [{1}]Hidden [{2}]System",
                ret_bools[0] ? "x" : " ",
                ret_bools[1] ? "x" : " ",
                ret_bools[2] ? "x" : " ");
        }

        #endregion
    }
}
