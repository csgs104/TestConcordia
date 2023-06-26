namespace ConcordiaServicesLibrary;

public static class ReporterSettings
{
    private static string ReportsDirectory = "ConcordiaReports";

    public static string GetReportsDirectory()
    {
        return ReportsDirectory;
    }

    public static void SetReportsDirectory(string directory)
    {
        ReportsDirectory = directory;
    }

    public static string ConcordiaReportsPath()
    {
        return Path.Combine(PathSettings.BaseDirectoryPath(), ReportsDirectory);
    }
}