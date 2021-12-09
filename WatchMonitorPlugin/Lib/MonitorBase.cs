using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;
using Audit.Lib;

namespace Audit.Lib
{
    internal class MonitorBase
    {
        public virtual string CheckTarget { get; }

        public virtual bool CompareFile(CompareMonitoring monitoring, Dictionary<string, string> dictionary, int serial) { return true; }

        public virtual bool CompareDirectory(CompareMonitoring monitoring, Dictionary<string, string> dictionary, int serial) { return true; }

        public virtual bool CompareRegistryKey(CompareMonitoring monitoring, Dictionary<string, string> dictionary, int serial) { return true; }

        public virtual bool CompareRegistryValue(CompareMonitoring monitoring, Dictionary<string, string> dictionary, int serial) { return true; }

        public virtual bool WatchFile(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial) { return false; }

        public virtual bool WatchDirectory(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial) { return false; }

        public virtual bool WatchRegistryKey(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial) { return false; }

        public virtual bool WatchRegistryValue(MonitoringWatch monitoring, Dictionary<string, string> dictionary, int serial) { return false; }
    }
}
