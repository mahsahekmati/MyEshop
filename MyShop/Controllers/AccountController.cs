using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyShop.Core.Interfaces;
using MyShop.Core.Securities;
using MyShop.Core.Sender;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.ViewModels;

namespace MyShop.Controllers
{
    public class AccountController : Controller
    {
         IUserService userService;
        MyShopDbContext dbcontext;

        public AccountController(IUserService userService, MyShopDbContext dbcontext)
        {
            this.userService = userService;
            this.dbcontext = dbcontext;
        }

        #region Register

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (userService.IsEmailExist(register.Email))
            {
                ModelState.AddModelError("Email","ایمیل تکراری است");
                return View(register);
            }

            if (userService.IsUserNameExist(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری تکراری است");
                return View(register);
            }

            userService.RegisterUser(register);

            #region Send email to user

            string activeCode = dbcontext.Users.Single(u => u.Email == register.Email.Trim().ToLower())
                .ActiveCode;
            string emailBody = $"<p>{register.UserName} عزیز ، با تشکر از ثبت نام شما</p>" +
                               "<p>جهت فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید</p>" +
                               $"<p><a href='https://localhost:7159/Account/ActiveUser?activeCode={activeCode}'>فعالسازی</a></p>";
            SendEmail.Send(register.Email, "ایمیل فعالسازی", emailBody);

            #endregion
            return View("SuccessRegister",register);
        }


        #endregion

        #region ActiveAccount

        public IActionResult ActiveUser(string activecode)
        {
          bool res=  userService.ActiveUser(activecode);
          return View(res);
        }

        #endregion

        #region Login

        public IActionResult Login(/*string ReturnUrl = ""*/)
        {
            //ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login/*,string ReturnUrl*/)
        {
            ////////////////////////////////////////
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            /////////////////////////////////////////

            string hashpass = PasswordHelper.EncodePasswordMd5(login.Password);
            var user = dbcontext.Users.SingleOrDefault
            (u => u.Email == login.Email.Trim().ToLower() && u.Password == hashpass);

            ////////////////////////////////////////
            if (user==null)
            {
                ModelState.AddModelError("Email","اطلاعات وارد شده صحیح نیست");
                return View(login);
            }
            //////////////////////////////////////////

            if (!user.IsActive)
            {
                ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
                return View(login);
            }
            /////////////////////////////////////////
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("IsAdmin",user.IsAdmin.ToString()),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);

            /////////////////////////////////////////////////
            //if (ReturnUrl!=""&& Url.IsLocalUrl(ReturnUrl))
            //{
            //    return Redirect(ReturnUrl);

            //}


            return Redirect("/");
        }


        #endregion

        #region SignOut

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        #endregion

    }
}
