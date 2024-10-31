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

namespace UP_2024.pages
{
    /// <summary>
    /// Логика взаимодействия для tests_page.xaml
    /// </summary>
    public partial class tests_page : Page
    {
        public tests_page()
        {
            InitializeComponent();
            Refresh();


        }

        public void Refresh()
        {
            IEnumerable<Product> products = App.db.Product.Where(x => x.Test.Count() > 0);

            

            MyList.ItemsSource = products.ToList();
        }
  

        private void AddOrderBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_test_page(new Product()));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_test_page((sender as Image).DataContext as Product));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Methods.TakeChoice("Вы точно хотите удалить тест продукта?"))
                    return;
                List<Test> tests = ((sender as Image).DataContext as Product).Test.ToList();
                foreach (var test in tests)
                    App.db.Test.Remove(test);
                App.db.SaveChanges();
                Refresh();
                Methods.TakeInformation("Тест успешно удален!");
            }
            catch (Exception ex) { Methods.TakeWarning("Невозможео удалить тесты!\n" + ex.Message); }
        }
    }

}
