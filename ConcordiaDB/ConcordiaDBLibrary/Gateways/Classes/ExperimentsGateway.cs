namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class ExperimentsGateway : ITrelloEntityGateway<Experiment>
{
    private readonly ConcordiaContext _context;

    public ExperimentsGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<Experiment> GetAll()
    {
        var experiments = _context.Experiments.Include(e => e.Priority).Include(e => e.State).AsNoTracking();
        return experiments;
    }

    public Experiment? GetById(int id)
    {
        var experiment = _context.Experiments.Include(e => e.Priority).Include(r => r.State).AsNoTracking().SingleOrDefault(b => b.Id == id);
        return experiment;
    }

    public IEnumerable<Experiment>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        Experiment? experiment = null;
        foreach (var id in ids)
        {
            experiment = GetById(id);
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

    public Experiment Insert(Experiment entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var experiment = _context.Experiments.Add(entity);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? InsertMulti(IEnumerable<Experiment>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var experiments = new List<Experiment>();
        foreach (var entity in entities) experiments.Add(_context.Experiments.Add(entity).Entity);
        _context.SaveChanges();
        return experiments;
    }

    public Experiment Update(Experiment entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var experiment = _context.Experiments.Update(entity);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? UpdateMulti(IEnumerable<Experiment>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var experiments = new List<Experiment>();
        Experiment? experiment = null;
        foreach (var entity in entities)
        {
            experiment = _context.Experiments.Update(entity).Entity;
            experiments.Add(experiment);
        }
        _context.SaveChanges();
        return experiments;
    }

    public Experiment Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var experiment = _context.Experiments.Remove(entityOld);
        _context.SaveChanges();
        return experiment.Entity;
    }

    public IEnumerable<Experiment>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var experiments = new List<Experiment>();
        Experiment? experiment = null;
        foreach (var id in ids)
        {
            experiment = _context.Experiments.Remove(GetById(id)!).Entity;
            experiments.Add(experiment);
        }
        _context.SaveChanges();
        return experiments;
    }
}