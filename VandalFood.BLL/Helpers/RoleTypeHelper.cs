using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.BLL.Helpers
{
    public static class RoleTypeHelper
    {
        private static readonly Dictionary<int, string> _statusTypePairs = new Dictionary<int, string>
        {
            { 1,"Оператор"},
            { 2,"Кухар"},
            { 3,"Кур'єр"}
        };

        public static string GetTitle(int id)
        {
            return _statusTypePairs[id];
        }
    }
}
