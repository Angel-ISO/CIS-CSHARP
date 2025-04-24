using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Persistence.seeds;
  public class SeedOrchestrator : IHostedService
    {
        private readonly IMongoDatabase _database;
        private readonly TopicSeed _topicSeed;
        private readonly IdeaSeed _ideaSeed;
        private readonly VoteSeed _voteSeed;

     
        public SeedOrchestrator(IMongoDatabase database, TopicSeed topicSeed, IdeaSeed ideaSeed, VoteSeed voteSeed)
        {
            _database = database;
            _topicSeed = topicSeed;
            _ideaSeed = ideaSeed;
            _voteSeed = voteSeed;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _topicSeed.InitTopicsAsync(_database);
            await _ideaSeed.InitIdeasAsync(_database);
            await _voteSeed.InitVotesAsync(_database);
            

            Console.WriteLine("All seeds initialized successfully.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
