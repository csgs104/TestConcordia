namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class StateExtension
{
    public static string ToShortString(this State state)
    {
        return $"{nameof(State.Id)}:{state.Id},{nameof(State.Name)}:{state.Name}";
    }

    public static string ToLongString(this State state)
    {
        throw new NotImplementedException(); 
    }
}

