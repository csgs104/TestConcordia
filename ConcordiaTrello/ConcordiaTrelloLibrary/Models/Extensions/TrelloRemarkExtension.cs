namespace ConcordiaTrelloLibrary.Models.Extensions;

using System.Text.RegularExpressions;
using Classes;
using TrelloDotNet.Model;

public static class TrelloRemarkExtension
{
    public const string numberRegex = "^[0-9]+$";
    public const string letterRegex = "^[a-zA-Z\\s]+$";

    public static void SetScientistFromText(this TrelloRemark remark)
    {
        var members = remark.TrelloDatabase.GetScientistsFromDescription().ToList();
        var text = remark.Text.Split(TrelloSmartSettings.GetRemarkDivisor());
        remark.Text = text[0].Trim();
        var scientist = TrelloSmartSettings.GetAnonymous();
        if (text.Length > 1 && members is not null)
        {
            var writer = text[1].Trim(' ', '.');
            TrelloScientist? author = null;
            if (Regex.Match(writer, numberRegex).Success)
            {
                author = members.FirstOrDefault(t => t.Code!.Equals(writer, StringComparison.OrdinalIgnoreCase));
            }
            if (Regex.Match(writer, letterRegex).Success)
            {
                author = members.FirstOrDefault(t => t.FullName.Equals(writer, StringComparison.OrdinalIgnoreCase));
            }
            if (author is not null) 
	        {
                scientist = author;
	        }
        }
        remark.TrelloScientist = scientist;
    }
}