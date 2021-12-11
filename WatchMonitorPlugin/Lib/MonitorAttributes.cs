using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorAttributes : MonitorBase
    {
        public override string CheckTarget { get { return "Attributes"; } }





        #region Simple

        public override bool Compare(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAttributes ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                bool[] retA = GetAttributes(monitoring.PathA);
                bool[] retB = GetAttributes(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool Watch(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAttributes ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    bool[] result = GetAttributes(monitoring.Path);
                    ret = !result.SequenceEqual(monitoring.Attributes ?? new bool[0] { });
                    if (monitoring.Attributes != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            string.Format("{0} -> {1}", ToReadable(monitoring.Attributes), ToReadable(result)) :
                            ToReadable(result);
                    }
                }
                else
                {
                    monitoring.Attributes = null;
                }
            }
            return ret;
        }

        #endregion


        #region Compare method

        public override bool CompareFile(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAttributes ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                bool[] retA = GetAttributes(monitoring.PathA);
                bool[] retB = GetAttributes(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        public override bool CompareDirectory(MonitoringCompare monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsAttributes ?? false)
            {
                if (!monitoring.TestExists_old()) { return false; }

                bool[] retA = GetAttributes(monitoring.PathA);
                bool[] retB = GetAttributes(monitoring.PathB);

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = ToReadable(retA);
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = ToReadable(retB);
                return retA.SequenceEqual(retB);
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAttributes ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    bool[] result = GetAttributes(monitoring.Path);
                    ret = !result.SequenceEqual(monitoring.Attributes ?? new bool[0] { });
                    if (monitoring.Attributes != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            string.Format("{0} -> {1}", ToReadable(monitoring.Attributes), ToReadable(result)) :
                            ToReadable(result);
                    }
                }
                else
                {
                    monitoring.Attributes = null;
                }
            }
            return ret;
        }

        public override bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAttributes ?? false)
            {
                if (monitoring.TestExists_old())
                {
                    bool[] result = GetAttributes(monitoring.Path);
                    ret = !result.SequenceEqual(monitoring.Attributes ?? new bool[0] { });
                    if (monitoring.Attributes != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            string.Format("{0} -> {1}", ToReadable(monitoring.Attributes), ToReadable(result)) :
                            ToReadable(result);
                    }
                }
                else
                {
                    monitoring.Attributes = null;
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
