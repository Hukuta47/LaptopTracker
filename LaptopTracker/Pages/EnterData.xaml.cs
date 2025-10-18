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
    public partial class EnterData : Page
    {
        List<Device> SelectedDevicesToGive;
        public EnterData(List<Device> SelectedDevicesToGive)
        {
            InitializeComponent();
            this.SelectedDevicesToGive = SelectedDevicesToGive;
            Combobox_SelectEmployee.ItemsSource = App.entities.Employee.Where(e => e.EmployeePosition.Any(p => p.Id == 1 || p.Id == 2 || p.Id == 4 || p.Id == 5)).ToList();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            GiveRequest giveRequest = new GiveRequest()
            {
                Comment = Textbox_EnterComment.Text,
                Device = SelectedDevicesToGive,
                GivedDate = DateTime.Now,
                WhoGivedEmployeeId = (int)Combobox_SelectEmployee.SelectedValue,
            };



            MainWindow.Frame_MainFrame.Navigate(new GivePageResult(giveRequest));
        }

        private void Return_Click(object sender, RoutedEventArgs e) => MainWindow.Frame_MainFrame.GoBack();
    }
}
