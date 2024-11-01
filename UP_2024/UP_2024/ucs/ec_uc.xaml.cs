using System.Windows;
using System.Windows.Controls;
using UP_2024.db;
using UP_2024.pages;

namespace UP_2024.ucs
{
    public partial class ec_uc : UserControl
    {
        HardwareFailure hardwareFailure;
        public ec_uc(HardwareFailure _hardwareFailure)
        {
            InitializeComponent();
            hardwareFailure = _hardwareFailure;
            LoadData();
        }

        private void LoadData()
        {
            failure_date_tb.Text = hardwareFailure.DateStart?.ToString("dd.MM.yyyy HH:mm") ?? "Неизвестно"; 
            failure_equipment_tb.Text = hardwareFailure.Equipment?.Model ?? "Неизвестно"; 
            failure_reason_tb.Text = hardwareFailure.Reason ?? "Неизвестно"; 
            failure_date_start_tb.Text = hardwareFailure.DateEnd?.ToString("dd.MM.yyyy HH:mm") ?? "В процессе"; 
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.MainFrame.NavigationService.Navigate(new add_edit_equipment_page(hardwareFailure));
        }
    }
}
