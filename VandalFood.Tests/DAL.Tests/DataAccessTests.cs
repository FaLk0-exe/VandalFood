using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;

namespace VandalFood.Tests.DAL.Tests
{
    public class DataAccessTests
    {
    /*    private ProductRepository _productRepository;
        private OperatorRepository _operatorRepository;
        private CustomerRepository _customerRepository;
        private DatabaseContext _dbContext;
        [SetUp]
        public void Setup()
        {
            _dbContext = new atabaseContext();
            _productRepository = new ProductRepository(_dbContext);
            _operatorRepository = new OperatorRepository(_dbContext);
            _customerRepository = new CustomerRepository(_dbContext);
        }

        #region Products
        [Test]
        public void GetDoesntCrash()
        {
           
                _productRepository.Get();
                Assert.Pass();
            
        }
        [Test]
        public void GetByIdDoesntCrash()
        {
            
                var result = _productRepository.Get(-1);
                Assert.Pass();
           
        }
        *//*[Test]
        public void DeleteDoesntCrash()
        {
            try
            {
                _productRepository.Delete();
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }*//*
        #endregion
        #region Customers
        [Test]
        public void CustomerGetDoesntCrash()
        {

            _customerRepository.Get();
            Assert.Pass();

        }
        [Test]
        public void CustomerGetByIdDoesntCrash()
        {

            var result = _customerRepository.Get(-1);
            Assert.Pass();

        }
        #endregion*/
    }
}