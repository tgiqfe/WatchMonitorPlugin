using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class MonitoredTarget
    {
        public PathType PathType { get; set; }
        public string FullPath { get; set; }

        public string CreationTime { get; set; }
        public string LastWriteTime { get; set; }
        public string LastAccessTime { get; set; }
        public string Access { get; set; }
        public string Owner { get; set; }
        public bool? Inherited { get; set; }
        public bool[] Attributes { get; set; }
        public string MD5Hash { get; set; }
        public string SHA256Hash { get; set; }
        public string SHA512Hash { get; set; }
        public long? Size { get; set; }
        public int[] ChildCount { get; set; }
        public string RegistryType { get; set; }

        public bool? Exists { get; set; }

        public bool? IsCreationTime { get; set; }
        public bool? IsLastWriteTime { get; set; }
        public bool? IsLastAccessTime { get; set; }
        public bool? IsAccess { get; set; }
        public bool? IsOwner { get; set; }
        public bool? IsInherited { get; set; }
        public bool? IsAttributes { get; set; }
        public bool? IsMD5Hash { get; set; }
        public bool? IsSHA256Hash { get; set; }
        public bool? IsSHA512Hash { get; set; }
        public bool? IsSize { get; set; }
        public bool? IsChildCount { get; set; }
        public bool? IsRegistryType { get; set; }
        public bool? IsDateOnly { get; set; }
        public bool? IsTimeOnly { get; set; }

        public const string REGPATH_PREFIX = "[registry]";

        #region Target path


        [JsonIgnore]
        public string PathTypeName { get; set; }

        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public RegistryKey Key { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

        private FileInfo _FileInfo = null;
        [JsonIgnore]
        public FileInfo FileInfo
        {
            get
            {
                if (_FileInfo == null && this.PathType == PathType.File)
                {
                    this._FileInfo = new FileInfo(Path);
                }
                return _FileInfo;
            }
        }
        private DirectoryInfo _DirectoryInfo = null;
        [JsonIgnore]
        public DirectoryInfo DirectoryInfo
        {
            get
            {
                if (_DirectoryInfo == null && this.PathType == PathType.Directory)
                {
                    this._DirectoryInfo = new DirectoryInfo(Path);
                }
                return _DirectoryInfo;
            }
        }

        #endregion

        public MonitoredTarget(PathType pathType, string path)
        {
            this.PathType = pathType;
            this.Path = path;
        }
        public MonitoredTarget(PathType pathType, string path, string name)
        {
            this.PathType = pathType;
            this.Path = path;
            this.Name = name;
        }

        #region TestExists

        public void CheckExists()
        {
            this.Exists = this.PathType switch
            {
                PathType.File => FileInfo.Exists,
                PathType.Directory => DirectoryInfo.Exists,
                PathType.Registry => this.Name == null ?
                    Key != null :
                    Key?.GetValueNames().Any(x => x.Equals(Name, StringComparison.OrdinalIgnoreCase)) ?? false,
                _ => null,
            };
        }

        #endregion

        public void CheckCreationTime()
        {
            this.CreationTime = PathType switch
            {
                PathType.File => MonitorFunctions.GetCreationTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                PathType.Directory => MonitorFunctions.GetCreationTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                _ => null
            };
        }
        public void CheckLastWriteTime()
        {
            this.LastWriteTime = PathType switch
            {
                PathType.File => MonitorFunctions.GetLastWriteTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                PathType.Directory => MonitorFunctions.GetLastWriteTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                _ => null
            };
        }
        public void CheckLastAccessTime()
        {
            this.LastAccessTime = PathType switch
            {
                PathType.File => MonitorFunctions.GetLastAccessTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                PathType.Directory => MonitorFunctions.GetLastAccessTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false),
                _ => null
            };
        }
        public void CheckAccess()
        {
            this.Access = PathType switch
            {
                PathType.File => AccessRuleSummary.FileToAccessString(this.FileInfo),
                PathType.Directory => AccessRuleSummary.DirectoryToAccessString(this.DirectoryInfo),
                PathType.Registry => AccessRuleSummary.RegistryKeyToAccessString(this.Key),
                _ => null
            };
        }
        public void CheckOwner()
        {
            this.Owner = PathType switch
            {
                PathType.File => this.FileInfo.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Directory => this.DirectoryInfo.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                PathType.Registry => this.Key.GetAccessControl().GetOwner(typeof(NTAccount)).Value,
                _ => null
            };
        }
        public void CheckInherited()
        {
            this.Inherited = PathType switch
            {
                PathType.File => !this.FileInfo.GetAccessControl().AreAccessRulesProtected,
                PathType.Directory => !this.DirectoryInfo.GetAccessControl().AreAccessRulesProtected,
                PathType.Registry => !this.Key.GetAccessControl().AreAccessRulesProtected,
                _ => null
            };
        }
        public void CheckAttributes()
        {
            this.Attributes = PathType switch
            {
                PathType.File => MonitorFunctions.GetAttributes(this.Path),
                PathType.Directory => MonitorFunctions.GetAttributes(this.Path),
                _ => null
            };
        }
        public void CheckMD5Hash()
        {
            this.MD5Hash = PathType switch
            {
                PathType.File => MonitorFunctions.GetHash(this.Path, MD5.Create()),
                PathType.Registry => MonitorFunctions.GetHash(this.Key, this.Name, MD5.Create()),
                _ => null
            };
        }
        public void CheckSHA256Hash()
        {
            this.SHA256Hash = PathType switch
            {
                PathType.File => MonitorFunctions.GetHash(this.Path, SHA256.Create()),
                PathType.Registry => MonitorFunctions.GetHash(this.Key, this.Name, SHA256.Create()),
                _ => null
            };
        }
        public void CheckSHA512Hash()
        {
            this.SHA512Hash = PathType switch
            {
                PathType.File => MonitorFunctions.GetHash(this.Path, SHA512.Create()),
                PathType.Registry => MonitorFunctions.GetHash(this.Key, this.Name, SHA512.Create()),
                _ => null
            };
        }
        public void CheckSize()
        {
            this.Size = PathType switch
            {
                PathType.File => this.FileInfo.Length,
                _ => null
            };
        }
        public void CheckChildCount()
        {
            this.ChildCount = PathType switch
            {
                PathType.Directory => MonitorFunctions.GetDirectoryChildCount(this.Path),
                PathType.Registry => MonitorFunctions.GetRegistryKeyChildCount(this.Key),
                _ => null
            };
        }
        public void CheckRegistryType()
        {
            this.RegistryType = PathType switch
            {
                PathType.Registry => RegistryControl.ValueKindToString(this.Key.GetValueKind(this.Name)),
                _ => null
            };
        }



        public void CheckFile()
        {
            if (IsCreationTime ?? false) { this.CreationTime = MonitorFunctions.GetCreationTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsLastWriteTime ?? false) { this.LastWriteTime = MonitorFunctions.GetLastWriteTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsLastAccessTime ?? false) { this.LastAccessTime = MonitorFunctions.GetLastAccessTime(this.FileInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsAccess ?? false) { this.Access = AccessRuleSummary.FileToAccessString(this.FileInfo); }
            if (IsOwner ?? false) { this.Owner = this.FileInfo.GetAccessControl().GetOwner(typeof(NTAccount)).Value; }
            if (IsInherited ?? false) { this.Inherited = !this.FileInfo.GetAccessControl().AreAccessRulesProtected; }
            if (IsAttributes ?? false) { this.Attributes = MonitorFunctions.GetAttributes(this.Path); }
            if (IsMD5Hash ?? false) { this.MD5Hash = MonitorFunctions.GetHash(this.Path, MD5.Create()); }
            if (IsSHA256Hash ?? false) { this.SHA256Hash = MonitorFunctions.GetHash(this.Path, SHA256.Create()); }
            if (IsSHA512Hash ?? false) { this.SHA512Hash = MonitorFunctions.GetHash(this.Path, SHA512.Create()); }
            if (IsSize ?? false) { this.Size = this.FileInfo.Length; }
            //if (IsChildCount ?? false) { }
            //if (IsRegistryType ?? false) { }
        }

        public void CheckDirectory()
        {
            if (IsCreationTime ?? false) { this.CreationTime = MonitorFunctions.GetCreationTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsLastWriteTime ?? false) { this.LastWriteTime = MonitorFunctions.GetLastWriteTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsLastAccessTime ?? false) { this.LastAccessTime = MonitorFunctions.GetLastAccessTime(this.DirectoryInfo, this.IsDateOnly ?? false, this.IsTimeOnly ?? false); }
            if (IsAccess ?? false) { this.Access = AccessRuleSummary.DirectoryToAccessString(this.DirectoryInfo); }
            if (IsOwner ?? false) { this.Owner = this.DirectoryInfo.GetAccessControl().GetOwner(typeof(NTAccount)).Value; }
            if (IsInherited ?? false) { this.Inherited = !this.DirectoryInfo.GetAccessControl().AreAccessRulesProtected; }
            if (IsAttributes ?? false) { this.Attributes = MonitorFunctions.GetAttributes(this.Path); }
            //if (IsMD5Hash ?? false) { }
            //if (IsSHA256Hash ?? false) { }
            //if (IsSHA512Hash ?? false) { }
            //if (IsSize ?? false) { }
            if (IsChildCount ?? false) { this.ChildCount = MonitorFunctions.GetDirectoryChildCount(this.Path); }
            //if (IsRegistryType ?? false) { }
        }

        public void CheckRegistryKey()
        {
            //if (IsCreationTime ?? false) { }
            //if (IsLastWriteTime ?? false) { }
            //if (IsLastAccessTime ?? false) { }
            if (IsAccess ?? false) { this.Access = AccessRuleSummary.RegistryKeyToAccessString(this.Key); }
            if (IsOwner ?? false) { this.Owner = this.Key.GetAccessControl().GetOwner(typeof(NTAccount)).Value; }
            if (IsInherited ?? false) { this.Inherited = !this.Key.GetAccessControl().AreAccessRulesProtected; }
            //if (IsAttributes ?? false) { }
            //if (IsMD5Hash ?? false) { }
            //if (IsSHA256Hash ?? false) { }
            //if (IsSHA512Hash ?? false) { }
            //if (IsSize ?? false) { }
            if (IsChildCount ?? false) { this.ChildCount = MonitorFunctions.GetRegistryKeyChildCount(this.Key); }
            //if (IsRegistryType ?? false) { }
        }

        public void CheckRegistryValue()
        {
            //if (IsCreationTime ?? false) { }
            //if (IsLastWriteTime ?? false) { }
            //if (IsLastAccessTime ?? false) { }
            //if (IsAccess ?? false) { }
            //if (IsOwner ?? false) { }
            //if (IsInherited ?? false) { }
            //if (IsAttributes ?? false) { }
            if (IsMD5Hash ?? false) { this.MD5Hash = MonitorFunctions.GetHash(this.Key, this.Name, MD5.Create()); }
            if (IsSHA256Hash ?? false) { this.SHA256Hash = MonitorFunctions.GetHash(this.Key, this.Name, SHA256.Create()); }
            if (IsSHA512Hash ?? false) { this.SHA512Hash = MonitorFunctions.GetHash(this.Key, this.Name, SHA512.Create()); }
            //if (IsSize ?? false) { }
            //if (IsChildCount ?? false) { }
            if (IsRegistryType ?? false) { this.RegistryType = RegistryControl.ValueKindToString(this.Key.GetValueKind(this.Name)); }
        }
    }
}
