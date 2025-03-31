using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces;
public interface IUnitOfWork
{

    Iidea Ideas {get;} 
  
    Itopic Topics {get;} 
  
    Ivote Votes {get;}
   Task<int> SaveAsync();  
}
