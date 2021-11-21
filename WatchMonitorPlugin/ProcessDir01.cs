using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchMonitorPlugin.Lib;

namespace WatchMonitorPlugin
{
    internal class ProcessDir01
    {
        public string _Serial { get; set; }
        public string[] _Path { get; set; }

        public bool? _IsCreationTime { get; set; }
        public bool? _IsLastWriteTime { get; set; }
        public bool? _IsLastAccessTime { get; set; }
        public bool? _IsAccess { get; set; }
        public bool? _IsOwner { get; set; }
        public bool? _IsInherited { get; set; }
        public bool? _IsAttributes { get; set; }
        public bool? _IsMD5Hash { get; set; }
        public bool? _IsSHA256Hash { get; set; }
        public bool? _IsSHA512Hash { get; set; }
        public bool? _IsSize { get; set; }
        public bool? _IsChildCount { get; set; }
        //public bool? _IsRegistryType { get; set; }

        public bool? _IsDateOnly { get; set; }
        public bool? _IsTimeOnly { get; set; }

        public int? _MaxDepth { get; set; }

        public bool _IsStart { get; set; }

        private int _serial;
        private string _checkingPath;

        protected bool Success { get; set; }

        public void MainProcess()
        {
            string dbDir = @"C:\Users\User\Downloads\aaaa\dbdbdb";
            var collection = WatchPathCollection.Load(dbDir, _Serial);

            var dictionary = new Dictionary<string, string>();

            this._MaxDepth ??= 5;

            foreach (string path in _Path)
            {
                _serial++;
                _checkingPath = path;


            }

            collection.Save(dbDir, _Serial);



            //  確認
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }
            if (Success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed");
                Console.ResetColor();
            }
        }

        private bool RecursiveTree(ref WatchPath watch, Dictionary<string, string> dictionary, string path)
        {
            bool ret = false;
            var info = new DirectoryInfo(path);

            dictionary[$"directory_{_serial}"] = path.Replace(_checkingPath, "\\");
            if (watch == null || this._IsStart)
            {
                ret = true;
                watch = new WatchPath(PathType.Directory);

                if (Directory.Exists(path))
                {
                    watch.Exists = true;
                    if (_IsCreationTime ?? false)
                    {
                        watch.CreationTime = MonitorTimeStamp.GetDirectoryCreationTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                        watch.IsDateOnly = _IsDateOnly;
                        watch.IsTimeOnly = _IsTimeOnly;
                    }
                    if (_IsLastWriteTime ?? false)
                    {
                        watch.LastWriteTime = MonitorTimeStamp.GetDirectoryLastWriteTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                        watch.IsDateOnly = _IsDateOnly;
                        watch.IsTimeOnly = _IsTimeOnly;
                    }
                    if (_IsLastAccessTime ?? false)
                    {
                        watch.LastAccessTime = MonitorTimeStamp.GetDirectoryLastAccessTime(info, _IsDateOnly ?? false, _IsTimeOnly ?? false);
                        watch.IsDateOnly = _IsDateOnly;
                        watch.IsTimeOnly = _IsTimeOnly;
                    }
                    if (_IsAccess ?? false) { watch.Access = AccessRuleSummary.DirectoryToAccessString(info); }
                    if (_IsOwner ?? false) { watch.Owner = MonitorSecurity.GetDirectoryOwner(info); }
                    if (_IsInherited ?? false) { watch.Inherited = MonitorSecurity.GetDirectoryInherited(info); }
                    if (_IsAttributes ?? false) { watch.Attributes = MonitorAttributes.GetAttributes(path); }





                }
                else
                {

                }
            }




            return ret;
        }

        private bool WatchFileCheck()
        {
            
            return false;
        }

        private bool WatchDirectoryCheck()
        {
            return false;
        }
    }
}
