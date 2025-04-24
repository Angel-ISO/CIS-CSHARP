using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface Ivote : IGenericRepository<Vote>
{
  Task<Vote?> GetByUserAndIdeaAsync(Guid userId, string ideaId);
}
