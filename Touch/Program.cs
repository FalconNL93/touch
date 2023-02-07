using static System.Reflection.Assembly;

namespace Touch;

public static class Program
{
    private static readonly string BinaryName = AppDomain.CurrentDomain.FriendlyName;
    private static readonly string? AssemblyName = GetExecutingAssembly().GetName().Name;
    private static readonly string? AssemblyVersion = GetExecutingAssembly().GetName().Version?.ToString();
    private static AppOptions AppOptions { get; set; } = new();

    private static void Main(string[] args)
    {
        AppOptions = CliParser.Parse(args);
        if (string.IsNullOrEmpty(AppOptions.FileName))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        TouchFile();

        if (AppOptions.OpenFile)
        {
            OpenFile();
        }

        Environment.Exit(0);
    }

    private static void UpdateTimes()
    {
        if (!AppOptions.ChangeAccessTime || string.IsNullOrEmpty(AppOptions.FileName) || !File.Exists(AppOptions.FileName))
        {
            return;
        }

        Verbose($"Updating file");
        try
        {
            File.SetLastAccessTime(AppOptions.FileName, DateTime.Now);
            File.SetLastWriteTime(AppOptions.FileName, DateTime.Now);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }

    private static void TouchFile()
    {
        if (string.IsNullOrEmpty(AppOptions.FileName))
        {
            Verbose("Invalid filename");
            return;
        }

        Verbose($"File: {AppOptions.FileName}");
        try
        {
            if (!File.Exists(AppOptions.FileName) && AppOptions.CreateFile)
            {
                Verbose("Creating file");
                File.Create(AppOptions.FileName);
            }
            
            UpdateTimes();
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
        Console.WriteLine("Usage: {0} <file> <flags>", BinaryName);
        Console.WriteLine("Flags:");
        Console.WriteLine("-o       Open file after creating/updating with default editor");
        Console.WriteLine("-a       Change access/write time on file when file already exists");
        Console.WriteLine("-c       Do not create any files");
        Console.WriteLine("-v       Verbose");
    }

    private static void Verbose(string message)
    {
        if (!AppOptions.Verbose)
        {
            return;
        }
        
        Console.WriteLine(message);
    }

    private static void OpenFile()
    {
        if (!File.Exists(AppOptions.FileName))
        {
            Console.WriteLine($"{AppOptions.FileName} does not exist");
            return;
        }

        try
        {
            ShellHelper.OpenFile(AppOptions.FileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}