﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.Win32;
using System.Security.Cryptography;
using IO.Lib;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Audit.Lib
{
    public class MonitoringWatch : Monitoring
    {
        /// <summary>
        /// ファイル/ディレクトリ/レジストリキーの場合は、Targetへのパス
        /// レジストリ値の場合は、パスの頭に[registry]を付けて管理
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 監視中Targetの各情報。
        /// </summary>
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

        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public RegistryKey Key { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

        private FileSystemInfo _FSInfo = null;

        [JsonIgnore]
        public FileSystemInfo Info
        {
            get
            {
                if (_FSInfo == null)
                {
                    return PathType switch
                    {
                        PathType.File => new FileInfo(Path),
                        PathType.Directory => new DirectoryInfo(Path),
                        _ => null
                    };
                }
                return _FSInfo;
            }
        }

        public const string REGPATH_PREFIX = "[registry]";

        public MonitoringWatch(PathType pathType)
        {
            PathType = pathType;
        }
    }
}