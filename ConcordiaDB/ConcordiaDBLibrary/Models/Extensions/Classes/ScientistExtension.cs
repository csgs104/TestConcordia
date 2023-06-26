namespace ConcordiaDBLibrary.Models.Extensions.Classes;

using Models.Abstract;
using Models.Classes;

public static class ScientistExtension
{
    public static string ToShortString(this Scientist scientist)
    {
        return $"{nameof(Scientist.Id)}:{scientist.Id},{nameof(Scientist.FullName)}:{scientist.FullName}";
    }

    public static string ToLongString(this Scientist scientist)
    {
        throw new NotImplementedException();
    }
}

