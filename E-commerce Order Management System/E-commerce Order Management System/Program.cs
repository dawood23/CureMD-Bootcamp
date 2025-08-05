using E_commerce_Order_Management_System.Factories;
using E_commerce_Order_Management_System.Models;
using E_commerce_Order_Management_System.Repositories;
using E_commerce_Order_Management_System.Singletons;
using E_commerce_Order_Management_System.Strategies;

namespace E_commerce_Order_Management_System
{
    class Program
    {
        private static ProductRepository _productRepo = new ProductRepository();
        private static OrderRepository _orderRepo = new OrderRepository();
        private static Logger _logger = Logger.Instance;
        private static ConfigManager _config = ConfigManager.Instance;

        static void Main(string[] args)
        {
            _logger.Log("E-Commerce System Started");

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ViewProducts(); break;
                        case "2": AddProduct(); break;
                        case "3": CreateOrder(); break;
                        case "4": ProcessPayment(); break;
                        case "5": ViewOrders(); break;
                        case "6": CheckInventory(); break;
                        case "7": _logger.ShowLogs(); break;
                        case "8": _config.ShowSettings(); break;
                        case "9":
                            _logger.Log("System Shutdown");
                            return;
                        default: Console.WriteLine("Invalid choice!"); break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error: {ex.Message}");
                }

            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== E-Commerce Order Management System ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add New Product");
            Console.WriteLine("3. Create Order");
            Console.WriteLine("4. Process Payment");
            Console.WriteLine("5. View Orders");
            Console.WriteLine("6. Check Inventory");
            Console.WriteLine("7. System Logs");
            Console.WriteLine("8. Configuration");
            Console.WriteLine("9. Exit");
            Console.Write("Choose option: ");
        }

        static void ViewProducts()
        {
            Console.WriteLine("\n=== Products ===");
            var products = _productRepo.GetAll();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i].GetDetails()}");
            }
        }

        static void AddProduct()
        {
            Console.WriteLine("\n=== Add Product ===");
            Console.WriteLine("1. Electronics  2. Clothing  3. Books  4. Home & Garden");
            Console.Write("Select category: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
            {
                var category = (ProductCategory)(choice - 1);
                var product = ProductFactory.CreateProduct(category);

                Console.Write("Name: ");
                product.Name = Console.ReadLine();

                Console.Write("Price: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    product.Price = price;

                Console.Write("Stock: ");
                if (int.TryParse(Console.ReadLine(), out int stock))
                    product.Stock = stock;

                switch (product)
                {
                    case Electronics e:
                        Console.Write("Brand: ");
                        e.Brand = Console.ReadLine();
                        Console.Write("Warranty (months): ");
                        if (int.TryParse(Console.ReadLine(), out int warranty))
                            e.WarrantyMonths = warranty;
                        break;
                    case Clothing c:
                        Console.Write("Size: ");
                        c.Size = Console.ReadLine();
                        Console.Write("Color: ");
                        c.Color = Console.ReadLine();
                        break;
                    case Book b:
                        Console.Write("Author: ");
                        b.Author = Console.ReadLine();
                        Console.Write("ISBN: ");
                        b.ISBN = Console.ReadLine();
                        break;
                    case HomeGarden h:
                        Console.Write("Plant Type: ");
                        h.PlantType = Console.ReadLine();
                        Console.Write("Season: ");
                        h.Season = Console.ReadLine();
                        break;
                }

                _productRepo.Add(product);
                _logger.Log($"Product added: {product.Name}");
                Console.WriteLine("Product added successfully!");
            }
        }

        static void CreateOrder()
        {
            Console.WriteLine("\n=== Create Order ===");
            Console.Write("Customer ID: ");
            string customerId = Console.ReadLine();

            var order = new Order { CustomerId = customerId };
            var products = _productRepo.GetAll();

            while (true)
            {
                Console.WriteLine("\nAvailable Products:");
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i].GetDetails()}");
                }

                Console.Write("Select product (0 to finish): ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0) break;
                    if (choice > 0 && choice <= products.Count)
                    {
                        var product = products[choice - 1];
                        Console.Write("Quantity: ");

                        if (int.TryParse(Console.ReadLine(), out int qty) && qty <= product.Stock)
                        {
                            order.Items.Add(new OrderItem
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Quantity = qty,
                                Price = product.Price
                            });

                            product.Stock -= qty;
                            _productRepo.Update(product);
                            Console.WriteLine($"Added {qty} x {product.Name}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity or insufficient stock!");
                        }
                    }
                }
            }

            if (order.Items.Any())
            {
                _orderRepo.Add(order);
                _logger.Log($"Order created: {order.Id} for customer {customerId}");
                Console.WriteLine($"Order created! Total: ${order.Total}");
            }
        }

        static void ProcessPayment()
        {
            Console.WriteLine("\n=== Process Payment ===");
            Console.Write("Payment amount: $");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Payment Methods:");
                Console.WriteLine("1. Credit Card  2. PayPal  3. Bank Transfer");
                Console.Write("Select method: ");

                var processor = new PaymentProcessor();
                string choice = Console.ReadLine();

                IPaymentStrategy strategy = choice switch
                {
                    "1" => new CreditCardPayment(),
                    "2" => new PayPalPayment(),
                    "3" => new BankTransferPayment(),
                    _ => null
                };

                if (strategy != null)
                {
                    processor.SetStrategy(strategy);
                    bool success = processor.ProcessPayment(amount);
                    _logger.Log($"Payment {(success ? "successful" : "failed")}: ${amount}");
                }
            }
        }

        static void ViewOrders()
        {
            Console.WriteLine("\n=== Orders ===");
            var orders = _orderRepo.GetAll();

            if (!orders.Any())
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"\nOrder {order.Id}");
                Console.WriteLine($"Customer: {order.CustomerId}");
                Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Total: ${order.Total}");
                Console.WriteLine("Items:");

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.ProductName} x{item.Quantity} @ ${item.Price}");
                }
                Console.WriteLine(new string('-', 40));
            }
        }

        static void CheckInventory()
        {
            Console.WriteLine("\n=== Inventory ===");
            var products = _productRepo.GetAll();
            var lowStock = _productRepo.GetLowStock(10);

            Console.WriteLine("All Products:");
            foreach (var product in products)
            {
                string status = product.Stock <= 5 ? " ⚠️ LOW" : product.Stock <= 10 ? " ⚡ MEDIUM" : " ✅ GOOD";
                Console.WriteLine($"{product.Name}: {product.Stock} units{status}");
            }

            if (lowStock.Any())
            {
                Console.WriteLine("\nLow Stock Alert:");
                foreach (var product in lowStock)
                {
                    Console.WriteLine($"⚠️ {product.Name}: {product.Stock} units");
                }
            }
        }
    }
}