using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VandalFood.BLL.Services;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;
using VandalFood.Models.Customer;

namespace VandalFood.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        public ActionResult Details([FromServices] CustomerService customerService)
        {
            var customer = customerService.Get(Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value));
            return View(customer);
        }

        public ActionResult Edit([FromServices] CustomerService customerService)
        {
            var customer = customerService.Get(Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value));
            var model = new CustomerModel();
            model.Address = customer.CustomerContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Address)?.Value ?? "";
            model.Mail = customer.CustomerContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Mail)?.Value ?? "";
            model.Phone = customer.CustomerContacts.FirstOrDefault(s => s.ContactTypeId == (int)ContactTypeEnum.Phone)?.Value ?? "";
            model.CustomerName = customer.LeftName;
            return View(model);
        }

        public ActionResult TryEdit([FromServices] CustomerService customerService,CustomerModel model)
        {
            var customer = customerService.Get(Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value));
            customer.LeftName = model.CustomerName;
            customer.CustomerContacts.Clear();
            customer.CustomerContacts.Add(
                new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Phone, CustomerId = customer.Id, Value = model.Phone });
            if(!model.Address.IsNullOrEmpty())
            {
                customer.CustomerContacts.Add(
               new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Address, CustomerId = customer.Id, Value = model.Address });
            }

            if (!model.Mail.IsNullOrEmpty())
            {
                customer.CustomerContacts.Add(
               new CustomerContact { ContactTypeId = (int)ContactTypeEnum.Mail, CustomerId = customer.Id, Value = model.Mail });
            }
            customerService.Update(customer);
            return RedirectToAction(controllerName: "Customer", actionName: "Details");
        }
    }
}
