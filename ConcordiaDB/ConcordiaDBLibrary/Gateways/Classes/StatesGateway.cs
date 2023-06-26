namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class StatesGateway : ITrelloEntityGateway<State>
{
    private readonly ConcordiaContext _context;

    public StatesGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<State> GetAll()
    {
        var states = _context.States.AsNoTracking();
        return states;
    }

    public State? GetById(int id)
    {
        var state = _context.States.AsNoTracking().SingleOrDefault(e => e.Id == id);
        return state;
    }

    public IEnumerable<State>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        State? state = null;
        foreach (var id in ids)
        {
            state = GetById(id);
            if (state is not null)
            {
                yield return state;
            }
            else
            {
                throw new Exception("No valid entity.");
            }
        }
    }

    public State Insert(State entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var state = _context.States.Add(entity);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? InsertMulti(IEnumerable<State>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var states = new List<State>();
        foreach (var entity in entities) states.Add(_context.States.Add(entity).Entity);
        _context.SaveChanges();
        return states;
    }

    public State Update(State entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var state = _context.States.Update(entity);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? UpdateMulti(IEnumerable<State>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var states = new List<State>();
        State? state = null;
        foreach (var entity in entities)
        {
            state = _context.States.Update(entity).Entity;
            states.Add(state);
        }
        _context.SaveChanges();
        return states;
    }

    public State Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var state = _context.States.Remove(entityOld);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var states = new List<State>();
        State? state = null;
        foreach (var id in ids)
        {
            state = _context.States.Remove(GetById(id)!).Entity;
            states.Add(state);
        }
        _context.SaveChanges();
        return states;
    }
}