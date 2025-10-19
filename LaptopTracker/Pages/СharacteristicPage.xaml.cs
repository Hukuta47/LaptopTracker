
using LaptopTracker.Database;
using LaptopTracker.UserControls;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LaptopTracker.Pages
{
    public partial class СharacteristicPage : Page
    {
        private DeviceCard deviceCard;

        public СharacteristicPage(DeviceCard deviceCard)
        {
            InitializeComponent();
            this.deviceCard = deviceCard;

            if (deviceCard.Image_PictureDevice.Source != null)
            {
                Image_PictureDevice.Source = deviceCard.Image_PictureDevice.Source;
                Image_NoImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Image_NoImage.Visibility = Visibility.Visible;
            }

            TextBlock_Description.Text = GenerateDescpriptionText();

            Button_Select.Content = deviceCard.isSelected ? "СНЯТЬ" : "ВЫБРАТЬ";
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            deviceCard.ToggleSelection();
            MainWindow.Frame_MainFrame.GoBack();
        }
        string GenerateDescpriptionText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Модель: {deviceCard.Device.DeviceModel.Model}");
            sb.AppendLine($"Производитель: {deviceCard.Device.DeviceModel.Manufacturer}");
            sb.AppendLine($"Инвентарный номер: {deviceCard.Device.InventoryNumber}");
            sb.AppendLine($"Серийный номер: {deviceCard.Device.SerialNumber}");
            sb.AppendLine($"Короткое имя (имя хоста netbios): {deviceCard.Device.ShortName}");

            return sb.ToString().ToUpper();
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.GoBack();
        }
    }
}
