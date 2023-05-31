using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Interfaces;
using VandalFood.DAL.Models;

namespace VandalFood.BLL.Validators
{
    public class OrderValidator : IValidator<CustomerOrder>
    {
        public void Validate(CustomerOrder order)
        {
            if (order.CustomerName is null)
                throw new ArgumentException("Customer name was null");
            if (!order.OrderContacts.Any())
                throw new ArgumentException("OrderContacts should has more than 0 records");
            if (!order.OrderItems.Any())
                throw new ArgumentException("OrderContacts should has more than 0 records");
            if (order.OrderContacts.DistinctBy(s => s.ContactTypeId).Count() != order.OrderContacts.Count())
                throw new ArgumentException("Contacts should be a distinct by contact type");
      /*      if (order.OrderContacts.Any(s => s.CustomerOrderId == 0 || s.ContactTypeId == 0 || s.Value.IsNullOrEmpty()))
                throw new ArgumentException("Each contact should has non-empty Value, CustomerOrderId and ContactId");*/
          /*  if(order.OrderItems.Any(s=>s.Amount<=0 || s.Price<0 || s.ProductId == 0 || s.CustomerOrderId ==0))
                throw new ArgumentException("Each item should has non-empty Amount, Price, ProductId and CustomerOrderId");*/
            if (order.OrderItems.DistinctBy(s => s.ProductId).Count() != order.OrderItems.Count())
                throw new ArgumentException("Order items should be a distonct by ProductId");
            var equalItemId = order.OrderItems.First().CustomerOrderId;
            if (order.OrderItems.Any(s => equalItemId != s.CustomerOrderId))
                throw new ArgumentException("All items of order should have the same CustomerOrderValue");
        }
    }
}
