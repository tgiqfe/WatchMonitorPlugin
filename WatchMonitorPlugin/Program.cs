using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;



string path1 = @"C:\Users\User\Downloads\aaaa\aaaa.txt";
string path2 = @"C:\Users\User\Downloads\aaaa\bbbb.jpg";
string path3 = @"C:\Users\User\Downloads\aaaa\cccc.xlsx";

ProcessFile06 p6 = new ProcessFile06()
{
    _Serial = "FileWatch",
    _Path = new string[] { path1, path2, path3 },
    _IsLastWriteTime = true,
    _IsSize = true,
    _IsOwner = true,
};
p6.MainProcess();

Console.ReadLine();

ProcessFile06 p7 = new ProcessFile06()
{
    _Serial = "FileWatch",
    _Path = new string[] { path1, path2, path3 },
};
p7.MainProcess();


Console.ReadLine();



/*
 ProcessDir02 p2 = new ProcessDir02()
{
    _Serial = "aaaa",
    _Path = new string[] { path },
    _IsLastWriteTime = true,
    _IsAccess = true,
};
p2.MainProcess();

ProcessDir02 p3 = new ProcessDir02()
{
    _Serial = "aaaa",
    _Path = new string[] { path },
};
p3.MainProcess();
*/


/*
ProcessDir01 proc1 = new ProcessDir01()
{
    _Serial = "aaaa",
    _Path = new string[]{ path },
    _IsAccess = true,
    _IsAttributes = true,
    _IsLastWriteTime = true,
    _IsChildCount = true,
};
proc1.MainProcess();



ProcessDir01 proc2 = new ProcessDir01()
{
    _Serial = "aaaa",
    _Path = new string[] { path },
};
proc2.MainProcess();
*/

/*
Process05 proc1 = new Process05()
{
    _Serial = "aaaa",
    _Path = new string[] {path},
    _IsLastWriteTime = true,
    _IsSize = true,
    _IsStart = true,
};
proc1.MainProcess();

Console.ReadLine();

Process05 proc2 = new Process05()
{
    _Serial = "aaaa",
    _Path = new string[] { path },
};
proc2.MainProcess();
*/

/*
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
bool ret = proc01.WatchFileCheck(ref watch, path);
Console.WriteLine(ret);

Console.ReadLine();

var proc02 = new Process05();
ret = proc02.WatchFileCheck(ref watch, path);
Console.WriteLine(ret);

collection.SetWatchPath(path, watch);

//  複数ファイルの場合のforeach終了部

collection.Save(confFile);
*/