﻿using CourierApplication.Services;
using CourierApplication.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VandalFood.BLL.Services;

namespace CourierApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OperatorService _operatorService;
        private AuthService _authService;
        private OrderWindow _orderWindow;
        public MainWindow(OperatorService operatorService, AuthService authService, OrderWindow window)
        {
            _orderWindow = window;
            _authService = authService;
            _operatorService = operatorService;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var operators = _operatorService.Get();
            if (_authService.Authenticate(LoginTextBox.Text, PasswordTextBox.Password.ToString()))
            {
                this.Hide();
                _orderWindow.Show();
            }
            else
            {
                MessageBox.Show("Такий користувач не знайдений у системі!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}