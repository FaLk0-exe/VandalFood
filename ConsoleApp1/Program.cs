using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;

/*
var customerRepository = new CustomerRepository();
var customers = customerRepository.Get();
foreach(var customer in customers)
{
    Console.WriteLine($"{customer.Login} {customer.Password} {customer.LeftName}");
    foreach(var contact in customer.CustomerContacts)
    {
        Console.WriteLine($"{contact.ContactTypeId} {contact.Value}");
    }
}
*/
