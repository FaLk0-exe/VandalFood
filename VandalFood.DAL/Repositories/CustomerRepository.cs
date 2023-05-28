using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Mappers;
using VandalFood.DAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VandalFood.DAL.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        private const string CREATE_CONTACT_QUERY = "INSERT INTO CustomerContacts (CustomerId, ContactTypeId, Value) VALUES (@CustomerId, @ContactTypeId, @Value)";
        private const string CREATE_QUERY = "INSERT INTO Customers (Login, Password, LeftName) OUTPUT INSERTED.ID VALUES (@Login, @Password, @LeftName)";
        private const string DELETE_QUERY = "DELETE FROM Customers WHERE Id = @CustomerId";
        private const string UPDATE_QUERY = "UPDATE Customers SET Login = @Login, Password = @Password, LeftName = @LeftName WHERE Id = @Id";
        private const string GET_CONTACTS_QUERY = "SELECT * FROM CustomerContacts WHERE CustomerId=@Id";
        private const string GET_QUERY = "SELECT Id,[Login],[Password],LeftName FROM Customers";
        private const string GET_BY_ID_QUERY = "SELECT Id,[Login],[Password],LeftName FROM Customers WHERE Id=@Id";
        private const string DELETE_CONTACTS_QUERY = "DELETE FROM CustomerContacts WHERE CustomerId = @Id";

        public CustomerRepository(IConfiguration config) : base(config)
        {
        }
        public CustomerRepository()
        {
        }
        public override void Create(Customer entity)
        {
            var customerParameters = new SqlParameter[] {
                new SqlParameter("@Login", entity.Login),
            new SqlParameter("@Password", entity.Password),
            new SqlParameter("@LeftName", entity.LeftName)
            };
            var customerId = (int)ExecuteScalarCommand(CREATE_QUERY, customerParameters);

            foreach (var contact in entity.CustomerContacts)
            {
                var contactParameters = new SqlParameter[] {
                    new SqlParameter("@ContactTypeId", contact.ContactTypeId),
                    new SqlParameter("@Value", contact.Value),
                    new SqlParameter("@CustomerId", customerId)
                };
                ExecuteCommand(CREATE_CONTACT_QUERY, contactParameters);
            }
        }


        public override void Delete(Customer entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@CustomerId", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Customer Get(int id)
        {
            Customer customer;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                var customerParameter = new SqlParameter("@Id", id);
                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(customerParameter);
                    customer = new CustomerMapper().MapSingle(command);

                }
                if (customer != null)
                {
                    using (var command = new SqlCommand(GET_CONTACTS_QUERY, connection))
                    {
                        command.Parameters.Add(customerParameter);
                        customer.CustomerContacts = new CustomerContactMapper().Map(command);
                    }
                }
            }
            return customer;
        }

        public override IEnumerable<Customer> Get()
        {
            List<Customer> customers;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    customers = new CustomerMapper().Map(command);

                }
                foreach (var customer in customers)
                {
                    using (var command = new SqlCommand(GET_CONTACTS_QUERY, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Id", customer.Id));
                        customer.CustomerContacts = new CustomerContactMapper().Map(command);
                    }
                }
            }
            return customers;
        }

    public override void Update(Customer entity)
    {
        var customerParameters = new SqlParameter[] {
                    new SqlParameter("@Login", entity.Login),
                    new SqlParameter("@Password", entity.Password),
                    new SqlParameter("@LeftName", entity.LeftName),
                    new SqlParameter("@Id", entity.Id)
            };
        ExecuteCommand(UPDATE_QUERY, customerParameters);
        var deleteContactsParameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
        ExecuteCommand(DELETE_CONTACTS_QUERY, deleteContactsParameters);
        foreach (var contact in entity.CustomerContacts)
        {
            var createContactParameters = new SqlParameter[] {
                    new SqlParameter("@CustomerId", entity.Id),
                    new SqlParameter("@ContactTypeId", contact.ContactTypeId),
                    new SqlParameter("@Value", contact.Value)
                   };
            ExecuteCommand(CREATE_CONTACT_QUERY, createContactParameters);
        }
    }
}
}
