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
    /// Логика взаимодействия для order_uc.xaml
    /// </summary>
    public partial class order_uc : UserControl
    {
        private Order order;

        public order_uc(Order _order)
        {
            InitializeComponent();

            int RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).FirstOrDefault().RoleId;
            if (RoleId !=4 )
            {
                EditBtn.Visibility = Visibility.Collapsed;
                DelBtn.Visibility = Visibility.Collapsed;
            }





            order = _order;
            Info();
            var status = App.db.OrderStatus.FirstOrDefault(x => x.Order_status_id == order.Order_status_id);
            if (status != null && status.Order_status_id > 5)
            {
                EditBtn.Visibility = Visibility.Collapsed;
            }
            if ( status.Order_status_id == 2)
            {
                EditBtn.Visibility = Visibility.Collapsed;
            }
        }

        public void Info()
        {

            var status = App.db.OrderStatus.FirstOrDefault(x => x.Order_status_id == order.Order_status_id);

            var manager = App.db.User.FirstOrDefault(x => x.User_id == order.id_Manager);
            var sup = App.db.User.FirstOrDefault(x => x.User_id == order.id_Customer);
            order_num_tb.Text = order.Id.ToString();
            order_date_tb.Text = order.DateOrder.ToString("dd.MM.yyyy");
            order_name_tb.Text = order.Name;
            order_price_tb.Text = $"{order.Amount} руб";
            order_supplier_id_tb.Text = $"{manager.Surname} {manager.Name} {manager.Patronymic}";
            order_manager_id_tb.Text = $"{sup.Surname} {sup.Name} {sup.Patronymic}";
            order_date_end_tb.Text = order.DateEnd.Value.ToString("dd.MM.yyyy");
            order_status_tb.Text = status.Status_name;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            
            var status = App.db.OrderStatus.FirstOrDefault(x => x.Order_status_id == order.Order_status_id);

            
            if (status != null && status.Order_status_id < 5)
            {
                order.Order_status_id = 2;
                App.db.SaveChanges();
                MessageBox.Show("Заказ отклонен");
                App.mainWindow.MainFrame.Navigate(new orders_list_page()); 
            }
            else
            {
                MessageBox.Show("Данный заказ нельзя отклонить, так как он уже на стадии 'Закупка' или выше.");
            }
        }


        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            App.db.Order.Remove(order);
            App.db.SaveChanges();
            App.mainWindow.MainFrame.NavigationService.Navigate(new orders_list_page());
        }

        private void OrderEditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.MainFrame.NavigationService.Navigate(new add_edit_order_page(order));
        }
    }

}
