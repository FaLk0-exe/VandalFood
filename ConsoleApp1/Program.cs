using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using VandalFood.BLL.Helpers;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Mappers;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;

var map = new CustomerOrderMapper();
var res = map.GetFields();
foreach(var f in res)
    Console.WriteLine(f.Second);