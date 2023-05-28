using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public class CustomerContactMapper : Mapper<CustomerContact>
    {
        public override List<CustomerContact> Map(SqlCommand command)
        {
            var contacts = new List<CustomerContact>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var contact = new CustomerContact();
                    var fields = resultFields.Where(s => !s.First.Name.Contains("Customer") && s.Second != "ContactType")
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(contact, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    contacts.Add(contact);
                }
            }
            return contacts;
        }
    }
}
