using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataLayer.Context;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyShop.DataLayer.Domain;
using ZarinpalSandbox;

namespace MyShop.Controllers
{
    public class ProductController : Controller
    {
        private MyShopDbContext context;

        public ProductController(MyShopDbContext context)
        {
            this.context = context;
        }

        [Route("/Group/{id}/{title}")]
        public IActionResult GetProductByGroupId(int id, string title, int take=2, int pageid = 1)
        {
           

            int skip = (pageid - 1) * take;
            int pagecount = context.Products.Count(p => p.GroupId == id) / take;
            var result = context.Products.Where(p => p.GroupId == id)
                .OrderByDescending(p => p.ProductId)
                .Skip(skip).Take(take);

            ViewBag.pagecount = pagecount;
            ViewBag.pageid = pageid;
            ViewBag.groupid = id;
            ViewBag.grouptitle = title;

            return View(result);
        }

        [Route("Product/{id}")]
        public IActionResult ShowProduct(int id)
        {
            return View(context.Products.Find(id));

        }

        [Authorize]

        public IActionResult AddToCart(int id)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var order = context.Orders.FirstOrDefault(o => o.UserId == userid && !o.IsFinaly);
            if (order == null)
            {
                order = new Order()
                {
                    IsFinaly = false,
                    UserId = userid,
                    CreateDate = DateTime.Now

                };
                context.Orders.Add(order);
                context.SaveChanges();
            }

            var detail = context.OrderDetails
                .FirstOrDefault(d => d.OrderId == order.OrderId
                                     && d.ProductId == id);
            if (detail != null)
            {
                detail.Count += 1;
                context.Update(detail);
            }
            else
            {
                context.OrderDetails.Add(new OrderDetail()
                {
                    Count = 1,
                    OrderId = order.OrderId,
                    Price = context.Products.Find(id).Price,
                    ProductId = id
                });
            }

            context.SaveChanges();
            return Redirect("/Product/ShowOrder/" + order.OrderId);

        }

        [Authorize]
        public IActionResult ShowOrder(int id)
        {
            var order = context.Orders.Where(o => o.OrderId == id)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault();
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userid != order.UserId)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Payment(int id)
        {
            var order = context.Orders.Where(o => o.OrderId == id)
                .Include(o => o.OrderDetails).FirstOrDefault();
            int amount = order.OrderDetails.Sum(d => d.Count * d.Price);




            var payment = new Payment(amount);
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
                "https://localhost:7159/Home/OnlinePayment/" + order.OrderId, "Iman@Madaeny.com", "09197070750");
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }

            return null;
        }

        public IActionResult ShowComment(int id)
        {
            return PartialView(context.ProductComments.Where(c => c.ProductId == id)
                .Include(c => c.User).ToList());
        }

        [Authorize]
        public void AddComment(int id, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                context.ProductComments.Add(new ProductComment()
                {
                    Comment = comment,
                    UserId = userid,
                    ProductId = id


                });
                context.SaveChanges();
            }



        }
    }
}
