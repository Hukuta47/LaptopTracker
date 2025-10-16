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
            MainFrame.Navigate(new MainMenu());
        }
    }
}
