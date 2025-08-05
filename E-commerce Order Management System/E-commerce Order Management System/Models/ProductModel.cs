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
    public abstract class ProductModel
    {
            public int id { get; set; }
            public string name { get; set; }
            public float price { get; set; }
            public ProductCategory Category { get; set; }

            public ProductModel() { }
  
            public abstract string GetProductDetails(int id);
    }
}
