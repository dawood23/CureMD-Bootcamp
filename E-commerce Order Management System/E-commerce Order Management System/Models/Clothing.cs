using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    class Clothing : ProductModel
    {
        public string color;
        public Clothing() : base() { }
        public override string GetProductDetails(int id)
        {
            return $"[Clothing] Product ID: {id}, Name: {name}, Price: ${price}, Category: {Category}, Color: {color}";
        }
    }
}
