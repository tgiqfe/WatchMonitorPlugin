using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using Microsoft.Win32;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Audit.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class MonitoringCompare : Monitoring
    {
        /// <summary>
        /// 比較する対象のパス(レジストリ値の場合はNameも)
        /// </summary>
        public string PathA { get; set; }
        public string PathB { get; set; }
        public RegistryKey KeyA { get; set; }
        public RegistryKey KeyB { get; set; }
        public string NameA { get; set; }
        public string NameB { get; set; }

        #region Target path

        private FileSystemInfo _FSInfoA = null;
        private FileSystemInfo _FSInfoB = null;

        public FileSystemInfo InfoA
        {
            get
            {
                if (_FSInfoA == null)
                {
                    return PathType switch
                    {
                        PathType.File => new FileInfo(PathA),
                        PathType.Directory => new DirectoryInfo(PathA),
                        _ => null
                    };
                }
                return _FSInfoA;
            }
        }

        public FileSystemInfo InfoB
        {
            get
            {
                if (_FSInfoB == null)
                {
                    return PathType switch
                    {
                        PathType.File => new FileInfo(PathB),
                        PathType.Directory => new DirectoryInfo(PathB),
                        _ => null
                    };
                }
                return _FSInfoB;
            }
        }

        #endregion
        #region Exists method

        public override bool TestExists()
        {
            switch (PathType)
            {
                case PathType.File:
                case PathType.Directory:
                    return InfoA.Exists && InfoB.Exists;
                case PathType.Registry:
                    if(NameA == null && NameB == null)
                    {
                        return KeyA != null && KeyB != null;
                    }
                    else
                    {
                        return (KeyA?.GetValueNames().Any(x => x.Equals(NameA, StringComparison.OrdinalIgnoreCase)) ?? false) &&
                            (KeyB?.GetValueNames().Any(x => x.Equals(NameB, StringComparison.OrdinalIgnoreCase)) ?? false);
                    }
            }
            return false;
        }

        /*
        public bool FileExists()
        {
            return InfoA.Exists && InfoB.Exists;
        }

        public bool DirectoryExists()
        {
            return InfoA.Exists && InfoB.Exists;
        }

        public bool RegistryKeyExists()
        {
            return KeyA != null && KeyB != null;
        }

        public bool RegistryValueExists()
        {
            return (KeyA?.GetValueNames().Any(x => x.Equals(NameA, StringComparison.OrdinalIgnoreCase)) ?? false) &&
                (KeyB?.GetValueNames().Any(x => x.Equals(NameB, StringComparison.OrdinalIgnoreCase)) ?? false);
        }
        */

        #endregion

        public MonitoringCompare(PathType pathType)
        {
            PathType = pathType;
        }
    }
}