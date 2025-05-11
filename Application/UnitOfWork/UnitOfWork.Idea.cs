using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;

namespace Application.UnitOfWork;
public partial class UnitOfWork
{
    private IIdea? _ideas;

    public IIdea Ideas
    {
        get
        {
            return _ideas ??= new IdeaRepository(_context);
        }
    }
}
