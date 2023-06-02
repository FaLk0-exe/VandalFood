using CourierApplication.Models;
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

namespace CourierApplication.Windows
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
            Orders = _orderService.Get().Where(s => s.OrderStatusId == (int)OrderStatusEnum.AwaitingDelivery).ToList();
        }
        public void FillFields()
        {
            var orderViews = Orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                Address = order.OrderContacts.First(s=>s.ContactTypeId==(int)ContactTypeEnum.Address)?.Value??"",
                Phone = order.OrderContacts.First(s => s.ContactTypeId == (int)ContactTypeEnum.Phone)?.Value??"",
                
            }).ToList();

            OrderGrid.ItemsSource = orderViews;
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateSource();
            FillFields();
        }

        private void OrderGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var item = e.AddedCells.FirstOrDefault().Item;
            if(item is OrderViewModel model)
            {
                var orderDetailsWindow = new OrderDetailsWindow(_orderService,model.Id);
                 orderDetailsWindow.Show();
                 this.Close();               
            }
        }
    }
}
