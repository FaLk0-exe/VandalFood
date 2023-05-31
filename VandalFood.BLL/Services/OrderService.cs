using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Validators;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;

namespace VandalFood.BLL.Services
{
    public class OrderService
    {
        private OrderValidator _orderValidator;
        private CustomerOrderRepository _orderRepository;
        private ProductRepository _productRepository;
        public OrderService(OrderValidator operatorValidator, CustomerOrderRepository operatorRepository,ProductRepository productRepository)
        {
            _orderValidator = operatorValidator;
            _orderRepository = operatorRepository;
            _productRepository = productRepository;
        }
        public void Create(CustomerOrder order)
        {
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _orderRepository.Create(order);
        }
        public void Update(CustomerOrder order)
        {
            if (_orderRepository.Get(order.Id) is null)
                throw new Exception($"CustomerOrder with ID {order.Id} is not found");
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _orderRepository.Update(order);
        }

        public void Delete(int id)
        {
            var order = _orderRepository.Get(id);
            if (order is null)
                throw new Exception($"CustomerOrder with ID {id} is not found");
            _orderRepository.Delete(order);
        }

        public IEnumerable<CustomerOrder> Get()
        {
            return _orderRepository.Get();
        }
        public CustomerOrder Get(int id)
        {
            return _orderRepository.Get(id);
        }

        public OrderItem GetItem(int productId, int customerOrderId)
        {
            return _orderRepository.GetItem(productId, customerOrderId);
        }

        public void UpdateItem(OrderItem item)
        {
            _orderRepository.UpdateItem(item);
        }

        public void DeleteItem(int customerOrderId, int productId)
        {
            _orderRepository.DeleteItem(productId, customerOrderId);
        }

        public void AddItem(int productId,int customerOrderId)
        {
            var product = _productRepository.Get(productId);
            _orderRepository.CreateItem(new OrderItem
            {
                Price = product.Price,
                ProductId = productId,
                CustomerOrderId = customerOrderId,
                Amount = 1,
                Title = product.Title
            });
        }

        public void AddContact(OrderContact orderContact)
        {
            _orderRepository.CreateContact(orderContact);
        }

    }
}
