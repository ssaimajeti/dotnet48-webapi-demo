using MicroProjectApplication.Data;
using MicroProjectApplication.Models;
using MicroProjectApplication.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MicroProjectApplication.Repositories
{
    

    public class CustomerRepository : ICustomerRepository
    {
        public List<Customer> GetAll()
        {
            return InMemoryDataStore.Customers;
        }

        public Customer GetById(int id)
        {
            return InMemoryDataStore.Customers.FirstOrDefault(x => x.Id == id);
        }

        public Customer Add(Customer customer)
        {
            customer.Id = InMemoryDataStore.GetNextCustomerId();
            InMemoryDataStore.Customers.Add(customer);
            return customer;
        }

        public Customer Update(int id, Customer updatedCustomer)
        {
            var existing = GetById(id);
            if (existing == null)
                return null;

            existing.Name = updatedCustomer.Name;
            existing.City = updatedCustomer.City;

            return existing;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
                return false;

            InMemoryDataStore.Customers.Remove(existing);
            return true;
        }
    }
}