using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface Iidea : IGenericRepository<Idea>
{
    public Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(Guid topicId);
}
