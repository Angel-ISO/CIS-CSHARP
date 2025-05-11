using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Domain.Entities;
using MongoDB.Driver;
using Persistence;
using Application.Repository;
using MongoDB.Bson;
using Mongo2Go;

namespace MyProject.Tests.Repository
{
    [TestFixture]
    public class IdeaRepositoryTest
    {
        private MongoDbRunner _runner;
        private IMongoClient _client;
        private CisContext _context;
        private IdeaRepository _repository;

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _context = new CisContext(_client);
            _repository = new IdeaRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.DropDatabase(_context.GetDatabase().DatabaseNamespace.DatabaseName);
            _client?.Dispose();
            _runner?.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllIdeas()
        {
            var ideas = new List<Idea>
            {
                new Idea
                {
                    TopicId = ObjectId.GenerateNewId().ToString(),
                    Title = "Idea 1",
                    Content = "Content 1",
                    UserId = Guid.NewGuid(),
                    Username = "user1",
                    CreatedAt = DateTime.UtcNow
                },
                new Idea
                {
                    TopicId = ObjectId.GenerateNewId().ToString(),
                    Title = "Idea 2",
                    Content = "Content 2",
                    UserId = Guid.NewGuid(),
                    Username = "user2",
                    CreatedAt = DateTime.UtcNow
                }
            };
            foreach (var idea in ideas)
            {
                _repository.Add(idea);
            }

            var (total, registros) = await _repository.GetAllAsync(1, 10, "");

            Assert.AreEqual(2, total, "Debe haber exactamente 2 ideas en total.");
            Assert.AreEqual(2, registros.Count(), "Se deben devolver las 2 ideas.");
            CollectionAssert.AreEquivalent(
                ideas.Select(i => i.Title),
                registros.Select(i => i.Title)
            );
        }

        [Test]
        public async Task Add_ShouldInsertIdeaCorrectly()
        {
            var idea = new Idea
            {
                TopicId = ObjectId.GenerateNewId().ToString(),
                Title = "Test Idea",
                Content = "This is a test idea",
                UserId = Guid.Parse("f4d5b2d0-e5c5-4c1c-b0c4-f8a4a8e8c0e1"),
                Username = "angelito_374",
                CreatedAt = DateTime.UtcNow
            };

            _repository.Add(idea);
            var result = await _repository.GetByIdAsync(idea.Id!);

            Assert.IsNotNull(result);
            Assert.AreEqual(idea.Title, result!.Title);
            Assert.AreEqual(idea.Content, result.Content);
        }

        [Test]
        public async Task GetIdeasByTopicIdAsync_ShouldReturnOnlyThatTopicIdeas()
        {
            var topicA = ObjectId.GenerateNewId().ToString();
            var topicB = ObjectId.GenerateNewId().ToString();

            var ideaA1 = new Idea { TopicId = topicA, Title = "A1", Content = "C1", UserId = Guid.NewGuid(), Username = "u1" };
            var ideaA2 = new Idea { TopicId = topicA, Title = "A2", Content = "C2", UserId = Guid.NewGuid(), Username = "u2" };
            var ideaB1 = new Idea { TopicId = topicB, Title = "B1", Content = "C3", UserId = Guid.NewGuid(), Username = "u3" };

            _repository.Add(ideaA1);
            _repository.Add(ideaA2);
            _repository.Add(ideaB1);

            var ideasForA = await _repository.GetIdeasByTopicIdAsync(topicA);

            Assert.AreEqual(2, ideasForA.Count());
            CollectionAssert.AreEquivalent(
                new[] { "A1", "A2" },
                ideasForA.Select(i => i.Title)
            );
        }

        [Test]
        public async Task Delete_ShouldRemoveIdeaCorrectly()
        {
            var idea = new Idea
            {
                TopicId = ObjectId.GenerateNewId().ToString(),
                Title = "Idea to Delete",
                Content = "Will be deleted",
                UserId = Guid.NewGuid(),
                Username = "tester",
                CreatedAt = DateTime.UtcNow
            };
            _repository.Add(idea);
            var inserted = await _repository.GetByIdAsync(idea.Id!);
            Assert.IsNotNull(inserted, "Debe existir la idea antes de borrarla.");

            _repository.Remove(idea);
            var deleted = await _repository.GetByIdAsync(idea.Id!);

            Assert.IsNull(deleted, "La idea debe ser nula tras eliminarla.");
        }

        [Test]
        public async Task Update_ShouldModifyIdeaCorrectly()
        {
            var idea = new Idea
            {
                TopicId = ObjectId.GenerateNewId().ToString(),
                Title = "Original Title",
                Content = "Original Content",
                UserId = Guid.NewGuid(),
                Username = "original_user",
                CreatedAt = DateTime.UtcNow
            };
            _repository.Add(idea);
            var inserted = await _repository.GetByIdAsync(idea.Id!);
            Assert.IsNotNull(inserted, "La idea debe existir antes de actualizarla.");

            inserted!.Title = "Updated Title";
            inserted.Content = "Updated Content";

            _repository.Update(inserted);
            var updated = await _repository.GetByIdAsync(idea.Id!);

            Assert.IsNotNull(updated, "La idea debe seguir existiendo tras la actualización.");
            Assert.AreEqual("Updated Title", updated!.Title);
            Assert.AreEqual("Updated Content", updated.Content);
        }
    }
}
