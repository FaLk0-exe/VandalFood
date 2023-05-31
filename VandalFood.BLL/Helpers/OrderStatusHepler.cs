using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.BLL.Helpers
{
    public static class OrderStatusHelper
    {
        private static readonly Dictionary<int, string> _statusTypePairs = new Dictionary<int, string>
        {
            { 1,"Очікує обробки"},
            { 2,"Підтверджено"},
            { 3,"Очікує приготування"},
            { 4,"Очікує відправки"},
            { 5,"Відправлено"},
            {6,"Готово" },
            {7, "Відхилено" }
        };

        public static string GetTitle(int id)
        {
            return _statusTypePairs[id];
        }
    }
}
