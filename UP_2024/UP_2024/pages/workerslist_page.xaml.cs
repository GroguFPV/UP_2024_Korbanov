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
using UP_2024.db;
using UP_2024.ucs;

namespace UP_2024.pages
{
    /// <summary>
    /// Логика взаимодействия для workerslist_page.xaml
    /// </summary>
    public partial class workerslist_page : Page
    {
        public workerslist_page()
        {
            InitializeComponent();
            foreach (User user in App.db.User.Where(x => x.RoleId != 3)) worker_list_wp.Children.Add(new worker_uc(user));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_worker_page());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }
    }
}
