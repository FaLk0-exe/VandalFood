using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public class OrderContactMapper:Mapper<OrderContact>
    {
        public override List<OrderContact> Map(SqlCommand command)
        {
            var orderContacts = new List<OrderContact>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderContact = new OrderContact();
                    var fields = resultFields.Where(s => !s.First.Name.Contains("OrderContacts") && !s.First.Name.Contains("OrderItems")).ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(orderContact, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    orderContacts.Add(orderContact);
                }
            }
            return orderContacts;
        }
    }
}
