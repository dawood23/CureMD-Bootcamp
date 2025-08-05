using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    public class PaymentProcessor
    {
        private IPaymentStrategy _strategy;

        public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;
        public bool ProcessPayment(decimal amount) => _strategy?.ProcessPayment(amount) ?? false;
    }
}
