using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchMonitorPlugin.Lib
{
    internal class WatchRegistryKey : WatchPath
    {
        public WatchRegistryKey(bool? isAccess, bool? isOwner, bool? isInherited, bool? isChildCount)
        {
            this.PathType = PathType.File;
            this.IsAccess = isAccess;
            this.IsOwner = isOwner;
            this.IsInherited = isInherited;
            this.IsChildCount = isChildCount;
        }
    }
}
