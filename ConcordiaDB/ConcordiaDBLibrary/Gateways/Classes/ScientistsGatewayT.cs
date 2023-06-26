namespace ConcordiaDBLibrary.Gateways.Classes;

using Gateways.Abstract;
using Models.Abstract;
using Models.Classes;
using Microsoft.EntityFrameworkCore;

public partial class ScientistsGateway : ITrelloEntityGateway<Scientist>
{
    public Scientist? GetByCode(string? code)
    {
        var scientist = _context.Scientists.AsNoTracking().FirstOrDefault(e => e.Code == code);
        return scientist;
    }

    public IEnumerable<Scientist>? GetByCodeMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        Scientist? scientist = null;
        foreach (var code in codes)
        {
            scientist = GetByCode(code);
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

    public Scientist InsertByCode(Scientist tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Id is not null) throw new Exception("Not null id.");
        if (tentity.Code is null) throw new Exception("No valid codes.");
        var scientist = _context.Scientists.Add(tentity);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? InsertMultiByCode(IEnumerable<Scientist>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        var scientists = new List<Scientist>();
        foreach (var tentity in tentities) scientists.Add(_context.Scientists.Add(tentity).Entity);
        _context.SaveChanges();
        return scientists;
    }

    public Scientist UpdateByCode(Scientist tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        var scientist = _context.Scientists.Update(tentity);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? UpdateMultiByCode(IEnumerable<Scientist>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        foreach (var tentity in tentities)
        {
            if (tentity is null) throw new Exception("No valid entity.");
            if (tentity.Code is null) throw new Exception("No valid codes.");
            if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        }
        var scientists = new List<Scientist>();
        Scientist? scientist = null;
        foreach (var entity in tentities)
        {
            scientist = _context.Scientists.Update(entity).Entity;
            scientists.Add(scientist);
        }
        _context.SaveChanges();
        return scientists;
    }

    public Scientist DeleteByCode(string? code)
    {
        var entityOld = GetByCode(code);
        if (entityOld is null) throw new Exception("No valid entity.");
        var scientist = _context.Scientists.Remove(entityOld);
        _context.SaveChanges();
        return scientist.Entity;
    }

    public IEnumerable<Scientist>? DeleteMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        foreach (var code in codes)
        {
            if (GetByCode(code) is null) throw new Exception("No valid entity.");
        }
        var scientists = new List<Scientist>();
        Scientist? scientist = null;
        foreach (var code in codes)
        {
            scientist = _context.Scientists.Remove(GetByCode(code)!).Entity;
            scientists.Add(scientist);
        }
        _context.SaveChanges();
        return scientists;
    }
}