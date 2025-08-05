
using E_commerce_Order_Management_System.Strategies;

namespace E_commerce_Order_Management_System
{
    class Program
    {
        public static void Main(string[] args) {
            menu();
        }

        public static void menu()
        {
            Console.WriteLine("=== E-Commerce Order Management System ===");
            while(true) {
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add New Product");
                Console.WriteLine("3. Create Order");
                Console.WriteLine("4. Process Payment");
                Console.WriteLine("5. View Orders");
                Console.WriteLine("6. Check Inventory");
                Console.WriteLine("7. System Logs");
                Console.WriteLine("8. Configuration");
                Console.WriteLine("9. Exit");
                Console.Write("Enter Choice: ");
                string choice= Console.ReadLine();
               
                switch (choice)
                {
           
                    case "1":
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.WriteLine();
                        break;
                    case "3":
                        Console.WriteLine();
                        break;
                    case "4":
                        ProcessPayment();
                        Console.WriteLine();
                        break;
                    case "5":
                        Console.WriteLine();
                        break;
                    case "6":
                        Console.WriteLine();
                        break;
                    case "7":
                        Console.WriteLine();
                        break;
                    case "8":
                        Console.WriteLine();
                        break;
                    case "9":
                        return;

                    default: Console.WriteLine("Invalid Choice");
                        break;

                }

            }

        }

        public static void ProcessPayment()
        {
            PaymentContext pay;
            
                Console.WriteLine("Choose Payment Method");
                Console.WriteLine("1. Credit Card Payment");
                Console.WriteLine("2. Paypal Payment");
                Console.WriteLine("3. Crypto Payment");

                Console.Write("Enter choice: ");
                string choice=Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        pay = new PaymentContext(new CreditCardPayment());
                        break;
                    
                    case "2":
                        pay=new PaymentContext(new PaypalPayment());
                        break;
                    case "3":
                        pay = new PaymentContext(new CryptoPayment());
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        return;
                }

            pay?.PayAmount(100);
                
            
        }
    }
}