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
    /// Логика взаимодействия для material_uc.xaml
    /// </summary>
    public partial class material_uc : UserControl
    {
        Material material;

        public material_uc(Material _material)
        {
            InitializeComponent();
            int RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).FirstOrDefault().RoleId;
            if (RoleId != 3 && RoleId != 5)
            {
                EditBtn.Visibility = Visibility.Collapsed;
                DelBtn.Visibility = Visibility.Collapsed;
            }
            material = _material;
            Load();
        }

        public void Load()
        {
            mat_art_tb.Text = material.Article;
            mat_count_tb.Text = material.Count.ToString();
            mat_name_tb.Text = material.Name;
            mat_supplier_tb.Text = material.SupplierName;
            mat_unit_tb.Text = material.Unit.Name.ToString();
            mat_price_tb.Text = material.PriceOneKg.ToString();
            mat_date_tb.Text = material.Supplier.DeliveryTime;
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот материал?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                App.db.Material.Remove(material);
                App.db.SaveChanges();
                MessageBox.Show("Материал удален");
                App.mainWindow.MainFrame.Navigate(new materials_components_list());
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.MainFrame.Navigate(new add_edit_mat_page(material));
        }
    }
}