using System.Windows;
using LaptopTracker.Pages;

namespace LaptopTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainMenu());
        }
    }
}
