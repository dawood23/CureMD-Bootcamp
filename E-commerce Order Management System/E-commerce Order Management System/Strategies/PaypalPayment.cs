using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    public class PayPalPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.Write("Enter PayPal Email: ");
            string email = Console.ReadLine();

            if (!string.IsNullOrEmpty(email) && email.Contains("@"))
            {
                Console.WriteLine($"PayPal Payment of ${amount} processed!");
                return true;
            }

            Console.WriteLine("SInvalid email!");
            return false;
        }
    }
}