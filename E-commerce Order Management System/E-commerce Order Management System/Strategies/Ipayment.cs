using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    public interface IPaymentStrategy
    {
        bool ProcessPayment(decimal amount);
    }

}
