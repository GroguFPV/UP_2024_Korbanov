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
using static System.Net.Mime.MediaTypeNames;
using UP_2024.pages;
using UP_2024.db;
namespace UP_2024.ucs
{
    /// <summary>
    /// Логика взаимодействия для test_uc.xaml
    /// </summary>
    public partial class test_uc : UserControl
    {
        public Test test;
        add_edit_test_page page;
        public test_uc(Test test, add_edit_test_page page)
        {
            InitializeComponent();
            this.page = page;
            this.test = test;

            if (test.Test_id == 0)
                PassedCb.IsChecked = true;
            if (test.isPassed != null && test.isPassed == false)
            {
                PassedCb.IsChecked = false;
                ReasonTb.Visibility = System.Windows.Visibility.Visible;
            }
            else if (test.isPassed != null && test.isPassed == true)
            {
                PassedCb.IsChecked = true;
                ReasonTb.Visibility = System.Windows.Visibility.Collapsed;
            }

            DataContext = test;
        }

        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Methods.TakeChoice("Вы точно хотите удалить критерий?"))
            {
                page.tests.Remove(this);
                if (test.Test_id != 0)
                    App.db.Test.Remove(test);
                page.Refresh();
                Methods.TakeInformation("Критерий успешно удален!");
            }
        }

        private void PassedCb_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ReasonTb.Visibility = System.Windows.Visibility.Collapsed;
            test.isPassed = PassedCb.IsChecked;
        }

        private void PassedCb_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ReasonTb.Visibility = System.Windows.Visibility.Visible;
            test.isPassed = PassedCb.IsChecked;
        }

        private void ReasonTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            test.Description = ReasonTb.Text;
        }
    }
}

