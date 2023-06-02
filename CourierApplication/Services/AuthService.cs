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

namespace CourierApplication.Services
{
    public class AuthService
    {
        private Operator _courier;
        private OrderService _orderService { get; set; }
        private OperatorService _operatorService { get; set; }
        public AuthService(OrderService orderService, OperatorService operatorService)
        {
            _orderService = orderService;
            _operatorService = operatorService;

        }

        public bool Authenticate(string login,string password)
        {
            if (_courier is not null)
                return false;
            var @operator = _operatorService.Get().FirstOrDefault(s => s.RoleTypeId == (int)RoleTypeEnum.Courier && s.Login == login
            && s.Password == Md5Helper.GetMd5(password));
            if (@operator is null)
                return false;
            _courier = @operator;
            return true;
        }

        public void LogOut()
        {
            _courier = null;
        }
    }
}
