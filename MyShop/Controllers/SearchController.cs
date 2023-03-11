using Microsoft.AspNetCore.Mvc;
using MyShop.DataLayer.Context;

namespace MyShop.Controllers
{
    
    
    public class SearchController : Controller
    {
        MyShopDbContext context;

        public SearchController(MyShopDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(string q)
        {
            var result = context.Products.Where(p => p.Title.Contains(q)
                                                     || p.Tags.Contains(q)).ToList();
            return View(result);
        }
    }
}
