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
            var product = MyList.SelectedItem as Product;
            if (product != null)
            {
                NavigationService.Navigate(new add_edit_test_page(product));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования.");
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyList.SelectedItem is Product selectedProduct)
                {
                    
                    var testsToDelete = App.db.Test.Where(t => t.product_id == selectedProduct.Id).ToList();

                    
                    if (testsToDelete.Any())
                    {
                        App.db.Test.RemoveRange(testsToDelete);
                        App.db.SaveChanges();
                        Refresh();
                        Methods.TakeInformation("Тесты успешно удалены!");
                    }
                    else
                    {
                        MessageBox.Show("Нет тестов для удаления у выбранного продукта.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите продукт для удаления тестов.");
                }
            }
            catch (Exception ex)
            {
                Methods.TakeWarning("Невозможно удалить тесты!\n" + ex.Message);
            }
        }


        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }
    }

}
