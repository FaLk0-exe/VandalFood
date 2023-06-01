using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.BLL.Helpers
{
    public static class ContactTypeHelper
    {
        private static readonly Dictionary<int, string> _statusTypePairs = new Dictionary<int, string>
        {
            { 1,"Телефон"},
            { 2,"Вайбер"},
            { 3,"Адреса"},
            { 4,"Пошта"}
        };

        public static string GetTitle(int id)
        {
            return _statusTypePairs[id];
        }
    }
}
