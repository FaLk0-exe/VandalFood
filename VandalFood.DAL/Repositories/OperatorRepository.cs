using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Mappers;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Repositories
{
    public class OperatorRepository : Repository<Operator>
    {
        private const string CREATE_QUERY = "INSERT INTO Operators (Login, Password, LeftName, RightName, RoleTypeId) VALUES (@Login, @Password, @LeftName, @RightName, @RoleTypeId)";
        private const string DELETE_QUERY = "DELETE FROM Operators WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE Operators SET Login = @Login, Password = @Password, LeftName = @LeftName, RightName = @RightName, RoleTypeId = @RoleTypeId WHERE Id = @Id";
        private const string GET_BY_ID_QUERY = "SELECT Id,[Login],[Password],LeftName,RightName,RoleTypeId,RoleType.Title as 'rt.Title' FROM Operators JOIN RoleTypes ON Operators.RoleTypeId = RoleTypes.Id WHERE Id = @Id";
        private const string GET_QUERY = "SELECT Id,[Login],[Password],LeftName,RightName,RoleTypeId,RoleType.Title as 'rt.Title' FROM Operators JOIN RoleTypes ON Operators.RoleTypeId = RoleTypes.Id";
        public OperatorRepository(IConfiguration configuration):base(configuration)
        {
        }


        public override void Create(Operator entity)
        {
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@Login", entity.Login),
            new SqlParameter("@Password", entity.Password),
            new SqlParameter("@LeftName", entity.LeftName),
            new SqlParameter("@RightName", entity.RightName),
            new SqlParameter("@RoleTypeId", entity.RoleTypeId)
            };
            ExecuteCommand(CREATE_QUERY, parameters);
        }

        public override void Delete(Operator entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", entity.Id)
            };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Operator Get(int id)
        {
            Operator @operator;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    @operator = new OperatorMapper().MapSingle(command);
                }
                connection.Close();
            }
            return @operator;
        }

        public override IEnumerable<Operator> Get()
        {
            List<Operator> operators;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    operators = new OperatorMapper().Map(command);
                }
                connection.Close();
            }
            return operators;
        }

        public override void Update(Operator entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Id", entity.Id),
            new SqlParameter("@Login", entity.Login),
            new SqlParameter("@Password", entity.Password),
            new SqlParameter("@LeftName", entity.LeftName),
            new SqlParameter("@RightName", entity.RightName),
            new SqlParameter("@RoleTypeId", entity.RoleTypeId)
            };
            ExecuteCommand(UPDATE_QUERY, parameters);
        }
    }
}
