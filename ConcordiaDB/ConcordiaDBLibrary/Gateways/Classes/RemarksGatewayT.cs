namespace ConcordiaDBLibrary.Gateways.Classes;

using Gateways.Abstract;
using Models.Abstract;
using Models.Classes;
using Microsoft.EntityFrameworkCore;

public partial class RemarksGateway : ITrelloEntityGateway<Remark>
{
    public Remark? GetByCode(string? code)
    {
        var remark = _context.Remarks.Include(r => r.Experiment).Include(r => r.Scientist).AsNoTracking().FirstOrDefault(e => e.Code == code);
        return remark;
    }

    public IEnumerable<Remark>? GetByCodeMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        Remark? remark = null;
        foreach (var code in codes)
        {
            remark = GetByCode(code);
            if (remark is not null)
            {
                yield return remark;
            }
            else
            {
                throw new Exception("No valid entity.");
            }
        }
    }

    public Remark InsertByCode(Remark tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Id is not null) throw new Exception("Not null id.");
        if (tentity.Code is null) throw new Exception("No valid codes.");
        var remarks = _context.Remarks.Add(tentity);
        _context.SaveChanges();
        return remarks.Entity;
    }

    public IEnumerable<Remark>? InsertMultiByCode(IEnumerable<Remark>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        var remarks = new List<Remark>();
        foreach (var tentity in tentities) remarks.Add(_context.Remarks.Add(tentity).Entity);
        _context.SaveChanges();
        return remarks;
    }

    public Remark UpdateByCode(Remark tentity)
    {
        if (tentity is null) throw new Exception("No valid entity.");
        if (tentity.Code is null) throw new Exception("No valid code.");
        if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        var remark = _context.Remarks.Update(tentity);
        _context.SaveChanges();
        return remark.Entity;
    }

    public IEnumerable<Remark>? UpdateMultiByCode(IEnumerable<Remark>? tentities)
    {
        if (tentities is null || !tentities.Any()) throw new Exception("No valid entities.");
        if (tentities.Any(e => e.Code is null)) throw new Exception("No valid codes.");
        foreach (var tentity in tentities)
        {
            if (tentity is null) throw new Exception("No valid entity.");
            if (tentity.Code is null) throw new Exception("No valid codes.");
            if (GetByCode(tentity.Code) is null) throw new Exception("No valid entity.");
        }
        var remarks = new List<Remark>();
        Remark? remark = null;
        foreach (var entity in tentities)
        {
            remark = _context.Remarks.Update(entity).Entity;
            remarks.Add(remark);
        }
        _context.SaveChanges();
        return remarks;
    }

    public Remark DeleteByCode(string? code)
    {
        var entityOld = GetByCode(code);
        if (entityOld is null) throw new Exception("No valid entity.");
        var remark = _context.Remarks.Remove(entityOld);
        _context.SaveChanges();
        return remark.Entity;
    }

    public IEnumerable<Remark>? DeleteMulti(IEnumerable<string>? codes)
    {
        if (codes is null || !codes.Any()) throw new Exception("No valid codes.");
        if (codes.Any(c => c is null)) throw new Exception("No valid codes.");
        foreach (var code in codes)
        {
            if (GetByCode(code) is null) throw new Exception("No valid entity.");
        }
        var remarks = new List<Remark>();
        Remark? remark = null;
        foreach (var code in codes)
        {
            remark = _context.Remarks.Remove(GetByCode(code)!).Entity;
            remarks.Add(remark);
        }
        _context.SaveChanges();
        return remarks;
    }
}