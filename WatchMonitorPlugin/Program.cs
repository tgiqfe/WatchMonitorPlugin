using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;



string path = @"C:\Users\User\Downloads\aaaa\aaaa.txt";
string confFile = @"C:\Users\User\Downloads\aaaa\test.json";

var collection = WatchPathCollection.Load(confFile);

//  複数ファイルの場合のforeach開始部

WatchPath watch = collection.GetWatchPath(path);

var proc01 = new Process05()
{
    _IsCreationTime = true,
    _IsLastWriteTime = true,
    _IsAccess = true,
    _IsStart = true,
};
bool ret = proc01.Monitor(ref watch, path);
Console.WriteLine(ret);

Console.ReadLine();

var proc02 = new Process05();
ret = proc02.Monitor(ref watch, path);
Console.WriteLine(ret);

collection.SetWatchPath(path, watch);

//  複数ファイルの場合のforeach終了部

collection.Save(confFile);

Console.ReadLine();

