using E_commerce_Order_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Factories
{
    public class ProductFactory
    {
                public static ProductModel ChooseProductType(ProductCategory category)
                {
                    switch (category) {

                        case ProductCategory.Electronics:
                            return new Electronics { Category = category };
                        case ProductCategory.Clothing:
                            return new Clothing { Category = category };    
                        case ProductCategory.Books: 
                            return new Books { Category = category };
                        case ProductCategory.HomeGarden:
                            return new HomeGarden { Category = category };
                        default:
                            throw new ArgumentException("The category doesnt exist");

                    }
                }
        }
}
