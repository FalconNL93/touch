using static System.Reflection.Assembly;

namespace Touch;

public static class Program
{
    private static readonly string BinaryName = AppDomain.CurrentDomain.FriendlyName;
    private static string _filePath = string.Empty;
    private static readonly string? AssemblyName = GetExecutingAssembly().GetName().Name;
    private static readonly string? AssemblyVersion = GetExecutingAssembly().GetName().Version?.ToString();

    private static readonly Dictionary<string, Func<Task>> Parameters = new()
    {
        {"-o", OpenFile}
    };

    private static void Main(string[] args)
    {
        var destination = args.FirstOrDefault(x => !x.StartsWith("-"));
        if (string.IsNullOrEmpty(destination))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        _filePath = destination;
        if (!File.Exists(_filePath))
        {
            CreateFile();
            Environment.Exit(0);
        }

        UpdateTimes();
        ParseSwitches(args.Where(x => x.StartsWith("-")));

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

    private static void ParseSwitches(IEnumerable<string> switches)
    {
        foreach (var arg in switches)
        {
            var argument = Parameters.Where(x => x.Key == arg).ToList();
            if (!argument.Any())
            {
                Console.WriteLine($"Unknown switch: {arg}");
                continue;
            }

            try
            {
                argument.FirstOrDefault().Value.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private static Task OpenFile()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"{_filePath} does not exist");
        }

        ShellHelper.OpenFile(_filePath);
        return Task.CompletedTask;
    }
}