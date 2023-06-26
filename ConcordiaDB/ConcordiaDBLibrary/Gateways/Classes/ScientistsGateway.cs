namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class ScientistsGateway : ITrelloEntityGateway<Scientist>
{
    private readonly ConcordiaContext _context;

    public ScientistsGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<Scientist> GetAll()
    {
        var scientists = _context.Scientists.AsNoTracking();
        return scientists;
    }

    public Scientist? GetById(int id)
    {
        var scientist = _context.Scientists.AsNoTracking().SingleOrDefault(e => e.Id == id);
        return scientist;
    }

    public IEnumerable<Scientist>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        Scientist? scientist = null;
        foreach (var id in ids)
        {
            scientist = GetById(id);
            if (scientist is not null)
            {
                yield return scientist;
            }
            else
            {
                throw new Exception("No valid entity.");
            }
        }
    }

    public Scientist Insert(Scientist entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var scientist = _context.Scientists.Add(entity);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? InsertMulti(IEnumerable<Scientist>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var scientists = new List<Scientist>();
        foreach (var entity in entities) scientists.Add(_context.Scientists.Add(entity).Entity);
        _context.SaveChanges();
        return scientists;
    }

    public Scientist Update(Scientist entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var scientist = _context.Scientists.Update(entity);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? UpdateMulti(IEnumerable<Scientist>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var scientists = new List<Scientist>();
        Scientist? scientist = null;
        foreach (var entity in entities)
        {
            scientist = _context.Scientists.Update(entity).Entity;
            scientists.Add(scientist);
        }
        _context.SaveChanges();
        return scientists;
    }

    public Scientist Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var scientist = _context.Scientists.Remove(entityOld);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var scientists = new List<Scientist>();
        Scientist? scientist = null;
        foreach (var id in ids)
        {
            scientist = _context.Scientists.Remove(GetById(id)!).Entity;
            scientists.Add(scientist);
        }
        _context.SaveChanges();
        return scientists;
    }
}

