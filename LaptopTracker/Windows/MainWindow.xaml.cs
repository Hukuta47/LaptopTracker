using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LaptopTracker.Pages;

namespace LaptopTracker
{
    public partial class MainWindow : Window
    {
        static public Frame Frame_MainFrame;
        public MainWindow()
        {
            InitializeComponent();
            Frame_MainFrame = MainFrame;
            MainFrame.Navigate(new Load());
            if (App.entities.Laptop.Any(Laptop => Laptop.Issued == true))
            {
                MainFrame.Navigate(new MainMenuWithTable());
            }
            else
            {
                MainFrame.Navigate(new MainMenu());
            }
        }
        public static void GoToMainPage()
        {
            Frame_MainFrame.Navigate(new Load());
            MainWindow.Frame_MainFrame.NavigationService.RemoveBackEntry();
            if (App.entities.Laptop.Any(Laptop => Laptop.Issued == true))
            {
                Frame_MainFrame.Navigate(new MainMenuWithTable());
            }
            else
            {
                Frame_MainFrame.Navigate(new MainMenu());
            }
        }
    }
}
