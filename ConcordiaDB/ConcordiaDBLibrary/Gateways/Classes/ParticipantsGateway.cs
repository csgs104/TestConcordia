namespace ConcordiaDBLibrary.Gateways.Classes;

using Data;
using Models.Abstract;
using Models.Classes;
using Gateways.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class ParticipantsGateway : IEntityGateway<Participant>
{
    private readonly ConcordiaContext _context;

    public ParticipantsGateway(ConcordiaContext context)
    {
        _context = context;
    }

    public IEnumerable<Participant> GetAll()
    {
        var participants = _context.Participants.Include(e => e.Experiment).Include(e => e.Scientist).AsNoTracking();
        return participants;
    }

    public Participant? GetById(int id)
    {
        var participant = _context.Participants.Include(e => e.Experiment).Include(r => r.Scientist).AsNoTracking().SingleOrDefault(b => b.Id == id);
        return participant;
    }

    public IEnumerable<Participant>? GetByIdMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        Participant? participant = null;
        foreach (var id in ids)
        {
            participant = GetById(id);
            if (participant is not null)
            {
                yield return participant;
            }
            else 
	        {
                throw new Exception("No valid entity.");
            }
        }
    }

    public Participant Insert(Participant entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is not null) throw new Exception("Not null id.");
        var participant = _context.Participants.Add(entity);
        _context.SaveChanges();
        return participant.Entity;
    }

    public IEnumerable<Participant>? InsertMulti(IEnumerable<Participant>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is not null)) throw new Exception("Not null ids.");
        var participants = new List<Participant>();
        foreach (var entity in entities) participants.Add(_context.Participants.Add(entity).Entity);
        _context.SaveChanges();
        return participants;
    }

    public Participant Update(Participant entity)
    {
        if (entity is null) throw new Exception("No valid entity.");
        if (entity.Id is null) throw new Exception("No valid id.");
        if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        var participant = _context.Participants.Update(entity);
        _context.SaveChanges();
        return participant.Entity;
    }

    public IEnumerable<Participant>? UpdateMulti(IEnumerable<Participant>? entities)
    {
        if (entities is null || !entities.Any()) throw new Exception("No valid entities.");
        if (entities.Any(e => e.Id is null)) throw new Exception("No valid ids.");
        foreach (var entity in entities)
        {
            if (entity is null) throw new Exception("No valid entity.");
            if (entity.Id is null) throw new Exception("No valid id.");
            if (GetById((int)entity.Id) is null) throw new Exception("No valid entity.");
        }
        var participants = new List<Participant>();
        Participant? participant = null;
        foreach (var entity in entities)
        {
            participant = _context.Participants.Update(entity).Entity;
            participants.Add(participant);
        }
        _context.SaveChanges();
        return participants;
    }

    public Participant Delete(int id)
    {
        var entityOld = GetById(id);
        if (entityOld is null) throw new Exception("No valid entity.");
        var participant = _context.Participants.Remove(entityOld);
        _context.SaveChanges();
        return participant.Entity;
    }

    public IEnumerable<Participant>? DeleteMulti(IEnumerable<int>? ids)
    {
        if (ids is null || !ids.Any()) throw new Exception("No valid ids.");
        foreach (var id in ids)
        {
            if (GetById(id) is null) throw new Exception("No valid entity.");
        }
        var participants = new List<Participant>();
        Participant? participant = null;
        foreach (var id in ids)
        {
            participant = _context.Participants.Remove(GetById(id)!).Entity;
            participants.Add(participant);
        }
        _context.SaveChanges();
        return participants;
    }
}