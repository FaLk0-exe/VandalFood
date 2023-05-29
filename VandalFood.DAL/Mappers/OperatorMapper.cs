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
                    var fields = resultFields
                        .ToList();
                    foreach (var f in fields)
                    {
                        f.First.SetValue(@operator, Convert.ChangeType(reader[f.Second], f.First.FieldType));
                    }
                    operators.Add(@operator);

                }
            }
            return operators;
        }


    }
}
