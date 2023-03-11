using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataLayer.Domain;

namespace MyShop.DataLayer.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User GetUserById(int userid);
        int AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int userid);
        void DeleteUser(User user);
        bool IsEmailExist(string email);
        bool IsUserNameExist(string username);
        User GetUserByActiveCode(string activecode);
        void Save();


    }
}
