using WatchMonitorPlugin;
using System.IO;
using Audit.Lib;
using System.Text;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class Program
    {
        const string Id = "sample01";

        public static void Main(string[] args)
        {
            //  Watch用テスト
            Watching();

            //  Compare用テスト
            //Comparing();

            Console.ReadLine();
        }

        private static void Watching()
        {
            Console.WriteLine("Watch対象を指定");
            string targetFile = Console.ReadLine();

            string[] targetPaths = targetFile.Contains(";") ?
                targetFile.Split(';').Select(x => x.Trim()).ToArray() :
                new string[1] { targetFile };

            if (targetPaths[0].StartsWith("[reg]", StringComparison.OrdinalIgnoreCase))
            {
                string targetPath = targetPaths[0].Substring(0, targetPaths[0].LastIndexOf("\\")).Substring(5);
                string valueName = targetPaths[0].Substring(targetPaths[0].LastIndexOf("\\") + 1);
                string[] targetNames = valueName.Contains(";") ?
                    valueName.Split(';').Select(x => x.Trim()).ToArray() :
                    new string[1] { valueName };
                //TestWatchRegistryValue(targetPath, targetNames);
            }
            else
            {
                TestWatchFile(targetPaths);
                //TestWatchDirectory(targetPaths);
                //TestWatchRegistryKey(targetPaths);
            }
        }

        private static void Comparing()
        {
            Console.WriteLine("Compare対象を指定 (PathA)");
            string targetPAthA = Console.ReadLine();

            Console.WriteLine("Compare対象を指定 (PathB)");
            string targetPathB = Console.ReadLine();

            TestCompareFile(targetPAthA, targetPathB);
        }

        #region Watch

        private static void TestWatchFile(string[] targetPaths)
        {
            WatchFile beginWatch = new WatchFile()
            {
                _Id = Id,
                _Path = targetPaths,
                _IsLastWriteTime = true,
                _IsMD5Hash = true,
                _IsSize = true,
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
                            _Id = Id,
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
                _Id = Id,
                _Path = targetPaths,
                _IsLastWriteTime = true,
                _IsMD5Hash = true,
                _IsChildCount = true,
            };
            checkWatchDirectory(beginWatch);

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchDirectory()
                        {
                            _Id = Id,
                            _Path = targetPaths,
                        };
                        checkWatchDirectory(tempWatch);
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
                _Id = Id,
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
                            _Id = Id,
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
                _Id = Id,
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
                            _Id = Id,
                            _Path = new string[1] { targetPath },
                            _Name = targetNames,
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

        #endregion
        #region Compare

        private static void TestCompareFile(string pathA, string pathB)
        {
            CompareFile compare = new CompareFile()
            {
                _PathA = pathA,
                _PathB = pathB,
                _IsLastWriteTime = true,
                _IsMD5Hash = true,
                _IsSize = true,
            };

            compare.MainProcess();
            bool success = compare.Success;
            Dictionary<string, string> dictionary = compare.Propeties;

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

        private static void TestCompareDirectory(string pathA, string pathB)
        {
            CompareDirectory compare = new CompareDirectory()
            {
                _PathA = pathA,
                _PathB = pathB,
                _IsLastWriteTime = true,
                _IsChildCount = true,
                _IsAccess = true,
            };

            compare.MainProcess();
            bool success = compare.Success;
            Dictionary<string, string> dictionary = compare.Propeties;

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

        #endregion
    }
}
