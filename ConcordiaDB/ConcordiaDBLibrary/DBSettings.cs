namespace ConcordiaDBLibrary;

public static class DBSettings
{
    private static string ConnectionString = "Server=127.0.0.1,1433; Database=Concordia; User Id=SA; Password=R00t.r00T; TrustServerCertificate=true;";
    private static List<string> PrioritiesNames = new List<string>();
    private static List<string> StatesNames = new List<string>();

    public static string GetConnectionString()
    {
        return ConnectionString;
    }

    public static List<string> GetPrioritiesNames()
    {
        return PrioritiesNames;
    }

    public static List<string> GetStatesNames()
    {
        return StatesNames;
    }

    public static void SetConnectionString(string connectionstring) 
    {
        ConnectionString = connectionstring;
    }

    public static void SetPrioritiesNames(List<string> names)
    {
        PrioritiesNames = names;
    }

    public static void SetStatesNames(List<string> names)
    {
        StatesNames = names;
    }
}