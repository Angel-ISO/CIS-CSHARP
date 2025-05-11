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


namespace MyProject.Tests.Repository;

public class TopicRepositoryTest
{
    private MongoDbRunner _runner;
    private IMongoClient _client;
    private CisContext _context;
    private TopicRepository _repository;

    [SetUp]
    public void Setup()
    {
        _runner = MongoDbRunner.Start();

        _client = new MongoClient(_runner.ConnectionString);

        _context = new CisContext(_client);
        _repository = new TopicRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.DropDatabase(_context.GetDatabase().DatabaseNamespace.DatabaseName);
        _client?.Dispose();
        _runner?.Dispose();
    }


    [Test]
    public async Task GetAllAsync_ShouldReturnAllTopics()
    {
    
    var topics = new List<Topic>
    {
        new Topic
        {
            Title = "Topic 1",
            Description = "Description 1",
            UserId = Guid.NewGuid(),
            Username = "user1",
            CreatedAt = DateTime.UtcNow
        },
        new Topic
        {
            Title = "Topic 2",
            Description = "Description 2",
            UserId = Guid.NewGuid(),
            Username = "user2",
            CreatedAt = DateTime.UtcNow
        }
    };

    foreach (var topic in topics)
    {
        _repository.Add(topic);
    }

    
    var allTopics = await _repository.GetAllAsync();

   
    Assert.IsNotNull(allTopics);
    Assert.AreEqual(2, allTopics.Count(), "There should be exactly 2 topics in the collection.");
    CollectionAssert.AreEquivalent(topics.Select(t => t.Title), allTopics.Select(t => t.Title));
}

    [Test]
    public void Add_ShouldInsertTopicCorrectly()
    {
        var topic = new Topic
        {
            Title = "Test Topic",
            Description = "This is a test topic",
            UserId = Guid.Parse("f4d5b2d0-e5c5-4c1c-b0c4-f8a4a8e8c0e1"),
            Username = "angelito_374",
            CreatedAt = DateTime.UtcNow

        };

        _repository.Add(topic);

        var result = _repository.GetByIdAsync(topic.Id).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(topic.Title, result!.Title);
        Assert.AreEqual(topic.Description, result.Description);
    }

    [Test]
    public void Delete_ShouldRemoveTopicCorrectly()
    {
        var topic = new Topic
        {
            Title = "Topic to Delete",
            Description = "This topic will be deleted",
            UserId = Guid.NewGuid(),
            Username = "tester",
            CreatedAt = DateTime.UtcNow
        };

        _repository.Add(topic);

        var inserted = _repository.GetByIdAsync(topic.Id).Result;
        Assert.IsNotNull(inserted, "The topic should be inserted before deletion.");

        _repository.Remove(topic);

        var deleted = _repository.GetByIdAsync(topic.Id).Result;
        Assert.IsNull(deleted, "The topic should be null after deletion.");
    }
        
    [Test]
    public void Update_ShouldModifyTopicCorrectly()
    {
        var topic = new Topic
        {
            Title = "Original Title",
            Description = "Original Description",
            UserId = Guid.NewGuid(),
            Username = "original_user",
            CreatedAt = DateTime.UtcNow
        };

        _repository.Add(topic);

        var inserted = _repository.GetByIdAsync(topic.Id).Result;
        Assert.IsNotNull(inserted, "The topic should exist before update.");

        inserted!.Title = "Updated Title";
        inserted.Description = "Updated Description";
        inserted.Username = "updated_user";

        _repository.Update(inserted);

        var updated = _repository.GetByIdAsync(topic.Id).Result;
        Assert.IsNotNull(updated, "The topic should still exist after update.");
        Assert.AreEqual("Updated Title", updated!.Title);
        Assert.AreEqual("Updated Description", updated.Description);
        Assert.AreEqual("updated_user", updated.Username);
    }

}
