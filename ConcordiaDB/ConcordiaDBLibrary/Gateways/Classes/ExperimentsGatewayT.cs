namespace ConcordiaDBLibrary.Gateways.Classes;

using Gateways.Abstract;
using Models.Abstract;
using Models.Classes;
using Microsoft.EntityFrameworkCore;

public partial class ExperimentsGateway : ITrelloEntityGateway<Experiment>
{
    public Experiment? GetByCode(string? code) 
    {
        var experiment = _context.Experiments.Include(e => e.Priority).Include(e => e.State).AsNoTracking().FirstOrDefault(e => e.Code == code);
        return experiment;
    }

    public IEnumerable<Experiment>? GetByCodeMulti(IEnumerable<string>? codes) 
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        Experiment? experiment = null;
        foreach (var code in codes)
        {
            experiment = GetByCode(code);
            if (experiment is not null)
            {
                yield return experiment;
            }
            else
            {
                throw new Exception("No valid entity.");
            }
        }
    }

    public Experiment InsertByCode(Experiment tentity) 
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Id is not null) throw new Exception("Not null id.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        var experiment = _context.Experiments.Add(tentity);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? InsertMultiByCode(IEnumerable<Experiment>? tentities) 
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        var experiments = new List<Experiment>();
        foreach (var tentity in tentities) experiments.Add(_context.Experiments.Add(tentity).Entity);
        _context.SaveChanges();
        return experiments;
    }

    public Experiment UpdateByCode(Experiment tentity) 
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        var experiment = _context.Experiments.Update(tentity);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? UpdateMultiByCode(IEnumerable<Experiment>? tentities) 
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        foreach (var tentity in tentities)
        {
            if (tentity is null) throw new Exception("No valid entity.");
            if (tentity.Code is null) throw new Exception("No valid codes.");
            if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        }
        var experiments = new List<Experiment>();
        Experiment? experiment = null;
        foreach (var entity in tentities)
        {
            experiment = _context.Experiments.Update(entity).Entity;
            experiments.Add(experiment);
        }
        _context.SaveChanges();
        return experiments;
    }

    public Experiment DeleteByCode(string? code) 
    {
        var entityOld = GetByCode(code);
        if (entityOld is null) throw new Exception("No valid entity.");
        var experiment = _context.Experiments.Remove(entityOld);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? DeleteMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        foreach (var code in codes)
        {
            if (GetByCode(code) is null) throw new Exception("No valid entity.");
        }
        var experiments = new List<Experiment>();
        Experiment? experiment = null;
        foreach (var code in codes)
        {
            experiment = _context.Experiments.Remove(GetByCode(code)!).Entity;
            experiments.Add(experiment);
        }
        _context.SaveChanges();
        return experiments;
    }
}