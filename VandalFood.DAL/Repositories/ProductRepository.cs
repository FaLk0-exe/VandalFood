﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.DAL.Interfaces;
using VandalFood.DAL.Models;

namespace VandalFood.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private DatabaseContext _databaseContext;
        public ProductRepository(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public void Create(Product entity)
        {
            var titleParam = new SqlParameter("@Title", entity.Title);
            var isActiveParam = new SqlParameter("@IsActive", entity.IsActive);
            var descriptionParam = new SqlParameter("@Description", entity.Description);
            var weightParam = new SqlParameter("@Weight", entity.Weight);
            var priceParam = new SqlParameter("@Price", entity.Price);
            var productTypeIdParam = new SqlParameter("@ProductTypeId", entity.ProductTypeId);

            _databaseContext.Database.ExecuteSqlRaw("INSERT INTO Products (Title, IsActive, Description, Weight, Price, ProductTypeId) VALUES (@Title, @IsActive, @Description, @Weight, @Price, @ProductTypeId)",
                titleParam, isActiveParam, descriptionParam, weightParam, priceParam, productTypeIdParam);
        }

        public void Delete(Product entity)
        {
            var idParam = new SqlParameter("@Id", entity.Id);
            _databaseContext.Database.ExecuteSqlRaw("DELETE FROM Products WHERE Id = @Id", idParam);
        }

        public Product Get(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            var product = _databaseContext.Products
                .FromSqlRaw("SELECT * FROM Products WHERE Id = @Id", idParam)
                .AsNoTracking()
                .FirstOrDefault();
            if (product != null)
            {
                var productType = _databaseContext.ProductTypes
                    .FromSqlRaw("SELECT * FROM ProductTypes WHERE Id = @Id", new SqlParameter("@Id", product.ProductTypeId))
                    .AsNoTracking()
                    .FirstOrDefault();
                product.ProductType = productType;
            }
            return product;
        }

        public IEnumerable<Product> Get()
        {
            var productDtos = _databaseContext.Products.FromSqlRaw(
                    $"SELECT Products.Id, Products.IsActive, Products.Description, Products.Weight, Products.Price, Products.ProductTypeId, ProductTypes.Title AS ProductTypeTitle FROM Products")
                .Include(s=>s.ProductType).AsEnumerable();

            return null;
        }

        public void Update(Product entity)
        {
            var idParam = new SqlParameter("@Id", entity.Id);
            var titleParam = new SqlParameter("@Title", entity.Title);
            var isActiveParam = new SqlParameter("@IsActive", entity.IsActive);
            var descriptionParam = new SqlParameter("@Description", entity.Description);
            var weightParam = new SqlParameter("@Weight", entity.Weight);
            var priceParam = new SqlParameter("@Price", entity.Price);
            var productTypeIdParam = new SqlParameter("@ProductTypeId", entity.ProductTypeId);

            _databaseContext.Database.ExecuteSqlRaw("UPDATE Products SET Title = @Title, IsActive = @IsActive, Description = @Description, Weight = @Weight, Price = @Price, ProductTypeId = @ProductTypeId WHERE Id = @Id",
                titleParam, isActiveParam, descriptionParam, weightParam, priceParam, productTypeIdParam, idParam);
        }

        class ProductDto
        {
            public int Id { get; set; }

            public bool? IsActive { get; set; }

            public string Description { get; set; }

            public int Weight { get; set; }

            public decimal Price { get; set; }

            public int ProductTypeId { get; set; }

            public string ProductTypeTitle { get; set; }
        }
    }
}
