using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;
using Microsoft.Win32;

namespace WatchMonitorPlugin
{
    internal class ProcessReg01
    {
        public string _Serial { get; set; }
        public string[] _Path { get; set; }
        public string[] _Name { get; set; }

        //public bool? _IsCreationTime { get; set; }
        //public bool? _IsLastWriteTime { get; set; }
        //public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        //public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        //public bool? _IsSize { get; set; }
        public bool? _IsChildCount { get; set; }
        public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public bool _Begin { get; set; }

        private int _serial;

        protected bool Success { get; set; }

        public void MainProcess()
        {
            if (_Name?.Length > 0)
            {
                //  レジストリ値のWatch
            }
            else
            {
                //  レジストリキーのWatch
            }
        }
    }
}
