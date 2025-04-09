using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
public class VoteRepository : GenericRepository<Vote>, Ivote
{
    private readonly CisContext _context;
    public VoteRepository(CisContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<(int totalRegistros, IEnumerable<Vote> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Votes as IQueryable<Vote>;
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.IdeaId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Include(p => p.Idea)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }

    public async Task<Vote?> GetByUserAndIdeaAsync(Guid userId, Guid ideaId)
    {
        return await _context.Votes
        .FirstOrDefaultAsync(v => v.UserId == userId && v.IdeaId == ideaId);
    }

    
}
