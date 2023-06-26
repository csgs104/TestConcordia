namespace ConcordiaMVC.Models;

using ConcordiaDBLibrary.Models.Classes;

public class UserMultiList
{
    public Scientist Scientist { get; set; }
    public IEnumerable<Experiment>? ExperimentsIn0 { get; set; }
    public IEnumerable<Experiment>? ExperimentsIn1 { get; set; }
    public IEnumerable<Experiment>? ExperimentsIn2 { get; set; }


    public UserMultiList(Scientist scientist,
                         IEnumerable<Experiment>? experimentsIn0,
                         IEnumerable<Experiment>? experimentsIn1,
                         IEnumerable<Experiment>? experimentsIn2)
    {
        Scientist = scientist;
        ExperimentsIn0 = experimentsIn0;
        ExperimentsIn1 = experimentsIn1;
        ExperimentsIn2 = experimentsIn2;
    }

    public UserMultiList(Scientist scientist)
    : this(scientist, new List<Experiment>(), new List<Experiment>(), new List<Experiment>())
    { }
}
