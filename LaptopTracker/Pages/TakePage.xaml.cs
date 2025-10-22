using LaptopTracker.Database;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LaptopTracker.Pages
{
    public partial class TakePage : Page
    {
        GiveRequest giveRequest;
        public TakePage(GiveRequest giveRequest)
        {
            InitializeComponent();
            this.giveRequest = giveRequest;
            TextBlock_FinalMessage.Text = GenerateFinalMessage(giveRequest);
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToMainPage();
        }
        string GenerateFinalMessage(GiveRequest request)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var giver = App.entities.Employee.First(e => e.Id == request.WhoGivedEmployeeId).FirstAndSecondName;
            stringBuilder.AppendLine($"ВЫДАЕТ НОУТБУКИ: {giver}".ToUpper());
            stringBuilder.AppendLine("НОУТБУКИ:".ToUpper());

            foreach (var device in request.Device.OrderBy(Device => Device.ShortName))
            {
                stringBuilder.AppendLine($"    - ID: {device.ShortName}, {device.DeviceModel.Manufacturer} {device.DeviceModel.Model};".ToUpper());
            }

            stringBuilder.AppendLine($"КОММЕНТАРИЙ: \"{request.Comment}\"".ToUpper());

            return stringBuilder.ToString();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.Navigate(new ConfirmPage(giveRequest));
        }
    }
}
