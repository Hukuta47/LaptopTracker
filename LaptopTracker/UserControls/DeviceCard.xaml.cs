using LaptopTracker.Database;
using LaptopTracker.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LaptopTracker.UserControls
{
    public partial class DeviceCard : UserControl
    {
        public bool isSelected = false;
        private bool movedTooFar = false;
        private bool infoShown = false;

        private Point touchStart;
        private DispatcherTimer holdTimer;
          
        private int activeTouchId = -1;
        private Window hostWindow;

        private bool mouseActive = false;
        private Point mouseStart;

        private DateTime lastTouchTime = DateTime.MinValue;
        private DateTime lastMouseTime = DateTime.MinValue;

        public string Info { get; }
        public int DeviceId { get; }


        
        public DeviceCard(Device device)
        {
            InitializeComponent();

            

            TextBlock_Title.Text = $"{device.DeviceModel.Manufacturer} {device.DeviceModel.Model}";
            TextBlock_ShortName.Text = device.ShortName;
            DeviceId = device.Id;






            if (!string.IsNullOrEmpty(device.DeviceModel.Image))
            {
                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(device.DeviceModel.Image);
                bitmap.DownloadCompleted += (s, e) =>
                {
                    Image_NoImage.Visibility = Visibility.Hidden;
                    Console.WriteLine("Изображение успешно загружено!");
                };
                bitmap.DownloadFailed += (s, e) =>
                {
                    Image_NoImage.Visibility = Visibility.Visible;
                    Console.WriteLine("Ошибка загрузки изображения!");
                };
                bitmap.EndInit();

                Image_PictureDevice.Source = bitmap;
            }
            else
            {
                Image_PictureDevice.Source = null;
                Image_NoImage.IsEnabled = true;
            }


            holdTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            holdTimer.Tick += HoldTimer_Tick;

            PreviewTouchDown += DeviceCard_TouchDown;
            PreviewTouchMove += DeviceCard_TouchMove;
            PreviewTouchUp += DeviceCard_TouchUp;

            PreviewMouseDown += DeviceCard_MouseDown;
            PreviewMouseMove += DeviceCard_MouseMove;
            PreviewMouseUp += DeviceCard_MouseUp;
        }

        private void DeviceCard_TouchDown(object sender, TouchEventArgs e)
        {
            lastTouchTime = DateTime.UtcNow;

            activeTouchId = e.TouchDevice.Id;
            hostWindow = Window.GetWindow(this) ?? Application.Current.MainWindow;
            touchStart = (hostWindow != null) ? e.GetTouchPoint(hostWindow).Position : e.GetTouchPoint(this).Position;
            movedTooFar = false;
            infoShown = false;

            if (hostWindow != null)
            {
                hostWindow.PreviewTouchMove += HostWindow_PreviewTouchMove;
                hostWindow.PreviewTouchUp += HostWindow_PreviewTouchUp;
            }

            ((Storyboard)Resources["PressDownAnimation"]).Begin();
            holdTimer.Start();
        }

        private void DeviceCard_TouchMove(object sender, TouchEventArgs e)
        {
            if (activeTouchId != -1 && e.TouchDevice.Id != activeTouchId) return;

            var current = e.GetTouchPoint(this).Position;
            double dx = current.X - (touchStart.X - (hostWindow != null ? this.TransformToAncestor(hostWindow).Transform(new Point(0, 0)).X : 0));
            double dy = current.Y - (touchStart.Y - (hostWindow != null ? this.TransformToAncestor(hostWindow).Transform(new Point(0, 0)).Y : 0));

            if (Math.Abs(dx) > 15 || Math.Abs(dy) > 15)
            {
                movedTooFar = true;
                holdTimer.Stop();
                ((Storyboard)Resources["ReleaseAnimation"]).Begin();
                RemoveHostWindowHandlers();
            }
        }

        private void DeviceCard_TouchUp(object sender, TouchEventArgs e)
        {
            if (activeTouchId != -1 && e.TouchDevice.Id != activeTouchId) return;

            ((Storyboard)Resources["ReleaseAnimation"]).Begin();
            RemoveHostWindowHandlers();

            if (movedTooFar || infoShown)
            {
                holdTimer.Stop();
                activeTouchId = -1;
                return;
            }

            if (holdTimer.IsEnabled)
            {
                holdTimer.Stop();
                ToggleSelection();
            }

            activeTouchId = -1;
        }

        private void HostWindow_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (activeTouchId == -1 || e.TouchDevice.Id != activeTouchId) return;
            if (hostWindow == null) return;

            var current = e.GetTouchPoint(hostWindow).Position;
            double dx = current.X - touchStart.X;
            double dy = current.Y - touchStart.Y;

            if (Math.Abs(dx) > 15 || Math.Abs(dy) > 15)
            {
                movedTooFar = true;
                holdTimer.Stop();
                ((Storyboard)Resources["ReleaseAnimation"]).Begin();
                RemoveHostWindowHandlers();
            }
        }

        private void HostWindow_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (activeTouchId == -1 || e.TouchDevice.Id != activeTouchId) return;

            ((Storyboard)Resources["ReleaseAnimation"]).Begin();
            RemoveHostWindowHandlers();

            if (movedTooFar || infoShown)
            {
                holdTimer.Stop();
                activeTouchId = -1;
                return;
            }

            if (holdTimer.IsEnabled)
            {
                holdTimer.Stop();
                ToggleSelection();
            }

            activeTouchId = -1;
        }


        private void DeviceCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.UtcNow - lastTouchTime).TotalMilliseconds < 300) return;

            lastMouseTime = DateTime.UtcNow;

            mouseActive = true;
            activeTouchId = -1;
            hostWindow = Window.GetWindow(this) ?? Application.Current.MainWindow;
            mouseStart = (hostWindow != null) ? e.GetPosition(hostWindow) : e.GetPosition(this);
            movedTooFar = false;
            infoShown = false;

            if (hostWindow != null)
            {
                hostWindow.PreviewMouseMove += HostWindow_PreviewMouseMove;
                hostWindow.PreviewMouseUp += HostWindow_PreviewMouseUp;
            }

            ((Storyboard)Resources["PressDownAnimation"]).Begin();
            holdTimer.Start();
        }

        private void DeviceCard_MouseMove(object sender, MouseEventArgs e)
        {
            if ((DateTime.UtcNow - lastTouchTime).TotalMilliseconds < 300) return;
            if (!mouseActive) return;

            var current = e.GetPosition(hostWindow ?? (IInputElement)this);
            double dx = current.X - mouseStart.X;
            double dy = current.Y - mouseStart.Y;

            if (Math.Abs(dx) > 15 || Math.Abs(dy) > 15)
            {
                movedTooFar = true;
                holdTimer.Stop();
                ((Storyboard)Resources["ReleaseAnimation"]).Begin();
                RemoveHostWindowHandlers();
            }
        }

        private void DeviceCard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.UtcNow - lastTouchTime).TotalMilliseconds < 300) return;
            if (!mouseActive) return;

            ((Storyboard)Resources["ReleaseAnimation"]).Begin();
            RemoveHostWindowHandlers();

            if (movedTooFar || infoShown)
            {
                holdTimer.Stop();
                mouseActive = false;
                return;
            }

            if (holdTimer.IsEnabled)
            {
                holdTimer.Stop();
                ToggleSelection();
            }

            mouseActive = false;
        }

        private void HostWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((DateTime.UtcNow - lastTouchTime).TotalMilliseconds < 300) return;
            if (!mouseActive || hostWindow == null) return;

            var current = e.GetPosition(hostWindow);
            double dx = current.X - mouseStart.X;
            double dy = current.Y - mouseStart.Y;

            if (Math.Abs(dx) > 15 || Math.Abs(dy) > 15)
            {
                movedTooFar = true;
                holdTimer.Stop();
                ((Storyboard)Resources["ReleaseAnimation"]).Begin();
                RemoveHostWindowHandlers();
            }
        }

        private void HostWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.UtcNow - lastTouchTime).TotalMilliseconds < 300) return;
            if (!mouseActive) return;

            ((Storyboard)Resources["ReleaseAnimation"]).Begin();
            RemoveHostWindowHandlers();

            if (movedTooFar || infoShown)
            {
                holdTimer.Stop();
                mouseActive = false;
                return;
            }

            if (holdTimer.IsEnabled)
            {
                holdTimer.Stop();
                ToggleSelection();
            }

            mouseActive = false;
        }

        private void RemoveHostWindowHandlers()
        {
            if (hostWindow != null)
            {
                hostWindow.PreviewTouchMove -= HostWindow_PreviewTouchMove;
                hostWindow.PreviewTouchUp -= HostWindow_PreviewTouchUp;

                hostWindow.PreviewMouseMove -= HostWindow_PreviewMouseMove;
                hostWindow.PreviewMouseUp -= HostWindow_PreviewMouseUp;
            }
        }

        private void HoldTimer_Tick(object sender, EventArgs e)
        {
            if (activeTouchId == -1 && !mouseActive)
            {
                holdTimer.Stop();
                return;
            }

            holdTimer.Stop();
            infoShown = true;

            ((Storyboard)Resources["ReleaseAnimation"]).Begin();

            RemoveHostWindowHandlers();
            ShowInfo();

            activeTouchId = -1;
            mouseActive = false;
        }

        public void ToggleSelection()
        {
            isSelected = !isSelected;
            var sb = (Storyboard)Resources[isSelected ? "ShowCheckAnimation" : "HideCheckAnimation"];
            sb.Begin();
            GivePage.OnSelectElement();
        }

        private void ShowInfo()
        {
            MainWindow.Frame_MainFrame.Navigate(new СharacteristicPage(this));
        }
    }
}
