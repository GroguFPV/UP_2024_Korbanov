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

namespace UP_2024.pages
{
    /// <summary>
    /// Логика взаимодействия для nav_page.xaml
    /// </summary>
    public partial class nav_page : Page
    {
        public nav_page()
        {
            InitializeComponent();


            int roleId = (int)App.db.User.FirstOrDefault(x => x.Login == App.currentUser)?.RoleId;
            if (roleId == 1) eq_list_btn.Visibility = Visibility.Visible;
                if (roleId != 3) workers_card.Visibility = Visibility.Collapsed; plan_btn.Visibility = Visibility.Visible;
            if (roleId == 4) d_card.Visibility = Visibility.Collapsed;
            //if (roleId != 4) eq_list_btn.Visibility = Visibility.Collapsed; 

            SetPageTitle(roleId);
        }

        private void SetPageTitle(int roleId)
        {

            switch (roleId)
            {
                case 1: 
                    App.mainWindow.Title = "Экран мастера";
                    break;
                case 2:
                    App.mainWindow.Title = "Экран конструктора";
                    break;
                case 3:
                    App.mainWindow.Title = "Экран директора";
                    break;
                case 4:
                    App.mainWindow.Title = "Экран заказчика";
                    break;
                case 5:
                    App.mainWindow.Title = "Экран менеджера";
                    break;
                case 6:
                    App.mainWindow.Title = "Экран рабочего";
                    break;
                default:
                    App.mainWindow.Title = "Главная страница";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new workerslist_page());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new materials_components_list());
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new orders_list_page());
        }

        private void OrderListBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new orders_list_page());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new eq_list_page());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new tests_page());
        }

        private void plan_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new plan_page());
        }
    }
}
