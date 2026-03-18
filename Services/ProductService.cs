using MicroProjectApplication.Models;
using MicroProjectApplication.Repositories.Interfaces;
using MicroProjectApplication.Services.Interfaces;
using System.Collections.Generic;

namespace MicroProjectApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product Create(Product product)
        {
            if (product == null)
                return null;

            return _productRepository.Add(product);
        }

        public Product Update(int id, Product product)
        {
            if (product == null)
                return null;

            return _productRepository.Update(id, product);
        }

        public bool Delete(int id)
        {
            return _productRepository.Delete(id);
        }
    }
}