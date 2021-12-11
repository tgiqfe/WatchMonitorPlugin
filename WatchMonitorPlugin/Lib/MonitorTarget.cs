using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitorTarget
    {
        public PathType PathType { get; set; }

        public string FullPath { get; set; }
        public const string REGPATH_PREFIX = "[registry]";

        #region Watch Parameter

        private string _PathTypeName = null;
        public string PathTypeName { get { _PathTypeName ??= PathType.ToString().ToLower(); return _PathTypeName; } }

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

        #endregion

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

        #region Target path

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
        #region TestExists

        public bool TestExists()
        {
            switch (PathType)
            {
                case PathType.File:
                    return FileInfo.Exists;
                case PathType.Directory:
                    return DirectoryInfo.Exists;
                case PathType.Registry:
                    if (Name == null)
                    {
                        return Key != null;
                    }
                    else
                    {
                        return Key?.GetValueNames().Any(x => x.Equals(Name, StringComparison.OrdinalIgnoreCase)) ?? false;
                    }
            }
            return false;
        }

        #endregion

        public MonitorTarget(PathType pathType)
        {
            this.PathType = pathType;
        }
    }
}
