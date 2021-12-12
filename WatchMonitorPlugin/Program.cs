using WatchMonitorPlugin;
using System.IO;
using Audit.Lib;
using System.Text;

namespace WatchMonitorPlugin
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Watch対象を指定");
            string targetFile = Console.ReadLine();

            WatchFile watch = new WatchFile()
            {
                _ID = "sample01",
                _Path = new string[] { targetFile },
                _IsLastWriteTime = true,
            };


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


            Console.ReadLine();

        }
    }
}
