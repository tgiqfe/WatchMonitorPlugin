using WatchMonitorPlugin;
using System.IO;
using WatchMonitorPlugin.Lib;
using System.Text;


string path1 = @"C:\Users\User\Downloads\aaaa\aaaa.txt";
string path2 = @"C:\Users\User\Downloads\aaaa\bbbb.jpg";
string path3 = @"C:\Users\User\Downloads\aaaa\cccc.xlsx";

string dir1 = @"C:\Users\User\Downloads\aaaa\bbbb";
string dir2 = @"C:\Users\User\Downloads\aaaa\cccc";
string dir3 = @"C:\Users\User\Downloads\aaaa\ssss";

string reg1 = @"HKEY_CURRENT_USER\SOFTWARE\Test\Example01";
string reg2 = @"HKEY_CURRENT_USER\SOFTWARE\Test\Example02";

ProcessReg r1 = new ProcessReg()
{
    _Serial = "TestRegistry01",
    _Path = new string[] { reg1, reg2 },
    _IsAccess = true,
    _IsMD5Hash = true,
};
r1.MainProcess();

ProcessReg r2 = new ProcessReg()
{
    _Serial = "TestRegistry01",
    _Path = new string[] { reg1, reg2 },
};
r2.MainProcess();

Console.ReadLine();



/*

ProcessDir03 d1 = new ProcessDir03()
{
    _Serial = "TestDirectory01",
    _Path = new string[] { dir1, dir2, dir3 },
    _IsAccess = true,
    _IsLastWriteTime = true,
    _IsMD5Hash = true,
    _IsInherited = true,
};
d1.MainProcess();

ProcessDir03 d2 = new ProcessDir03()
{
    _Serial = "TestDirectory01",
    _Path = new string[] { dir1, dir2, dir3 },
};
d2.MainProcess();

*/

/*
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
*/

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
