namespace ConcordiaTrelloLibrary;

using Networks;

public static class TrelloSettings
{
    private static string KeyAD = string.Empty;
    private static string TokenAD = string.Empty;
    private static string BoardCode = string.Empty;
    private static string BoardURL = string.Empty;

    private static List<string> TrelloPrioritiesNames = new List<string>();
    private static List<string> TrelloStatesNames = new List<string>();

    public static string GetBoardCode()
    {
        return BoardCode;
    }

    public static string GetBoardURL()
    {
        return BoardURL;
    }

    public static TrelloNetwork GetBoardAD()
    {
        return new TrelloNetwork(KeyAD, TokenAD);
    }

    public static List<string> GetTrelloPrioritiesNames()
    {
        return TrelloPrioritiesNames;
    }

    public static List<string> GetTrelloStatesNames()
    {
        return TrelloStatesNames;
    }

    public static void SetKeyAD(string keyAD)
    {
        KeyAD = keyAD;
    }

    public static void SetTokenAD(string tokenAD)
    {
        TokenAD = tokenAD;
    }

    public static void SetBoardCode(string boardcode)
    {
        BoardCode = boardcode;
    }

    public static void SetBoardURL(string boardURL)
    {
        BoardURL = boardURL;
    }

    public static void SetTrelloPrioritiesNames(List<string> names)
    {
        TrelloPrioritiesNames = names;
    }

    public static void SetTrelloStatesNames(List<string> names)
    {
        TrelloStatesNames = names;
    }

    public static async Task<bool> IsBoardAccessibleAsync()
    {
        try
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Head, BoardURL);
            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch /* (Exception ex) */
        {
            return false;
        }
    }
}