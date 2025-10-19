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
using System.Windows.Threading;

namespace LaptopTracker.Pages
{
    public partial class SuccesPage : Page
    {
        int TimeToExit = 5;
        private DispatcherTimer Timer;
        public SuccesPage()
        {
            InitializeComponent();

            Timer = Timer = new DispatcherTimer( TimeSpan.FromSeconds(1), DispatcherPriority.Render, Timer_Tick, Application.Current.Dispatcher);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeToExit--;
            Textblock_TimeMessage.Text = $"ВОЗВРАЩЕНИЕ НА НАЧАЛЬНУЮ СТРАНИЦУ ЧЕРЕЗ {TimeToExit}...";
            if (TimeToExit == 0)
            {
                Timer.Stop();
                MainWindow.GoToMainPage();
            }
            
        }
    }
}
