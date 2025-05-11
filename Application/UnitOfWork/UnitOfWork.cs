using Application.Repository;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;
public partial  class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly CisContext _context;
    
    public UnitOfWork(CisContext context)
    {
        _context = context;
    }


    
     public void Dispose()
    {
        throw new NotImplementedException();
    }
}
