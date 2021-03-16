using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public static class CookieHelper
    {

        const string CartCookie = "CartCookie";

        /// <summary>
        /// Returns the current list of cart products. if cart is empty 
        /// an empty list will be returned
        /// </summary>
        /// <param name="http"></param>
        /// <returns>An empty list if cart is empty</returns>
        public static List<Product> GetCartProducts(IHttpContextAccessor http)
        {

            // Get existing cart items. https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
            string existingItems = http.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();

            // looking to see if the cart has something attached to it
            if (existingItems != null)
            {
                // will deserialize the object and puts it in a list to have a cart working
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems);
            }

            return cartProducts;
        }

        public static void AddProductToCart(IHttpContextAccessor http, Product p)
        {
            List<Product> cartProducts = GetCartProducts(http);

            cartProducts.Add(p);

            string data = JsonConvert.SerializeObject(cartProducts);

            CookieOptions options = new CookieOptions()
            {
                // cookie will last as long as you set it however users can always delete them.
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            http.HttpContext.Response.Cookies.Append(CartCookie, data, options);
        }

        public static int GetTotalCartProducts(IHttpContextAccessor http)
        {
            List<Product> cartProducts = GetCartProducts(http);
            return cartProducts.Count;
        }
    }
}
