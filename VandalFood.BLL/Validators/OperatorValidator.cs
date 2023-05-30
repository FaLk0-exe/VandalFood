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
    public class OperatorValidator : IValidator<Operator>
    {
        public void Validate(Operator oper)
        {
            if (oper.LeftName.IsNullOrEmpty())
                throw new ArgumentException("LeftName was null");
            if (oper.LeftName.IsNullOrEmpty())
                throw new ArgumentException("RightName was null");
            if (oper.Login.IsNullOrEmpty())
                throw new ArgumentException("Login was null");
            if (oper.Password.IsNullOrEmpty())
                throw new ArgumentException("Password was null");
            var enumValues = Enum.GetValues(typeof(RoleTypeEnum)).Cast<int>().ToList();
            if (!enumValues.Contains(oper.RoleTypeId))
                throw new ArgumentException("RoleTypeId has incorrect value");
        }
    }
}
