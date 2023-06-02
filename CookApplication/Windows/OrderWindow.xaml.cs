
using CookApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using VandalFood.BLL.Services;
using VandalFood.DAL.Enums;
using VandalFood.DAL.Models;

namespace CookApplication.Windows
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private OrderService _orderService;
        private DispatcherTimer _timer;
        public List<CustomerOrder> Orders { get; private set; }
        public OrderWindow(OrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 5);
            UpdateSource();
            FillFields();
            _timer.Start();
        }

        public void UpdateSource()
        {
            Orders = _orderService.Get().Where(s => s.OrderStatusId == (int)OrderStatusEnum.Confirmed).ToList();
        }
        public void FillFields()
        {
            var orderViews = Orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Products = string.Join(", ", order.OrderItems.Select(item => $"{item.Title} x{item.Amount}"))
            }).ToList();

            OrderGrid.ItemsSource = orderViews;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderView = button.DataContext as OrderViewModel;

            var orderDetailsWindow = new OrderDetailsWindow(_orderService,orderView.Id);
            orderDetailsWindow.Show();
            this.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateSource();
            FillFields();
        }

    }
}
