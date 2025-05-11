using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Domain.Entities;
using MongoDB.Driver;
using Persistence;
using Application.Repository;
using Mongo2Go;
using MongoDB.Bson;

namespace MyProject.Tests.Repository;

public class VoteRepositoryTest
{
    private MongoDbRunner _runner;
    private IMongoClient _client;
    private CisContext _context;
    private VoteRepository _repository;

    [SetUp]
    public void Setup()
    {
        _runner = MongoDbRunner.Start();
        _client = new MongoClient(_runner.ConnectionString);
        _context = new CisContext(_client);
        _repository = new VoteRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.DropDatabase(_context.GetDatabase().DatabaseNamespace.DatabaseName);
        _client?.Dispose();
        _runner?.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllVotes()
    {
        var votes = new List<Vote>
        {
            new Vote { UserId = Guid.NewGuid(), Value = 1, VotedAt = DateTime.UtcNow, IdeaId = ObjectId.GenerateNewId().ToString() },
            new Vote { UserId = Guid.NewGuid(), Value = -1, VotedAt = DateTime.UtcNow, IdeaId = ObjectId.GenerateNewId().ToString() }
        };

        foreach (var vote in votes)
        {
            _repository.Add(vote);
        }

        var allVotes = await _repository.GetAllAsync();

        Assert.IsNotNull(allVotes);
        Assert.AreEqual(2, allVotes.Count());
    }

    [Test]
    public void Add_ShouldInsertVoteCorrectly()
    {
        var vote = new Vote
        {
            UserId = Guid.NewGuid(),
            Value = 1,
            VotedAt = DateTime.UtcNow,
            IdeaId = ObjectId.GenerateNewId().ToString()
        };

        _repository.Add(vote);
        var result = _repository.GetByIdAsync(vote.Id).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(vote.Value, result!.Value);
        Assert.AreEqual(vote.IdeaId, result.IdeaId);
    }

    [Test]
    public void Delete_ShouldRemoveVoteCorrectly()
    {
        var vote = new Vote
        {
            UserId = Guid.NewGuid(),
            Value = -1,
            VotedAt = DateTime.UtcNow,
            IdeaId = ObjectId.GenerateNewId().ToString()
        };

        _repository.Add(vote);

        var inserted = _repository.GetByIdAsync(vote.Id).Result;
        Assert.IsNotNull(inserted);

        _repository.Remove(vote);
        var deleted = _repository.GetByIdAsync(vote.Id).Result;

        Assert.IsNull(deleted);
    }

    [Test]
    public void Update_ShouldModifyVoteCorrectly()
    {
        var vote = new Vote
        {
            UserId = Guid.NewGuid(),
            Value = 0,
            VotedAt = DateTime.UtcNow,
            IdeaId = ObjectId.GenerateNewId().ToString()
        };

        _repository.Add(vote);

        var inserted = _repository.GetByIdAsync(vote.Id).Result;
        Assert.IsNotNull(inserted);

        inserted!.Value = 1;
        _repository.Update(inserted);

        var updated = _repository.GetByIdAsync(vote.Id).Result;

        Assert.IsNotNull(updated);
        Assert.AreEqual(1, updated!.Value);
    }
}
