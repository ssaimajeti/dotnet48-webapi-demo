using MicroProjectApplication.Models;
using MicroProjectApplication.Repositories.Interfaces;
using MicroProjectApplication.Services.Interfaces;
using System.Collections.Generic;

namespace MicroProjectApplication.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public Order Create(Order order)
        {
            if (order == null)
                return null;

            var customer = _customerRepository.GetById(order.CustomerId);
            if (customer == null)
                return null;

            var product = _productRepository.GetById(order.ProductId);
            if (product == null)
                return null;

            if (order.Quantity <= 0)
                return null;

            return _orderRepository.Add(order);
        }

        public bool Delete(int id)
        {
            return _orderRepository.Delete(id);
        }
    }
}