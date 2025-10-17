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
    /// Логика взаимодействия для EnterData.xaml
    /// </summary>
    public partial class EnterData : Page
    {
        public EnterData()
        {
            InitializeComponent();
            Combobox_SelectEmployee.ItemsSource = App.entities.Employee.Where(e => e.EmployeePosition.Any(p => p.Id == 1 || p.Id == 2 || p.Id == 4 || p.Id == 5)).ToList();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.Navigate(new GivePageResult());
        }
    }
}
