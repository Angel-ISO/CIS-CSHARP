using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.seeds;
public class TopicSeed
{
      public async Task InitTopicsAsync(IMongoDatabase database)
    {
        var topicCollection = database.GetCollection<Topic>("Topics");

        var existing = await topicCollection.Find(_ => true).AnyAsync();
        if (existing)
        {
            Console.WriteLine("Topics already initialized. Skipping seed.");
            return;
        }

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "seeds", "Data", "Topic.json");
        var jsonText = await File.ReadAllTextAsync(jsonPath);
        using var document = JsonDocument.Parse(jsonText);

        var topics = new List<Topic>();

        foreach (var element in document.RootElement.EnumerateArray())
        {
            if (element.GetProperty("type").GetString() == "table")
            {
                foreach (var item in element.GetProperty("data").EnumerateArray())
                {
                    topics.Add(new Topic
                    {
                        Title = item.GetProperty("Title").GetString(),
                        Description = item.GetProperty("Description").GetString(),
                        Username = item.GetProperty("Username").GetString(),
                        UserId = Guid.Parse(item.GetProperty("UserId").GetString()!),
                        CreatedAt = DateTime.Parse(item.GetProperty("CreatedAt").GetString()!)
                    });
                }
            }
        }

        await topicCollection.InsertManyAsync(topics);
        Console.WriteLine("Topics migration completed.");
    }
}

