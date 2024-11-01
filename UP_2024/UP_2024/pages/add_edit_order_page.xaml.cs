using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_2024.db;

namespace UP_2024.pages
{
    public partial class add_edit_order_page : Page
    {
        private readonly Order _order;
        private readonly bool _isEditMode;

        public add_edit_order_page(Order order = null)
        {
            InitializeComponent();
            _order = order ?? new Order();
            _isEditMode = _order.Id != 0;
            ClientCb.ItemsSource = App.db.User.Where(x => x.RoleId == 4).ToList();
            ClientCb.DisplayMemberPath = "Surname";
            InitializePage();
        }

        private void InitializePage()
        {
            Add_or_edit_tb.Text = _isEditMode ? "Редактировать заказ" : "Добавить заказ";

            if (_isEditMode)
            {
                LoadOrderData();
            }
            else
            {
                var currentUser = App.db.User.FirstOrDefault(x => x.Login == App.currentUser);
                EmployeeTb.Text = currentUser.Surname;
                DateOrderDp.SelectedDate = DateTime.Today;
            }
        }

        private void LoadOrderData()
        {
            if (_order == null)
            {
                MessageBox.Show("Не удалось загрузить данные заказа.");
                return;
            }
            var currentUser = App.db.User.FirstOrDefault(x => x.Login == App.currentUser);
            EmployeeTb.Text = currentUser.Surname;
            OrderNumberTb.Text = _order.Id.ToString();
            NameTb.Text = _order.Name;
            DateOrderDp.SelectedDate = _order.DateOrder;
            DateEndDp.SelectedDate = _order.DateEnd;
            AmountTb.Text = _order.Amount.ToString();
            var client = App.db.User.FirstOrDefault(x => x.User_id == _order.id_Customer);
            if (client != null)
            {
                ClientCb.SelectedItem = client;
            }
            else
            {
                MessageBox.Show("Заказчик не найден.");
            }
            ClientCb.IsEnabled = !_isEditMode;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User selectedClient = ClientCb.SelectedItem as User;
                if (selectedClient == null)
                {
                    MessageBox.Show("Пожалуйста, выберите клиента.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(NameTb.Text) ||
                    !DateOrderDp.SelectedDate.HasValue ||
                    !DateEndDp.SelectedDate.HasValue ||
                    string.IsNullOrWhiteSpace(AmountTb.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                var currentUser = App.db.User.FirstOrDefault(x => x.Login == App.currentUser);
                if (currentUser == null)
                {
                    MessageBox.Show("Не удалось определить текущего пользователя.");
                    return;
                }

                _order.Name = NameTb.Text;
                _order.DateOrder = DateOrderDp.SelectedDate ?? DateTime.Today;
                _order.DateEnd = DateEndDp.SelectedDate;
                _order.Amount = decimal.TryParse(AmountTb.Text, out var amount) ? amount : 0;
                _order.id_Customer = selectedClient.User_id;
                _order.id_Manager = currentUser.User_id;

                if (!_isEditMode)
                {
                    _order.Order_status_id = 3;
                    App.db.Order.Add(_order);
                }

                App.db.SaveChanges();
                MessageBox.Show("Заказ успешно сохранен!", "Сохранение");
                NavigationService.Navigate(new orders_list_page());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}");
            }
        }

        private void RetBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new orders_list_page());
        }
    }
}
