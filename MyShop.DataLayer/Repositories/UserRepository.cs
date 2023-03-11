using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.Domain;
using MyShop.DataLayer.Interfaces;

namespace MyShop.DataLayer.Repositories
{
    public class UserRepository:IUserRepository
    {
         MyShopDbContext _context;

         public UserRepository(MyShopDbContext context)
         {
             _context = context;

         }
        public IEnumerable<User> GetAllUser()
        {
            return _context.Users;
        }

        public User GetUserById(int userid)
        {
            return _context.Users.Find(userid);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            return user.UserId;

        }

        public void EditUser(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteUser(int userid)
        {
            var user = GetUserById(userid);
            DeleteUser(user);
        }

        public void DeleteUser(User user)
        {
            _context.Remove(user);
        }

        public bool IsEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsUserNameExist(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public User GetUserByActiveCode(string activecode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
