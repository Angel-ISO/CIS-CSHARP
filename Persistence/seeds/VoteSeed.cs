using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        var existingVotes = await voteCollection.Find(_ => true).AnyAsync();
        if (existingVotes)
        {
            Console.WriteLine("Votes already initialized. Skipping seed.");
            return;
        }

        var ideaJsonPath = Path.Combine(AppContext.BaseDirectory, "seeds", "Data", "Idea.json");
        var ideaJsonText = await File.ReadAllTextAsync(ideaJsonPath);
        using var ideaDoc = JsonDocument.Parse(ideaJsonText);

        var ideaIdToTitle = new Dictionary<string, string>();
        foreach (var element in ideaDoc.RootElement.EnumerateArray())
        {
            if (element.GetProperty("type").GetString() == "table")
            {
                foreach (var item in element.GetProperty("data").EnumerateArray())
                {
                    var oldId = item.GetProperty("Id_Idea").GetString()!;
                    var title = item.GetProperty("Title").GetString()!;
                    ideaIdToTitle[oldId] = title;
                }
            }
        }

        var voteJsonPath = Path.Combine(AppContext.BaseDirectory, "seeds", "Data", "Vote.json");
        var voteJsonText = await File.ReadAllTextAsync(voteJsonPath);
        using var voteDoc = JsonDocument.Parse(voteJsonText);

        var votes = new List<Vote>();

        foreach (var element in voteDoc.RootElement.EnumerateArray())
        {
            if (element.GetProperty("type").GetString() == "table")
            {
                foreach (var item in element.GetProperty("data").EnumerateArray())
                {
                    var oldIdeaId = item.GetProperty("IdeaId").GetString()!;

                    if (!ideaIdToTitle.TryGetValue(oldIdeaId, out var title))
                    {
                        Console.WriteLine($"IdeaId {oldIdeaId} not found in Idea.json. Skipping vote.");
                        continue;
                    }

                    var ideaInMongo = await ideaCollection.Find(i => i.Title == title).FirstOrDefaultAsync();
                    if (ideaInMongo == null)
                    {
                        Console.WriteLine($"Idea with title '{title}' not found in Mongo. Skipping vote.");
                        continue;
                    }

                    votes.Add(new Vote
                    {
                        UserId = Guid.Parse(item.GetProperty("UserId").GetString()!),
                        Value = int.Parse(item.GetProperty("Value").GetString()!),
                        IdeaId = ideaInMongo.Id,
                        VotedAt = DateTime.Parse(item.GetProperty("VotedAt").GetString()!)
                    });
                }
            }
        }

        await voteCollection.InsertManyAsync(votes);
        Console.WriteLine("Votes seeded.");
    }
}