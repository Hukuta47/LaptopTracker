using LaptopTracker.Database;
using LaptopTracker.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace LaptopTracker.Pages
{
    public partial class GivePage : Page
    {
        static WrapPanel staticItemsPanel;
        static Button staticButton_Next;
        public GivePage()
        {
            InitializeComponent();
            staticItemsPanel = ItemsPanel;
            staticButton_Next = Button_Next;
            List<Device> devices = App.entities.Device.Where(laptop => laptop.DeviceModel.DeviceTypeId == 9).OrderBy(laptop => laptop.DeviceModelId).ToList();
            List<Device> selectedDevices = new List<Device>();

            foreach (var device in devices)
            {
                if (device.Laptop.Issued == false)
                {
                    string Title = $"{device.DeviceModel.Manufacturer} {device.DeviceModel.Model}";
                    var card = new DeviceCard(device);
                    ItemsPanel.Children.Add(card);
                }
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.GoBack();
        }

        public static void OnSelectElement()
        {
            
            List<Device> devices = App.entities.Device.Where(laptop => laptop.DeviceModel.DeviceTypeId == 9).OrderBy(laptop => laptop.DeviceModelId).ToList();
            bool isOneShosen = staticItemsPanel.Children.Cast<DeviceCard>().Where(Card => Card.isSelected == true).ToList().Count >= 1;


            if (isOneShosen)
            {
                staticButton_Next.Visibility = Visibility.Visible;
            }
            else
            {
                staticButton_Next.Visibility = Visibility.Hidden;
            }
        }



        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            List<DeviceCard> selectedDevices = ItemsPanel.Children.Cast<DeviceCard>().Where(Card => Card.isSelected == true).ToList();
            List<Device> devices = new List<Device>();

            foreach (DeviceCard deviceCard in selectedDevices)
            {
                devices.Add(App.entities.Device.First(device => device.Id == deviceCard.DeviceId));
            }

            MainWindow.Frame_MainFrame.Navigate(new EnterData(devices));
            





        }
    }
}
