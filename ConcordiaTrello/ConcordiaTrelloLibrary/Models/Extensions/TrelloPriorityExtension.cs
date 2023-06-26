namespace ConcordiaTrelloLibrary.Models.Extensions;

using Classes;
using TrelloDotNet.Model;

public static class TrelloPriorityExtension
{
    public static TrelloPriority? GetTrelloPriorityByName(IEnumerable<TrelloPriority> priorities, string name)
    {
        return priorities.SingleOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}