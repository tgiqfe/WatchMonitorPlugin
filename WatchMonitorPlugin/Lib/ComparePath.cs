using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using Microsoft.Win32;

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

        public ComparePath(PathType pathType)
        {
            PathType = pathType;    
        }
    }
}
