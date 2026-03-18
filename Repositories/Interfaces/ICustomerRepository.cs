using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Customer Add(Customer customer);
        Customer Update(int id, Customer customer);
        bool Delete(int id);
    }
}
