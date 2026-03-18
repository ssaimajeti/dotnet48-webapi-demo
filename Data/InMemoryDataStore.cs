using MicroProjectApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace MicroProjectApplication.Data
{
    public static class InMemoryDataStore
    {
        public static List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Pen", Price = 10 },
            new Product { Id = 2, Name = "Book", Price = 50 },
            new Product { Id = 3, Name = "Bag", Price = 500 }
        };

        public static List<Customer> Customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Mark", City = "Delhi" },
            new Customer { Id = 2, Name = "Ravi", City = "Bangalore" }
        };

        public static List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, ProductId = 2, Quantity = 2 },
            new Order { Id = 2, CustomerId = 2, ProductId = 1, Quantity = 5 }
        };

        public static int GetNextProductId()
        {
            return Products.Count == 0 ? 1 : Products.Max(x => x.Id) + 1;
        }

        public static int GetNextCustomerId()
        {
            return Customers.Count == 0 ? 1 : Customers.Max(x => x.Id) + 1;
        }

        public static int GetNextOrderId()
        {
            return Orders.Count == 0 ? 1 : Orders.Max(x => x.Id) + 1;
        }
    }
}