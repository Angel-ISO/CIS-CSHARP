using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
public class TopicRepository  : GenericRepository<Topic>, Itopic
{
    private readonly CisContext _context;
    public TopicRepository(CisContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<(int totalRegistros, IEnumerable<Topic> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Topics as IQueryable<Topic>;
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Title.ToLower().Contains(search));
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Include(p => p.Ideas)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }



    public override async Task<Topic> GetByIdAsync(Guid id)
{
    return await _context.Topics
        .Include(p => p.Ideas)
        .FirstOrDefaultAsync(p => p.Id == id);
}



    


    public override async Task<IEnumerable<Topic>> GetAllAsync()
    {
        return await _context.Topics.Include(p => p.Ideas)
        .ToListAsync();
    }

}
