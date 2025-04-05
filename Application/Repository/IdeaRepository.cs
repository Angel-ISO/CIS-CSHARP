using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
public class IdeaRepository : GenericRepository<Idea>, Iidea
{
    private readonly CisContext _context;
    public IdeaRepository(CisContext context) : base(context)
    {
        _context = context;
    }

     public override async Task<(int totalRegistros, IEnumerable<Idea> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Ideas as IQueryable<Idea>;
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Title != null && p.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Include(p => p.Votes)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }



    public override async Task<Idea?> GetByIdAsync(Guid id)
{
    return await _context.Ideas
        .Include(p => p.Votes)
        .FirstOrDefaultAsync(p => p.Id == id);
}


public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(Guid topicId)
{
    return await _context.Ideas
        .OrderByDescending(i => i.CreatedAt)
        .Include(i => i.Votes)
        .Where(i => i.TopicId == topicId)
        .ToListAsync();
}


    

}
