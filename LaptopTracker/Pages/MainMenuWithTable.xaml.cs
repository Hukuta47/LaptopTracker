using System.Windows;
using System.Windows.Controls;
using LaptopTracker.Database;
using LaptopTracker.UserControls;

namespace LaptopTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWithTable.xaml
    /// </summary>
    public partial class MainMenuWithTable : Page
    {
        public MainMenuWithTable()
        {
            InitializeComponent();
            foreach (GiveRequest request in App.entities.GiveRequest)
            {
                StackPanel_ListRequests.Children.Add(new GiveRquestRow(request));
            }
        }
        private void Give_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.Navigate(MainWindow.GivePage);
        }
    }
}
