namespace ConcordiaDBInitializer;

public static class InizializerSettings
{
    private static string ConnectionString = "Server=127.0.0.1,1433; Database=Master; User Id=SA; Password=R00t.r00T; TrustServerCertificate=true;";

    public static string GetConnectionString()
    {
        return ConnectionString;
    }

    public static void SetConnectionString(string connectionstring)
    {
        ConnectionString = connectionstring;
    }
}