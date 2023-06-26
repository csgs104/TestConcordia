namespace ConcordiaLocalServerConsole.Services.Modules.Abstract;

public interface ILocalClientTestModule : IModule
{
    public Task StringUpperizerAsync();
    public Task StringLowerizerAsync();
    public Task StringRepeaterAsync();
}