using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    public class Electronics : Product
    {
        public int WarrantyMonths { get; set; }
        public string Brand { get; set; }

        public override string GetDetails()
        {
            return $"{Name} - ${Price} | Brand: {Brand} | Warranty: {WarrantyMonths}mo | Stock: {Stock}";
        }
    }
}
