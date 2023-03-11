using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.Domain;
using MyShop.Utilities;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly MyShopDbContext _context;

        public ProductsController(MyShopDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var myShopContext = _context.Products.Include(p => p.ProductGroup);
            return View(await myShopContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductGroup)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.ProductGroups, "GroupId", "GroupTitle");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,
            IFormFile imgup)
        {
            if (ModelState.IsValid)
            {
                string imageName = "NoPhoto.jpg";
                if (imgup != null)
                {
                    imageName = Guid.NewGuid().ToString().Replace("-", "")
                                + Path.GetExtension(imgup.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/ProductImages", imageName);
                    using (var fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        imgup.CopyTo(fileStream);
                    }

                }
                product.ImageName = imageName;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.ProductGroups, "GroupId", "GroupTitle", product.GroupId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.ProductGroups, "GroupId", "GroupTitle", product.GroupId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile imgup)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imgup != null/* && imgup.IsImage()*/)
                    {
                        if (product.ImageName != "NoPhoto.jpg")
                        {
                            string deletepath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/ProductImages", product.ImageName);
                            if (System.IO.File.Exists(deletepath))
                            {
                                System.IO.File.Delete(deletepath);
                            }

                        }
                        product.ImageName = Guid.NewGuid().ToString().Replace("-", "")
                                    + Path.GetExtension(imgup.FileName);
                        string savepath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/ProductImages", product.ImageName);
                        using (var fileStream = new FileStream(savepath, FileMode.Create))
                        {
                            imgup.CopyTo(fileStream);
                        }

                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.ProductGroups, "GroupId", "GroupTitle", product.GroupId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductGroup)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MyShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
