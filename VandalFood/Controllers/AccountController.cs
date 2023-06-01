using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View(new Operator());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TryCreate([FromServices] OperatorService operatorService, Operator @operator)
        {
            operatorService.CreateOperator(@operator);
            return RedirectToAction(actionName: "Get",controllerName:"Account");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Get([FromServices] OperatorService operatorService)
        {
            return View(operatorService.Get());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit([FromServices] OperatorService operatorService,int id)
        {
            var @operator = operatorService.Get(id);
            if (@operator is null)
                return NotFound();

            return View(@operator);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TryEdit([FromServices] OperatorService operatorService, Operator @operator)
        {
            var existedOperator = operatorService.Get(@operator.Id);
            if (@operator.Password.IsNullOrEmpty())
                @operator.Password = existedOperator.Password;
            operatorService.UpdateOperator(@operator);
            return RedirectToAction(actionName: "Get", controllerName: "Account");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TryDelete([FromServices] OperatorService operatorService,int id)
        {
            var @operator = operatorService.Get(id);
            if (@operator is null)
                return NotFound();
            operatorService.DeleteOperator(id);
            return RedirectToAction(actionName: "Get", controllerName: "Account");
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
                HttpContext.SignOutAsync();
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
                HttpContext.SignOutAsync();
                claims =
                    new List<Claim>()
                    {
                        new Claim("Login",correctOperator.Login),
                        new Claim("LeftName", correctOperator.LeftName),
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

        public ActionResult Admin()
        {
            var claims =
                    new List<Claim>()
                    {
                        new Claim("Admin","Admin"),
                        new Claim(ClaimTypes.Role,"Admin")
                    };
            var adminClaimsIdentity = new ClaimsIdentity(claims, "AdminIdentity");
            var adminClaimsPrincipal = new ClaimsPrincipal(adminClaimsIdentity);
            HttpContext.SignInAsync(adminClaimsPrincipal);
            return RedirectToAction(controllerName: "Product", actionName: "Get");
        }

        public ActionResult TrySignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(controllerName: "Product", actionName: "Get");
        }
    }
}
