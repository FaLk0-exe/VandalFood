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
                throw new ArgumentException("Title was null");
            if (product.Weight<0)
                throw new ArgumentException("Weight has incorrect value");
            if (product.Price<0)
                throw new ArgumentException("Price has incorrect value");
            var enumValues = Enum.GetValues(typeof(ProductTypeEnum)).Cast<int>().ToList();
            if (!enumValues.Contains(product.ProductTypeId))
                throw new ArgumentException("ProductTypeId has incorrect value");
        }
    }
}
