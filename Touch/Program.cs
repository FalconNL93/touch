using static System.Reflection.Assembly;

namespace Touch;

public static class Program
{
    private static readonly string BinaryName = AppDomain.CurrentDomain.FriendlyName;
    private static string _filePath = string.Empty;
    private static readonly string? AssemblyName = GetExecutingAssembly().GetName().Name;
    private static readonly string? AssemblyVersion = GetExecutingAssembly().GetName().Version?.ToString();

    private static void Main(string[] args)
    {
        _filePath = args.Length > 0 ? args[0] : string.Empty;

        if (string.IsNullOrEmpty(_filePath))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        if (!File.Exists(_filePath))
        {
            CreateFile();
            Environment.Exit(0);
        }

        UpdateTimes();
        Environment.Exit(0);
    }

    private static void UpdateTimes()
    {
        try
        {
            File.SetLastAccessTime(_filePath, DateTime.Now);
            File.SetLastWriteTime(_filePath, DateTime.Now);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }

    private static void CreateFile()
    {
        try
        {
            File.Create(_filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("{0} {1}\n", AssemblyName, AssemblyVersion);
        Console.WriteLine("Usage: {0} path", BinaryName);
        Console.WriteLine("{0} textfile.txt", BinaryName);
        Console.WriteLine("{0} \"C:\\Path To\\File\\filename.ext\"", BinaryName);
    }
}