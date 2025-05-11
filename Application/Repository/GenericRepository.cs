using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Persistence;

namespace Application.Repository;
public class GenericRepository <T> : IGenericRepository<T> where T : BaseEntity
{
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }


        public virtual void Add(T entity)
        {
            _collection.InsertOne(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _collection.InsertMany(entities);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return _collection.Find(filter).ToEnumerable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual void Remove(T entity)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            _collection.DeleteOne(filter);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            var ids = entities.Select(e => e.Id);
            var filter = Builders<T>.Filter.In(e => e.Id, ids);
            _collection.DeleteMany(filter);
        }

        public virtual void Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            _collection.ReplaceOne(filter, entity);
        }

        public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var filter = FilterDefinition<T>.Empty; 
            long totalRegistrosLong = await _collection.CountDocumentsAsync(filter);

            int totalRegistros = (int)totalRegistrosLong;

            var registros = await _collection.Find(filter)
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();
            return (totalRegistros, registros);
        }

}