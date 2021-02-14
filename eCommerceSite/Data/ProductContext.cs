using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// In order to install EF Core for web design Right click the name of project ie. eCommerceSite -> manage nuget packages -> install -> Microsoft.EntityFrameworkCore to be able to get
// EF core to start working so we can start using database and connecting them with our website.

namespace eCommerceSite.Data
{   
    public class ProductContext : DbContext // requires using Microsoft.EntityFrameworkCore
    {
        /// <summary>
        /// A constructor that going to setup our connection string or whatever else we need from it
        /// </summary>
        /// <param name="products"></param>
        public ProductContext(DbContextOptions<ProductContext> items)
            : base(items){}

        public DbSet<Product> Products { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
