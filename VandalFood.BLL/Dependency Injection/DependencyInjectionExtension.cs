using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Services;
using VandalFood.BLL.Validators;
using VandalFood.DAL.Repositories;

namespace VandalFood.BLL.Dependency_Injection
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<ProductRepository>();
            services.AddTransient<CustomerOrderRepository>();
            services.AddTransient<OperatorRepository>();
            services.AddTransient<CustomerRepository>();
            services.AddTransient<CustomerService>();
            services.AddTransient<ProductService>();
            services.AddTransient<OrderService>();
            services.AddTransient<OperatorService>();
            services.AddTransient<CustomerValidator>();
            services.AddTransient<ProductValidator>();
            services.AddTransient<OrderValidator>();
            services.AddTransient<OperatorValidator>();
        }
    }
}
