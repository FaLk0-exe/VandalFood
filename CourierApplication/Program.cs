
using CourierApplication;
using CourierApplication.Services;
using CourierApplication.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandalFood.BLL.Dependency_Injection;
using VandalFood.BLL.Services;

namespace CourierApplication
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDependencies();
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<AuthService>();
                    services.AddSingleton<OrderWindow>();
                })
                .Build();
            var app = host.Services.GetService<App>();
            app?.Run();
        }
    }
}
