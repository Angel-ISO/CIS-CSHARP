using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.seeds;

public class IdeaSeed
{
      public async Task InitIdeasAsync(IMongoDatabase database)
    {
        var ideaCollection = database.GetCollection<Idea>("Ideas");
        var topicCollection = database.GetCollection<Topic>("Topics");

        var existingIdeas = await ideaCollection.Find(_ => true).AnyAsync();
        if (existingIdeas)
        {
            Console.WriteLine("Ideas already initialized. Skipping seed.");
            return;
        }

        var mongoTopics = await topicCollection.Find(_ => true).ToListAsync();

        var topicIdMap = new Dictionary<string, string>();

        var jsonPathTopics = Path.Combine(AppContext.BaseDirectory, "seeds", "Data", "Topic.json");
        var jsonTextTopics = await File.ReadAllTextAsync(jsonPathTopics);
        using var documentTopics = JsonDocument.Parse(jsonTextTopics);

        foreach (var element in documentTopics.RootElement.EnumerateArray())
        {
            if (element.GetProperty("type").GetString() == "table")
            {
                foreach (var topic in element.GetProperty("data").EnumerateArray())
                {
                    var originalId = topic.GetProperty("Id_Topic").GetString();
                    if (!string.IsNullOrEmpty(originalId))
                    {
                        var title = topic.GetProperty("Title").GetString();
                        
                        var matchingTopic = mongoTopics.FirstOrDefault(t => t.Title == title);
                        if (matchingTopic != null && !string.IsNullOrEmpty(matchingTopic.Id))
                        {
                            topicIdMap[originalId] = matchingTopic.Id;
                        }
                    }
                }
            }
        }

        var jsonPathIdeas = Path.Combine(AppContext.BaseDirectory, "seeds", "Data", "Idea.json");
        var jsonTextIdeas = await File.ReadAllTextAsync(jsonPathIdeas);
        using var documentIdeas = JsonDocument.Parse(jsonTextIdeas);

        var ideas = new List<Idea>();

        foreach (var element in documentIdeas.RootElement.EnumerateArray())
        {
            if (element.GetProperty("type").GetString() == "table")
            {
                foreach (var item in element.GetProperty("data").EnumerateArray())
                {
                    var originalTopicId = item.GetProperty("TopicId").GetString();

                    if (!string.IsNullOrEmpty(originalTopicId) && topicIdMap.ContainsKey(originalTopicId))
                    {
                        ideas.Add(new Idea
                        {
                            Title = item.GetProperty("Title").GetString(),
                            Content = item.GetProperty("Description").GetString(),
                            Username = item.GetProperty("Username").GetString(),
                            UserId = Guid.Parse(item.GetProperty("UserId").GetString()!),
                            TopicId = topicIdMap[originalTopicId], 
                            CreatedAt = DateTime.Parse(item.GetProperty("CreatedAt").GetString()!)
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Topic with id '{originalTopicId}' not found. Skipping idea.");
                    }
                }
            }
        }

        await ideaCollection.InsertManyAsync(ideas);
        Console.WriteLine("Ideas migration completed.");
    }
}
 

