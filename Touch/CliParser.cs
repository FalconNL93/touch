namespace Touch;

public static class CliParser
{
    public static AppOptions Parse(IEnumerable<string> args)
    {
        var arguments = args.ToList();

        var filename = arguments.FirstOrDefault(x => !x.StartsWith("-"));
        if (filename == null)
        {
            return new AppOptions();
        }

        var flags = arguments.Where(x => x.StartsWith("-")).ToList();
        return new AppOptions
        {
            FileName = filename,
            OpenFile = flags.Any(x => x == "-o"),
            ChangeAccessTime = flags.Any(x => x == "-a"),
            CreateFile = flags.Any(x => x != "-c")
        };
    }
}