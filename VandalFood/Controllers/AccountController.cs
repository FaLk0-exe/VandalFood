using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VandalFood.BLL.Helpers;
using VandalFood.BLL.Services;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;
using VandalFood.Models.Authorization;

namespace VandalFood.Controllers
{
    public class AccountController:Controller
    {
        public AccountController()
        {
                
        }

        public ActionResult SignUp(string? message)
        {
            if (message is not null)
                ViewData["message"] = message;
            return View();
        }

        public ActionResult TrySignUp([FromServices] CustomerRepository customerRepository, RegistrationModel model)
        {
            var customers = customerRepository.Get();
            if (customers.Any(s => s.CustomerContacts.Any(c => c.Value == model.PhoneNumber)))
                return RedirectToAction(controllerName: "Account", actionName: "SignUp", routeValues:new { message = "Користувач з таким номером телефону вже існує у системі" });
            if (customers.Any(s => s.Login==model.Login))
                return RedirectToAction(controllerName: "Account", actionName: "SignUp", routeValues: new { message = "Користувач з таким логіном вже існує у системі" });
            var customer = new Customer
            {
                LeftName = model.LeftName,
                Login = model.Login,
                Password = Md5Helper.GetMd5(model.Password),
                CustomerContacts = new List<CustomerContact>
                {
                    new CustomerContact
                    {
                        Value=model.PhoneNumber,
                        CustomerId=0,
                        ContactTypeId=(int)ContactTypeEnum.Phone
                    }
                }
            };
            try
            {
                customerRepository.Create(customer);
                return RedirectToAction(controllerName:"Account",actionName:"TrySignIn", routeValues:(model as AuthorizationModel));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public ActionResult SignIn(string? message)
        {
            if (message is not null)
                ViewData["message"] = message;
            return View(new AuthorizationModel());
        }

        public ActionResult TrySignIn([FromServices] OperatorService operatorService,[FromServices] CustomerService customerService,
            AuthorizationModel model)
        {
            List<Claim> claims;
            var customers = customerService.Get();
            var operators = operatorService.Get();
            var correctCustomer = customers.FirstOrDefault(c => c.Login == model.Login && Md5Helper.GetMd5(model.Password) == c.Password);
            if(correctCustomer != null)
            {
                claims = 
                    new List<Claim>()
                    {
                        new Claim("Login",correctCustomer.Login),
                        new Claim("Name", correctCustomer.LeftName),
                        new Claim("Id", correctCustomer.Id.ToString()),
                        new Claim(ClaimTypes.Role,"Customer")

                    };
                var customerClaimsIdentity = new ClaimsIdentity(claims, "CustomerIdentity");
                var customerClaimsPrincipal = new ClaimsPrincipal(customerClaimsIdentity);
                HttpContext.SignInAsync(customerClaimsPrincipal);
                return RedirectToAction(controllerName: "Product", actionName: "Get");
            }
            var correctOperator = operators
                .FirstOrDefault(c =>c.Login == model.Login && Md5Helper.GetMd5(model.Password) == c.Password 
                                && c.RoleTypeId == (int)RoleTypeEnum.Operator);

            if (correctOperator != null)
            {
                claims =
                    new List<Claim>()
                    {
                        new Claim("Login",correctCustomer.Login),
                        new Claim("LeftName", correctCustomer.LeftName),
                        new Claim("RightName",correctOperator.RightName),
                        new Claim(ClaimTypes.Role,"Operator")
                    };
                var operatorClaimsIdentity = new ClaimsIdentity(claims, "OperatorIdentity");
                var operatorClaimsPrincipal = new ClaimsPrincipal(operatorClaimsIdentity);
                HttpContext.SignInAsync(operatorClaimsPrincipal);
                return RedirectToAction(controllerName:"Product",actionName:"Get");
            }
            return RedirectToAction(controllerName: "Account", actionName: "SignIn", routeValues:new { message = "Неправильний логін або пароль" });
        }

        public ActionResult TrySignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(controllerName: "Product", actionName: "Get");
        }
    }
}
