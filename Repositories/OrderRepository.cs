using MicroProjectApplication.Data;
using MicroProjectApplication.Models;
using MicroProjectApplication.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MicroProjectApplication.Repositories
{
    

    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAll()
        {
            return InMemoryDataStore.Orders;
        }

        public Order GetById(int id)
        {
            return InMemoryDataStore.Orders.FirstOrDefault(x => x.Id == id);
        }

        public Order Add(Order order)
        {
            order.Id = InMemoryDataStore.GetNextOrderId();
            InMemoryDataStore.Orders.Add(order);
            return order;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
                return false;

            InMemoryDataStore.Orders.Remove(existing);
            return true;
        }
    }
}