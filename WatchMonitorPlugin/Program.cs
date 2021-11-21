using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;



string path = @"C:\Users\User\Downloads\aaaa\aaaa.txt";
string ConfFile = @"C:\Users\User\Downloads\aaaa\test.json";

var collection = WatchPathCollection.Load(ConfFile);

WatchPath watch = collection.GetWatchPath(path);

var proc01 = new Process05()
{
    _IsCreationTime = true,
    _IsLastWriteTime = true,
    _IsAccess = true,
};
bool ret = proc01.Monitor(ref watch, path);
Console.WriteLine(ret);

Console.ReadLine();

var proc02 = new Process05();
ret = proc02.Monitor(ref watch, path);
Console.WriteLine(ret);



Console.ReadLine();

