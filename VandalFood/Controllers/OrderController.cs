using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using VandalFood.BLL.Services;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;
using VandalFood.Models.Cart;
using VandalFood.Models.Order;

namespace VandalFood.Controllers
{
    [Authorize(Roles ="Operator,Admin")]
    public class OrderController : Controller
    {
        [AllowAnonymous]
        public ActionResult Create([FromServices] CustomerService customerRepository)
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

        [AllowAnonymous]
        public ActionResult TryCreate([FromServices] OrderService customerOrderService, [FromServices] CustomerService customerRepository, OrderModel model)
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
                OperatorId = null
            };
            customerOrder.OrderContacts = new List<OrderContact>();
            if (model.Mail != null)
            {
                customerOrder.OrderContacts.Add(new OrderContact { Value = model.Mail, ContactTypeId = (int)ContactTypeEnum.Mail });
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
            customerOrderService.Create(customerOrder);
            HttpContext.Session.Clear();
            return RedirectToAction(controllerName: "Order", actionName: "Complete");
        }

        [AllowAnonymous]
        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult Get([FromServices] OrderService customerOrderService)
        {
            var customerOrders = customerOrderService.Get();
            if(!HttpContext.User.IsInRole("Admin"))
            {
                customerOrders = customerOrders.Where(s => s.OrderStatusId == (int)OrderStatusEnum.AwaitingProcessing);
            }
            return View(customerOrders);
        }

        public ActionResult Details([FromServices] OperatorService operatorService,[FromServices] OrderService customerOrderService, int id)
        {
            var order = customerOrderService.Get(id);
            if(order.OperatorId.HasValue)
            {
                var @operator = operatorService.Get(order.OperatorId.Value);
                ViewData["operator"] = $"{@operator.LeftName} {@operator.RightName}";
            }
            return View(order);
        }

        public ActionResult Edit([FromServices]OperatorService operatorService,
            [FromServices] OrderService customerOrderService,
            [FromServices] ProductService productService, int id)
        {
            var order = customerOrderService.Get(id);
            OrderModel model = new OrderModel
            {
                Id = order.Id,
                Items = order.OrderItems.Select(s =>
                new CartModel
                {
                    Product = productService.Get(s.ProductId),
                    Count = s.Amount,
                }),
                CustomerName = order.CustomerName,
                Address = order.OrderContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Address)?.Value ?? "",
                Mail = order.OrderContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Mail)?.Value ?? "",
                Phone = order.OrderContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Phone)?.Value ?? ""
            };
            ViewBag.Products = new SelectList(productService.Get().Where(s=>model.Items.All(p=>p.Product.Id!=s.Id) && s.IsActive),"Id","Title");
            return View(model);
        }

        public ActionResult TryEdit([FromServices] OrderService customerOrderService, OrderModel model)
        {
            var customerOrder = customerOrderService.Get(model.Id.Value);

            customerOrder.CustomerName = model.CustomerName;
            var address = customerOrder.OrderContacts.First(s => s.ContactTypeId == (int)ContactTypeEnum.Address);
            address.Value = model.Address;
            var mail = customerOrder.OrderContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Mail);
            if(mail is null && model.Mail is not null)
            {
                customerOrderService.AddContact(
                    new OrderContact
                    {
                        ContactTypeId = (int)ContactTypeEnum.Mail,
                        Value = model.Mail,
                        CustomerOrderId = model.Id.Value
                    });
            }
            customerOrderService.Update(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "Details", routeValues: new { id=model.Id});
        }

        public ActionResult Decrease([FromServices] OrderService orderService, int productId, int customerOrderId)
        {
            var orderItem = orderService.GetItem(productId, customerOrderId);
            if (orderItem.Amount != 1)
            {
                orderItem.Amount -= 1;
                orderService.UpdateItem(orderItem);
            }
            return RedirectToAction(controllerName: "Order", actionName: "Edit", routeValues: new { id = customerOrderId });
        }
        public ActionResult Increase([FromServices] OrderService orderService, int productId, int customerOrderId)
        {
            var orderItem = orderService.GetItem(productId, customerOrderId);
            orderItem.Amount += 1;
            orderService.UpdateItem(orderItem);
            return RedirectToAction(controllerName: "Order", actionName: "Edit", routeValues: new { id = customerOrderId });
        }

        public ActionResult DeleteItem([FromServices] OrderService orderService, int productId, int customerOrderId)
        {
            orderService.DeleteItem(customerOrderId, productId);
            return RedirectToAction(controllerName: "Order", actionName: "Edit", routeValues: new { id = customerOrderId });
        }

        public ActionResult AddItem([FromServices] OrderService orderService,int productId,int customerOrderId)
        {
            orderService.AddItem(productId, customerOrderId);
            return RedirectToAction(controllerName: "Order", actionName: "Edit", routeValues: new { id = customerOrderId });
        }

        public ActionResult Confirm([FromServices] OrderService orderService,int id)
        {
            var customerOrder = orderService.Get(id);
            customerOrder.OrderStatusId = (int)OrderStatusEnum.Confirmed;
            orderService.Update(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "Get");
        }

        public ActionResult Decline([FromServices] OrderService orderService, int id)
        {
            var customerOrder = orderService.Get(id);
            customerOrder.OrderStatusId = (int)OrderStatusEnum.Rejected;
            customerOrder.OperatorId = Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value);
            orderService.Update(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "Get");
        }
    }
}
