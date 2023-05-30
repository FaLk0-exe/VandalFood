using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Helpers;
using VandalFood.BLL.Validators;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;

namespace VandalFood.BLL.Services
{
    public class CustomerService
    {
        private CustomerValidator _operatorValidator;
        private CustomerRepository _customerRepository;
        public CustomerService(CustomerValidator operatorValidator, CustomerRepository operatorRepository)
        {
            _operatorValidator = operatorValidator;
            _customerRepository = operatorRepository;
        }
        public void CreateOperator(Customer customer)
        {
            try
            {
                _operatorValidator.Validate(customer);
            }
            catch (ArgumentException)
            {
                throw;
            }
            customer.Password = Md5Helper.GetMd5(customer.Password);
            _customerRepository.Create(customer);
        }
        public void UpdateOperator(Customer customer)
        {
            var existedCustomer = _customerRepository.Get(customer.Id);
            if (existedCustomer is null)
                throw new Exception($"Customer with ID {customer.Id} is not found");
            try
            {
                _operatorValidator.Validate(customer);
            }
            catch (ArgumentException)
            {
                throw;
            }
            if (existedCustomer.Password != customer.Password)
                customer.Password = Md5Helper.GetMd5(customer.Password);
            _customerRepository.Update(customer);
        }

        public void DeleteOperator(int id)
        {
            var customer = _customerRepository.Get(id);
            if (customer is null)
                throw new Exception($"Customer with ID {id} is not found");
            _customerRepository.Delete(customer);
        }

        public IEnumerable<Customer> Get()
        {
            return _customerRepository.Get();
        }
        public Customer Get(int id)
        {
            return _customerRepository.Get(id);
        }
    }
}
