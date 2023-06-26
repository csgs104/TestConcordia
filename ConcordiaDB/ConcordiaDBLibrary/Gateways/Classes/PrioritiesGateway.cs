namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class PrioritiesGateway : ITrelloEntityGateway<Priority>
{
    private readonly ConcordiaContext _context;

    public PrioritiesGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<Priority> GetAll()
    {
        var priorities = _context.Priorities.AsNoTracking();
        return priorities;
    }

    public Priority? GetById(int id)
    {
        var priority = _context.Priorities.AsNoTracking().SingleOrDefault(e => e.Id == id);
        return priority;
    }

    public IEnumerable<Priority>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        Priority? priority = null;
        foreach (var id in ids)
        {
            priority = GetById(id);
            if (priority is not null)
            {
                yield return priority;
            }
            else
            {
                throw new Exception("No valid entity.");
            }
        }
    }

    public Priority Insert(Priority entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var priority = _context.Priorities.Add(entity);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? InsertMulti(IEnumerable<Priority>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var priorities = new List<Priority>();
        foreach (var entity in entities) priorities.Add(_context.Priorities.Add(entity).Entity);
        _context.SaveChanges();
        return priorities;
    }

    public Priority Update(Priority entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var priority = _context.Priorities.Update(entity);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? UpdateMulti(IEnumerable<Priority>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var priorities = new List<Priority>();
        Priority? priority = null;
        foreach (var entity in entities)
        {
            priority = _context.Priorities.Update(entity).Entity;
            priorities.Add(priority);
        }
        _context.SaveChanges();
        return priorities;
    }


    public Priority Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var priority = _context.Priorities.Remove(entityOld);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var priorities = new List<Priority>();
        Priority? priority = null;
        foreach (var id in ids)
        {
            priority = _context.Priorities.Remove(GetById(id)!).Entity;
            priorities.Add(priority);
        }
        _context.SaveChanges();
        return priorities;
    }
}