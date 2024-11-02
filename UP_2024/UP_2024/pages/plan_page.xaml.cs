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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UP_2024.db;
using UP_2024.ucs;
using Image = System.Windows.Controls.Image;
namespace UP_2024.pages
{
    /// <summary>
    /// Логика взаимодействия для plan_page.xaml
    /// </summary>
    public partial class plan_page : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private List<ItemLocation> itemLocations = new List<ItemLocation>();

        public plan_page()
        {
            InitializeComponent();
            WorkshopCb.ItemsSource = App.db.WorkshopItem.Where(x => x.IsWorkShop == true).ToList();
            WorkshopCb.SelectedIndex = 0;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 600);
            timer.Tick += new EventHandler(Tick);

            List<WorkshopItem> items = App.db.WorkshopItem.Where(x => x.IsWorkShop == true).ToList();
            foreach (WorkshopItem item in items)
                ItemPanel.Children.Add(new item_uc(item));
        }
        private void Tick(object sender, EventArgs e)
        {
            MyFilterPanel.Visibility = Visibility.Collapsed;
            timer.Stop();
        }


        private void Refresh()
        {
            foreach (var item in itemLocations)
                canvas.Children.Remove(item.image);
            itemLocations.Clear();
            
            WorkshopItem workshop = App.db.WorkshopItem.FirstOrDefault(x => x.IsWorkShop == false);

            // Проверьте, что объект найден, перед установкой источника изображения
            if (workshop != null)
            {
                PlanImage.Source = Methods.GetBitmapImageFromBytes(workshop.Photo);
            }
            else
            {
                MessageBox.Show("No workshop with IsWorkShop set to true found.");
            }


            foreach (var item in (WorkshopCb.SelectedItem as WorkshopItem).Location)
            {
                ItemLocation itemLocation = new ItemLocation()
                {
                    item = item.WorkshopItem,
                    location = item
                };
                itemLocations.Add(AddImageToCanvas(itemLocation));
                MoveImage(itemLocation.image,
                    new Point((double)itemLocation.location.FromLeft, (double)itemLocation.location.FromUp));
            }
        }

        public void OpenPanel()
        {
            MyFilterPanel.Visibility = Visibility.Visible;
            Storyboard slideOutboard = (Storyboard)this.Resources["SlideIn"];
            slideOutboard.Begin(MyFilterPanel);
        }
        public void ClosePanel()
        {
            timer.Start();
            Storyboard slideOutboard = (Storyboard)this.Resources["SlideOut"];
            slideOutboard.Begin(MyFilterPanel);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in itemLocations)
            {
                if (item.location.Id == 0)
                {
                    App.db.Location.Add(item.location);
                }
            }
            App.db.SaveChanges();

            Methods.TakeInformation("Схема успешно сохранена!");
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = new List<ItemLocation>(itemLocations);

            foreach (var item in itemsToRemove)
            {
                canvas.Children.Remove(item.image);
                if (item.location.Id != 0)
                    App.db.Location.Remove(item.location);
                itemLocations.Remove(item);
            }

            App.db.SaveChanges();

        }

        private void WorkshopCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is ItemLocation item)
            {
                if (item.image == null)
                    AddImageToCanvas(item);

                MoveImage(item.image, e.GetPosition(canvas));
            }
        }

        private ItemLocation AddImageToCanvas(ItemLocation item)
        {
            Image image = new Image { Source = Methods.GetBitmapImageFromBytes(item.item.Photo) };
            image.Width = 50;
            image.Height = 50;
            canvas.Children.Add(image);
            item.image = image;
            image.DataContext = item;
            image.MouseMove += image_MouseMove;
            return item;
        }
        private void MoveImage(Image image, Point point)
        {
            ItemLocation item = image.DataContext as ItemLocation;
            if (item.location == null)
            {
                item.location = new Location()
                {
                    IdWorkshop = (WorkshopCb.SelectedItem as WorkshopItem).Id,
                    IdItem = item.item.Id,
                    FromLeft = (decimal)point.X,
                    FromUp = (decimal)point.Y,
                };
                itemLocations.Add(item);
            }
            else
            {
                item.location.FromLeft = (decimal)point.X;
                item.location.FromUp = (decimal)point.Y;
            }

            Canvas.SetLeft(image, point.X);
            Canvas.SetTop(image, point.Y);
        }



        private void image_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Image image = sender as Image;
                DragDrop.DoDragDrop(image, new DataObject(DataFormats.Serializable,
                    image.DataContext as ItemLocation), DragDropEffects.Move);
            }
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? 1.1 : 0.9;

            // Определите координаты точки под курсором мыши
            Point mousePosition = e.GetPosition(canvas);

            // Сохраните текущие трансформации
            double currentScaleX = scaleTransform.ScaleX;
            double currentScaleY = scaleTransform.ScaleY;
            double currentTranslateX = translateTransform.X;
            double currentTranslateY = translateTransform.Y;

            Point position = e.GetPosition(Origin);

            // Рассчитайте новую позицию для компенсации сдвига
            double newTranslateX = currentTranslateX - ((15 * (zoom == 0.9 ? -1 : 1)) * (position.X < 0 ? -1 : 3) * currentScaleX);
            double newTranslateY = currentTranslateY - ((15 * (zoom == 0.9 ? -1 : 1)) * (position.Y < 0 ? -1 : 3) * currentScaleY);


            // Примените новую трансформацию масштаба и позицию
            scaleTransform.ScaleX *= zoom;
            scaleTransform.ScaleY *= zoom;
            translateTransform.X = newTranslateX;
            translateTransform.Y = newTranslateY;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            OpenPanel();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClosePanel();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new nav_page());
        }
    }
}

