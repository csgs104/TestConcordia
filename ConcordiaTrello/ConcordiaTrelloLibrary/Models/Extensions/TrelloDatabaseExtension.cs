namespace ConcordiaTrelloLibrary.Models.Extensions;

using System.Text.RegularExpressions;
using Classes;
using TrelloDotNet.Model;
using ConcordiaTrelloLibrary;

public static class TrelloDatabaseExtension
{
    public static IEnumerable<TrelloScientist> GetScientistsFromDescription(this TrelloDatabase database)
    {
        var description = database.Description.Replace(TrelloSmartSettings.GetDatabaseDivisor(), string.Empty).Trim(' ', '.');
        var scientists = new List<TrelloScientist>();
        var code = TrelloSmartSettings.GetScientistBaseCode();
        scientists.Add(TrelloSmartSettings.GetAnonymous());
        if (description.Length > 1)
        {
            int index = 1;
            foreach (var member in description.Split(TrelloSmartSettings.GetSeparator()))
            {
                string padding = index.ToString($"D{code.Length}");
                scientists.Add(new TrelloScientist(code.Substring(0, code.Length - padding.Length) + padding, member.Trim()));
                index++;
            }
        }
        return scientists;
    }
}