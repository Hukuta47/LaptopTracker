using LaptopTracker.Database;
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
            TextBlock_FinalMessage.Text = GenerateFinalMessage(request);
        }

        private void Give_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show();
        }
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
            stringBuilder.AppendLine($"ВЫДАЕТ НОУТБУКИ: {giver}".ToUpper());
            stringBuilder.AppendLine("НОУТБУКИ:".ToUpper());

            foreach (var group in devicesGrouped)
            {
                stringBuilder.AppendLine($"    - {group.Manufacturer} {group.Model} (кол-во: {group.Count});".ToUpper());
            }

            stringBuilder.AppendLine($"КОММЕНТАРИЙ: \"{request.Comment}\"".ToUpper());

            return stringBuilder.ToString();
        }
        private void Return_Click(object sender, RoutedEventArgs e) => MainWindow.Frame_MainFrame.GoBack();
    }
}
