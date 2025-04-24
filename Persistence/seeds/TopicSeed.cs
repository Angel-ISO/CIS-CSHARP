using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.seeds;
public class TopicSeed
{
     public async Task InitTopicsAsync(IMongoDatabase database)
        {
            var topicCollection = database.GetCollection<Topic>("Topics");

            var existingTopics = await topicCollection.Find(_ => true).ToListAsync();
            if (existingTopics.Count > 0)
            {
                Console.WriteLine("Topics already initialized. Skipping seed.");
                return;
            }

            var topics = new List<Topic>
            {
                new Topic
                {
                    Title = "Climate Change",
                    Description = "A discussion about the effects of climate change.",
                    Username = "catriel_72",
                    UserId = Guid.Parse("44416e81-dd00-4336-8401-e1457cd2cf9e"),
                    CreatedAt = DateTime.UtcNow
                },
                new Topic
                {
                    Title = "Technology in Education",
                    Description = "The impact of technology on modern education.",
                    Username = "angelito_374",
                    UserId = Guid.Parse("e208b071-d6fc-4117-abae-093dc0420864"),
                    CreatedAt = DateTime.UtcNow
                }
            };

            await topicCollection.InsertManyAsync(topics);
            Console.WriteLine("Topics created successfully.");
        }
}

