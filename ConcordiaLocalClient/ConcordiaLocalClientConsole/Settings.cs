namespace ConcordiaLocalClientConsole;

public static class ClientSettings
{
    private const string ServerIp = "127.0.0.1";

    private const string ServerPort = "12345";

    public static string GetLocalServerIp()
    {
        return ServerIp;
    }

    public static string GetLocalServerPort()
    {
        return ServerPort;
    }
}