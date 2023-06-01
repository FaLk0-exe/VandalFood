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
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private DispatcherTimer _timer;
        private OrderService _orderService;
        private CustomerOrder _customerOrder;
        public TimeSpan _cookTimeSpan;
        public List<OrderItem> OrderItems { get; private set; }
        public string CookTimeSpan { get { return _cookTimeSpan.ToString(@"hh\:mm\:ss"); } }
        public OrderDetailsWindow(OrderService orderService, int id)
        {
            _orderService = orderService;
            _cookTimeSpan = new TimeSpan(0, 0, 0, 0);
            InitializeComponent();
            _customerOrder = _orderService.Get(id);
            OrderItems = _customerOrder.OrderItems.ToList();
            ItemsGrid.ItemsSource = OrderItems;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ви впевнені, що хочете взяти замовлення для приготування?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                TimeLabel.Content = CookTimeSpan;
                _customerOrder.OrderStatusId = (int)OrderStatusEnum.AwaitingPreparation;
                _orderService.Update(_customerOrder);
                CompleteButton.Visibility = Visibility.Visible;
                AcceptButton.Visibility = Visibility.Hidden;
                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(timer_Tick);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Start();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = CookTimeSpan;
            _cookTimeSpan = _cookTimeSpan.Add(new TimeSpan(0, 0, 1));
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ви впевнені, що хочете завершити замовлення?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _timer.Stop();
                _customerOrder.OrderStatusId = (int)OrderStatusEnum.AwaitingDelivery;
                _orderService.Update(_customerOrder);
                OrderWindow window = new OrderWindow(_orderService);
                window.Show();
                this.Close();
            }
        }
    }
}
