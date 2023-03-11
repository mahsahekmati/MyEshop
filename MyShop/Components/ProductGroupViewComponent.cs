using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.ViewModels;

namespace MyShop.Components
{
    public class ProductGroupViewComponent:ViewComponent
    {
         MyShopDbContext context;

        public ProductGroupViewComponent(MyShopDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = context.ProductGroups
                .Include(p => p.Products)
                .Select(p => new ShowGroupViewModel()
                {
                    GroupId = p.GroupId,
                    GroupTitle = p.GroupTitle,
                    ProductCount = p.Products.Count
                }).ToList();
            return View("/Views/Components/ShowGroup.cshtml", result);
        }
    }
}
