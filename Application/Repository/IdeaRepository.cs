using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;
public class IdeaRepository : GenericRepository<Idea>, Iidea
{
    private readonly CisContext _context;
    public IdeaRepository(CisContext context) : base(context)
    {
        _context = context;
    }

    

}
