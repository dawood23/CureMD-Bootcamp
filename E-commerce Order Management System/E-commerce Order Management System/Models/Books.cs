using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace E_commerce_Order_Management_System.Models
{
    class Books:ProductModel
    {
        public string ISBN;
        public Books() : base() { }

        public override string GetProductDetails(int id)
        {
            return $"[Books] Product ID: {id}, Name: {name}, Price: ${price}, Category: {Category}, ISBN: {ISBN}";
        }
    }
}
