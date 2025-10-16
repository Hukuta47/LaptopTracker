using LaptopTracker.UserControls;
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
    /// Логика взаимодействия для GivePage.xaml
    /// </summary>
    public partial class GivePage : Page
    {
        public GivePage()
        {
            InitializeComponent();

            for (int i = 0; i < 8; i++)
            {
                var card = new DeviceCard();
                card.SetData($"HP Hue PRO #{i + 1}", "Laptop-100", $"Это устройство №{i + 1}");
                ItemsPanel.Children.Add(card);
            }
        }
    }
}
