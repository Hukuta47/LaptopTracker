using LaptopTracker.Database;
using LaptopTracker.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LaptopTracker.Pages
{
    public partial class GivePage : Page
    {
        static WrapPanel staticItemsPanel;
        static Button staticButton_Next;
        private static bool? _isButtonShown = false;
        private static ComboBox staticComboBox_MethodSort;
        private static TextBox staticTextBox_Search;

        private static HashSet<int> SelectedDeviceIds = new HashSet<int>();

        List<string> SortMethods = new List<string>()
        {
            "ПО МОДЕЛИ",
            "ПО КОРОТКОМУ ИМЕНИ",
            "ПО ПРОИЗВОДИТЕЛЬНОСТИ"
        };

        public GivePage()
        {
            InitializeComponent();
            staticItemsPanel = ItemsPanel;
            staticButton_Next = Button_Next;
            staticComboBox_MethodSort = ComboBox_MethodSort;
            staticTextBox_Search = TextBox_Search;

            ComboBox_MethodSort.ItemsSource = SortMethods;
            _isButtonShown = false;

            RefreshDevices();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            SelectedDeviceIds.Clear();
            MainWindow.GoToMainPage();
        }

        public static void OnSelectElement()
        {
            var visibleSelectedIds = staticItemsPanel.Children
                .OfType<DeviceCard>()
                .Where(card => card.isSelected)
                .Select(card => card.Device.Id)
                .ToHashSet();

            foreach (var id in visibleSelectedIds)
                SelectedDeviceIds.Add(id);

            var visibleIds = staticItemsPanel.Children
                .OfType<DeviceCard>()
                .Select(card => card.Device.Id)
                .ToHashSet();

            var toRemove = SelectedDeviceIds
                .Where(id => visibleIds.Contains(id) && !visibleSelectedIds.Contains(id))
                .ToList();

            foreach (var id in toRemove)
                SelectedDeviceIds.Remove(id);

            bool isOneChosen = SelectedDeviceIds.Any();

            if (_isButtonShown != isOneChosen)
            {
                _isButtonShown = isOneChosen;
                ShowButton(isOneChosen);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            var selectedDevices = App.entities.Device
                .Where(device => SelectedDeviceIds.Contains(device.Id))
                .ToList();

            MainWindow.EnterData = new EnterData(selectedDevices);
            MainWindow.Frame_MainFrame.Navigate(MainWindow.EnterData);
        }

        public static void ShowButton(bool isShow)
        {
            var animation = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, isShow ? -113 : 16),
                To = new Thickness(0, 0, 0, isShow ? 16 : -113),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            staticButton_Next.BeginAnimation(MarginProperty, animation);
        }

        private void ComboBox_MethodSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDevices();
        }

        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDevices();
        }

        private static void RefreshDevices()
        {
            string searchText = staticTextBox_Search?.Text?.ToLower() ?? "";
            string sortMethod = staticComboBox_MethodSort?.SelectedItem as string ?? "ПО МОДЕЛИ";

            var query = App.entities.Device
                .Where(laptop => laptop.DeviceModel.DeviceTypeId == 9 && laptop.Laptop.Issued == false);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(d =>
                    (d.DeviceModel.Manufacturer != null && d.DeviceModel.Manufacturer.ToLower().Contains(searchText)) ||
                    (d.DeviceModel.Model != null && d.DeviceModel.Model.ToLower().Contains(searchText)) ||
                    (d.ShortName != null && d.ShortName.ToLower().Contains(searchText)));
            }

            query = sortMethod switch
            {
                "ПО КОРОТКОМУ ИМЕНИ" => query.OrderBy(d => d.ShortName),
                "ПО ПРОИЗВОДИТЕЛЬНОСТИ" => query.OrderBy(d => d.Laptop.PerfomanceRateId),
                _ => query.OrderBy(d => d.DeviceModelId)
            };

            var devices = query.ToList();

            staticItemsPanel.Children.Clear();

            foreach (var device in devices)
            {
                var card = new DeviceCard(device);

                if (SelectedDeviceIds.Contains(device.Id))
                {
                    card.isSelected = true;
                    ((Storyboard)card.Resources["ShowCheckAnimation"]).Begin();
                }

                staticItemsPanel.Children.Add(card);
            }

            OnSelectElement();
        }
    }
}
