using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_2024.db;
using UP_2024.ucs;

namespace UP_2024.pages
{
    public partial class materials_components_list : Page
    {
        public materials_components_list()
        {
            InitializeComponent();

            int roleId = (int)App.db.User.FirstOrDefault(x => x.Login == App.currentUser)?.RoleId;

            if (roleId != 3 && roleId!= 5)
            {
                AddAccBtn.Visibility = Visibility.Collapsed;
                AddMatBtn.Visibility = Visibility.Collapsed;
            }

            LoadWarehouses();
            ListSelectCb.SelectedIndex = 0;
            UpdateContent();
        }

        private void LoadWarehouses()
        {
            MaterialWarehouseCb.ItemsSource = App.db.Storage.ToList();
            MaterialWarehouseCb.DisplayMemberPath = "Name";
        }

        private void RefreshMat()
        {
            mat_comp_wp.Children.Clear();

            Storage selectedWarehouse = MaterialWarehouseCb.SelectedItem as Storage;
            IEnumerable<Material> materials = selectedWarehouse == null
                ? App.db.Material.ToList()
                : App.db.Material.Where(m => m.IdStorage == selectedWarehouse.Id).ToList();

            decimal totalPrice = 0;
            foreach (Material material in materials)
            {
                mat_comp_wp.Children.Add(new material_uc(material));
                decimal priceOneKg = material.PriceOneKg ?? 0;
                
                totalPrice += priceOneKg;
            }

            MaterialCountTb.Text = $"{materials.Count()} из {App.db.Material.Count()}";
            MaterialPriceTb.Text = $"{totalPrice} руб.";
        }

        private void RefreshAcc()
        {
            mat_comp_wp.Children.Clear();

            Storage selectedWarehouse = MaterialWarehouseCb.SelectedItem as Storage;
            IEnumerable<Accessories> accessories = selectedWarehouse == null
                ? App.db.Accessories.ToList()
                : App.db.Accessories.Where(a => a.IdStorage == selectedWarehouse.Id).ToList();

            decimal totalPrice = 0;
            foreach (Accessories accessory in accessories)
            {
                mat_comp_wp.Children.Add(new accessories_uc(accessory));
                decimal price = accessory.Price ?? 0;
                
                totalPrice += price;
            }

            MaterialCountTb.Text = $"{accessories.Count()} из {App.db.Accessories.Count()}";
            MaterialPriceTb.Text = $"{totalPrice} руб.";
        }

        private void RefreshAll()
        {
            mat_comp_wp.Children.Clear();

            Storage selectedWarehouse = MaterialWarehouseCb.SelectedItem as Storage;
            var materials = selectedWarehouse == null
                ? App.db.Material.ToList()
                : App.db.Material.Where(m => m.IdStorage == selectedWarehouse.Id).ToList();

            var accessories = selectedWarehouse == null
                ? App.db.Accessories.ToList()
                : App.db.Accessories.Where(a => a.IdStorage == selectedWarehouse.Id).ToList();

            decimal totalPrice = 0;

            foreach (var material in materials)
            {
                mat_comp_wp.Children.Add(new material_uc(material));
                decimal priceOneKg = material.PriceOneKg ?? 0;
                
                totalPrice += priceOneKg;
            }

            foreach (var accessory in accessories)
            {
                mat_comp_wp.Children.Add(new accessories_uc(accessory));
                decimal price = accessory.Price ?? 0;
                
                totalPrice += price;
            }

            MaterialCountTb.Text = $"{materials.Count() + accessories.Count()} из {App.db.Material.Count() + App.db.Accessories.Count()}";
            MaterialPriceTb.Text = $"{totalPrice} руб.";
        }

        private void MaterialWarehouseCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateContent();
        }



        private void UpdateContent()
        {
            switch (ListSelectCb.SelectedIndex)
            {
                case 0: // Материалы и комплектующие
                    RefreshAll();
                    break;
                case 1: // Только материалы
                    RefreshMat();
                    break;
                case 2: // Только комплектующие
                    RefreshAcc();
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }

        private void AddMatBtn_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_mat_page(new Material()));
        }

        private void AddAccBtn_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new add_edit_acc_page(new Accessories()));
        }

        private void ListSelectCb_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateContent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new auth_page());
        }
    }
}
