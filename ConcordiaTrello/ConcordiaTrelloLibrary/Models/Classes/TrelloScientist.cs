namespace ConcordiaTrelloLibrary.Models.Classes;

using System.Drawing;
using System.Xml.Linq;
using Abstract;

public class TrelloScientist : TrelloObject // TrelloScientist = Member
{
    public string FullName { get; set; }

    public TrelloScientist(string? code, string fullname)
    : base(code)
    {
        FullName = fullname;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloScientist.FullName)}:{FullName}");
    }
}