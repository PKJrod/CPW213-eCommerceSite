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
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Map data to user account instance
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username

                };

                // add to database
                _context.UserAccounts.Add(acc);
                await _context.SaveChangesAsync();

                // redirect to home page
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// After info is submitted will do the check to make sure if user entered in correct credentials
        /// SingleOrDefaultAsync - Single(will not have two users with the same email and password), default(will find the account and populate or set it to null)
        /// </summary>
        /// <param name="model">model is from UserAccount -> the class name LoginViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            UserAccount account =
               await (from u in _context.UserAccounts
                where (u.Username == model.UsernameOfEmail
                    || u.Email == model.UsernameOfEmail)
                    && u.Password == model.password
                select u).SingleOrDefaultAsync();
            /* * method syntax below query syntax above. sometimes can only use method syntax to access database
             * UserAccount account = 
             *      await _context.UserAccounts
             *              .Where(userAcc => (userAcc.Username == model.UsernameOrEmail ||
             *                              userAcc.Email == model.UsernameOrEmail) &&
             *                              userAcc.Password == model.Password)
             *              .SingleOrDefaultAsync();
             * 
             * */
            if(account ==  null)
            {
                // Credentials did not match

                // Custom error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");
                return View(model);
            }

            // Log user, into website

            return RedirectToAction("Index", "Home");
        }
    }
}
