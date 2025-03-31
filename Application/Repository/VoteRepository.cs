using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;
public class VoteRepository : GenericRepository<Vote>, Ivote
{
    private readonly CisContext _context;
    public VoteRepository(CisContext context) : base(context)
    {
        _context = context;
    }
}
