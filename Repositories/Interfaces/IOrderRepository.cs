using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);
        Order Add(Order order);
        bool Delete(int id);
    }
}
