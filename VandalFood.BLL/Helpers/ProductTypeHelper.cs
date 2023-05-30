using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.BLL.Helpers
{
    public static class ProductTypeHelper
    {
        private static readonly Dictionary<int, string> _productTypePairs = new Dictionary<int, string>
        {
            { 1,"Напитки"},
            { 2,"Бургеры"},
            { 3,"Шаурма"},
            { 4,"Хотдоги"},
            { 5,"Пицца"}
        };

        public static string GetTitle(int id)
        {
            return _productTypePairs[id];
        }
    }
}
