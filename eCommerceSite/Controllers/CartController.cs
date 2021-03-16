using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Adds a product to the shopping cart
        /// </summary>
        /// <param name="id">the id of the product to add</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id) 
        {
            // Get product from the database
            Product p = await ProductDb.GetProductAsync(_context, id);

            CookieHelper.AddProductToCart(_httpContext, p);

            // Redirect back to previous page
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            /*
            // grabs the cookies data
            string cookieData = _httpContext.HttpContext.Request.Cookies["CartCookie"];

            // deserializes it so that it can be shown 
            List<Product> cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);

            // Display all products in shopping cart cookie
            return View(cartProducts);
            // shorter method below
            */

            // Display all products in shopping cart cookie
            return View(CookieHelper.GetCartProducts(_httpContext));
        }
    }
}
