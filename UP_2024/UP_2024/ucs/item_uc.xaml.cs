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

namespace UP_2024.ucs
{
    /// <summary>
    /// Логика взаимодействия для item_uc.xaml
    /// </summary>
    public partial class item_uc : UserControl
    {
        private ItemLocation itemLocation;
        public item_uc(WorkshopItem item)
        {
            InitializeComponent();
            DataContext = item;
            if (item.Name == "Заготовительный цех")
                NameTb.Text = "Equipment";
            else if (item.Name == "Механический цех")
                NameTb.Text = "Exit";
            else if (item.Name == "Покрасочный цех")
                NameTb.Text = "FireExtinguisher";
            else if (item.Name == "Сборочный цех")
                NameTb.Text = "FirstAid";
        }

        private void MainImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                itemLocation = new ItemLocation()
                {
                    item = DataContext as WorkshopItem
                };
                DragDrop.DoDragDrop(MainImage, new DataObject(DataFormats.Serializable,
                    itemLocation), DragDropEffects.Move);
            }
        }
    }
}
