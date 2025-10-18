using LaptopTracker.Database;
using LaptopTracker.Pages;
using System.Windows.Controls;

namespace LaptopTracker.UserControls
{
    public partial class GiveRquestRow : UserControl
    {
        GiveRequest giveRequest;
        public GiveRquestRow(GiveRequest giveRequest)
        {
            InitializeComponent();

            this.giveRequest = giveRequest;

            Label_Date.Content = this.giveRequest.GivedDate.ToShortDateString();
            Label_Time.Content = this.giveRequest.GivedDate.ToShortTimeString();
            TextBlock_Count.Text = this.giveRequest.Device.Count.ToString();
            TextBlock_Comment.Text = this.giveRequest.Comment;
        }

        private void Info_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.Frame_MainFrame.Navigate(new TakePage(giveRequest));
        }
    }
}
