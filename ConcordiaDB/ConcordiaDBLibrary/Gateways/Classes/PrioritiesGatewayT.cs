namespace ConcordiaDBLibrary.Gateways.Classes;

using Gateways.Abstract;
using Models.Abstract;
using Models.Classes;
using Microsoft.EntityFrameworkCore;

public partial class PrioritiesGateway : ITrelloEntityGateway<Priority>
{
    public Priority? GetByCode(string? code)
    {
        var priority = _context.Priorities.AsNoTracking().FirstOrDefault(e => e.Code == code);
        return priority;
    }

    public IEnumerable<Priority>? GetByCodeMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        Priority? priority = null;
        foreach (var code in codes)
        {
            priority = GetByCode(code);
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

    public Priority InsertByCode(Priority tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Id is not null) throw new Exception("Not null id.");
        if (tentity.Code is null) throw new Exception("No valid codes.");
        var priority = _context.Priorities.Add(tentity);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? InsertMultiByCode(IEnumerable<Priority>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        var priorities = new List<Priority>();
        foreach (var tentity in tentities) priorities.Add(_context.Priorities.Add(tentity).Entity);
        _context.SaveChanges();
        return priorities;
    }

    public Priority UpdateByCode(Priority tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        var priority = _context.Priorities.Update(tentity);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? UpdateMultiByCode(IEnumerable<Priority>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        foreach (var tentity in tentities)
        {
            if (tentity is null) throw new Exception("No valid entity.");
            if (tentity.Code is null) throw new Exception("No valid codes.");
            if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        }
        var priorities = new List<Priority>();
        Priority? priority = null;
        foreach (var entity in tentities)
        {
            priority = _context.Priorities.Update(entity).Entity;
            priorities.Add(priority);
        }
        _context.SaveChanges();
        return priorities;
    }

    public Priority DeleteByCode(string? code)
    {
        var entityOld = GetByCode(code);
        if (entityOld is null) throw new Exception("No valid entity.");
        var priority = _context.Priorities.Remove(entityOld);
        _context.SaveChanges();
        return priority.Entity;
    }

    public IEnumerable<Priority>? DeleteMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        foreach (var code in codes)
        {
            if (GetByCode(code) is null) throw new Exception("No valid entity.");
        }
        var priorities = new List<Priority>();
        Priority? priority = null;
        foreach (var code in codes)
        {
            priority = _context.Priorities.Remove(GetByCode(code)!).Entity;
            priorities.Add(priority);
        }
        _context.SaveChanges();
        return priorities;
    }
}