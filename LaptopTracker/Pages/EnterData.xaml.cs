using LaptopTracker.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LaptopTracker.Pages
{
    public partial class EnterData : Page
    {
        List<Device> SelectedDevicesToGive;
        public EnterData(List<Device> SelectedDevicesToGive)
        {
            InitializeComponent();
            this.SelectedDevicesToGive = SelectedDevicesToGive;
            Combobox_SelectEmployee.ItemsSource = App.entities.Employee.Where(e => e.EmployeePosition.Any(p => p.Id == 1 || p.Id == 2 || p.Id == 4 || p.Id == 5)).ToList();
        }
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (Combobox_SelectEmployee.SelectedItem == null)
            {
                MainWindow.ShowMessage("Поле выбора \"Кто выдал\" пустое");
            }
            else if (string.IsNullOrWhiteSpace(Textbox_EnterComment.Text) || string.IsNullOrEmpty(Textbox_EnterComment.Text))
            {
                MainWindow.ShowMessage("Поле описания введено не корректно.");
            }
            else
            {
                GiveRequest giveRequest = new GiveRequest()
                {
                    Comment = Textbox_EnterComment.Text,
                    Device = SelectedDevicesToGive,
                    GivedDate = DateTime.Now,
                    WhoGivedEmployeeId = (int)Combobox_SelectEmployee.SelectedValue,
                };

                MainWindow.Frame_MainFrame.Navigate(new GivePageResult(giveRequest));
            }   
        }

        private void Return_Click(object sender, RoutedEventArgs e) => MainWindow.Frame_MainFrame.GoBack();

        private void Textbox_EnterComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Textbox_EnterComment.Text.Length > 250)
            {
                Textbox_EnterComment.Text = Textbox_EnterComment.Text.Substring(0, 250);
                Textbox_EnterComment.CaretIndex = Textbox_EnterComment.Text.Length;
                MainWindow.ShowMessage("Допустимо только до 250 символов");
            }
        }
    }
}
