using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataLayer.ViewModels;

namespace MyShop.Core.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(RegisterViewModel register);
        bool IsEmailExist(string email);
        bool IsUserNameExist(string username);
        bool ActiveUser(string activecode);

    }
}
