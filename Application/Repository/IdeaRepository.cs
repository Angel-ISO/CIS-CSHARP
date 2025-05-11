using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Persistence;

namespace Application.Repository;
public class IdeaRepository : GenericRepository<Idea>, IIdea
{
     private readonly IMongoCollection<Idea> _ideas;
    private readonly IMongoCollection<Vote> _votes;
    private readonly IMongoCollection<Topic> _topics;

    public IdeaRepository(CisContext context) : base(context.Ideas)
    {
        _topics = context.Topics;
        _ideas = context.Ideas;
        _votes = context.Votes;
    }

    public override async Task<(int totalRegistros, IEnumerable<Idea> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var filter = string.IsNullOrEmpty(search)
            ? Builders<Idea>.Filter.Empty
            : Builders<Idea>.Filter.Regex(i => i.Title, new MongoDB.Bson.BsonRegularExpression(search, "i"));

        var totalRegistros = await _ideas.CountDocumentsAsync(filter);
        var registros = await _ideas.Find(filter)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();

        foreach (var idea in registros)
        {
            idea.Votes = await _votes.Find(v => v.IdeaId == idea.Id).ToListAsync();
            idea.Topic = await _topics.Find(t => t.Id == idea.TopicId).FirstOrDefaultAsync();
        }

        return ((int)totalRegistros, registros);
    }

    public override async Task<Idea?> GetByIdAsync(string id)
    {
        var idea = await _ideas.Find(i => i.Id == id).FirstOrDefaultAsync();

        if (idea != null)
        {
            idea.Votes = await _votes.Find(v => v.IdeaId == idea.Id).ToListAsync();
            idea.Topic = await _topics.Find(t => t.Id == idea.TopicId).FirstOrDefaultAsync();
        }

        return idea;
    }

    public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId)
    {
        var ideas = await _ideas.Find(i => i.TopicId == topicId)
                                .SortByDescending(i => i.CreatedAt)
                                .ToListAsync();

        foreach (var idea in ideas)
        {
            idea.Votes = await _votes.Find(v => v.IdeaId == idea.Id).ToListAsync();
            idea.Topic = await _topics.Find(t => t.Id == idea.TopicId).FirstOrDefaultAsync();
        }

        return ideas;
    }

}
