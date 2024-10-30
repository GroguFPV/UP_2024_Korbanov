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
using UP_2024.pages;

namespace UP_2024.ucs
{
    /// <summary>
    /// Логика взаимодействия для accessories_uc.xaml
    /// </summary>
    public partial class accessories_uc : UserControl
    {
        Accessories accessories;

        public accessories_uc(Accessories _accessories)
        {
            InitializeComponent();
            int RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).FirstOrDefault().RoleId;
            if (RoleId != 3 && RoleId != 5)
            {
                EditBtn.Visibility = Visibility.Collapsed;
                DelBtn.Visibility = Visibility.Collapsed;
            }

            accessories = _accessories;
            Load();
        }

        public void Load()
        {
            mat_art_tb.Text = accessories.Article;
            mat_count_tb.Text = accessories.Count.ToString();
            mat_name_tb.Text = accessories.Name;
            mat_supplier_tb.Text = accessories.SupplierName;
            mat_unit_tb.Text = accessories.Unit.Name.ToString();
            mat_price_tb.Text = accessories.Price.ToString();
            mat_date_tb.Text = accessories.Supplier.DeliveryTime;
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                App.db.Accessories.Remove(accessories);
                App.db.SaveChanges();
                MessageBox.Show("Удалено!");
                App.mainWindow.MainFrame.Navigate(new materials_components_list());
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.MainFrame.Navigate(new add_edit_acc_page(accessories));
        }
    }

}
