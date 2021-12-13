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

            Test(targetFile);

            Console.ReadLine();

        }

        private static void Test(params string[] targetPath)
        {
            WatchFile watch = new WatchFile()
            {
                _ID = ID,
                _Path = targetPath,
                _IsLastWriteTime = true,
            };
            CheckWatchFile(watch);

            while (true)
            {
                var key = Console.ReadKey(false);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        var tempWatch = new WatchFile()
                        {
                            _ID = ID,
                            _Path = targetPath,
                        };
                        CheckWatchFile(tempWatch);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private static void CheckWatchFile(WatchFile watch)
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
