using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchMonitorPlugin.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class WatchPathCollection : Dictionary<string, WatchPath>
    {
        public WatchPath GetWatchPath(string path, PathType pathType)
        {
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(path, StringComparison.OrdinalIgnoreCase));
            return matchKey == null ?
                new WatchPath(pathType) :
                this[matchKey];
        }

        public void SetWatchPath(string path, WatchPath watchData)
        {
            this[path] = watchData;
        }
    }
}
