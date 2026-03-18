using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product Add(Product product);
        Product Update(int id, Product product);
        bool Delete(int id);
    }
}
