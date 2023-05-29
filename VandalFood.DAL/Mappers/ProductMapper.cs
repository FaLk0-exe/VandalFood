using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    internal class ProductMapper : Mapper<Product>
    {
        public override List<Product> Map(SqlCommand command)
        {
            var products = new List<Product>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var product = new Product();
                    var fields = resultFields
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(product, Convert.ChangeType(reader[f.Second],f.First.FieldType));
                    }
                    products.Add(product);

                }
            }
            return products;
        }  
    }
}
