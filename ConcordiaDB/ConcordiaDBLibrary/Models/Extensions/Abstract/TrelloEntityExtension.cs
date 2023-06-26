namespace ConcordiaDBLibrary.Models.Extensions.Abstract;

using System.Text.RegularExpressions;
using Models.Abstract;

public static class TrelloEntityExtension
{
    public static string ToString(this Entity entity)
    {
        return $"{nameof(Entity.Id)}:{entity.Id}";
    }

    public static bool IsCode(this TrelloEntity entity)
    {
        var regex = @"^[0-9a-zA-Z]{24}$";
        return entity.Code is null? false : Regex.Match(entity.Code, regex).Success;
    }
}