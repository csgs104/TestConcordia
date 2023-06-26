namespace ConcordiaTrelloLibrary.Models.Classes;

using Abstract;
using Microsoft.VisualBasic;

public class TrelloDatabase : TrelloObject // TrelloDatabase = Board
{
    public string Name { get; set; }
    public string Description { get; set; }

    public TrelloDatabase(string? code, string name, string description)
    : base(code)
    {
        Name = name;
        Description = description;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloDatabase.Name)}:{Name}, ",
               $"{nameof(TrelloDatabase.Description)}:{Description}");
    }
}