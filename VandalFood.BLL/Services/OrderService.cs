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
        public void CreateOperator(Order order)
        {
            try
            {
                _orderValidator.Validate(oper);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _orderRepository.Create(oper);
        }
        public void UpdateOperator(Operator oper)
        {
            if (_orderRepository.Get(oper.Id) is null)
                throw new Exception($"Operator with ID {oper.Id} is not found");
            try
            {
                _orderValidator.Validate(oper);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _orderRepository.Update(oper);
        }

        public void DeleteOperator(int id)
        {
            var oper = _orderRepository.Get(id);
            if (oper is null)
                throw new Exception($"Operator with ID {id} is not found");
            _orderRepository.Delete(oper);
        }

        public IEnumerable<Operator> Get()
        {
            return _orderRepository.Get();
        }
        public Operator Get(int id)
        {
            return _orderRepository.Get(id);
        }
    }
}
