using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;

namespace Application.UnitOfWork;
public partial class UnitOfWork
{
    private IVote? _votes;

    public IVote Votes
    {
        get
        {
           return _votes ??= new VoteRepository(_context);
        }
    }
}
