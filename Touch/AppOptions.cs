namespace Touch;

public class AppOptions
{
    public string? FileName { get; init; }
    public bool OpenFile { get; init; }
    public bool ChangeAccessTime { get; init; }
    public bool CreateFile { get; init; } = true;
}