namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class RemarksGateway : ITrelloEntityGateway<Remark>
{
    private readonly ConcordiaContext _context;

    public RemarksGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<Remark> GetAll()
    {
        var remarks = _context.Remarks.Include(e => e.Experiment).AsNoTracking().Include(e => e.Scientist);
        return remarks;
    }

    public Remark? GetById(int id)
    {
        var remark = _context.Remarks.Include(e => e.Experiment).Include(e => e.Scientist).AsNoTracking().SingleOrDefault(e => e.Id == id);
        return remark;
    }

    public IEnumerable<Remark>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        Remark? remark = null;
        foreach (var id in ids)
        {
            remark = GetById(id);
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

    public Remark Insert(Remark entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var remark = _context.Remarks.Add(entity);
        _context.SaveChanges();
        return remark.Entity;
    }

    public IEnumerable<Remark>? InsertMulti(IEnumerable<Remark>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var remarks = new List<Remark>();
        foreach (var entity in entities) remarks.Add(_context.Remarks.Add(entity).Entity);
        _context.SaveChanges();
        return remarks;
    }

    public Remark Update(Remark entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var remark = _context.Remarks.Update(entity);
        _context.SaveChanges();
        return remark.Entity;
    }

    public IEnumerable<Remark>? UpdateMulti(IEnumerable<Remark>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var remarks = new List<Remark>();
        Remark? remark = null;
        foreach (var entity in entities)
        {
            remark = _context.Remarks.Update(entity).Entity;
            remarks.Add(remark);
        }
        _context.SaveChanges();
        return remarks;
    }


    public Remark Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var remark = _context.Remarks.Remove(entityOld);
        _context.SaveChanges();
        return remark.Entity;
    }

    public IEnumerable<Remark>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var remarks = new List<Remark>();
        Remark? remark = null;
        foreach (var id in ids)
        {
            remark = _context.Remarks.Remove(GetById(id)!).Entity;
            remarks.Add(remark);
        }
        _context.SaveChanges();
        return remarks;
    }
}