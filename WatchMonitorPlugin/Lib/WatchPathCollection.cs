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
            return matchKey == null ? null : this[matchKey];
        }

        public void SetWatchPath(string path, WatchPath watchPath)
        {
            watchPath.FullPath = path;
            this[path] = watchPath;
            /*
            if(watchPath.PathType == PathType.Registry)
            {
                if (path.EndsWith("\\"))
                {
                    //  レジストリキーとしてセット
                    watchPath.FullPath = path;
                    watchPath.ContainerPath = path.TrimEnd('\\');
                    watchPath.LeafName = null;
                }
                else
                {
                    //  レジストリ値としてセット
                    watchPath.FullPath = path;
                    watchPath.ContainerPath = Path.GetDirectoryName(path);
                    watchPath.LeafName = Path.GetFileName(path);
                }
            }
            else
            {
                watchPath.FullPath = path;
                watchPath.ContainerPath = Path.GetDirectoryName(path);
                watchPath.LeafName = Path.GetFileName(path);
            }
            this[watchPath.FullPath] = watchPath;
            */
        }

        #region Load/Save

        public static WatchPathCollection Load(string dbDir, string serial)
        {
            try
            {
                using (var sr = new StreamReader(Path.Combine(dbDir, serial), Encoding.UTF8))
                {
                    return JsonSerializer.Deserialize<WatchPathCollection>(sr.ReadToEnd());
                }
            }
            catch { }
            return new WatchPathCollection();
        }

        public void Save(string dbDir, string serial)
        {
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            using (var sw = new StreamWriter(Path.Combine(dbDir, serial), false, Encoding.UTF8))
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                sw.WriteLine(json);
            }
        }

        #endregion
    }
}
