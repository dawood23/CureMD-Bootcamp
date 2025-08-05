using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Models
{
    public class HomeGarden : Product
    {
        public string PlantType { get; set; }
        public string Season { get; set; }

        public override string GetDetails()
        {
            return $"{Name} - ${Price} | Type: {PlantType} | Season: {Season} | Stock: {Stock}";
        }
    }

}
