using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Lib;

namespace Audit.Lib
{
    public class Monitoring
    {
        public PathType PathType { get; set; }

        private string _PathTypeName = null;
        public string PathTypeName { get { _PathTypeName ??= PathType.ToString().ToLower();  return _PathTypeName; } }

        public bool? IsCreationTime { get; set; }
        public bool? IsLastWriteTime { get; set; }
        public bool? IsLastAccessTime { get; set; }
        public bool? IsAccess { get; set; }
        public bool? IsOwner { get; set; }
        public bool? IsInherited { get; set; }
        public bool? IsAttributes { get; set; }
        public bool? IsMD5Hash { get; set; }
        public bool? IsSHA256Hash { get; set; }
        public bool? IsSHA512Hash { get; set; }
        public bool? IsSize { get; set; }
        public bool? IsChildCount { get; set; }
        public bool? IsRegistryType { get; set; }
        public bool? IsDateOnly { get; set; }
        public bool? IsTimeOnly { get; set; }

        public virtual bool TestExists() { return false; }

        public virtual bool TestExistsA() { return false; }

        public virtual bool TestExistsB() { return false; }
    }
}
