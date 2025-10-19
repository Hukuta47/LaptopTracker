
using LaptopTracker.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace LaptopTracker.Pages
{
    public partial class СharacteristicPage : Page
    {
        DeviceCard deviceCard;
        public СharacteristicPage(DeviceCard deviceCard)
        {
            this.deviceCard = deviceCard;
            InitializeComponent();
            if (deviceCard.isSelected)
            {
                Button_Select.Content = "СНЯТЬ";
            } 
            else
            {
                Button_Select.Content = "ВЫБРАТЬ";
            }
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            deviceCard.ToggleSelection();
            MainWindow.Frame_MainFrame.GoBack();
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.GoBack();
        }
    }
}
