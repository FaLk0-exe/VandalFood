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
    public class CustomerRepository : IRepository<Customer>
    {
        private DatabaseContext _databaseContext;
        public CustomerRepository(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public void Create(Customer entity)
        {
            using var transaction = _databaseContext.Database.BeginTransaction();
            try
            {
                var loginParam = new SqlParameter("@Login", entity.Login);
                var passwordParam = new SqlParameter("@Password", entity.Password);
                var leftNameParam = new SqlParameter("@LeftName", entity.LeftName);

                _databaseContext.Database.ExecuteSqlRaw(
                    "INSERT INTO Customers (Login, Password, LeftName) OUTPUT INSERTED.ID VALUES (@Login, @Password, @LeftName)",
                    loginParam, passwordParam, leftNameParam);

                var customerId = entity.Id;
                foreach (var contact in entity.CustomerContacts)
                {
                    var contactTypeIdParam = new SqlParameter("@ContactTypeId", contact.ContactTypeId);
                    var valueParam = new SqlParameter("@Value", contact.Value);
                    var customerIdParam = new SqlParameter("@CustomerId", customerId);

                    _databaseContext.Database.ExecuteSqlRaw(
                        "INSERT INTO CustomerContacts (CustomerId, ContactTypeId, Value) VALUES (@CustomerId, @ContactTypeId, @Value)",
                        customerIdParam, contactTypeIdParam, valueParam);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        public void Delete(Customer entity)
        {
            var customerIdParam = new SqlParameter("@CustomerId", entity.Id);
            _databaseContext.Database.ExecuteSqlRaw("DELETE FROM Customers WHERE Id = @CustomerId", customerIdParam);
        }

        public Customer Get(int id)
        {
            var rawData = _databaseContext.Database.SqlQueryRaw<CustomerContactData>(
                @$"SELECT c.Id, c.Login, c.Password, c.LeftName, cc.ContactTypeId, cc.Value 
          FROM Customers c 
          LEFT JOIN CustomerContacts cc ON c.Id = cc.CustomerId 
          WHERE c.Id = @Id", new SqlParameter("@Id", id)).ToList();

            var customerData = rawData
                .GroupBy(d => new { d.Id, d.Login, d.Password, d.LeftName })
                .Select(g => new Customer
                {
                    Id = g.Key.Id,
                    Login = g.Key.Login,
                    Password = g.Key.Password,
                    LeftName = g.Key.LeftName,
                    CustomerContacts = g.Where(x => x.ContactTypeId != null)
                        .Select(x => new CustomerContact
                        {
                            CustomerId = x.Id,
                            ContactTypeId = x.ContactTypeId.Value,
                            Value = x.Value
                        }).ToList()
                }).FirstOrDefault();

            return customerData;
        }

        public IEnumerable<Customer> Get()
        {
            var rawData = _databaseContext.Database.SqlQuery<CustomerContactData>(
                @$"SELECT c.Id, c.Login, c.Password, c.LeftName, cc.ContactTypeId, cc.Value 
          FROM Customers c 
          LEFT JOIN CustomerContacts cc ON c.Id = cc.CustomerId").ToList();

            var groupedData = rawData
                .GroupBy(d => new { d.Id, d.Login, d.Password, d.LeftName })
                .Select(g => new Customer
                {
                    Id = g.Key.Id,
                    Login = g.Key.Login,
                    Password = g.Key.Password,
                    LeftName = g.Key.LeftName,
                    CustomerContacts = g.Where(x => x.ContactTypeId != null)
                        .Select(x => new CustomerContact
                        {
                            CustomerId = x.Id,
                            ContactTypeId = x.ContactTypeId.Value,
                            Value = x.Value
                        }).ToList()
                }).ToList();

            return groupedData;
        }

        public void Update(Customer entity)
        {
            using var transaction = _databaseContext.Database.BeginTransaction();
            try
            {
                _databaseContext.Database.ExecuteSqlRaw(
                    "UPDATE Customers SET Login = @Login, Password = @Password, LeftName = @LeftName WHERE Id = @Id",
                    new SqlParameter("@Login", entity.Login),
                    new SqlParameter("@Password", entity.Password),
                    new SqlParameter("@LeftName", entity.LeftName),
                    new SqlParameter("@Id", entity.Id)
                );
                _databaseContext.Database.ExecuteSqlRaw(
                    "DELETE FROM CustomerContacts WHERE CustomerId = @Id",
                    new SqlParameter("@Id", entity.Id)
                );

                foreach (var contact in entity.CustomerContacts)
                {
                    _databaseContext.Database.ExecuteSqlRaw(
                        "INSERT INTO CustomerContacts (CustomerId, ContactTypeId, Value) VALUES (@CustomerId, @ContactTypeId, @Value)",
                        new SqlParameter("@CustomerId", entity.Id),
                        new SqlParameter("@ContactTypeId", contact.ContactTypeId),
                        new SqlParameter("@Value", contact.Value)
                    );
                }
                transaction.Commit();
            }
            catch 
            {
                transaction.Rollback();
            }
        }

        class CustomerContactData
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string LeftName { get; set; }
            public int? ContactTypeId { get; set; }
            public string Value { get; set; }
        }
    }
}
