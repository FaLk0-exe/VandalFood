using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.BLL.Validators
{
    public class OperatorValidator
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
            


        }
    }
}
