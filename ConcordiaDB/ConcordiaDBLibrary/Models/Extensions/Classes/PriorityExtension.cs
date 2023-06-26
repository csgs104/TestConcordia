namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class PriorityExtension
{
    public static string ToShortString(this Priority priority)
    {
        return $"{nameof(Priority.Id)}:{priority.Id},{nameof(Priority.Name)}:{priority.Name}";
    }

    public static string ToLongString(this Priority priority)
    {
        throw new NotImplementedException();
    }
}

