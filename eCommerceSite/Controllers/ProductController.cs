using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
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
        /// </summary>
        /// <returns></returns> 
        public IActionResult Index()
        {
            //Get all products from database
            // List<Product> products = _context.Products.ToList();
            List<Product> products =
                (from p in _context.Products
                 select p).ToList();

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
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Product p)
        {
            if(ModelState.IsValid)
            {
                // Add to DB
                _context.Products.Add(p);
                _context.SaveChanges();

                // viewdata and tempdata are almost the exact same thing, viewdata will last on the current request, tempdata will last over one redirect
                TempData["Message"] = $"{p.Title} was added successfully";
                
                // redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
