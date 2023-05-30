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
    public class OperatorService
    {
        private OperatorValidator _operatorValidator;
        private OperatorRepository _operatorRepository;
        public OperatorService(OperatorValidator operatorValidator,OperatorRepository operatorRepository)
        {
            _operatorValidator = operatorValidator;
            _operatorRepository = operatorRepository;
        }
        public void CreateOperator(Operator oper)
        {
            try
            {
                _operatorValidator.Validate(oper);
            }
            catch (ArgumentException)
            {
                throw;
            }
            oper.Password = Md5Helper.GetMd5(oper.Password);
            _operatorRepository.Create(oper);
        }
       

        public void UpdateOperator(Operator oper)
        {
            var existedOperator = _operatorRepository.Get(oper.Id);
            if (existedOperator is null)
                throw new Exception($"Operator with ID {oper.Id} is not found");
            try
            {
                _operatorValidator.Validate(oper);
            }
            catch (ArgumentException)
            {
                throw;
            }
            if(existedOperator.Password != oper.Password)
                oper.Password = Md5Helper.GetMd5(oper.Password);
            _operatorRepository.Update(oper);
        }

        public void DeleteOperator(int id)
        {
            var oper = _operatorRepository.Get(id);
            if (oper is null)
                throw new Exception($"Operator with ID {id} is not found");
            _operatorRepository.Delete(oper);
        }

        public IEnumerable<Operator> Get()
        {
            return _operatorRepository.Get();
        }
        public Operator Get(int id)
        { 
            return _operatorRepository.Get(id);
        }
    }
}
