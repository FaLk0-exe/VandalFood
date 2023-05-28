using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    internal class OperatorMapper : Mapper<Operator>
    {
        public override List<Operator> Map(SqlCommand command)
        {
            var operators = new List<Operator>();
            var resultFields = GetFields();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var @operator = new Operator();
                    var fields = resultFields.Where(s => !s.First.Name.Contains("RoleType") && !s.First.Name.Contains("CustomerOrders"))
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(@operator, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    @operator.RoleType = new RoleType
                    {
                        Id = (int)reader["RoleTypeId"],
                        Title = (string)reader["rt.Title"],
                    };
                    @operator.RoleTypeId = (int)reader["RoleTypeId"];
                    operators.Add(@operator);

                }
            }
            return operators;
        }

    }
}
