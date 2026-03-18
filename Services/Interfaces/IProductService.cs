using MicroProjectApplication.Models;
using System.Collections.Generic;

namespace MicroProjectApplication.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product Create(Product product);
        Product Update(int id, Product product);
        bool Delete(int id);
    }
}
