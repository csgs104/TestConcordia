namespace ConcordiaTrelloLibrary.Models.Extensions;

using System.Text.RegularExpressions;
using Classes;
using TrelloDotNet.Model;

public static class TrelloExperimentExtension
{
    public const string numberRegex = "^[0-9]+$";
    public const string letterRegex = "^[a-zA-Z\\s]+$";

    public static void SetScientistsFromDescription(this TrelloExperiment experiment)
    {
        var members = experiment.TrelloState.TrelloDatabase.GetScientistsFromDescription().ToList();
        var description = experiment.Description.Split(TrelloSmartSettings.GetExperimentDivisor());
        experiment.Description = description[0].Trim();
        var scientists = new List<TrelloScientist>();
        if (description.Length > 1 && members is not null)
        {
            var assignees = description[1].Trim(' ', '.').Split(TrelloSmartSettings.GetSeparator());
            foreach (var assignee in assignees)
            {
                var assigneed = assignee.Trim();
                TrelloScientist? scientist = null;
                if (Regex.Match(assigneed, numberRegex).Success) 
		        {
                    scientist = members.FirstOrDefault(s => s.Code!.Equals(assigneed, StringComparison.OrdinalIgnoreCase));
                }
                if (Regex.Match(assigneed, letterRegex).Success)
                {
                    scientist = members.FirstOrDefault(s => s.FullName.Equals(assigneed, StringComparison.OrdinalIgnoreCase));
                }
                if (scientist is not null)
                {
                    scientists.Add(scientist);
                }
            }
        }
        experiment.TrelloScientists = scientists;
    }
}