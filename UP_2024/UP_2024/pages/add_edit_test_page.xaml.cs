using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using UP_2024.db;
using UP_2024.ucs;

namespace UP_2024.pages
{
    /// <summary>
    /// Interaction logic for add_edit_test_page.xaml
    /// </summary>
    // C#
    public partial class add_edit_test_page : Page
    {
        public Product product;
        public List<test_uc> tests = new List<test_uc>();
        bool isNew = true;
        public add_edit_test_page(Product product)
        {
            InitializeComponent();
            this.product = product;

            Order order = product.Order.FirstOrDefault();
            List<Product> products = new List<Product>();
            if (order != null && order.Order_status_id >= 8)
                products = App.db.Product.ToList();
            else
            {
                foreach (var pro in App.db.Product)
                {
                    Order order1 = pro.Order.FirstOrDefault();
                    if (order1 != null && order1.Order_status_id != 2 && order1.Order_status_id < 8)
                        products.Add(pro);
                }
            }
            ProductCb.ItemsSource = products;
            ProductCb.DisplayMemberPath = "Name";

            if (product.Test.Count() != 0)
            {
                TitleTb.Text = "Редактировать тест продукта";
                isNew = true;
                ProductCb.IsEnabled = false;
            }
            foreach (var test in product.Test)
                tests.Add(new test_uc(test, this));

            DataContext = product;
            Refresh();

            ProductCb.SelectedItem = product;
        }

        public void Refresh()
        {
            MyPanel.Children.Clear();
            foreach (var test in tests)
                MyPanel.Children.Add(test);
        }

        private void Back_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string mistake = "";

            if (ProductCb.SelectedIndex == -1 && mistake == "")
                mistake = "Вы не выбрали изделие для теста!";
            if (tests.Count() == 0 && mistake == "")
                mistake = "У теста нет критериев оценки!";

            if (mistake != "")
            {
                Methods.TakeWarning(mistake);
                return;
            }

            foreach (var test in tests)
            {
                test.test.product_id = product.Id;
                if (test.test.Test_id == 0)
                    App.db.Test.Add(test.test);
            }

            App.db.SaveChanges();
            NavigationService.Navigate(new tests_page());
            Methods.TakeInformation("Изменения успешно сохранены!");
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tests.Add(new test_uc(new Test(), this));
            Refresh();
        }

        private void ProductCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            product = ProductCb.SelectedItem as Product;
            foreach (var test in tests)
                test.test.product_id = product.Id;
        }

        private void RetBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

}
