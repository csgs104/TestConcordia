namespace ConcordiaTrelloLibrary.Models.Classes;

using Abstract;

public class TrelloPriority : TrelloObject // TrelloPriority = Label
{
    public string Name { get; set; }
    public string Color { get; set; }

    public TrelloPriority(string? code, string name, string color)
    : base(code)
    {
        Name = name;
        Color = color;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloPriority.Name)}:{Name}, ",
               $"{nameof(TrelloPriority.Color)}:{Color}");
    }
}