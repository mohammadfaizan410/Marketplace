using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IRoleService
    {
        public List<RoleModel> GetAllRoles();
        public IQueryable<RoleModel> Query();
       }
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(Db db) : base(db)
        {
        }

        public List<RoleModel> GetAllRoles()
        {
            return Query().ToList();
        }
        public IQueryable<RoleModel> Query()
        {
            return _db.Roles
                .Select(e => new RoleModel()
                {
                    Id = e.Id,
                    UserCountOutput = e.Users.Count,
                    Name = e.Name
                });
        }

      
    }
}
