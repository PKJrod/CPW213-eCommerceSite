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
        /// <returns></returns> 
        public async Task<IActionResult> Index()
        {
            //Get all products from database
            // List<Product> products = _context.Products.ToList();
            List<Product> products =
                await (from p in _context.Products
                 select p).ToListAsync();

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
    }
}
