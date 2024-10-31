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
    /// Логика взаимодействия для auth_page.xaml
    /// </summary>
    public partial class auth_page : Page
    {
        public auth_page()
        {
            InitializeComponent();
            App.mainWindow.Title = "Авторизация";
            App.currentUser = null;
            App.Current.Properties[0] = null;
        }
        private void EnterButt_Click(object sender, RoutedEventArgs e)
        {
            if (App.db.User.Any(x => x.Login == LoginTbx.Text && x.Password == PassTbx.Password))
            {
                App.currentUser = App.db.User.Where(x => x.Login == LoginTbx.Text && x.Password == PassTbx.Password).First().Login;
                if ((bool)RemberCheck.IsChecked) App.Current.Properties[0] = App.currentUser;
                NavigationService.Navigate(new nav_page());
            }
            else MessageBox.Show("Неверный логин или пароль");
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new reg_page());
        }
    }
}
