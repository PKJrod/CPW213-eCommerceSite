using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async
        /// </summary>
        /// <param name="id">int? is nullable<int> which we can pass in a id but if it doesn't have an id will result in null</param>
        /// <returns></returns> 
        public async Task<IActionResult> Index(int? id)
        {
             // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            int pageNum = id ?? 1;
            const int PageSize = 3;
            ViewData["CurrentPage"] = pageNum;

            /* 
             * same as above
             *if(id == null)
             *{
             *   pageNum = 1;
             *}
             *else
             *{
             *   pageNum = id.Value;
             *}
             * same as above but a bit shorter
             * https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
             * int pageNum = id.HasValue ? id.Value : 1;
             */

            int numProducts = await (from p in _context.Products
                              select p).CountAsync();
            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);

            ViewData["MaxPage"] = totalPages;

            //Get all products from database
            // List<Product> products = _context.Products.ToList();
            List < Product > products =
                await (from p in _context.Products
                       orderby p.Title ascending
                       select p)
                       .Skip(PageSize * (pageNum - 1)) // Skip() must be before Take()
                       .Take(PageSize)
                       .ToListAsync();

            // Send list of products to view to be displayed
            return View(products);
        }

        /// <summary>
        /// Gets the user input.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Add the user input and then stores it to the database, in order to add razor right click method name "Add" ->
        /// Add View -> Razor View -> Create View.
        /// Asynchronous code is much more efficient than synchronous code because everything is being runned at once, anytime we are
        /// doing database code we will be using Asynchronous code.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if(ModelState.IsValid)
            {
                // Add to DB
                _context.Products.Add(p);
                await _context.SaveChangesAsync();

                // viewdata and tempdata are almost the exact same thing, viewdata will last on the current request, tempdata will last over one redirect
                TempData["Message"] = $"{p.Title} was added successfully";
                
                // redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">want to call id because it matches MatchControllerRoute pattern Ex. Url link will show up as Product/Edit/5 when editing a product.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get product with corresponding id
            Product p =
                await (from prod in _context.Products
                 where prod.ProductId == id
                 select prod).SingleAsync();
            // Alternate query
            //Product p2 =
            //    await _context
            //            .Products
            //            .Where(prod => prod.ProductId == id)
            //            .SingleAsync();
            
            // pass product to view

            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            // if everything is valid
            if(ModelState.IsValid)
            {
                // mark it as updated/modified
                _context.Entry(p).State = EntityState.Modified;
                // then it saves to the database
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Product updated successfully";
            }

            return View(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">parameter to the action has to match to action name to the route asp-route-id="p.product"</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p =
                await (from prod in _context.Products
                 where prod.ProductId == id
                 select prod).SingleAsync();

            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")] // this attribute is nicknaming the deleteConfirmed to Delete 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p =
                await (from prod in _context.Products
                         where prod.ProductId == id
                         select prod).SingleAsync();

            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{p.Title} was deleted";

            return RedirectToAction("Index");
        }
    }
}
