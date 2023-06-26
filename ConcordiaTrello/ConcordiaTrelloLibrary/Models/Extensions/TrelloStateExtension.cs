namespace ConcordiaTrelloLibrary.Models.Extensions;

using Classes;
using TrelloDotNet.Model;

public static class TrelloStateExtension
{
    public static TrelloState? GetTrelloStateByName(IEnumerable<TrelloState> states, string name)
    {
        return states.SingleOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}