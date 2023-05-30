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
        public OrderService(OrderValidator operatorValidator, CustomerOrderRepository operatorRepository)
        {
            _orderValidator = operatorValidator;
            _orderRepository = operatorRepository;
        }
        public void CreateOperator(CustomerOrder order)
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
        public void UpdateOperator(CustomerOrder order)
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

        public void DeleteOperator(int id)
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
    }
}
