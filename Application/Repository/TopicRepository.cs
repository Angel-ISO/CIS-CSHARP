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


public class TopicRepository : GenericRepository<Topic>, Itopic
{
    private readonly IMongoCollection<Topic> _topics;
    private readonly IMongoCollection<Idea> _ideas;
    private readonly IMongoCollection<Vote> _votes;


    public TopicRepository(CisContext context) : base(context.Topics)
    {
        _topics = context.Topics;
        _ideas = context.Ideas;
        _votes = context.Votes;
    }

    public override async Task<(int totalRegistros, IEnumerable<Topic> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var filter = string.IsNullOrEmpty(search)
            ? Builders<Topic>.Filter.Empty
            : Builders<Topic>.Filter.Regex(t => t.Title, new MongoDB.Bson.BsonRegularExpression(search, "i"));

        var totalRegistros = await _topics.CountDocumentsAsync(filter);
        var registros = await _topics.Find(filter)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Limit(pageSize)
                                     .ToListAsync();

       foreach (var topic in registros)
        {
            var ideas = await _ideas.Find(i => i.TopicId == topic.Id).ToListAsync();

            foreach (var idea in ideas)
            {
                idea.Votes = await _votes.Find(v => v.IdeaId == idea.Id).ToListAsync();
            }

            topic.Ideas = ideas;
        }

        return ((int)totalRegistros, registros);
    }

    public override async Task<Topic?> GetByIdAsync(string id)
    {
        var topic = await _topics.Find(t => t.Id == id).FirstOrDefaultAsync();

        if (topic != null)
        {
            var ideas = await _ideas.Find(i => i.TopicId == topic.Id).ToListAsync();

            foreach (var idea in ideas)
            {
                idea.Votes = await _votes.Find(v => v.IdeaId == idea.Id).ToListAsync();
            }

            topic.Ideas = ideas;
        }

        return topic;
    }

}