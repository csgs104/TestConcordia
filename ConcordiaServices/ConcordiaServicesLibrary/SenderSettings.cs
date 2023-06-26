namespace ConcordiaServicesLibrary;

using ConcordiaServicesLibrary.Senders;

public static class SenderSettings
{
    private static string FromEmail = string.Empty;
    private static string FromPassword = string.Empty;
    private static string ToEmail = string.Empty;
    private static string Host = string.Empty;
    private static int Port = 587;

    public static Sender GetSender()
    {
        return new Sender(FromEmail, FromPassword, ToEmail, Host, Port);
    }

    public static void SetFromEmail(string email)
    {
        FromEmail = email;
    }

    public static void SetFromPassword(string password)
    {
        FromPassword = password;
    }

    public static void SetToEmail(string email)
    {
        ToEmail = email;
    }

    public static void SetHost(string host)
    {
        Host = host;
    }

    public static void SetPort(int port)
    {
        Port = port;
    }
}