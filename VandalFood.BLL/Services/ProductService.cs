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
    public class ProductService
    {
        private ProductValidator _productValidator;
        private ProductRepository _productRepository;
        public ProductService(ProductValidator productValidator, ProductRepository operatorRepository)
        {
            _productValidator = productValidator;
            _productRepository = operatorRepository;
        }
        public void CreateOperator(Product product)
        {
            try
            {
                _productValidator.Validate(product);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _productRepository.Create(product);
        }
        public void UpdateOperator(Product product)
        {
            if (_productRepository.Get(product.Id) is null)
                throw new Exception($"Product with ID {product.Id} is not found");
            try
            {
                _productValidator.Validate(product);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _productRepository.Update(product);
        }

        public void DeleteOperator(int id)
        {
            var product = _productRepository.Get(id);
            if (product is null)
                throw new Exception($"Product with ID {id} is not found");
            _productRepository.Delete(product);
        }

        public IEnumerable<Product> Get()
        {
            return _productRepository.Get();
        }
        public Product Get(int id)
        {
            return _productRepository.Get(id);
        }
    }
}
