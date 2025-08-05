using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    class CreditCardPayment:Ipayment
    {
        public void Pay(float amount)
        {
            if (validate())
            {
                Console.WriteLine($"{amount} Paid");
            }
            else
            {
                Console.WriteLine("Couldnt Process Payment!");
                Console.WriteLine("Check your credentials and retry");
            }
        }
        public bool validate()
        {
            Console.Write("Enter Card Number: ");
            string card=Console.ReadLine();
            if(card==null || card.Length < 5)
            {
                return false;
            }
            return true;
        }
    }
}
