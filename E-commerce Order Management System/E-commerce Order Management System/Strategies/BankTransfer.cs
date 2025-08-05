using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    public class BankTransferPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.Write("Enter Account Number: ");
            string account = Console.ReadLine();

            if (!string.IsNullOrEmpty(account) && account.Length >= 8)
            {
                Console.WriteLine($"Bank Transfer of ${amount} processed!");
                return true;
            }

            Console.WriteLine("Invalid account number!");
            return false;
        }
    }

}
