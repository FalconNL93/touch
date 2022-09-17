using System.Diagnostics;

namespace Touch;

public class ShellHelper
{
    public static void OpenFile(string path)
    {
        using var process = new Process();
        process.StartInfo.FileName = "explorer";
        process.StartInfo.Arguments = "\"" + path + "\"";
        process.Start();
    }
}