using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Services.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Customer Create(Customer customer);
        Customer Update(int id, Customer customer);
        bool Delete(int id);
    }
}
