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
    public class ProductValidator : IValidator<Product>
    {
        public void Validate(Product product)
        {
            if (product.Title.IsNullOrEmpty())
                throw new ArgumentException("Назва не може бути пустою");
            if (product.Weight<0)
                throw new ArgumentException("Вага повинна бути більше 0");
            if (product.Price<0)
                throw new ArgumentException("Ціна повинна бути більше 0");
            var enumValues = Enum.GetValues(typeof(ProductTypeEnum)).Cast<int>().ToList();
            if (!enumValues.Contains(product.ProductTypeId))
                throw new ArgumentException("Некоректний тип продукту");
        }
    }
}
