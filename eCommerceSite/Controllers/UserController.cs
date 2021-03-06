﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
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
                // Check if username/email is in use
                bool isEmailTaken = await (from account in _context.UserAccounts
                                           where account.Email == reg.Email
                                           select account).AnyAsync();

                // if so, add custom error and send back to view
                if(isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That email is already in use");
                }

                bool isUsernameTaken = await (from account in _context.UserAccounts
                                             where account.Username == reg.Username
                                             select account).AnyAsync();

                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "That username is already in use");
                }

                if( isEmailTaken || isUsernameTaken)
                {
                    return View(reg);
                }

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

                LogUserIn(acc.UserId);

                // redirect to home page
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        public IActionResult Login()
        {
            //Check if user already logged in
            if(HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

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
            if (!ModelState.IsValid)
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
            if (account == null)
            {
                // Credentials did not match

                // Custom error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");
                return View(model);
            }

            // Log user, into website // Key V, can be whatever you want as long as it is memoriable.
            LogUserIn(account.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // Removes all current session data
            HttpContext.Session.Clear();

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
