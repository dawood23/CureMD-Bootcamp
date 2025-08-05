using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    class PaypalPayment : Ipayment
    {
        private string _dummyEmail = "pay@gmail.com";
        private string _dummyPass = "1234";
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
            
            Console.Write($"Enter Email ({_dummyEmail}): ");
            string email=Console.ReadLine();
            Console.Write($"Enter Pass ({_dummyPass}): ");
            string pass=Console.ReadLine(); 

            if(email!=_dummyEmail ||  pass!=_dummyPass)
            {
                return false;
            }
            
            return true;
        }
    }
}
