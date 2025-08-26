using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        List<User> GetUsers();
        List<User> GetUsersbyRole(string role);
        void AddUser(User user);
        User? GetByID(int id);
        User? GetUserWithIntersections(int id);
        void DeleteUser(User user);
        void UpdateUser(User user);
        string HashPassword(string password);
        bool VerifyUser(string email, string password);
    }
}
