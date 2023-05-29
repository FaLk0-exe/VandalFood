using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public class CustomerOrderMapper : Mapper<CustomerOrder>
    {
        public override List<CustomerOrder> Map(SqlCommand command)
        {
            var customerOrders = new List<CustomerOrder>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var customerOrder = new CustomerOrder();
                    var fields = resultFields.Where(s => !s.First.Name.Contains("OrderContacts") && !s.First.Name.Contains("OrderItems")).ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(customerOrder, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    customerOrders.Add(customerOrder);
                }
            }
            return customerOrders;
        }
    }
}
