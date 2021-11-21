using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;



string path = @"C:\Users\User\Downloads\aaaa\aaaa.txt";


//WatchPath watch = new WatchPath(PathType.File);

/*
var process01 = new Process01();
bool ret01 = process01.Monitor(watch, path);
Console.WriteLine(ret01);

var process02 = new Process02();
bool ret02 = process02.Monitor(watch, path);
Console.WriteLine(ret02);

var process03 = new Process03();
bool ret03 = process03.Monitor(watch, path);
Console.WriteLine(ret03);
*/


WatchPath watch = null;

var proc01 = new Process05()
{
    _IsCreationTime = true,
    _IsLastWriteTime = true,
    _IsDateOnly = true
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

