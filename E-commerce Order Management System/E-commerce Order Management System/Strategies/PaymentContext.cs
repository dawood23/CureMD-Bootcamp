using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Strategies
{
    class PaymentContext
    {
        private Ipayment _Payment;

        public PaymentContext(Ipayment payment)
        {
            _Payment = payment;
        }

        public void PayAmount(float amount)
        {
            if (_Payment == null)
            {
                throw new InvalidOperationException("No Payment Method Assigned");
            }
            _Payment.Pay(amount);
        }

    }
}
