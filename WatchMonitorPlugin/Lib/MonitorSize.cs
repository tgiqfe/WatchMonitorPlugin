using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorSize : MonitorBase
    {
        public override string CheckTarget { get { return "Size"; } }

        #region Compare method

        public override bool CompareFile(CompareMonitoring monitoring, Dictionary<string, string> dictionary, int serial)
        {
            if (monitoring.IsSize ?? false)
            {
                if (!monitoring.InfoA.Exists || !monitoring.InfoB.Exists) { return false; }

                long retA = ((FileInfo)monitoring.InfoA).Length;
                long retB = ((FileInfo)monitoring.InfoB).Length;

                dictionary[$"{monitoring.PathTypeName}A_{this.CheckTarget}_{serial}"] = string.Format("{0} ({1})", retA, ToReadable(retA));
                dictionary[$"{monitoring.PathTypeName}B_{this.CheckTarget}_{serial}"] = string.Format("{0} ({1})", retB, ToReadable(retB));
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(WatchMonitoring monitoring, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (monitoring.IsAccess ?? false)
            {
                if (monitoring.Info.Exists)
                {
                    long result = ((FileInfo)monitoring.Info).Length;
                    ret = result != monitoring.Size;
                    if (monitoring.Size != null)
                    {
                        dictionary[$"{monitoring.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            string.Format("{0} -> {1} ({2})", monitoring.Size, result, ToReadable(result)) :
                            string.Format("{0} ({1})", result, ToReadable(result));
                    }
                    monitoring.Size = result;
                }
                else
                {
                    monitoring.Size = null;
                }
            }
            return ret;
        }


        /*

        const string CHECK_TARGET = "size";

        #region Compare method

        public static bool CompareFile(ComparePath compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsSize ?? false)
            {
                if (!compare.InfoA.Exists || !compare.InfoB.Exists) { return false; }

                long retA = ((FileInfo)compare.InfoA).Length;
                long retB = ((FileInfo)compare.InfoB).Length;

                dictionary[$"{compare.PathTypeName}A_{CHECK_TARGET}_{serial}"] = string.Format("{0} ({1})", retA, ToReadable(retA));
                dictionary[$"{compare.PathTypeName}B_{CHECK_TARGET}_{serial}"] = string.Format("{0} ({1})", retB, ToReadable(retB));
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method

        public static bool WatchFile(
            WatchPath watch, Dictionary<string, string> dictionary, int serial, FileInfo info)
        {
            bool ret = false;
            if (watch.IsSize ?? false)
            {
                if (info.Exists)
                {
                    long ret_long = info.Length;
                    ret = ret_long != watch.Size;
                    if (watch.Size != null)
                    {
                        dictionary[$"{watch.PathTypeName}_{CHECK_TARGET}_{serial}"] = ret ?
                            string.Format("{0} -> {1} ({2})", watch.Size, ret_long, ToReadable(ret_long)) :
                            string.Format("{0} ({1})", ret_long, ToReadable(ret_long));
                    }
                    watch.Size = ret_long;
                }
                else
                {
                    watch.Size = null;
                }
            }
            return ret;
        }

        */

        #endregion
        #region Get method

        public static string ToReadable(long size)
        {
            if (size < 1024 * 0.9)
            {
                //  byte
                return $"{size} byte";
            }
            else if (size < 1024 * 1024 * 0.9)
            {
                //  KB
                return $"{Math.Round(size / 1024.0, 2, MidpointRounding.AwayFromZero)} KB";
            }
            else if (size < 1024 * 1024 * 1024 * 0.9)
            {
                //  MB
                return $"{Math.Round(size / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} MB";
            }
            else if (size < (1024L * 1024L * 1024L * 1024L * 0.9))
            {
                //  GB
                return $"{Math.Round(size / 1024.0 / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} GB";
            }
            else
            {
                //  TB
                return $"{Math.Round(size / 1024.0 / 1024.0 / 1024.0 / 1024.0, 2, MidpointRounding.AwayFromZero)} TB";
            }
        }

        #endregion
    }
}
