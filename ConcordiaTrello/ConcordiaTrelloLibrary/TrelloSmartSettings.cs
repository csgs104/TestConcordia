namespace ConcordiaTrelloLibrary;

using Models.Classes;

public static class TrelloSmartSettings
{
    private static string DatabaseDivisor = "SCIENTISTS:";
    private static string ExperimentDivisor = "ASSIGNEES:";
    private static string RemarkDivisor = "AUTHOR:";
    private static string Separator = ",";

    private static string ScientistBaseCode = "000000000000000000000000";
    private static string ScientitsBaseName = "Anonymous";

    public static string GetDatabaseDivisor()
    {
        return DatabaseDivisor;
    }

    public static string GetExperimentDivisor()
    {
        return ExperimentDivisor;
    }

    public static string GetRemarkDivisor()
    {
        return RemarkDivisor;
    }

    public static string GetSeparator()
    {
        return Separator;
    }

    public static string GetScientistBaseCode()
    {
        return ScientistBaseCode;
    }

    public static string GetScientitsBaseName()
    {
        return ScientistBaseCode;
    }

    public static TrelloScientist GetAnonymous()
    {
        return new TrelloScientist(ScientistBaseCode, ScientitsBaseName);
    }

    public static void SetDatabaseDivisor(string divisor)
    {
        DatabaseDivisor = divisor;
    }

    public static void SetExperimentDivisor(string divisor)
    {
        ExperimentDivisor = divisor;
    }

    public static void SetRemarkDivisor(string divisor)
    {
        RemarkDivisor = divisor;
    }

    public static void SetSeparator(string separator)
    {
        Separator = separator;
    }

    public static void SetScientistBaseCode(string code)
    {
        ScientistBaseCode = code;
    }

    public static void SetScientitsBaseName(string name)
    {
        ScientitsBaseName = name;
    }
}