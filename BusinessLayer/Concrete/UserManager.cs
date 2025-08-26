using BCrypt.Net;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public void AddUser(User user)
        {
            _userDal.Insert(user);
        }

        public void DeleteUser(User user)
        {
            _userDal.Delete(user);
        }

        public User? GetByID(int id)
        {
            return _userDal.Get(x => x.UserID == id);
        }

        public List<User> GetUsers()
        {
            return _userDal.List();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }
        public bool VerifyUser(string email, string password)
        {
            var user = _userDal.Get(x => x.UserEmail == email);
            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public void UpdateUser(User user)
        {
            _userDal.Update(user);
        }

        public List<User> GetUsersbyRole(string role)
        {
            var users = _userDal.List(u => u.Role == role, u => u.Intersections);

            foreach (var u in users)
                u.Intersections = u.Intersections
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(3)
                    .ToList();

            return users;
        }

        //public User? GetUserWithIntersections(int id)
        //{
        //    return _userDal.Get(x => x.UserID == id, x => x.Intersections, x => x.Reports);
        //}
        public User? GetUserWithIntersections(int id)
        {
            var val = _userDal.Get(
                x => x.UserID == id,
                q => q.Include(x => x.Intersections)
                      .Include(x => x.Reports)
                      .ThenInclude(x => x.intersection)
                      );
            if (val == null) return null;

            val.Intersections = val.Intersections
                .OrderByDescending(i => i.CreatedAt)
                .ToList();
            val.Reports = val.Reports
                .OrderByDescending(r => r.ReportDate)
                .ToList();

            return val;
        }
    }
}
