using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;

namespace Application.UnitOfWork;
public partial class UnitOfWork
{
    private ITopic? _topics;

    public ITopic Topics
    {
        get
        {
            return _topics ??= new TopicRepository(_context);
        }
    }
}
