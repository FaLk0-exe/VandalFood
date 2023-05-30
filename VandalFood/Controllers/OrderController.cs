using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;
using VandalFood.Models.Cart;
using VandalFood.Models.Order;

namespace VandalFood.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Create([FromServices] CustomerRepository customerRepository)
        {
            var model = new OrderModel();
            var cartItems = JsonSerializer.Deserialize<List<CartModel>>(HttpContext.Session.GetString("cart"));
            model.Items = cartItems;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var contacts = customerRepository.Get(Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value)).CustomerContacts;
                model.Address = contacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Address)?.Value ?? "";
                model.Mail = contacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Mail)?.Value ?? "";
                model.Phone = contacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Phone)?.Value ?? "";
                model.CustomerName = HttpContext.User.Claims.First(s => s.Type == "Name").Value;
            }
            return View(model);
        }

        public ActionResult TryCreate([FromServices] CustomerOrderRepository customerOrderRepository, [FromServices] CustomerRepository customerRepository,OrderModel model)
        {
            model.Items = JsonSerializer.Deserialize<List<CartModel>>(HttpContext.Session.GetString("cart"));
            var customerOrder = new CustomerOrder
            {
                CustomerName = model.CustomerName,
                OrderDate = DateTime.Now,
                OrderStatusId = (int)OrderStatusEnum.AwaitingProcessing,
                OrderItems = model.Items.Select(s => new OrderItem
                {
                    ProductId = s.Product.Id,
                    Amount = s.Count,
                    Price = s.Count * s.Product.Price,
                    Title = s.Product.Title,
                    

                }).ToList(),
                OperatorId=null
            };
            customerOrder.OrderContacts = new List<OrderContact>();
            if(model.Mail!=null)
            {
                customerOrder.OrderContacts.Add(new OrderContact { Value = model.Mail,ContactTypeId=(int)ContactTypeEnum.Mail });
            }
            customerOrder.OrderContacts.Add(new OrderContact { Value = model.Phone, ContactTypeId = (int)ContactTypeEnum.Phone });
            customerOrder.OrderContacts.Add(new OrderContact { Value = model.Address, ContactTypeId = (int)ContactTypeEnum.Address });
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var customer = customerRepository.Get(Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value));
                if (customer.CustomerContacts
                    .FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Mail) is null && model.Mail is not null)
                    customer.CustomerContacts.Add(new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Mail, Value = model.Mail });
                if (customer.CustomerContacts
                 .FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Phone) is null && model.Phone is not null)
                    customer.CustomerContacts.Add(new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Phone, Value = model.Phone });
                if (customer.CustomerContacts
                 .FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Address) is null && model.Address is not null)
                    customer.CustomerContacts.Add(new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Address, Value = model.Address });
                customerRepository.Update(customer);
            }
            customerOrderRepository.Create(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "Complete");
        }
        public ActionResult Complete()
        {
            return View();
        }

        [Authorize(Roles ="Operator")]
        public ActionResult Get([FromServices] CustomerOrderRepository customerOrderRepository)
        {
            var customerOrders = customerOrderRepository.Get().Where(s=>s.OrderStatusId == (int)OrderStatusEnum.AwaitingProcessing);
            return View(customerOrders);
        }
    }
}
