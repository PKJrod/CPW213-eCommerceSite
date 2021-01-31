using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A sellable product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The products unique identification number, EF will automatically make it a primary key if it is a int and has a Id in the name.
        /// can also force to make column as primary key using [Key] has to have using System.ComponentModel.DataAnnotations;
        /// </summary>
        [Key]
        public int ProductId { get; set; }

        /// <summary>
        /// The Consumer facing name of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The retail price as US currency
        /// </summary>
        [DataType(DataType.Currency)]
        public double price { get; set; }

        /// <summary>
        /// Category product falls under. Ex. Electronics, Furniture, ..etc
        /// </summary>
        public string Category { get; set; }
    }
}
