using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Mappers
{
    public abstract class Mapper<T>
    {
        protected const string MAP_PATTERN = @"<(.*?)>";
        public abstract List<T> Map(SqlCommand command);
        public T MapSingle(SqlCommand command)
        {
            return Map(command).FirstOrDefault();
        }
        protected IEnumerable<(FieldInfo First, string Second)> GetFields()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return fields.Zip(fields.Select(s => Regex.Match(s.Name, MAP_PATTERN).Groups[1].Value).ToList());

        }
    }
}
