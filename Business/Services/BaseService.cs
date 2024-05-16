using DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public abstract class BaseService
    {
        protected readonly Db _db; 

        protected BaseService(Db db) 
        {
            _db = db;
        }
    }
}
