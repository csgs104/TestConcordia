namespace ConcordiaTrelloLibrary.Models.Classes;

using Abstract;

public class TrelloExperiment : TrelloObject // TrelloExperiment = Card
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public TrelloPriority TrelloPriority { get; set; }
    public TrelloState TrelloState { get; set; }
    public IEnumerable<TrelloScientist>? TrelloScientists { get; set; }
    public IEnumerable<TrelloRemark>? TrelloRemarks { get; set; }

    public TrelloExperiment(string? code, string name, string description, 
	                   DateTimeOffset? startDate, DateTimeOffset? dueDate, 
		               TrelloPriority priority, TrelloState state,
                       IEnumerable<TrelloScientist>? scientist, IEnumerable<TrelloRemark>? remarks) 
	: base(code) 
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        DueDate = dueDate;
        TrelloPriority = priority;
        TrelloState = state;
        TrelloScientists = scientist;
        TrelloRemarks = remarks;
    }

    public override string ToString()
    {
        return string.Concat($"{base.ToString()}, ",
               $"{nameof(TrelloExperiment.Name)}:{Name}, ",
               $"{nameof(TrelloExperiment.Description)}:{Description}, ",
               $"{nameof(TrelloExperiment.StartDate)}:{StartDate}, ",
               $"{nameof(TrelloExperiment.DueDate)}:{DueDate}, ",
               $"{nameof(TrelloExperiment.TrelloPriority)}:{TrelloPriority}, ",
               $"{nameof(TrelloExperiment.TrelloState)}:{TrelloState}, ",
               $"{nameof(TrelloExperiment.TrelloScientists)}:{TrelloScientists?.Aggregate(string.Empty, (c, t) => $"{c}{t.Code}, ").TrimEnd(' ', ',')}, ",
               $"{nameof(TrelloExperiment.TrelloRemarks)}:{TrelloRemarks?.Aggregate(string.Empty, (c, t) => $"{c}{t.Code}, ").TrimEnd(' ',',')}");
    }
}