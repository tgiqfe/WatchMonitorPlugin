using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class CompareFunctions
    {
        /// <summary>
        /// ファイルのCompareチェック
        /// </summary>
        /// <param name="targetA"></param>
        /// <param name="targetB"></param>
        /// <param name="dictionary"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        internal static bool CheckFile(MonitoredTarget targetA, MonitoredTarget targetB, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = true;

            if (targetA.IsCreationTime ?? false)
            {
                targetA.CheckCreationTime();
                targetB.CheckCreationTime();
                dictionary[$"{targetA.PathTypeName}_CreationTime_{serial}"] = targetA.CreationTime;
                dictionary[$"{targetB.PathTypeName}_CreationTime_{serial}"] = targetB.CreationTime;
                ret &= targetA.CreationTime == targetB.CreationTime;
            }
            if (targetA.IsLastWriteTime ?? false)
            {
                targetA.CheckLastWriteTime();
                targetB.CheckLastWriteTime();
                dictionary[$"{targetA.PathTypeName}_LastWriteTime_{serial}"] = targetA.LastWriteTime;
                dictionary[$"{targetB.PathTypeName}_LastWriteTime_{serial}"] = targetB.LastWriteTime;
                ret &= targetA.LastWriteTime == targetB.LastWriteTime;
            }
            if (targetA.IsLastAccessTime ?? false)
            {
                targetA.CheckLastAccessTime();
                targetB.CheckLastAccessTime();
                dictionary[$"{targetA.PathTypeName}_LastAccessTime_{serial}"] = targetA.LastAccessTime;
                dictionary[$"{targetB.PathTypeName}_LastAccessTime_{serial}"] = targetB.LastAccessTime;
                ret &= targetA.LastAccessTime == targetB.LastAccessTime;
            }
            if (targetA.IsAccess ?? false)
            {
                targetA.CheckAccess();
                targetB.CheckAccess();
                dictionary[$"{targetA.PathTypeName}_Access_{serial}"] = targetA.Access;
                dictionary[$"{targetB.PathTypeName}_Access_{serial}"] = targetB.Access;
                ret &= targetA.Access == targetB.Access;
            }
            if (targetA.IsOwner ?? false)
            {
                targetA.CheckOwner();
                targetB.CheckOwner();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Owner;
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Owner;
                ret &= targetA.Owner == targetB.Owner;
            }
            if (targetA.IsInherited ?? false)
            {
                targetA.CheckInherited();
                targetB.CheckInherited();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Inherited.ToString();
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Inherited.ToString();
                ret &= targetA.Owner == targetB.Owner;
            }
            if (targetA.IsAttributes ?? false)
            {
                targetA.CheckAttributes();
                targetB.CheckAttributes();
                dictionary[$"{targetA.PathTypeName}_Attributes_{serial}"] = MonitorFunctions.ToReadableAttributes(targetA.Attributes);
                dictionary[$"{targetB.PathTypeName}_Attributes_{serial}"] = MonitorFunctions.ToReadableAttributes(targetB.Attributes);
                ret &= targetA.Attributes.SequenceEqual(targetB.Attributes);
            }
            if (targetA.IsMD5Hash ?? false)
            {
                targetA.CheckMD5Hash();
                targetB.CheckMD5Hash();
                dictionary[$"{targetA.PathTypeName}_MD5Hash_{serial}"] = targetA.MD5Hash;
                dictionary[$"{targetB.PathTypeName}_MD5Hash_{serial}"] = targetB.MD5Hash;
                ret &= targetA.MD5Hash == targetB.MD5Hash;
            }
            if (targetA.IsSHA256Hash ?? false)
            {
                targetA.CheckSHA256Hash();
                targetB.CheckSHA256Hash();
                dictionary[$"{targetA.PathTypeName}_SHA256Hash_{serial}"] = targetA.SHA256Hash;
                dictionary[$"{targetB.PathTypeName}_SHA256Hash_{serial}"] = targetB.SHA256Hash;
                ret &= targetA.SHA256Hash == targetB.SHA256Hash;
            }
            if (targetA.IsSHA512Hash ?? false)
            {
                targetA.CheckSHA512Hash();
                targetB.CheckSHA512Hash();
                dictionary[$"{targetA.PathTypeName}_SHA512Hash_{serial}"] = targetA.SHA512Hash;
                dictionary[$"{targetB.PathTypeName}_SHA512Hash_{serial}"] = targetB.SHA512Hash;
                ret &= targetA.SHA512Hash == targetB.SHA512Hash;
            }
            if (targetA.IsSize ?? false)
            {
                targetA.CheckSize();
                targetB.CheckSize();
                dictionary[$"{targetA.PathTypeName}_Size_{serial}"] = targetA.Size.ToString();
                dictionary[$"{targetB.PathTypeName}_Size_{serial}"] = targetB.Size.ToString();
                ret &= targetA.SHA512Hash == targetB.SHA512Hash;
            }

            return ret;
        }

        /// <summary>
        /// ディレクトリのCompareチェック
        /// </summary>
        /// <param name="targetA"></param>
        /// <param name="targetB"></param>
        /// <param name="dictionary"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        internal static bool CheckDirectory(MonitoredTarget targetA, MonitoredTarget targetB, Dictionary<string, string> dictionary, int serial, int depth)
        {
            bool ret = false;

            if (targetA.IsCreationTime ?? false)
            {
                targetA.CheckCreationTime();
                targetB.CheckCreationTime();
                dictionary[$"{targetA.PathTypeName}_CreationTime_{serial}"] = targetA.CreationTime;
                dictionary[$"{targetB.PathTypeName}_CreationTime_{serial}"] = targetB.CreationTime;
                ret &= targetA.CreationTime == targetB.CreationTime;
            }
            if (targetA.IsLastWriteTime ?? false)
            {
                targetA.CheckLastWriteTime();
                targetB.CheckLastWriteTime();
                dictionary[$"{targetA.PathTypeName}_LastWriteTime_{serial}"] = targetA.LastWriteTime;
                dictionary[$"{targetB.PathTypeName}_LastWriteTime_{serial}"] = targetB.LastWriteTime;
                ret &= targetA.LastWriteTime == targetB.LastWriteTime;
            }
            if (targetA.IsLastAccessTime ?? false)
            {
                targetA.CheckLastAccessTime();
                targetB.CheckLastAccessTime();
                dictionary[$"{targetA.PathTypeName}_LastAccessTime_{serial}"] = targetA.LastAccessTime;
                dictionary[$"{targetB.PathTypeName}_LastAccessTime_{serial}"] = targetB.LastAccessTime;
                ret &= targetA.LastAccessTime == targetB.LastAccessTime;
            }
            if (targetA.IsAccess ?? false)
            {
                targetA.CheckAccess();
                targetB.CheckAccess();
                dictionary[$"{targetA.PathTypeName}_Access_{serial}"] = targetA.Access;
                dictionary[$"{targetB.PathTypeName}_Access_{serial}"] = targetB.Access;
                ret &= targetA.Access == targetB.Access;
            }
            if (targetA.IsOwner ?? false)
            {
                targetA.CheckOwner();
                targetB.CheckOwner();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Owner;
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Owner;
                ret &= targetA.Owner == targetB.Owner;
            }
            if (targetA.IsInherited ?? false)
            {
                targetA.CheckInherited();
                targetB.CheckInherited();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Inherited.ToString();
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Inherited.ToString();
                ret &= targetA.Owner == targetB.Owner;
            }
            if (targetA.IsAttributes ?? false)
            {
                targetA.CheckAttributes();
                targetB.CheckAttributes();
                dictionary[$"{targetA.PathTypeName}_Attributes_{serial}"] = MonitorFunctions.ToReadableAttributes(targetA.Attributes);
                dictionary[$"{targetB.PathTypeName}_Attributes_{serial}"] = MonitorFunctions.ToReadableAttributes(targetB.Attributes);
                ret &= targetA.Attributes.SequenceEqual(targetB.Attributes);
            }
            if ((targetA.IsChildCount ?? false) && depth == 0)
            {
                targetA.CheckChildCount();
                targetB.CheckChildCount();
                dictionary[$"{targetA.PathTypeName}_ChildCount_{serial}"] = MonitorFunctions.ToReadableChildCount(targetA.ChildCount, targetA.PathType == IO.Lib.PathType.Directory);
                dictionary[$"{targetB.PathTypeName}_ChildCount_{serial}"] = MonitorFunctions.ToReadableChildCount(targetB.ChildCount, targetB.PathType == IO.Lib.PathType.Directory);
                ret &= targetA.ChildCount.SequenceEqual(targetB.ChildCount);
            }

            return ret;
        }

        /// <summary>
        /// レジストリキーのCompareチェック
        /// </summary>
        /// <param name="targetA"></param>
        /// <param name="targetB"></param>
        /// <param name="dictionary"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        internal static bool CheckRegistryKey(MonitoredTarget targetA, MonitoredTarget targetB, Dictionary<string, string> dictionary, int serial, int depth)
        {
            bool ret = false;

            if (targetA.IsAccess ?? false)
            {
                targetA.CheckAccess();
                targetB.CheckAccess();
                dictionary[$"{targetA.PathTypeName}_Access_{serial}"] = targetA.Access;
                dictionary[$"{targetB.PathTypeName}_Access_{serial}"] = targetB.Access;
                ret &= targetA.Access == targetB.Access;
            }
            if (targetA.IsOwner ?? false)
            {
                targetA.CheckOwner();
                targetB.CheckOwner();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Owner;
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Owner;
                ret &= targetA.Owner == targetB.Owner;
            }
            if (targetA.IsInherited ?? false)
            {
                targetA.CheckInherited();
                targetB.CheckInherited();
                dictionary[$"{targetA.PathTypeName}_Owner_{serial}"] = targetA.Inherited.ToString();
                dictionary[$"{targetB.PathTypeName}_Owner_{serial}"] = targetB.Inherited.ToString();
                ret &= targetA.Owner == targetB.Owner;
            }
            if ((targetA.IsChildCount ?? false) && depth == 0)
            {
                targetA.CheckChildCount();
                targetB.CheckChildCount();
                dictionary[$"{targetA.PathTypeName}_ChildCount_{serial}"] = MonitorFunctions.ToReadableChildCount(targetA.ChildCount, targetA.PathType == IO.Lib.PathType.Directory);
                dictionary[$"{targetB.PathTypeName}_ChildCount_{serial}"] = MonitorFunctions.ToReadableChildCount(targetB.ChildCount, targetB.PathType == IO.Lib.PathType.Directory);
                ret &= targetA.ChildCount.SequenceEqual(targetB.ChildCount);
            }

            return ret;
        }

        /// <summary>
        /// レジストリ値のCompareチェック
        /// </summary>
        /// <param name="targetA"></param>
        /// <param name="targetB"></param>
        /// <param name="dictionary"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        internal static bool CheckRegistryValue(MonitoredTarget targetA, MonitoredTarget targetB, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            if (targetA.IsMD5Hash ?? false)
            {
                targetA.CheckMD5Hash();
                targetB.CheckMD5Hash();
                dictionary[$"{targetA.PathTypeName}_MD5Hash_{serial}"] = targetA.MD5Hash;
                dictionary[$"{targetB.PathTypeName}_MD5Hash_{serial}"] = targetB.MD5Hash;
                ret &= targetA.MD5Hash == targetB.MD5Hash;
            }
            if (targetA.IsSHA256Hash ?? false)
            {
                targetA.CheckSHA256Hash();
                targetB.CheckSHA256Hash();
                dictionary[$"{targetA.PathTypeName}_SHA256Hash_{serial}"] = targetA.SHA256Hash;
                dictionary[$"{targetB.PathTypeName}_SHA256Hash_{serial}"] = targetB.SHA256Hash;
                ret &= targetA.SHA256Hash == targetB.SHA256Hash;
            }
            if (targetA.IsSHA512Hash ?? false)
            {
                targetA.CheckSHA512Hash();
                targetB.CheckSHA512Hash();
                dictionary[$"{targetA.PathTypeName}_SHA512Hash_{serial}"] = targetA.SHA512Hash;
                dictionary[$"{targetB.PathTypeName}_SHA512Hash_{serial}"] = targetB.SHA512Hash;
                ret &= targetA.SHA512Hash == targetB.SHA512Hash;
            }
            if (targetA.IsRegistryType ?? false)
            {
                targetA.CheckRegistryType();
                targetB.CheckRegistryType();
                dictionary[$"{targetA.PathTypeName}_RegistryType_{serial}"] = targetA.RegistryType;
                dictionary[$"{targetB.PathTypeName}_RegistryType_{serial}"] = targetB.RegistryType;
                ret &= targetA.RegistryType == targetB.RegistryType;
            }

            return false;
        }
    }
}
