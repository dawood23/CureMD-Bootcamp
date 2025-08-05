using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Books,
        HomeGarden
    }

    public abstract class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public int Stock { get; set; }

        public abstract string GetDetails();
    }
}
