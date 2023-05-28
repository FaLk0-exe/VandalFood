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
                    var fields = resultFields.Where(s => !s.First.Name.Contains("ProductType") && !s.First.Name.Contains("OrderItems"))
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(product, Convert.ChangeType(reader[f.Second],f.First.FieldType));
                    }
                    product.ProductType = new ProductType
                    {
                        Id = (int)reader["ProductTypeId"],
                        Title = (string)reader["pt.Title"],
                    };
                    product.ProductTypeId = (int)reader["ProductTypeId"];
                    products.Add(product);

                }
            }
            return products;
        }  
    }
}
