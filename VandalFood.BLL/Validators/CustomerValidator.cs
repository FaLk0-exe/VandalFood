using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Interfaces;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;

namespace VandalFood.BLL.Validators
{
    public class CustomerValidator:IValidator<Customer>
    {
        public void Validate(Customer customer)
        {
            if (customer.LeftName.IsNullOrEmpty())
                throw new ArgumentException("RightName was null");
            if (customer.Login.IsNullOrEmpty())
                throw new ArgumentException("Login was null");
            if (customer.Password.IsNullOrEmpty())
                throw new ArgumentException("Password was null");
            if (customer.CustomerContacts.IsNullOrEmpty())
                throw new ArgumentException("Contacts shouldn't be a empty");
            if (customer.CustomerContacts.DistinctBy(s => s.ContactTypeId).Count() != customer.CustomerContacts.Count())
                throw new ArgumentException("Contacts should be a distinct by contact type");
            if (customer.CustomerContacts.Any(s => s.CustomerId == 0 || s.ContactTypeId == 0 || s.Value.IsNullOrEmpty()))
                throw new ArgumentException("Each contact should has non-empty Value, CustomerId and ContactId");
        }
    }
}
