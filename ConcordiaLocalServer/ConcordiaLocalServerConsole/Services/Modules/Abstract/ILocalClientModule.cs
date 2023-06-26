namespace ConcordiaLocalServerConsole.Services.Modules.Abstract;

public interface ILocalClientModule : IModule
{
    public Task SelectAllExperiments();
    public Task SelectExperimentsByState();
    public Task SelectExperimentById();
    public Task SelectAllRemarksInExperiment();
    public Task SelectLastRemarkInExperiment();
    public Task InsertRemarkInExperiment();
    public Task InsertScientist();
    public Task UpdateStateInExperiment();
}