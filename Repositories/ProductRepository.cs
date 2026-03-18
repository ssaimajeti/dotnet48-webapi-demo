using MicroProjectApplication.Data;
using MicroProjectApplication.Models;
using MicroProjectApplication.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MicroProjectApplication.Repositories
{
    

    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAll()
        {
            return InMemoryDataStore.Products;
        }

        public Product GetById(int id)
        {
            return InMemoryDataStore.Products.FirstOrDefault(x => x.Id == id);
        }

        public Product Add(Product product)
        {
            product.Id = InMemoryDataStore.GetNextProductId();
            InMemoryDataStore.Products.Add(product);
            return product;
        }

        public Product Update(int id, Product updatedProduct)
        {
            var existing = GetById(id);
            if (existing == null)
                return null;

            existing.Name = updatedProduct.Name;
            existing.Price = updatedProduct.Price;

            return existing;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
                return false;

            InMemoryDataStore.Products.Remove(existing);
            return true;
        }
    }
}