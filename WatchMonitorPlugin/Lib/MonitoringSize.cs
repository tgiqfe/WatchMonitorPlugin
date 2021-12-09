using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IO.Lib;
using Audit.Lib;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitoringSize : MonitorBase
    {
        public override string CheckTarget { get { return "Size"; } }

        #region Compare method

        public override bool CompareFile(CompareMonitoring compare, Dictionary<string, string> dictionary, int serial)
        {
            if (compare.IsSize ?? false)
            {
                if (!compare.InfoA.Exists || !compare.InfoB.Exists) { return false; }

                long retA = ((FileInfo)compare.InfoA).Length;
                long retB = ((FileInfo)compare.InfoB).Length;

                dictionary[$"{compare.PathTypeName}A_{this.CheckTarget}_{serial}"] = string.Format("{0} ({1})", retA, ToReadable(retA));
                dictionary[$"{compare.PathTypeName}B_{this.CheckTarget}_{serial}"] = string.Format("{0} ({1})", retB, ToReadable(retB));
                return retA == retB;
            }
            return true;
        }

        #endregion
        #region Watch method

        public override bool WatchFile(WatchMonitoring watch, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;
            if (watch.IsAccess ?? false)
            {
                if (watch.Info.Exists)
                {
                    long result =((FileInfo)watch.Info).Length;
                    ret = result != watch.Size;
                    if(watch.Size != null)
                    {
                        dictionary[$"{watch.PathTypeName}_{this.CheckTarget}_{serial}"] = ret ?
                            string.Format("{0} -> {1} ({2})", watch.Size, result, ToReadable(result)) :
                            string.Format("{0} ({1})", result, ToReadable(result));
                    }
                    watch.Size = result;
                }
                else
                {
                    watch.Size = null;
                }
            }
            return ret;
        }

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
