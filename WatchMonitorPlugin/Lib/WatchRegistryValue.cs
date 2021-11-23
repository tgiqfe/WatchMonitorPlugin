using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WatchMonitorPlugin.Lib
{
    internal class WatchRegistryValue : WatchPath
    {
        public WatchRegistryValue(bool? isMD5Hash, bool? isSHA256Hash, bool? isSHA512Hash, bool? isRegistryType)
        {
            this.PathType = PathType.File;
            this.IsMD5Hash = isMD5Hash;
            this.IsSHA256Hash = isSHA256Hash;
            this.IsSHA512Hash = isSHA512Hash;
            this.IsRegistryType = isRegistryType;
        }
    }
}
