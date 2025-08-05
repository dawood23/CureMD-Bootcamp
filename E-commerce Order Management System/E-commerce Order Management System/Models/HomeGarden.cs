using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    class HomeGarden:ProductModel
    {
        public string PlantType;
        public HomeGarden() : base() { }

        public override string GetProductDetails(int id)
        {
            return $"[HomeGarden] Product ID: {id}, Name: {name}, Price: ${price}, Category: {Category}, PlantType:{PlantType}";
        }
    }
}
