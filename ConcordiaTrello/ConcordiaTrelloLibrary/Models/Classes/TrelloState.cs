namespace ConcordiaTrelloLibrary.Models.Classes;

using Abstract;
using static System.Net.Mime.MediaTypeNames;

public class TrelloState : TrelloObject // TrelloState = List
{
    public string Name { get; set; }
    public TrelloDatabase TrelloDatabase { get; set; }

    public TrelloState(string? code, string name, TrelloDatabase database)
    : base(code)
    {
        Name = name;
        TrelloDatabase = database;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloState.Name)}:{Name}, ",
               $"{nameof(TrelloState.TrelloDatabase)}:{TrelloDatabase.Code}");
    }
}