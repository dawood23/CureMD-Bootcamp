using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    public class Clothing : Product
    {
        public string Size { get; set; }
        public string Color { get; set; }

        public override string GetDetails()
        {
            return $"{Name} - ${Price} | Size: {Size} | Color: {Color} | Stock: {Stock}";
        }
    }

}
