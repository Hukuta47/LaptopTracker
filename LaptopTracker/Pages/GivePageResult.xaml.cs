using LaptopTracker.Database;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace LaptopTracker.Pages
{
    public partial class GivePageResult : Page
    {
        GiveRequest request;
        public GivePageResult(GiveRequest request)
        {
            InitializeComponent();
            this.request = request;
            TextBlock_FinalMessage.Text = GenerateFinalMessage(request);
        }

        private void Give_Click(object sender, RoutedEventArgs e)
        {
            string LaptopsText = "";

            foreach (Device device in request.Device)
            {
                device.Laptop.Issued = true;
                LaptopsText += $"{device.DeviceModel.Manufacturer} {device.DeviceModel.Model} {device.InventoryNumber} {device.ShortName}; ";
            }
            App.entities.GiveRequest.Add(request);
            App.entities.GiveRequest_Log.Add(new GiveRequest_Log() { WhoGivedLaptops = App.entities.Employee.First(employee => employee.Id == request.WhoGivedEmployeeId).FullName, ReceivedDate = DateTime.Now, Comment = request.Comment, Laptops = LaptopsText });
            App.entities.SaveChanges();
            MainWindow.Frame_MainFrame.Navigate(new SuccesPage());
            
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
        private void Return_Click(object sender, RoutedEventArgs e) => MainWindow.Frame_MainFrame.Navigate(MainWindow.EnterData);
    }
}
