using LaptopTracker.Database;
using LaptopTracker.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace LaptopTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для GivePage.xaml
    /// </summary>
    public partial class GivePage : Page
    {
        public GivePage()
        {
            InitializeComponent();


            List<Device> devices = App.entities.Device.Where(laptop => laptop.DeviceType.Id == 9).ToList();

            //.First()

            foreach (var device in devices)
            {
                string Title = $"{device.DeviceModel.Manufacturer} {device.DeviceModel.Model}";

                var card = new DeviceCard(device);
                ItemsPanel.Children.Add(card);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.GoBack();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            List<DeviceCard> selectedDevices = new List<DeviceCard>();

        }
    }
}
