using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class WatchFile_kari
    {
        internal static bool Check(MonitoredTarget target_monitor, MonitoredTarget target_db, Dictionary<string, string> dictionary, int serial)
        {
            bool ret = false;

            //  CreationTime
            if (target_monitor.IsCreationTime ?? false)
            {
                target_monitor.CheckCreationTime();
                ret = target_monitor.CreationTime != target_db.CreationTime;
                if (target_db.CreationTime != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_CreationTime_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.CreationTime,
                            target_monitor.CreationTime) :
                        target_monitor.CreationTime;
                }
            }

            //  LastWriteTime
            if (target_monitor.IsLastWriteTime ?? false)
            {
                target_monitor.CheckLastWriteTime();
                ret = target_monitor.LastWriteTime != target_db.LastWriteTime;
                if (target_db.LastWriteTime != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_LastWriteTime_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.LastWriteTime,
                            target_monitor.LastWriteTime) :
                        target_monitor.LastWriteTime;
                }
            }

            //  LastAccessTime
            if (target_monitor.IsLastAccessTime ?? false)
            {
                target_monitor.CheckLastAccessTime();
                ret = target_monitor.LastAccessTime != target_db.LastAccessTime;
                if (target_db.LastAccessTime != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_LastAccessTime_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.LastAccessTime,
                            target_monitor.LastAccessTime) :
                        target_monitor.LastAccessTime;
                }
            }

            //  Access
            if (target_monitor.IsAccess ?? false)
            {
                target_monitor.CheckAccess();
                ret = target_monitor.Access != target_db.Access;
                if (target_db.Access != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_Access_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.Access,
                            target_monitor.Access) :
                        target_monitor.Access;
                }
            }

            //  Owner
            if (target_monitor.IsOwner ?? false)
            {
                target_monitor.CheckOwner();
                ret = target_monitor.Owner != target_db.Owner;
                if (target_db.Owner != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_Owner_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.Owner,
                            target_monitor.Owner) :
                        target_monitor.Owner;
                }
            }

            //  Inherited
            if (target_monitor.IsInherited ?? false)
            {
                target_monitor.CheckInherited();
                ret = target_monitor.Inherited != target_db.Inherited;
                if (target_db.Inherited != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_Inherited_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.Inherited,
                            target_monitor.Inherited) :
                        target_monitor.Inherited.ToString();
                }
            }

            //  Attributes
            if (target_monitor.IsAttributes ?? false)
            {
                target_monitor.CheckAttributes();
                ret = !target_monitor.Attributes.SequenceEqual(target_db.Attributes);
                if (target_db.Attributes != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_Attributes_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            MonitorFunctions.ToReadable(target_db.Attributes),
                            MonitorFunctions.ToReadable(target_monitor.Attributes)) :
                        MonitorFunctions.ToReadable(target_monitor.Attributes);
                }
            }

            //  MD5Hash
            if (target_monitor.IsMD5Hash ?? false)
            {
                target_monitor.CheckMD5Hash();
                ret = target_monitor.MD5Hash != target_db.MD5Hash;
                if (target_db.MD5Hash != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_MD5Hash_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.MD5Hash,
                            target_monitor.MD5Hash) :
                        target_monitor.MD5Hash;
                }
            }

            //  SHA256Hash
            if (target_monitor.IsSHA256Hash ?? false)
            {
                target_monitor.CheckSHA256Hash();
                ret = target_monitor.SHA256Hash != target_db.SHA256Hash;
                if (target_db.SHA256Hash != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_SHA256Hash_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.SHA256Hash,
                            target_monitor.SHA256Hash) :
                        target_monitor.SHA256Hash;
                }
            }

            //  SHA512Hash
            if (target_monitor.IsSHA512Hash ?? false)
            {
                target_monitor.CheckSHA512Hash();
                ret = target_monitor.SHA512Hash != target_db.SHA512Hash;
                if (target_db.SHA512Hash != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_SHA512Hash_{serial}"] = ret ?
                        string.Format("{0} -> {1}",
                            target_db.SHA512Hash,
                            target_monitor.SHA512Hash) :
                        target_monitor.SHA512Hash;
                }
            }

            //  Size
            if (target_monitor.IsSize ?? false)
            {
                target_monitor.CheckSize();
                ret = target_monitor.Size != target_db.Size;
                if (target_db.Size != null)
                {
                    dictionary[$"{target_monitor.PathTypeName}_Size_{serial}"] = ret ?
                        string.Format("{0}({1}) -> {2}({3})",
                            target_db.Size,
                            MonitorFunctions.ToReadable(target_db.Size ?? 0),
                            target_monitor.Size,
                            MonitorFunctions.ToReadable(target_monitor.Size ?? 0)) :
                        string.Format("{0}({1})",
                            target_monitor.Size,
                            MonitorFunctions.ToReadable(target_monitor.Size ?? 0));
                }
            }

            return ret;
        }
    }
}
