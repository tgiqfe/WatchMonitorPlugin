using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.Lib;


namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class CompareFile
    {
        public string _PathA { get; set; }
        public string _PathB { get; set; }

        public bool? _IsCreationTime { get; set; }
        public bool? _IsLastWriteTime { get; set; }
        public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        public bool? _IsSize { get; set; }
        //public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        protected bool Success { get; set; }
        private int _serial = 1;

        private MonitoredTarget CreateForFile(string path, string pathTypeName)
        {
            return new MonitoredTarget(IO.Lib.PathType.File, path)
            {
                PathTypeName = pathTypeName,
                IsCreationTime = _IsCreationTime,
                IsLastWriteTime = _IsLastWriteTime,
                IsLastAccessTime = _IsLastAccessTime,
                IsAccess = _IsAccess,
                IsOwner = _IsOwner,
                IsInherited = _IsInherited,
                IsAttributes = _IsAttributes,
                IsMD5Hash = _IsMD5Hash,
                IsSHA256Hash = _IsSHA256Hash,
                IsSHA512Hash = _IsSHA512Hash,
                IsSize = _IsSize,
                IsDateOnly = _IsDateOnly,
                IsTimeOnly = _IsTimeOnly,
            };
        }

        public void MainProcess()
        {
            var dictionary = new Dictionary<string, string>();
            this.Success = true;

            MonitoredTarget targetA = CreateForFile(_PathA, "fileA");
            MonitoredTarget targetB = CreateForFile(_PathB, "fileB");
            targetA.CheckExists();
            targetB.CheckExists();

            if ((targetA.Exists ?? false) && (targetB.Exists ?? false))
            {
                if (_IsCreationTime ?? false)
                {
                    targetA.CheckCreationTime();
                    targetB.CheckCreationTime();
                    dictionary[$"{targetA.PathTypeName}_CreationTime_{_serial}"] = targetA.CreationTime;
                    dictionary[$"{targetA.PathTypeName}_CreationTime_{_serial}"] = targetB.CreationTime;
                    Success &= targetA.CreationTime == targetB.CreationTime;
                }
                if (_IsLastWriteTime ?? false)
                {
                    targetA.CheckLastWriteTime();
                    targetB.CheckLastWriteTime();
                    dictionary[$"{targetA.PathTypeName}_LastWriteTime_{_serial}"] = targetA.LastWriteTime;
                    dictionary[$"{targetA.PathTypeName}_LastWriteTime_{_serial}"] = targetB.LastWriteTime;
                    Success &= targetA.LastWriteTime == targetB.LastWriteTime;
                }
                if (_IsLastAccessTime ?? false)
                {
                    targetA.CheckLastAccessTime();
                    targetB.CheckLastAccessTime();
                    dictionary[$"{targetA.PathTypeName}_LastAccessTime_{_serial}"] = targetA.LastAccessTime;
                    dictionary[$"{targetA.PathTypeName}_LastAccessTime_{_serial}"] = targetB.LastAccessTime;
                    Success &= targetA.LastAccessTime == targetB.LastAccessTime;
                }
                if (_IsAccess ?? false)
                {
                    targetA.CheckAccess();
                    targetB.CheckAccess();
                    dictionary[$"{targetA.PathTypeName}_Access_{_serial}"] = targetA.Access;
                    dictionary[$"{targetA.PathTypeName}_Access_{_serial}"] = targetB.Access;
                    Success &= targetA.Access == targetB.Access;
                }
                if (_IsOwner ?? false)
                {
                    targetA.CheckOwner();
                    targetB.CheckOwner();
                    dictionary[$"{targetA.PathTypeName}_Owner_{_serial}"] = targetA.Owner;
                    dictionary[$"{targetA.PathTypeName}_Owner_{_serial}"] = targetB.Owner;
                    Success &= targetA.Owner == targetB.Owner;
                }
                if (_IsInherited ?? false)
                {
                    targetA.CheckInherited();
                    targetB.CheckInherited();
                    dictionary[$"{targetA.PathTypeName}_Owner_{_serial}"] = targetA.Inherited.ToString();
                    dictionary[$"{targetA.PathTypeName}_Owner_{_serial}"] = targetB.Inherited.ToString();
                    Success &= targetA.Owner == targetB.Owner;
                }
                if (_IsAttributes ?? false)
                {
                    targetA.CheckAttributes();
                    targetB.CheckAttributes();
                    dictionary[$"{targetA.PathTypeName}_Attributes_{_serial}"] = MonitorFunctions.ToReadable(targetA.Attributes);
                    dictionary[$"{targetA.PathTypeName}_Attributes_{_serial}"] = MonitorFunctions.ToReadable(targetB.Attributes);
                    Success &= targetA.Attributes.SequenceEqual(targetB.Attributes);
                }
                if (_IsMD5Hash ?? false)
                {
                    targetA.CheckMD5Hash();
                    targetB.CheckMD5Hash();
                    dictionary[$"{targetA.PathTypeName}_MD5Hash_{_serial}"] = targetA.MD5Hash;
                    dictionary[$"{targetA.PathTypeName}_MD5Hash_{_serial}"] = targetB.MD5Hash;
                    Success &= targetA.MD5Hash == targetB.MD5Hash;
                }
                if (_IsSHA256Hash ?? false)
                {
                    targetA.CheckSHA256Hash();
                    targetB.CheckSHA256Hash();
                    dictionary[$"{targetA.PathTypeName}_SHA256Hash_{_serial}"] = targetA.SHA256Hash;
                    dictionary[$"{targetA.PathTypeName}_SHA256Hash_{_serial}"] = targetB.SHA256Hash;
                    Success &= targetA.SHA256Hash == targetB.SHA256Hash;
                }
                if (_IsSHA512Hash ?? false)
                {
                    targetA.CheckSHA512Hash();
                    targetB.CheckSHA512Hash();
                    dictionary[$"{targetA.PathTypeName}_SHA512Hash_{_serial}"] = targetA.SHA512Hash;
                    dictionary[$"{targetA.PathTypeName}_SHA512Hash_{_serial}"] = targetB.SHA512Hash;
                    Success &= targetA.SHA512Hash == targetB.SHA512Hash;
                }
                if (_IsSize ?? false)
                {
                    targetA.CheckSize();
                    targetB.CheckSize();
                    dictionary[$"{targetA.PathTypeName}_Size_{_serial}"] = targetA.Size.ToString();
                    dictionary[$"{targetA.PathTypeName}_Size_{_serial}"] = targetB.Size.ToString();
                    Success &= targetA.SHA512Hash == targetB.SHA512Hash;
                }



            }

        }
    }
}
