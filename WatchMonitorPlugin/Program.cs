﻿using WatchMonitorPlugin;
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

            string[] targetFiles = targetFile.Contains(";") ?
                targetFile.Split(';').Select(x => x.Trim()).ToArray() :
                new string[1] { targetFile };

            if(targetFiles[0].StartsWith("HKEY", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Watch対象のレジストリ値の名前を指定");
            }
            else
            {
                TestWatchDirectory(targetFiles);
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
                            _ID = ID,
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
    }
}
