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
        public void addRole(RoleModel role);
        public void removeRole(int id);
        public void UpdateRole(RoleModel role);
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
        public void addRole(RoleModel role)
        {
            var roleEntity = new Role()
            {
                Name = role.Name,
            };
            _db.Roles.Add(roleEntity);
            _db.SaveChanges();
        }

        public void removeRole(int id)
        {
            var roleEntity = _db.Roles.FirstOrDefault(r => r.Id == id);
            _db.Roles.Remove(roleEntity);
            _db.SaveChanges();
        }

        public void UpdateRole(RoleModel role)
        {
            var roleEntity = _db.Roles.FirstOrDefault(r => r.Id == role.Id);
            roleEntity.Name = role.Name;
            _db.Update(roleEntity);
            _db.SaveChanges();

        }
    }
}
