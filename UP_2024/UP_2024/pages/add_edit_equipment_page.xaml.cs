using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_2024.db;

namespace UP_2024.pages
{
    public partial class add_edit_equipment_page : Page
    {
        private HardwareFailure hardwareFailure;

        public add_edit_equipment_page(HardwareFailure _hardwareFailure)
        {
            InitializeComponent();
            hardwareFailure = _hardwareFailure;

            // Загружаем оборудование в комбобокс
            EquipmentCb.ItemsSource = App.db.Equipment.ToList();
            EquipmentCb.DisplayMemberPath = "Model"; // Определяем, как отображать элементы

            if (hardwareFailure.Fail_id != 0)
            {
                // Получаем оборудование по id
                var equipment = App.db.Equipment.FirstOrDefault(x => x.id_equipment == hardwareFailure.Equipment_id);

                // Устанавливаем сам объект Equipment
                EquipmentCb.SelectedItem = equipment;

                LoadEdit(); // Заполняем остальные поля
                EQTitleTb.Text = "Изменить данные сбоя";
            }
        }


        private void LoadEdit()
        {
            
            ReasonTb.Text = hardwareFailure.Reason;

            if (hardwareFailure.DateStart.HasValue)
            {
                StartDate.SelectedDate = hardwareFailure.DateStart.Value.Date;
                StartTimeTb.Text = hardwareFailure.DateStart.Value.ToString("HH\\:mm");
            }

            if (hardwareFailure.DateEnd.HasValue)
            {
                EndDate.SelectedDate = hardwareFailure.DateEnd.Value.Date;
                EndTimeTb.Text = hardwareFailure.DateEnd.Value.ToString("HH\\:mm");
            }

            
            EquipmentCb.IsEnabled = false;
        }

        private bool AreFieldsValid()
        {
            if (EquipmentCb.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите оборудование.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ReasonTb.Text))
            {
                MessageBox.Show("Пожалуйста, укажите причину поломки.");
                return false;
            }
            if (!StartDate.SelectedDate.HasValue || string.IsNullOrWhiteSpace(StartTimeTb.Text))
            {
                MessageBox.Show("Пожалуйста, укажите дату и время начала сбоя.");
                return false;
            }
            return true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsValid())
            {
                return;
            }

            try
            {
                var eq = EquipmentCb.SelectedItem as Equipment;

                // Проверяем, это новая запись или обновление существующей
                if (hardwareFailure.Fail_id == 0)
                {
                    // Создаём новую запись о сбое
                    hardwareFailure = new HardwareFailure
                    {
                        Reason = ReasonTb.Text,
                        Equipment_id = eq.id_equipment,
                        DateStart = CombineDateAndTime(StartDate.SelectedDate, StartTimeTb.Text)
                    };

                    // Если выбрана дата окончания, добавляем её в объект
                    if (EndDate.SelectedDate.HasValue)
                    {
                        hardwareFailure.DateEnd = CombineDateAndTime(EndDate.SelectedDate.Value, EndTimeTb.Text);
                    }

                    App.db.HardwareFailure.Add(hardwareFailure);
                }
                else
                {
                    
                    hardwareFailure.Reason = ReasonTb.Text;
                    hardwareFailure.Equipment_id = eq.id_equipment;
                    hardwareFailure.DateStart = CombineDateAndTime(StartDate.SelectedDate, StartTimeTb.Text);

                    if (EndDate.SelectedDate.HasValue)
                    {
                        hardwareFailure.DateEnd = CombineDateAndTime(EndDate.SelectedDate.Value, EndTimeTb.Text);
                    }
                    else
                    {
                        hardwareFailure.DateEnd = null; 
                    }
                }

          
                App.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService.Navigate(new eq_list_page());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        private DateTime? CombineDateAndTime(DateTime? date, string time)
        {
            if (!date.HasValue || string.IsNullOrWhiteSpace(time))
            {
                return null;
            }

            TimeSpan parsedTime;
            if (TimeSpan.TryParse(time, out parsedTime))
            {
                return date.Value.Date + parsedTime;
            }

            return null;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }

        private void RetBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new eq_list_page());
        }
    }
}
