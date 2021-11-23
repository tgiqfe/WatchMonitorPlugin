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

namespace WatchMonitorPlugin.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class WatchPath
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
        public bool? IsDateOnly { get; set; }
        public bool? IsTimeOnly { get; set; }

        public WatchPath(PathType pathType)
        {
            PathType = pathType;
        }
    }
}
