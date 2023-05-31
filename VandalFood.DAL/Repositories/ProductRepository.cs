using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Mappers;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Repositories
{
    public sealed class ProductRepository : Repository<Product>
    {
        private const string CREATE_QUERY = "INSERT INTO Products (Title, IsActive, Description, Weight, Price, ProductTypeId,PhotoPath) VALUES (@Title, @IsActive, @Description, @Weight, @Price, @ProductTypeId,@PhotoPath)";
        private const string UPDATE_QUERY = "UPDATE Products SET Title = @Title, IsActive = @IsActive, Description = @Description, Weight = @Weight, Price = @Price, ProductTypeId = @ProductTypeId, PhotoPath = @PhotoPath WHERE Id = @Id";
        private const string DELETE_QUERY = "DELETE FROM Products WHERE Id = @Id";
        private const string GET_QUERY = "SELECT Id,IsActive,[Description],[Weight],Price,Title, PhotoPath, ProductTypeId FROM Products";
        private const string GET_BY_ID_QUERY = "SELECT Id,IsActive,[Description],[Weight],Price, Title,PhotoPath, ProductTypeId FROM Products WHERE Id = @Id";
        public ProductRepository(IConfiguration configuration):base(configuration)
        {
        }


        public override void Create(Product entity)
        {

            var parameters = new SqlParameter[]
            {
                        new SqlParameter("@Title", entity.Title),
                        new SqlParameter("@IsActive", entity.IsActive),
                        new SqlParameter("@Description", entity.Description),
                        new SqlParameter("@Weight", entity.Weight),
                        new SqlParameter("@Price", entity.Price),
                        new SqlParameter("@ProductTypeId", entity.ProductTypeId),
                        new SqlParameter("@PhotoPath", entity.PhotoPath)
            };
            ExecuteCommand(CREATE_QUERY, parameters);
        }

        public override void Delete(Product entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Product Get(int id)
        {
            Product product;
            using (var connection = new SqlConnection(_configuration[CONNECTION_KEY]))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    product = new ProductMapper().MapSingle(command);
                }
                connection.Close();
            }
            return product;
        }

        public override IEnumerable<Product> Get()
        {
            List<Product> products;
            using (var connection = new SqlConnection(_configuration[CONNECTION_KEY]))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    products = new ProductMapper().Map(command);
                }
                connection.Close();
            }
            return products;
        }

        public override void Update(Product entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Id", entity.Id),
            new SqlParameter("@Title", entity.Title),
            new SqlParameter("@IsActive", entity.IsActive),
            new SqlParameter("@Description", entity.Description),
            new SqlParameter("@Weight", entity.Weight),
            new SqlParameter("@Price", entity.Price),
            new SqlParameter("@ProductTypeId", entity.ProductTypeId),
            new SqlParameter("@PhotoPath", entity.PhotoPath),
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }
    }
}
