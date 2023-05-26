using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Interfaces;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Repositories
{
    public class OperatorRepository : IRepository<Operator>
    {
        private DatabaseContext _databaseContext;
        public OperatorRepository(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public void Create(Operator entity)
        {
            var loginParam = new SqlParameter("@Login", entity.Login);
            var passwordParam = new SqlParameter("@Password", entity.Password);
            var leftNameParam = new SqlParameter("@LeftName", entity.LeftName);
            var rightNameParam = new SqlParameter("@RightName", entity.RightName);
            var roleTypeIdParam = new SqlParameter("@RoleTypeId", entity.RoleTypeId);

            _databaseContext.Database.ExecuteSqlRaw(
                "INSERT INTO Operators (Login, Password, LeftName, RightName, RoleTypeId) VALUES (@Login, @Password, @LeftName, @RightName, @RoleTypeId)",
                loginParam, passwordParam, leftNameParam, rightNameParam, roleTypeIdParam);
        }

        public void Delete(Operator entity)
        {
            var idParam = new SqlParameter("@Id", entity.Id);

            _databaseContext.Database.ExecuteSqlRaw("DELETE FROM Operators WHERE Id = @Id", idParam);
        }

        public Operator Get(int id)
        {
            var idParam = new SqlParameter("@Id", id);

            var operatorQuery = _databaseContext.Operators.FromSqlRaw("SELECT * FROM Operators WHERE Id = @Id", idParam).AsNoTracking().FirstOrDefault();

            return operatorQuery;
        }

        public IEnumerable<Operator> Get()
        {
            var operatorQuery = _databaseContext.Operators.FromSqlRaw("SELECT * FROM Operators").AsNoTracking().ToList();

            return operatorQuery.AsEnumerable();
        }

        public void Update(Operator entity)
        {
            var idParam = new SqlParameter("@Id", entity.Id);
            var loginParam = new SqlParameter("@Login", entity.Login);
            var passwordParam = new SqlParameter("@Password", entity.Password);
            var leftNameParam = new SqlParameter("@LeftName", entity.LeftName);
            var rightNameParam = new SqlParameter("@RightName", entity.RightName);
            var roleTypeIdParam = new SqlParameter("@RoleTypeId", entity.RoleTypeId);

            _databaseContext.Database.ExecuteSqlRaw(
                "UPDATE Operators SET Login = @Login, Password = @Password, LeftName = @LeftName, RightName = @RightName, RoleTypeId = @RoleTypeId WHERE Id = @Id",
                loginParam, passwordParam, leftNameParam, rightNameParam, roleTypeIdParam, idParam);
        }
    }
}
