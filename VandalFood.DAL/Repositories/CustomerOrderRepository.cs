using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Models;
using VandalFood.DAL.Mappers;

namespace VandalFood.DAL.Repositories
{
    public class CustomerOrderRepository : Repository<CustomerOrder>
    {
        private const string CREATE_QUERY = "INSERT INTO CustomerOrders (OperatorId, OrderStatusId, OrderDate, CustomerName) OUTPUT INSERTED.ID VALUES (@OperatorId, @OrderStatusId, @OrderDate, @CustomerName)";
        private const string CREATE_ITEM_QUERY = "INSERT INTO OrderItems (ProductId, CustomerOrderId, Amount, Price) VALUES (@ProductId, @CustomerOrderId, @Amount, @Price)";
        private const string CREATE_CONTACT_QUERY = "INSERT INTO OrderContacts (CustomerOrderId, ContactTypeId, Value) VALUES (@CustomerOrderId, @ContactTypeId, @Value)";
        private const string UPDATE_QUERY = "UPDATE CustomerOrders SET OperatorId = @OperatorId, OrderStatusId = @OrderStatusId, CustomerName = @CustomerName WHERE Id = @Id";
        private const string UPDATE_CONTACT_QUERY = "UPDATE OrderContacts SET Value = @Value WHERE CustomerOrderId = @CustomerOrderId AND ContactTypeId = @ContactTypeId";
        private const string UPDATE_ITEM_QUERY = "UPDATE OrderItems SET Amount = @Amount, Price = @Price WHERE ProductId = @ProductId AND CustomerOrderId = @CustomerOrderId";
        private const string DELETE_ITEM_QUERY = "DELETE FROM OrderItems WHERE ProductId = @ProductId AND CustomerOrderId = @CustomerOrderId";
        private const string DELETE_CONTACT_QUERY = "DELETE FROM OrderContacts WHERE CustomerOrderId = @CustomerOrderId AND ContactTypeId = @ContactTypeId";
        private const string DELETE_QUERY = "DELETE FROM CustomerOrder WHERE Id = @Id";
        private const string GET_QUERY = "SELECT * FROM CustomerOrders";
        private const string GET_BY_ID_QUERY = "SELECT * FROM CustomerOrders WHERE Id = @Id";
        private const string GET_ITEMS_QUERY = "SELECT ProductId,CustomerOrderId,Amount,Price,Products.[Title] as 'Title' FROM OrderItems INNER JOIN Products ON OrderItems.ProductId = Products.Id WHERE CustomerOrderId = @CustomerOrderId";
        private const string GET_CONTACTS_QUERY = "SELECT * FROM OrderContacts WHERE CustomerOrderId = @CustomerOrderId";
        public override void Create(CustomerOrder entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@OperatorId", entity.OperatorId),
                new SqlParameter("@OrderStatusId", entity.OrderStatusId),
                new SqlParameter("@OrderDate", entity.OrderDate),
                new SqlParameter("@CustomerName", entity.CustomerName)
            };
            var customerOrderId = (int)ExecuteScalarCommand(CREATE_QUERY, parameters);
            foreach (var contact in entity.OrderContacts)
            {
                contact.CustomerOrderId = customerOrderId;
                CreateContact(contact);
            }
            foreach (var item in entity.OrderItems)
            {
                item.CustomerOrderId = customerOrderId;
                CreateItem(item);
            }
        }

        public void CreateItem(OrderItem item)
        {
            var itemParams = new SqlParameter[]
                {
                    new SqlParameter("@ProductId", item.ProductId),
                    new SqlParameter("@CustomerOrderId", item.CustomerOrderId),
                    new SqlParameter("@Amount", item.Amount),
                    new SqlParameter("@Price", item.Price)
                };
            ExecuteCommand(CREATE_ITEM_QUERY, itemParams);
        }

        public void CreateContact(OrderContact contact)
        {
            var contactParams = new SqlParameter[]
                {
                    new SqlParameter("@CustomerOrderId", contact.CustomerOrderId),
                    new SqlParameter("@ContactTypeId", contact.ContactTypeId),
                    new SqlParameter("@Value", contact.Value)
                };
            ExecuteCommand(CREATE_CONTACT_QUERY, contactParams);
        }

        public override void Delete(CustomerOrder entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public void DeleteItem(int productId, int customerOrderId)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@ProductId", productId), new SqlParameter("@CustomerOrderId", customerOrderId) };
            ExecuteCommand(DELETE_ITEM_QUERY, parameters);
        }


        public void DeleteContact(int contactTypeId, int customerOrderId)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@ContactTypeId", contactTypeId), new SqlParameter("@CustomerOrderId", customerOrderId) };
            ExecuteCommand(DELETE_CONTACT_QUERY, parameters);
        }

        public override void Update(CustomerOrder entity)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@OperatorId", entity.OperatorId),
                new SqlParameter("@OrderStatusId", entity.OrderStatusId),
                new SqlParameter("@OrderDate", entity.OrderDate),
                new SqlParameter("@CustomerName", entity.CustomerName),
                new SqlParameter("@Id", entity.Id)
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }

        public void UpdateItem(OrderItem item)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductId", item.ProductId),
                new SqlParameter("@CustomerOrderId", item.CustomerOrderId),
                new SqlParameter("@Amount", item.Amount),
                new SqlParameter("@Price", item.Price)
            };
            ExecuteCommand(UPDATE_ITEM_QUERY, parameters);
        }

        public void UpdateContact(OrderContact orderContact)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ContactTypeId", orderContact.ContactTypeId),
                new SqlParameter("@Value", orderContact.Value),
                new SqlParameter("@CustomerOrderId", orderContact.CustomerOrderId)
            };
            ExecuteCommand(UPDATE_CONTACT_QUERY, parameters);
        }
        public override CustomerOrder Get(int id)
        {
            var orderMapper = new CustomerOrderMapper();
            var contactMapper = new OrderContactMapper();
            var itemMapper = new OrderItemMapper();
            CustomerOrder order;
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    order = orderMapper.MapSingle(command);
                }
                if (order != null)
                {
                    using (SqlCommand command = new SqlCommand(GET_CONTACTS_QUERY, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@CustomerOrderId", order.Id));
                        order.OrderContacts = contactMapper.Map(command);
                    }
                    using (SqlCommand command = new SqlCommand(GET_ITEMS_QUERY, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@CustomerOrderId", order.Id));
                        order.OrderItems = itemMapper.Map(command);
                    }
                }
                connection.Close();
            }
            return order;
        }

    public override IEnumerable<CustomerOrder> Get()
    {
        var orderMapper = new CustomerOrderMapper();
        var contactMapper = new OrderContactMapper();
        var itemMapper = new OrderItemMapper();
        List<CustomerOrder> orders;
        using (SqlConnection connection = new SqlConnection(con))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(GET_QUERY, connection))
            {
                orders = orderMapper.Map(command);
            }
            foreach (var order in orders)
            {
                using (SqlCommand command = new SqlCommand(GET_CONTACTS_QUERY, connection))
                {
                    command.Parameters.Add(new SqlParameter("@CustomerOrderId", order.Id));
                    order.OrderContacts = contactMapper.Map(command);
                }
                using (SqlCommand command = new SqlCommand(GET_ITEMS_QUERY, connection))
                {
                    command.Parameters.Add(new SqlParameter("@CustomerOrderId", order.Id));
                    order.OrderItems = itemMapper.Map(command);
                }

            }
            connection.Close();
        }
        return orders;

    }
}
}
