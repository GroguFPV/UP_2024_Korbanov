using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_2024.db;

namespace UP_2024.pages
{
    public partial class add_worker_page : Page
    {
        public add_worker_page()
        {
            InitializeComponent();
            StreetCbx.ItemsSource = App.db.Street.ToList();
            StreetCbx.DisplayMemberPath = "Title";
            CityCbx.ItemsSource = App.db.City.ToList();
            CityCbx.DisplayMemberPath = "Title";
            TaskCbx.ItemsSource = App.db.PerformTasks.ToList();
            TaskCbx.DisplayMemberPath = "Name";
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new workerslist_page());
        }

        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTbx.Text) || string.IsNullOrWhiteSpace(PassTbx.Text) ||
                string.IsNullOrWhiteSpace(SurnameTbx.Text) || string.IsNullOrWhiteSpace(NameTbx.Text) ||
                CityCbx.SelectedIndex == -1 || StreetCbx.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(HomeTbx.Text) || QualCbx.SelectedIndex == -1 ||
                EduCbx.SelectedIndex == -1 || string.IsNullOrWhiteSpace(BirthDP.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все необходимые данные.");
            }
            else if (App.db.User.Any(x => x.Login == LoginTbx.Text))
            {
                MessageBox.Show("Данный логин уже используется.");
            }
            else
            {
                try
                {
                    var user = new User
                    {
                        Login = LoginTbx.Text,
                        Password = PassTbx.Text,
                        Surname = SurnameTbx.Text,
                        Name = NameTbx.Text,
                        Patronymic = PatronymicTbx.Text,
                        BirthDate = BirthDP.DisplayDate,
                        Education = EduCbx.Text,
                        Qualification = QualCbx.Text,
                        RoleId = 6,
                        Id_Street = ((Street)StreetCbx.SelectedItem).Id,
                        House = HomeTbx.Text,
                        Flat = FlatTbx.Text
                    };

                    App.db.User.Add(user);
                    App.db.SaveChanges();

                    foreach (var item in TaskList.Items)
                    {
                        var task = (PerformTasks)item;
                        var userTask = new User_tasks
                        {
                            user_id = user.User_id,
                            id_perform_task = task.Id
                        };
                        App.db.User_tasks.Add(userTask);
                    }
                    App.db.SaveChanges();

                    MessageBox.Show("Работник успешно добавлен.");
                    NavigationService.Navigate(new workerslist_page());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            if (TaskCbx.SelectedIndex == -1)
            {
                MessageBox.Show("Сначала выберите задачу.");
            }
            else
            {
                var selectedTask = (PerformTasks)TaskCbx.SelectedItem;
                if (TaskList.Items.Cast<PerformTasks>().Any(task => task.Name == selectedTask.Name))
                {
                    MessageBox.Show("Эта задача уже выбрана.");
                }
                else
                {
                    TaskList.Items.Add(selectedTask);
                }
            }
        }
    }
}
