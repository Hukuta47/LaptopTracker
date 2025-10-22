using LaptopTracker.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LaptopTracker.Pages
{
    public partial class ConfirmPage : Page
    {
        GiveRequest giveRequest;

        public ConfirmPage(GiveRequest request)
        {
            giveRequest = request;
            InitializeComponent();

            StartBorderPulse();
        }

        private void StartBorderPulse()
        {
            // 🔹 Анимация толщины рамки от 5 до 20
            var animation = new ThicknessAnimation
            {
                From = new Thickness(5),
                To = new Thickness(20),
                Duration = TimeSpan.FromSeconds(1.2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            // 🔹 Запускаем анимацию
            PulseBorder.BeginAnimation(Border.BorderThicknessProperty, animation);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Останавливаем анимацию, чтобы не утекали ресурсы
            PulseBorder.BeginAnimation(Border.BorderThicknessProperty, null);

            foreach (Device laptop in giveRequest.Device)
                laptop.Laptop.Issued = false;

            App.entities.GiveRequest.Remove(giveRequest);
            App.entities.SaveChanges();

            MainWindow.Frame_MainFrame.Navigate(new SuccesPage());
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            // Тоже останавливаем анимацию
            PulseBorder.BeginAnimation(Border.BorderThicknessProperty, null);
            MainWindow.Frame_MainFrame.GoBack();
        }
    }
}
