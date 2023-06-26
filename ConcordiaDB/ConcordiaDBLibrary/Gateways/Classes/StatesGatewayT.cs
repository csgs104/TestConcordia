namespace ConcordiaDBLibrary.Gateways.Classes;

using Gateways.Abstract;
using Models.Abstract;
using Models.Classes;
using Microsoft.EntityFrameworkCore;

public partial class StatesGateway : ITrelloEntityGateway<State>
{
    public State? GetByCode(string? code)
    {
        var state = _context.States.AsNoTracking().FirstOrDefault(e => e.Code == code);
        return state;
    }

    public IEnumerable<State>? GetByCodeMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        State? state = null;
        foreach (var code in codes)
        {
            state = GetByCode(code);
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

    public State InsertByCode(State tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Id is not null) throw new Exception("Not null id.");
        if (tentity.Code is null) throw new Exception("No valid codes.");
        var state = _context.States.Add(tentity);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? InsertMultiByCode(IEnumerable<State>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        var states = new List<State>();
        foreach (var tentity in tentities) states.Add(_context.States.Add(tentity).Entity);
        _context.SaveChanges();
        return states;
    }

    public State UpdateByCode(State tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        var state = _context.States.Update(tentity);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? UpdateMultiByCode(IEnumerable<State>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        foreach (var tentity in tentities)
        {
            if (tentity is null) throw new Exception("No valid entity.");
            if (tentity.Code is null) throw new Exception("No valid codes.");
            if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        }
        var states = new List<State>();
        State? state = null;
        foreach (var entity in tentities)
        {
            state = _context.States.Update(entity).Entity;
            states.Add(state);
        }
        _context.SaveChanges();
        return states;
    }

    public State DeleteByCode(string? code)
    {
        var entityOld = GetByCode(code);
        if (entityOld is null) throw new Exception("No valid entity.");
        var state = _context.States.Remove(entityOld);
        _context.SaveChanges();
        return state.Entity;
    }

    public IEnumerable<State>? DeleteMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        foreach (var code in codes)
        {
            if (GetByCode(code) is null) throw new Exception("No valid entity.");
        }
        var states = new List<State>();
        State? state = null;
        foreach (var code in codes)
        {
            state = _context.States.Remove(GetByCode(code)!).Entity;
            states.Add(state);
        }
        _context.SaveChanges();
        return states;
    }
}