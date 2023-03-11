using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MyShop.DataLayer.Context;
using ZarinpalSandbox;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
         MyShopDbContext context;

         public HomeController(MyShopDbContext context)
         {
             this.context = context;
         }

         public IActionResult Index()
        {
            return View(context.Products.OrderByDescending(p=>p.ProductId));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = context.Orders.Where(o => o.OrderId == id)
                    .Include(o => o.OrderDetails).FirstOrDefault();
                int amount = order.OrderDetails.Sum(d => d.Count * d.Price);

                var payment = new Payment(amount);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    context.Orders.Update(order);
                    context.SaveChanges();
                    ViewBag.code = res.RefId;
                    return View();
                }

            }

            return NotFound();
        }
    }
}