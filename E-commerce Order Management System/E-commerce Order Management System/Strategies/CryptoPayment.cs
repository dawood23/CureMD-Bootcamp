using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    class CryptoPayment : Ipayment
    {
        private string _dummyWallet = "123abc123";
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
            Console.WriteLine($"Enter Crypto wallet ({_dummyWallet}): ");
            string wallet=Console.ReadLine();

            if (wallet!=_dummyWallet)
            {
                return false;
            }

            return true;
        }
    }
}
