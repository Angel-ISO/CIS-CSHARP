using System;
using System.Collections.Generic;
using System.Linq;
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

            var existingIdeas = await ideaCollection.Find(_ => true).ToListAsync();
            if (existingIdeas.Count > 0)
            {
                Console.WriteLine("Ideas already initialized. Skipping seed.");
                return;
            }

            var topic = await topicCollection.Find(t => t.Title == "Climate Change").FirstOrDefaultAsync();

            if (topic == null)
            {
                Console.WriteLine("Topic not found. Skipping idea seed.");
                return;
            }

            var ideas = new List<Idea>
            {
                new Idea
                {
                    Title = "Impact on Biodiversity",
                    Description = "Discussing how climate change affects biodiversity.",
                    Username = "RAODRIGO_234",
                    UserId = Guid.Parse("d7680648-76af-4125-81e7-ec4eab160d10"),
                    TopicId = topic.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Idea
                {
                    Title = "Carbon Emissions",
                    Description = "How carbon emissions are affecting global temperatures.",
                    Username = "RAODRIGO_234",
                    UserId = Guid.Parse("d7680648-76af-4125-81e7-ec4eab160d10"),
                    TopicId = topic.Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await ideaCollection.InsertManyAsync(ideas);
            Console.WriteLine("Ideas created successfully.");
        }
} 

