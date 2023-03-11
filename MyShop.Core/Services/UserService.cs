using MyShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataLayer.Domain;
using MyShop.DataLayer.Interfaces;
using MyShop.DataLayer.ViewModels;
using MyShop.Core.Securities;

namespace MyShop.Core.Services
{
    public class UserService: IUserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public bool RegisterUser(RegisterViewModel register)
        {
            User user = new User();
            user.UserName = register.UserName;
            user.Email = register.Email.Trim().ToLower();
            user.RegisterDate=DateTime.Now;
            user.Password = PasswordHelper.EncodePasswordMd5(register.Password);
            user.IsAdmin = false;
            user.IsActive = false;
            user.ActiveCode = Guid.NewGuid().ToString();
            userRepository.AddUser(user);
            userRepository.Save();
            return true;
        }

        public bool IsEmailExist(string email)
        {
            string _email = email.ToLower().Trim();
            return userRepository.IsEmailExist(_email);
        }

        public bool IsUserNameExist(string username)
        {
            return userRepository.IsUserNameExist(username);
        }

        public bool ActiveUser(string activecode)
        {
            var user = userRepository.GetUserByActiveCode(activecode);
            if (user==null)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            userRepository.EditUser(user);
            userRepository.Save();
            return true;
        }
    }
}
