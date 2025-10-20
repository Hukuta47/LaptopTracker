using LaptopTracker.Pages;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        static private TextBlock staticTextBlock_Message;
        static private Border staticBorder_Message;

        private static CancellationTokenSource _cts;
        public MainWindow()
        {
            InitializeComponent();
            Frame_MainFrame = MainFrame;
            staticBorder_Message = Border_Message;
            staticTextBlock_Message = TextBlock_Message;
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
                MainFrame.Navigate(e.Uri ?? e.Content);

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
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
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

        public static async void ShowMessage(string textMessage)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            staticTextBlock_Message.Text = textMessage.ToUpper();

            var moveIn = new ThicknessAnimation
            {
                From = new Thickness(16, -152, 16, 0),
                To = new Thickness(16, 16, 16, 0),
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            staticBorder_Message.BeginAnimation(MarginProperty, moveIn);

            try
            {
                await Task.Delay(3000, token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            var moveOut = new ThicknessAnimation
            {
                From = new Thickness(16, 16, 16, 0),
                To = new Thickness(16, -152, 16, 0),
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            staticBorder_Message.BeginAnimation(MarginProperty, moveOut);
        }

    }
}
