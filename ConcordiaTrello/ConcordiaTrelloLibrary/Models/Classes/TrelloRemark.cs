namespace ConcordiaTrelloLibrary.Models.Classes;

using System.Xml.Linq;
using Abstract;

public class TrelloRemark : TrelloObject // TrelloRemarks = Comments
{
    public string Text { get; set; }
    public DateTimeOffset? Date { get; set; }
    public TrelloScientist TrelloScientist { get; set; }
    public TrelloDatabase TrelloDatabase { get; set; }

    public TrelloRemark(string? code, string text, DateTimeOffset? date, 
	                    TrelloScientist scientist, TrelloDatabase database)
    : base(code)
    {
        Text = text;
        Date = date;
        TrelloScientist = scientist;
        TrelloDatabase = database;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloRemark.Text)}:{Text}, ",
               $"{nameof(TrelloRemark.Date)}:{Date}, ",
               $"{nameof(TrelloRemark.TrelloScientist)}:{TrelloScientist.ToString()}, ",
               $"{nameof(TrelloRemark.TrelloDatabase)}:{TrelloDatabase.Code}");
    }
}