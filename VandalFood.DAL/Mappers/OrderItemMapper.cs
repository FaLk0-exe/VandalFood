using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public class OrderItemMapper:Mapper<OrderItem>
    {
        public override List<OrderItem> Map(SqlCommand command)
        {
            var orderItems = new List<OrderItem>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderItem = new OrderItem();
                    var fields = resultFields;//.Where(f=>f.Second!="Product");
                    foreach (var f in fields)
                    {
                        f.First.SetValue(orderItem, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    orderItems.Add(orderItem);
                }
            }
            return orderItems;
        }
    }
}
