using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Core.Securities;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.ViewModels;

namespace MyShop.Areas.UserPanel.Controllers
{
    [Authorize]
    [Area("UserPanel")]
    public class HomeController : Controller
    {
        MyShopDbContext dbContext;

        public HomeController(MyShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string hasholdpass = PasswordHelper.EncodePasswordMd5(change.OldPassword);
                var user = dbContext.Users.SingleOrDefault(u => u.UserId == userid && u.Password == hasholdpass);
                if (user==null)
                {
                    ModelState.AddModelError("OldPassword","کلمه عبور فعلی صحیح نیست");
                    return View();
                }

                user.Password = PasswordHelper.EncodePasswordMd5(change.Password);
                dbContext.Update(user);
                dbContext.SaveChanges();
                return Redirect("/Account/LogOut");

            }
            return View();
        }
    }
}
