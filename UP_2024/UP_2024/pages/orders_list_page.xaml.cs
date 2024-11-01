using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_2024.db;
using UP_2024.ucs;

namespace UP_2024.pages
{
    /// <summary>
    /// Логика взаимодействия для orders_list_page.xaml
    /// </summary>
    public partial class orders_list_page : Page
    {
        private readonly int roleId;
        private readonly int managerId;

        public orders_list_page()
        {
            InitializeComponent();
            StatusCbx.SelectedIndex = 4;

            var currentUser = App.db.User.FirstOrDefault(x => x.Login == App.currentUser);
            if (currentUser != null)
            {
                roleId = (int)currentUser.RoleId;
                managerId = currentUser.User_id;
            }
            if (roleId != 5 && roleId != 4) { 
            AddAccBtn.Visibility = Visibility.Collapsed;
            }
            Refresh();

            
            StatusCbx_SelectionChanged(StatusCbx, null);
        }


        public void Refresh()
        {
            IEnumerable<Order> orders = App.db.Order.ToList();

      
            switch (roleId)
            {
                case 1: // Менеджер
                    orders = orders.Where(order => order.Order_status_id == 1 || order.id_Manager == managerId);
                    break;

                case 2: // Конструктор
                    orders = orders.Where(order => order.Order_status_id == 3);
                    break;

                case 3: // Мастер
                    orders = orders.Where(order => order.Order_status_id == 6 || order.Order_status_id == 7);
                    break;

                default:
                    orders = Enumerable.Empty<Order>();
                    break;
            }

           
            orders_list_wp.Children.Clear();
            foreach (Order order in orders)
            {
                orders_list_wp.Children.Add(new order_uc(order));
            }
        }

        private void StatusCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            IEnumerable<Order> orders = App.db.Order.ToList();

            
            switch (StatusCbx.SelectedIndex)
            {
                case 0:
                    orders = orders.Where(x => x.Order_status_id != 2 && x.Order_status_id < 5).ToList();
                    break;

                case 1: 
                    orders = orders.Where(x => x.Order_status_id >= 8).ToList();
                    break;

                case 2:
                    orders = orders.Where(x => x.Order_status_id > 4 && x.Order_status_id < 8).ToList();
                    break;

                case 3: 
                    orders = orders.Where(x => x.Order_status_id == 2).ToList();
                    break;

                case 4:
                    orders = orders.ToList();
                    break;
            }

          
            orders_list_wp.Children.Clear();
            foreach (Order order in orders)
            {
                orders_list_wp.Children.Add(new order_uc(order));
            }
        }

        private void AddAccBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_order_page(new Order()));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new nav_page());
        }
    }
}
