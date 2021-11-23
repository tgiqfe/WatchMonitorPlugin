using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;


string path1 = @"C:\Users\User\Downloads\aaaa\aaaa.txt";
string path2 = @"C:\Users\User\Downloads\aaaa\bbbb.jpg";
string path3 = @"C:\Users\User\Downloads\aaaa\cccc.xlsx";

ProcessFile07 file1 = new ProcessFile07()
{
    _Serial = "file01",
    _Path = new string[] { path1, path2, path3 },
    _IsAttributes = true,
    _IsAccess = true,
    _IsCreationTime = true,
};
file1.MainProcess();

ProcessFile07 file2 = new ProcessFile07()
{
    _Serial = "file01",
    _Path = new string[] { path1, path2, path3 },
};
file2.MainProcess();


Console.ReadLine();



/*
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
*/
