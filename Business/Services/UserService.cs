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
    public interface IUserService
    {

        IQueryable<UserModel> Query();

        List<UserModel> getAll();
        string Add(UserModel model);
        string Update(UserModel model);
        string DeleteUser(int id);

        bool ToggleAdmin(int id);
    }

    public class UserService : BaseService, IUserService
    {

        public UserService(Db db) : base(db)
        {
        }




        public IQueryable<UserModel> Query()
        {
            return _db.Users.Include(e => e.Role)
                .OrderByDescending(e => e.UserName)
                .Select(e => new UserModel()
                {
                    Id = e.Id,
                    Password = e.Password,
                    RoleId = e.RoleId,
                    UserName = e.UserName,
                    RoleOutput = e.Role.Name
        });
        }

        public string Add(UserModel model)
        {
            List<User> existingUsers = _db.Users.ToList();
            if (existingUsers.Any(u => u.UserName.Equals(model.UserName.Trim(), StringComparison.OrdinalIgnoreCase)))
                return "exists";

            User userEntity = new User()
            {
                UserName = model.UserName.Trim(),
                Password = model.Password.Trim(),
                RoleId = model.RoleId ?? 2,
            };
            _db.Users.Add(userEntity);
            _db.SaveChanges();

            return "success";
        }

        public string Update(UserModel model)
        {
            var existingUsers = _db.Users.Where(u => u.Id != model.Id).ToList();
            var existingUserWithNewUsername = _db.Users.FirstOrDefault(u => u.Id != model.Id && u.UserName == model.UserName);
            if (existingUserWithNewUsername != null)
                return "usernameExists";
            var userEntity = _db.Users.SingleOrDefault(u => u.Id == model.Id);
            if (userEntity is null)
                return "notfound";
            userEntity.UserName = model.UserName.Trim();
            userEntity.Password = model.Password.Trim();
            userEntity.RoleId = model.RoleId ?? 0;
            _db.Users.Update(userEntity);
            _db.SaveChanges();

            return "success";
        }


        public string DeleteUser(int id)
        {
            var userEntity = _db.Users.Include(u => u.Products).SingleOrDefault(u => u.Id == id);
            if (userEntity is null)
                return "notfound";
            _db.Products.RemoveRange(userEntity.Products);
            _db.Users.Remove(userEntity);
            _db.SaveChanges();
            return "success";
        }

        public List<UserModel> getAll()
        {
            return _db.Users
                       .Select(user => new UserModel
                       {
                           UserName = user.UserName,
                           Password = user.Password,
                           RoleId = user.RoleId,
                       })
                       .ToList();
        }

        public bool ToggleAdmin(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            user.RoleId = user.RoleId == 1 ? 2 : 1;

            _db.Users.Update(user);
            _db.SaveChanges();
            return true;
        }

    }
}
