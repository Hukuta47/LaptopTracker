using LaptopTracker.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace LaptopTracker
{
    public partial class MainWindow : Window
    {
        private bool isNavigating;

        static public Frame Frame_MainFrame;
        public MainWindow()
        {
            InitializeComponent();
            Frame_MainFrame = MainFrame;
            if (App.entities.Laptop.Any(Laptop => Laptop.Issued == true))
            {
                MainFrame.Navigate(new MainMenuWithTable());
            }
            else
            {
                MainFrame.Navigate(new MainMenu());
            }
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (isNavigating)
                return;

            e.Cancel = true;
            isNavigating = true;

            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            fadeOut.Completed += (s, _) =>
            {
                // Навигация после завершения анимации
                MainFrame.Navigate(e.Uri ?? e.Content);

                // Когда страница загрузится — запускаем обратную анимацию
                MainFrame.Navigated += Frame_Navigated;
            };

            MainFrame.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            MainFrame.Navigated -= Frame_Navigated;

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            fadeIn.Completed += (s, _) => isNavigating = false;

            MainFrame.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }


        public static void GoToMainPage()
        {
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
