using WatchMonitorPlugin;
using System.IO;
using Audit.Lib;
using System.Text;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class Program
    {
        const string ID = "sample01";

        public static void Main(string[] args)
        {
            Console.WriteLine("Watch対象を指定");
            string targetFile = Console.ReadLine();


            string[] targetPaths = targetFile.Contains(";") ?
                targetFile.Split(';').Select(x => x.Trim()).ToArray() :
                new string[1] { targetFile };

            if (targetPaths[0].StartsWith("[reg]", StringComparison.OrdinalIgnoreCase))
            {
                string targetPath = targetPaths[0].Substring(0, targetPaths[0].LastIndexOf("\\"));
                string valueName = targetPaths[0].Substring(0, targetPaths[0].LastIndexOf("\\") + 1);
                string[] targetNames = valueName.Contains(";") ?
                    valueName.Split(';').Select(x => x.Trim()).ToArray() :
                    new string[1] { valueName };
                TestWatchRegistryValue(targetPath, targetNames);
            }
            else
            {
                //TestWatchFile(targetPaths);
                //TestWatchDirectory(targetPaths);
                TestWatchRegistryKey(targetPaths);
            }

            Console.ReadLine();
        }

        private static void TestWatchFile(string[] targetPaths)
        {
            WatchFile beginWatch = new WatchFile()
            {
                _ID = ID,
                _Path = targetPaths,
                _IsLastWriteTime = true,
                _IsMD5Hash = true,
            };
            checkWatchFile(beginWatch);

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchFile()
                        {
                            _ID = ID,
                            _Path = targetPaths,
                        };
                        checkWatchFile(tempWatch);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

            void checkWatchFile(WatchFile watch)
            {
                watch.MainProcess();
                bool success = watch.Success;
                Dictionary<string, string> dictionary = watch.Propeties;

                //  確認用
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }

                if (success)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Success");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
            }
        }

        private static void TestWatchDirectory(string[] targetPaths)
        {
            WatchDirectory beginWatch = new WatchDirectory()
            {
                _ID = ID,
                _Path = targetPaths,
                _IsLastWriteTime = true,
                _IsMD5Hash = true,
                _IsChildCount = true,
<<<<<<< HEAD
                _IsAccess = true,
            };
            checkWatchFile(beginWatch);
=======
            };
            checkWatchDirectory(beginWatch);
>>>>>>> 796658ddd18cd6953a6cb41d299ee7c58b39415b

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchDirectory()
                        {
                            _ID = ID,
                            _Path = targetPaths,
                        };
<<<<<<< HEAD
                        checkWatchFile(tempWatch);
=======
                        checkWatchDirectory(tempWatch);
>>>>>>> 796658ddd18cd6953a6cb41d299ee7c58b39415b
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }


            void checkWatchDirectory(WatchDirectory watch)
            {
                watch.MainProcess();
                bool success = watch.Success;
                Dictionary<string, string> dictionary = watch.Propeties;

                //  確認用
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }

                if (success)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Success");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
            }
        }

        private static void TestWatchRegistryKey(string[] targetPaths)
        {
            WatchRegistry beginWatch = new WatchRegistry()
            {
                _ID = ID,
                _Path = targetPaths,
                _IsMD5Hash = true,
                _IsRegistryType = true,
            };
            checkWatchRegistryKey(beginWatch);

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchRegistry()
                        {
                            _ID = ID,
                            _Path = targetPaths,
                        };
                        checkWatchRegistryKey(tempWatch);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

            void checkWatchRegistryKey(WatchRegistry watch)
            {
                watch.MainProcess();
                bool success = watch.Success;
                Dictionary<string, string> dictionary = watch.Propeties;

                //  確認用
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }

                if (success)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Success");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
            }
        }

        private static void TestWatchRegistryValue(string targetPath, string[] targetNames)
        {
            WatchRegistry beginWatch = new WatchRegistry()
            {
                _ID = ID,
                _Path = new string[1] { targetPath },
                _Name = targetNames,
                _IsMD5Hash = true,
                _IsRegistryType = true,
            };
            checkWatchRegistryValue(beginWatch);

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchRegistry()
                        {
                            _ID = ID,
                            _Path = new string[1] { targetPath },
                        };
                        checkWatchRegistryValue(tempWatch);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

            void checkWatchRegistryValue(WatchRegistry watch)
            {
                watch.MainProcess();
                bool success = watch.Success;
                Dictionary<string, string> dictionary = watch.Propeties;

                //  確認用
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }

                if (success)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Success");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed");
                    Console.ResetColor();
                    Console.WriteLine("]");
                }
            }
        }
    }
}
