using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public class CustomerMapper : Mapper<Customer>
    {
        public override List<Customer> Map(SqlCommand command)
        {
            var customers = new List<Customer>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var customer = new Customer();
                    var fields = resultFields.Where(s =>  !s.First.Name.Contains("CustomerContacts"))
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(customer, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    customers.Add(customer);
                }
            }
            return customers;
        }
    }
}
