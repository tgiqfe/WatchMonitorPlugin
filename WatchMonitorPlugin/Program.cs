using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;



string path = @"C:\Users\User\Downloads\aaaa\aaaa.txt";


WatchPath watch = null;

var proc01 = new Process05()
{
    _IsCreationTime = true,
    _IsLastWriteTime = true,
};
bool ret = proc01.Monitor(ref watch, path);
Console.WriteLine(ret);

using(var sw = new StreamWriter(path, true, Encoding.UTF8))
{
    sw.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
}
var proc02 = new Process05();
ret = proc02.Monitor(ref  watch, path);
Console.WriteLine(ret);



Console.ReadLine();

