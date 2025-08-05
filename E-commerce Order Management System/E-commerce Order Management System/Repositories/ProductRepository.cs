using E_commerce_Order_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private List<Product> _products = new List<Product>();

        public ProductRepository()
        {
            var phone = new Electronics
            {
                Name = "iPhone 15",
                Price = 999,
                Category = ProductCategory.Electronics,
                Brand = "Apple",
                WarrantyMonths = 12,
                Stock = 10
            };

            var jeans = new Clothing
            {
                Name = "Blue Jeans",
                Price = 79,
                Category = ProductCategory.Clothing,
                Size = "M",
                Color = "Blue",
                Stock = 25
            };

            var book = new Book
            {
                Name = "Clean Code",
                Price = 45,
                Category = ProductCategory.Books,
                Author = "Robert Martin",
                ISBN = "978-0132350884",
                Stock = 15
            };

            _products.AddRange(new Product[] { phone, jeans, book });
        }

        public void Add(Product item) => _products.Add(item);
        public Product GetById(string id) => _products.FirstOrDefault(p => p.Id == id);
        public List<Product> GetAll() => _products;
        public void Update(Product item)
        {
            var existing = GetById(item.Id);
            if (existing != null)
            {
                var index = _products.IndexOf(existing);
                _products[index] = item;
            }
        }
        public void Delete(string id) => _products.RemoveAll(p => p.Id == id);

        public List<Product> GetLowStock(int threshold) => _products.Where(p => p.Stock <= threshold).ToList();
    }

    public class OrderRepository : IRepository<Order>
    {
        private List<Order> _orders = new List<Order>();

        public void Add(Order item) => _orders.Add(item);
        public Order GetById(string id) => _orders.FirstOrDefault(o => o.Id == id);
        public List<Order> GetAll() => _orders;
        public void Update(Order item){}
        public void Delete(string id) { }

        public List<Order> GetByCustomer(string customerId) => _orders.Where(o => o.CustomerId == customerId).ToList();
    }
}
