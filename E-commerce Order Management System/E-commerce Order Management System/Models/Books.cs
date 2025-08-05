using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace E_commerce_Order_Management_System.Models
{
    public class Book : Product
    {
        public string Author { get; set; }
        public string ISBN { get; set; }

        public override string GetDetails()
        {
            return $"{Name} - ${Price} | Author: {Author} | ISBN: {ISBN} | Stock: {Stock}";
        }
    }

}
