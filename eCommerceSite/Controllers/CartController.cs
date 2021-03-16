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

            const string CartCookie = "CartCookie";

            // Get existing cart items. https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
            string existingItems = _httpContext.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();

            // looking to see if the cart has something attached to it
            if(existingItems != null )
            {   
                // will deserialize the object and puts it in a list to have a cart working
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            // Add current product to existing cart
            cartProducts.Add(p);
            
            // Add product list to cart cookie
            string data = JsonConvert.SerializeObject(cartProducts);

            CookieOptions options = new CookieOptions()
            {
                // cookie will last as long as you set it however users can always delete them.
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            _httpContext.HttpContext.Response.Cookies.Append(CartCookie, data, options);

            // Redirect back to previous page
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            // grabs the cookies data
            string cookieData = _httpContext.HttpContext.Request.Cookies["CartCookie"];

            // deserializes it so that it can be shown 
            List<Product> cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);

            // Display all products in shopping cart cookie
            return View(cartProducts);
        }
    }
}
