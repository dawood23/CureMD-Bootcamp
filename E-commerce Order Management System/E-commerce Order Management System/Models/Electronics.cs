using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    class Electronics:ProductModel
    {
        public int warranty { get; set; }
        public Electronics():base() {}

        public override string GetProductDetails(int id)
        {
            return $"[Electronics] Product ID: {id}, Name: {name}, Price: ${price}, Category: {Category}, warranty: {warranty}";
        }
    }
}
