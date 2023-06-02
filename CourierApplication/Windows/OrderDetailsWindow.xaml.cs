using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace CourierApplication.Windows
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private DispatcherTimer _timer;
        private OrderService _orderService;
        private CustomerOrder _order;
        public TimeSpan _cookTimeSpan;
        public List<OrderItem> OrderItems { get; private set; }
        public OrderDetailsWindow(OrderService orderService, int id)
        {
            _orderService = orderService;
            _cookTimeSpan = new TimeSpan(0, 0, 0, 0);
            InitializeComponent();
            _order = _orderService.Get(id);
            IdLabel.Content = _order.Id.ToString("000000");
            OrderDateLabel.Content = _order.OrderDate;
            CustomerNameLabel.Content = _order.CustomerName;
            AddressLabel.Content = _order.OrderContacts.First(s => s.ContactTypeId == (int)ContactTypeEnum.Address)?.Value ?? "";
            PhoneLabel.Content = _order.OrderContacts.First(s => s.ContactTypeId == (int)ContactTypeEnum.Phone)?.Value ?? "";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ви впевнені, що хочете взяти замовлення до обробки?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _order.OrderStatusId = (int)OrderStatusEnum.Delivered;
                _orderService.Update(_order);
                CompleteButton.Visibility = Visibility.Visible;
                AcceptButton.Visibility = Visibility.Hidden;
            }
        }


        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ви впевнені, що хочете завершити замовлення?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _order.OrderStatusId = (int)OrderStatusEnum.Completed;
                _orderService.Update(_order);
                OrderWindow window = new OrderWindow(_orderService);
                window.Show();
                this.Close();
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderWindow window = new OrderWindow(_orderService);
            window.Show();
            this.Close();
        }
    }
}
