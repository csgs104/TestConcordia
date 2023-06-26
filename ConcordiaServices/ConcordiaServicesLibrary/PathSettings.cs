namespace ConcordiaServicesLibrary;

public static class PathSettings
{
    private static string BaseDirectory = "Concordia";

    public static string GetBaseDirectory()
    {
        return BaseDirectory;
    }

    public static void SetBaseDirectory(string directory)
    {
        BaseDirectory = directory;
    }

    public static string DirectoryPath(string directory)
    {
        var path = Directory.GetCurrentDirectory();
        var root = Path.GetPathRoot(path) ?? string.Empty;
        var b = Path.Combine(path.Split(Path.DirectorySeparatorChar).TakeWhile(s => !s.Equals(directory)).ToArray());
        return Path.Combine(root, b);
    }

    public static string BaseDirectoryPath()
    {
        return Path.Combine(DirectoryPath(BaseDirectory), BaseDirectory);
    }
}