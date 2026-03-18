using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order GetById(int id);
        Order Create(Order order);
        bool Delete(int id);
    }
}
