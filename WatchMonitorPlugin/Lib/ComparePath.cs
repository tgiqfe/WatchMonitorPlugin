using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using Microsoft.Win32;
using System.IO;

namespace Audit.Lib
{
    internal class ComparePath : Monitoring
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

        private FileSystemInfo _FSInfoA = null;
        private FileSystemInfo _FSInfoB = null;
        public FileSystemInfo InfoA
        {
            get
            {
                if(_FSInfoA == null)
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

        public ComparePath(PathType pathType)
        {
            PathType = pathType;
        }
    }
}