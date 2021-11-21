using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace WatchMonitorPlugin.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class WatchPathCollection : Dictionary<string, WatchPath>
    {
        public WatchPath GetWatchPath(string path)
        {
            string matchKey = this.Keys.FirstOrDefault(x => x.Equals(path, StringComparison.OrdinalIgnoreCase));
            /*
            return matchKey == null ?
                new WatchPath(pathType) :
                this[matchKey];
            */
            return matchKey == null ? null : this[matchKey];
        }

        public void SetWatchPath(string path, WatchPath watchData)
        {
            this[path] = watchData;
        }

        public static WatchPathCollection Load(string path)
        {
            try
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    return JsonSerializer.Deserialize<WatchPathCollection>(sr.ReadToEnd());
                }
            }
            catch { }

            return new WatchPathCollection();
        }

        public void Save(string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                JsonSerializer.Serialize(sw, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
        }
    }
}
