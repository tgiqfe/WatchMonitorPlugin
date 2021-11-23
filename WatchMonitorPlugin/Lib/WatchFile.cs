using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchMonitorPlugin.Lib
{
    internal class WatchFile : WatchPath
    {
        public WatchFile(bool? isCreationTime, bool? isLastWriteTime, bool? isLastAccessTime,
            bool? isAccess, bool? isOwner, bool? isInherited, bool? isAttributes,
            bool? isMD5Hash, bool? isSHA256Hash, bool? isSHA512Hash, bool? isSize)
        {
            this.IsCreationTime = isCreationTime;
            this.IsLastWriteTime = isLastWriteTime;
            this.IsLastAccessTime = isLastAccessTime;
            this.IsAccess = isAccess;
            this.IsOwner = isOwner;
            this.IsInherited = isInherited;
            this.IsAttributes = isAttributes;
            this.IsMD5Hash = isMD5Hash;
            this.IsSHA256Hash = isSHA256Hash;
            this.IsSHA512Hash = isSHA512Hash;
            this.IsSize = isSize;
        }
    }
}
