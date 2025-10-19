using LaptopTracker.Database;
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

namespace LaptopTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для TakePage.xaml
    /// </summary>
    public partial class TakePage : Page
    {
        GiveRequest giveRequest;
        public TakePage(GiveRequest giveRequest)
        {
            InitializeComponent();
            this.giveRequest = giveRequest;
            TextBlock_FinalMessage.Text = GenerateFinalMessage(giveRequest);
        }
        private void Return_Click(object sender, RoutedEventArgs e) => MainWindow.Frame_MainFrame.GoBack();
        string GenerateFinalMessage(GiveRequest request)
        {
            var devicesGrouped = request.Device.GroupBy(d => d.DeviceModelId).Select(
                g => new
                {
                    Manufacturer = g.First().DeviceModel.Manufacturer,
                    Model = g.First().DeviceModel.Model,
                    Count = g.Count()
                }).OrderBy(g => g.Manufacturer).ThenBy(g => g.Model);

            StringBuilder stringBuilder = new StringBuilder();

            var giver = App.entities.Employee.First(e => e.Id == request.WhoGivedEmployeeId).FirstAndSecondName;
            stringBuilder.AppendLine($"ВЫДАВАЛ НОУТБУКИ: {giver}".ToUpper());
            stringBuilder.AppendLine("НОУТБУКИ:".ToUpper());

            foreach (var group in devicesGrouped)
            {
                stringBuilder.AppendLine($"    - {group.Manufacturer} {group.Model} (кол-во: {group.Count});".ToUpper());
            }

            stringBuilder.AppendLine($"КОММЕНТАРИЙ: \"{request.Comment}\"".ToUpper());

            return stringBuilder.ToString();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            foreach (Device laptop in giveRequest.Device)
            {
                laptop.Laptop.Issued = false;
            }
            App.entities.GiveRequest.Remove(giveRequest);

            App.entities.SaveChanges();
        }
    }
}
