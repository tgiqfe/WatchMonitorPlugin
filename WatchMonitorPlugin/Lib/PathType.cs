﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchMonitorPlugin.Lib
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public enum PathType
    {
        None = 0,
        File = 1,
        Directory = 2,
        Registry = 3,
    }
}
