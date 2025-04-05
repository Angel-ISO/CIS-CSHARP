using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly CisContext _context;
    private Itopic ? _topics;
    private Iidea ? _ideas;
    private Ivote ? _votes;


     public UnitOfWork(CisContext context)
    {
        _context = context;
    }



     public Itopic Topics
    {
        get
        {
            _topics ??= new TopicRepository(_context);
            return _topics;
        }
    }

    public Iidea Ideas
    {
        get
        {
            _ideas ??= new IdeaRepository(_context);
            return _ideas;
        }
    }

    public Ivote Votes
    {
        get
        {
            _votes ??= new VoteRepository(_context);
            return _votes;
        }
    }

      public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
