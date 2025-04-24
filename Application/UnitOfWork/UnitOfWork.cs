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
    private Itopic? _topics;
    private Iidea? _ideas;
    private Ivote? _votes;

    public UnitOfWork(CisContext context)
    {
        _context = context;
    }

    public Itopic Topics => _topics ??= new TopicRepository(_context);
    public Iidea Ideas => _ideas ??= new IdeaRepository(_context);
    public Ivote Votes => _votes ??= new VoteRepository(_context);


      
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
