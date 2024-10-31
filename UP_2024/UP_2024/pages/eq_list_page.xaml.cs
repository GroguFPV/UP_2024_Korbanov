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
    /// Логика взаимодействия для eq_list_page.xaml
    /// </summary>
    public partial class eq_list_page : Page
    {
        public eq_list_page()
        {
            InitializeComponent();
            IEnumerable<HardwareFailure> list = App.db.HardwareFailure.ToList();
            foreach(HardwareFailure hardware in list)
            {
                eq_list_wp.Children.Add(new ec_uc(hardware));
            }
        }

        private void AddMatBtn_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_equipment_page(new HardwareFailure()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }
    }
}
