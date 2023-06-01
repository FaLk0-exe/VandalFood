using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VandalFood.BLL.Helpers;
using VandalFood.BLL.Services;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;

namespace CookApplication.Services
{
    public class AuthService
    {
        private Operator _cook;
        private OrderService _orderService { get; set; }
        private OperatorService _operatorService { get; set; }
        public AuthService(OrderService orderService, OperatorService operatorService)
        {
            _orderService = orderService;
            _operatorService = operatorService;

        }

        public bool Authenticate(string login,string password)
        {
            if (_cook is not null)
                return false;
            var @operator = _operatorService.Get().FirstOrDefault(s => s.RoleTypeId == (int)RoleTypeEnum.Cook && s.Login == login
            && s.Password == Md5Helper.GetMd5(password));
            if (@operator is null)
                return false;
            _cook = @operator;
            return true;
        }

        public void LogOut()
        {
            _cook = null;
        }
    }
}
