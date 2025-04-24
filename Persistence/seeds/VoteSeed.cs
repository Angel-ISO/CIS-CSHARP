using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.seeds;
public class VoteSeed
{
     public async Task InitVotesAsync(IMongoDatabase database)
        {
            var voteCollection = database.GetCollection<Vote>("Votes");
            var ideaCollection = database.GetCollection<Idea>("Ideas");

            var existingVotes = await voteCollection.Find(_ => true).ToListAsync();
            if (existingVotes.Count > 0)
            {
                Console.WriteLine("Votes already initialized. Skipping seed.");
                return;
            }

            var idea = await ideaCollection.Find(i => i.Title == "Impact on Biodiversity").FirstOrDefaultAsync();

            if (idea == null)
            {
                Console.WriteLine("Idea not found. Skipping vote seed.");
                return;
            }

            var votes = new List<Vote>
            {
                new Vote
                {
                    UserId = Guid.NewGuid(),
                    Value = 1, 
                    IdeaId = idea.Id,
                    VotedAt = DateTime.UtcNow
                },
                new Vote
                {
                    UserId = Guid.NewGuid(),
                    Value = -1, 
                    IdeaId = idea.Id,
                    VotedAt = DateTime.UtcNow
                }
            };

            await voteCollection.InsertManyAsync(votes);
            Console.WriteLine("Votes created successfully.");
        }
}