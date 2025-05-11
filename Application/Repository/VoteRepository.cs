using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence;

namespace Application.Repository;
public class VoteRepository : GenericRepository<Vote>, IVote
{
   private readonly IMongoCollection<Vote> _votes;
    private readonly IMongoCollection<Idea> _ideas;

    public VoteRepository(CisContext context) : base(context.Votes)
    {
        _ideas = context.Ideas;
        _votes = context.Votes;
    }

    public override async Task<(int totalRegistros, IEnumerable<Vote> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var filter = Builders<Vote>.Filter.Empty;

        if (!string.IsNullOrEmpty(search))
        {
            var regexFilter = Builders<Idea>.Filter.Regex(i => i.Title, new BsonRegularExpression(search, "i"));
            var matchingIdeas = await _ideas.Find(regexFilter)
                                            .Project(i => i.Id)
                                            .ToListAsync();

            filter = Builders<Vote>.Filter.In(v => v.IdeaId, matchingIdeas);
        }

        var totalRegistros = await _votes.CountDocumentsAsync(filter);
        var registros = await _votes.Find(filter)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();

        foreach (var vote in registros)
        {
            vote.Idea = await _ideas.Find(i => i.Id == vote.IdeaId).FirstOrDefaultAsync();
        }

        return ((int)totalRegistros, registros);
    }

    public async Task<Vote?> GetByUserAndIdeaAsync(Guid userId, string ideaId)
    {
        return await _votes
            .Find(v => v.UserId == userId && v.IdeaId == ideaId.ToString())
            .FirstOrDefaultAsync();
    }
    


      public override async Task<Vote?> GetByIdAsync(string id)
        {
            var vote = await _votes.Find(t => t.Id == id)
                                    .FirstOrDefaultAsync();

            if (vote != null)
            {
                vote.Idea = await _ideas.Find(i => i.Id == vote.IdeaId).FirstOrDefaultAsync();
            }

            return vote;
        }
}